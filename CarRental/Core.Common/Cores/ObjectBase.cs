using Core.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Common.Contract;

namespace Core.Common.Cores
{
    public class ObjectBase : ObjectNotification, IDirtyCapable
    {
        protected virtual void OnPropertyChanged(string PropertyName, bool makeDirty)
        {
            OnPropertyChanged(PropertyName);
            if (makeDirty)
                isDirty = true;
        }
        private bool isDirty;
        [NotNavigable]
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
            walk(this);
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
