using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contract
{
    public interface IDataRepository
    {

    }

    public interface IDataRepository<T>:IDataRepository
        where T : class, new() //Where T:class mean that T must be a reference type including class, interface, delegate or array type
                               //new() make sure that T have to have a default constructer
    {
        T Add(T entity); //add new entity

        void Remove(T entity);

        void Remove(int id); //remove entity with certain id

        T Update(T entity);
        IEnumerable<T> Get(); //get all entity

        T Get(int id); //Get entity with certain id
    }
}
