using System.ComponentModel.DataAnnotations;
using Benday.Common;

namespace Benday.EfCore.SqlServer.TestApi
{
    public class PersonNote : IInt32Identity
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PersonId { get; set; }

        public Person Person { get; set; }

        [Required]
        public string NoteText { get; set; }
    }
}
