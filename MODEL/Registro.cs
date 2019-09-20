using System;
using System.ComponentModel.DataAnnotations;

namespace MODEL
{
    public class Registro
    {
        public int CodRegistro { get; set; }
        [Required]
        [Range(0, 999.0)]
        public decimal Peso { get; set; }
        [Required]
        [Range(0, 999.0)]
        public int Altura { get; set; }
        [Required]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime DataRegistro { get; set; }
        public int CodUsuario { get; set; }
    }
}
