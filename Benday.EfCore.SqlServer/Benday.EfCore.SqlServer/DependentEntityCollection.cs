using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Benday.EfCore.SqlServer
{
    public class DependentEntityCollection<T> :
        IDependentEntityCollection where T : class, IEntityBase
    {
        private readonly IList<T> _entities;

        public DependentEntityCollection(IList<T> entities)
        {
            _entities = entities ?? throw new ArgumentNullException(nameof(entities), "Argument cannot be null.");
        }

        public void BeforeSave(DbContext dbContext)
        {
            foreach (var entity in _entities)
            {
                if (entity.IsMarkedForDelete == true)
                {
                    RemoveFromDbSet(dbContext, entity);
                }
            }
        }

        private void RemoveFromDbSet(DbContext dbContext, T entity)
        {
            dbContext.Remove<T>(entity);
        }

        public void AfterSave()
        {
            var deleteThese = _entities.Where(x => x.IsMarkedForDelete == true).ToList();

            deleteThese.ForEach(x => _entities.Remove(x));
        }
    }
}
