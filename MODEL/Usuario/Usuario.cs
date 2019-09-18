using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MODEL
{
    public class Usuario
    {
        public int CodUsuario { get; set; }
        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [StringLength(100, ErrorMessage = "Máximo de {1} caracteres.")]
        public string Email { get; set; }
        public byte[] Senha { get; set; }
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [StringLength(20, ErrorMessage = "Máximo de {1} caracteres.")]
        public string SenhaHash { get; set; }
        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}
