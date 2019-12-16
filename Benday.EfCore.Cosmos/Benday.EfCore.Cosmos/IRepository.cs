using System;
using System.Collections.Generic;
using Benday.Common;

namespace Benday.EfCore.Cosmos
{
    public interface IRepository<T> where T : IStringIdentity
    {
        IList<T> GetAll();
        IList<T> GetAll(int maxNumberOfRows);
        T GetById(string id);
        void Save(T saveThis);
        void Delete(T deleteThis);
    }
}
