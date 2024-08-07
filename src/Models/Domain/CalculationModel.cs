using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Domain
{
    public class CalculationModel : ModelBase
    {

        [Required]
        public string Type { get; set; }
        
        [Required]
        public string Expression { get; set; }

        public double Result { get; set; }
    }
}