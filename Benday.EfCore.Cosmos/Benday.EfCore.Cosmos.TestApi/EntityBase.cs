using Benday.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Benday.EfCore.Cosmos.TestApi
{
    public abstract class EntityBase : IStringIdentity
    {
        public EntityBase()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Id { get; set; }
    }
}