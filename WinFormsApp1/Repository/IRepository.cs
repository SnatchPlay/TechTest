using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Repository
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        void Create(T tempObj);
        void Delete(int id);
        void Read();
        T Get(int id);
        void Update(T obj);
        void Refresh();
    }
}
