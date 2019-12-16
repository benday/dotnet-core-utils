﻿using System.Collections.Generic;
using Benday.Common;

namespace Benday.EfCore.Cosmos
{
    public interface ISearchableRepository<T> : IRepository<T> 
        where T : IInt32Identity
    {
        SearchResult<T> Search(Search search);
    }
}
