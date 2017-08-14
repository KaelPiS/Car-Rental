using Core.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Common.Contract;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;
using Core.Common.Utils;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.Composition.Hosting;

namespace Core.Common.Cores
{
    public class ObjectBase : ObjectNotification, IDirtyCapable, IExtensibleDataObject, IDataErrorInfo
    {
        public ObjectBase()
        {
            _Validator = GetValidator();
            Validate();
        }
        public static CompositionContainer Container { get; set; }
        protected IValidator _Validator = null;
        protected IEnumerable<ValidationFailure> _ValidationErrors;
        protected override void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName, true);
        }
        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName, makeDirty);
        }
        protected virtual void OnPropertyChanged(string PropertyName, bool makeDirty)
        {
            base.OnPropertyChanged(PropertyName);
            if (makeDirty)
                isDirty = true;
            Validate();
        }
        private bool isDirty;
        [NotNavigable]
        public virtual bool IsDirty
        {
            get { return isDirty; }
            protected set
            {
                isDirty = value;
                OnPropertyChanged(() => IsDirty, false);
            }
        }

        protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject, Action<IList> snippetForCollection, params string[] exemptProperties)
        {
            List<ObjectBase> VisitedList = new List<ObjectBase>();
            Action<ObjectBase> walk = null;
            List<string> Exemptions = new List<string>();
            if (exemptProperties != null)
                Exemptions = exemptProperties.ToList();
            walk = (o) =>
            {
                if (o != null && !VisitedList.Contains(o))
                {
                    VisitedList.Add(o);
                    bool isExit = snippetForObject.Invoke(o);
                    if (!isExit)
                    {
                        PropertyInfo[] properties = o.GetBrowsableProperties();
                        foreach (PropertyInfo prop in properties)
                        {
                            if (!Exemptions.Contains(prop.Name))
                            {
                                if (prop.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                                {
                                    ObjectBase obj = (ObjectBase)prop.GetValue(o, null);
                                    walk(obj);
                                }
                                else
                                {
                                    IList collection = prop.GetValue(o, null) as IList;
                                    if (collection != null)
                                    {
                                        snippetForCollection.Invoke(collection);
                                        foreach (object item in collection)
                                        {
                                            if (item is ObjectBase)
                                                walk((ObjectBase)item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            walk(this);
        }



        public void CleanAll()
        {
            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                        o.IsDirty = false;
                    return false;
                }, coll => { });
        }

        public bool isAnythingDirty()
        {
            bool isDirty = false;
            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                    {
                        isDirty = true;
                        return true;
                    }
                    else
                        return false;
                }, coll => { });
            return isDirty;
        }

        public List<IDirtyCapable> GetDirtyObjects()
        {
            List<IDirtyCapable> DirtyObjects = new List<IDirtyCapable>();
            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                        DirtyObjects.Add(o);
                    return false;
                }, coll => { });
            return DirtyObjects;
        }

        protected virtual IValidator GetValidator()
        {
            return null;
        }
        [NotNavigable]
        public IEnumerable<ValidationFailure> ValidationErrors
        {
            get { return _ValidationErrors; }
            set { }
        }

        public void Validate()
        {
            if (_Validator != null)
            {
                ValidationResult result = _Validator.Validate(this);
                _ValidationErrors = result.Errors;
            }
        }
        [NotNavigable]
        public virtual bool IsValid
        {
            get
            {
                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                    return false;
                else
                    return true;
            }
        }

        public ExtensionDataObject ExtensionData { get; set; }

        string IDataErrorInfo.Error
        {
            get
            {
                return string.Empty;
            }
        } 

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                StringBuilder errors = new StringBuilder();

                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                {
                    foreach (ValidationFailure validationError in _ValidationErrors)
                    {
                        if (validationError.PropertyName == columnName)
                            errors.AppendLine(validationError.ErrorMessage);
                    }
                }

                return errors.ToString();
            }
        }
    }
}
