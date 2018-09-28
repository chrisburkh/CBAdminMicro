using CBAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBAdmin.Service
{
    public interface IApiService<T>
    {
        Task<IEnumerable<T>> GetAll();

        Task<IEnumerable<T>> GetAll(string searchstring);

        Task<T> Get(string id);
        Task Create(T entity);
        Task Write(T entity);
        Task Delete(string id);

        void SetBaseUrl(string url);
    }
}
