using System.ComponentModel.DataAnnotations;

namespace GUFOS_BackEnd.ViewModels
{
    public class LoginViewModel
    {
        // Data Annotations
        [Required]
        public string Email { get; set; }
        // definimos o tamanho do campo
        [StringLength(255, MinimumLength = 3)]
        public string Senha { get; set; }
    }
}