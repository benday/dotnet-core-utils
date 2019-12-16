using Benday.Common;
using System.ComponentModel.DataAnnotations;

namespace Benday.EfCore.Cosmos.TestApi
{
    public class PersonNote : EntityBase
    {
        [Required]
        public int PersonId { get; set; }

        public Person Person { get; set; }

        [Required]
        public string NoteText { get; set; }
    }
}