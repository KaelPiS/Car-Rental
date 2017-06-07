using Core.Common.Extensions;
using Core.Common.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Cores
{
    public class ObjectBase : INotifyPropertyChanged
    {
        private event PropertyChangedEventHandler _PropertyChanged;
        List<PropertyChangedEventHandler> PropertyChangedSubcribers = new List<PropertyChangedEventHandler>();
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!PropertyChangedSubcribers.Contains(value))
                {
                    _PropertyChanged += value;
                    PropertyChangedSubcribers.Add(value);
                }
            }

            remove
            {
                _PropertyChanged -= value;
                PropertyChangedSubcribers.Remove(value);
            }
        }
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            if (_PropertyChanged != null)
                _PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> PropertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(PropertyExpression);
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged(string PropertyName, bool makeDirty)
        {
            OnPropertyChanged(PropertyName);
            if (makeDirty)
                isDirty = true;
        }
        private bool isDirty;

        public bool IsDirty
        {
            get { return isDirty; }
            set { isDirty = value; }
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
        }

        public List<ObjectBase> GetDirtyObjects()
        {
            List<ObjectBase> DirtyObjects = new List<ObjectBase>();
            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                        DirtyObjects.Add(o);
                    return false;
                }, coll => { });
            return DirtyObjects;
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
                        IsDirty = true;
                        return true;
                    }
                    else
                        return false;
                }, coll => { });
            return isDirty;
        }
    }
}
