using System.ComponentModel.DataAnnotations;

namespace MODEL
{
    public class Usuario
    {
        public int CodUsuario { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Máximo de {1} caracteres.")]
        public string Email { get; set; }
        public byte[] Senha { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Máximo de {1} caracteres.")]
        public string SenhaHash { get; set; }
    }
}
