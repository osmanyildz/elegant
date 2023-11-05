using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.data.Abstract
{
    public interface IRepository<T> //T bir entity sınıfı
    {
        T GetById(int id);
        List<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        //create delete update getall getbyid
    }
}