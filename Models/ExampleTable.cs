using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EFCorePerformanceTest.Models
{
    public class ExampleTable
    {
        public ExampleTable()
        {
            Created = DateTime.UtcNow;            
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? Created { get; set; }
        public string Message { get; set; }
        public bool IsEfCore { get; set; }
    }
}
