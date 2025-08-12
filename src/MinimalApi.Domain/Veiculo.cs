using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.Domain.Entidades
{
    public class Veiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Marca { get; set; } = default!;

        [Required]
        [StringLength(100)]
        public string Modelo { get; set; } = default!;

        [Required]
        public int Ano { get; set; }

        [Required]
        [StringLength(50)]
        public string Cor { get; set; } = default!;
    }
}
