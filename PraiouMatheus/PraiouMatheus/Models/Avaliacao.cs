using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PraiouMatheus.Models
{
    public class Avaliacao
    {
        [Key]
        public int? AvaliacaoId { get; set; }

        [Range(1, 5, ErrorMessage = "A nota deve ser entre 1 e 5.")]
        [Required]
        public int? Nota { get; set; }  // Nota da avaliação, de 1 a 5

        [Required]
        [MaxLength(500)]
        public string? Comentario { get; set; }  // Comentário da avaliação

        public DateTime? DataAvaliacao { get; set; } = DateTime.Now;  // Data da avaliação

        [Required]
        public string? UsuarioEmail { get; set; }  // Email do Usuario que fez a avaliação

        [ForeignKey(nameof(Usuario))]
        [DisplayName("Email do Usuario")]
        public int? FK_UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
