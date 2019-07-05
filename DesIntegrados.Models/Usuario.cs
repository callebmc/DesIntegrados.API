using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text;

namespace DesIntegrados.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Nome { get; set; }

        public string tamanho { get; set; }

        public DateTime dataInicio { get; set; }

        public DateTime dataFim { get; set; }

        public int iteracoes { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Senha { get; set; }

        //public IList<float> Imagem { get; set; }
    }
}
