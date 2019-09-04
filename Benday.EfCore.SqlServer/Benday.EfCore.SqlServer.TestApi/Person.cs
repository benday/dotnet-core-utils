using Benday.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benday.EfCore.SqlServer.TestApi
{
    [Table("Person")]
    public class Person : IInt32Identity
    {
        public Person()
        {
            Notes = new List<PersonNote>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public List<PersonNote> Notes { get; set; }
    }
}