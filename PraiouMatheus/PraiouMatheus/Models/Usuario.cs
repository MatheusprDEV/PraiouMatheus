using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace PraiouMatheus.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required]
        public string? Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DataNasc { get; set; }

        [Required]
        public string Senha { get; set; } // A senha deve ser armazenada como um hash

        public ICollection<Avaliacao>? Avaliacoes { get; set; }
    }
}
