using System.ComponentModel.DataAnnotations;

namespace MODEL
{
    public class UsuarioCadastro : Usuario
    {
        [Required]
        [StringLength(60, ErrorMessage = "Máximo de {1} caracteres.")]
        public string Nome { get; set; }
    }
}
