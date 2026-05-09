using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteEndereco.Web.Models
{
    public class Endereco
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o CEP.")]
        [StringLength(8)]
        public string Cep { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o Logradouro.")]
        [StringLength(200)]
        public string Logradouro { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "Informe o Bairro.")]
        [StringLength(100)]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a Cidade.")]
        [StringLength(100)]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a UF.")]
        [StringLength(2)]
        public string Uf { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o Numero.")]
        [StringLength(20)]
        public string Numero { get; set; } = string.Empty;

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
    }
}