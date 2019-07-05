using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesIntegrados.Web.Models
{
    public class UsuarioViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Nome { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Nascimento { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Senha { get; set; }
    }
}
