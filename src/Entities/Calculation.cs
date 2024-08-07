using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Calculation
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; } = "mult";

        [Required]
        public string Expression { get; set; } = "test";

        [Required]
        public DateTime CreateDate { get; set; }

        public double Result { get; set; }
    }
}