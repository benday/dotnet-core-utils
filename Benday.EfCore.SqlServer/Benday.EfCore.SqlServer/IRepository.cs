﻿using System;
using System.Collections.Generic;

namespace Benday.EfCore.SqlServer
{
    public interface IRepository<T> where T : IInt32Identity
    {
        IList<T> GetAll();
        T GetById(int id);
        void Save(T saveThis);
        void Delete(T deleteThis);
    }
}
