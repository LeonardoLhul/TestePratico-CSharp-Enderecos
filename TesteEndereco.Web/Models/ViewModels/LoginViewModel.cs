using System.ComponentModel.DataAnnotations;

namespace TesteEndereco.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o usuário.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a senha.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
    }
}