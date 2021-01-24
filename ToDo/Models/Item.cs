using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ListId { get; set; }
        public virtual List list { get; set; }

        [Required]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Completion Date")]
        public DateTime? CompletionDate { get; set; }
    }
}
