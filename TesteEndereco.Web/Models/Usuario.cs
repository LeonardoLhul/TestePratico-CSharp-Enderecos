using System.ComponentModel.DataAnnotations;

namespace TesteEndereco.Web.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string SenhaHash { get; set; } = string.Empty;

        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
    }
}
