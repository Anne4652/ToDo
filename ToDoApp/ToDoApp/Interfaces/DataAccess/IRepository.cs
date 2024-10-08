using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Interfaces.DataAccess
{
    public interface IRepository<T, Type>
    {
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(Type Id);
    }
}
