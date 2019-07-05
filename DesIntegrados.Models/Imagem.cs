using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text;

namespace DesIntegrados.Models
{
    public class Imagem
    {

        public int id_imagem { get; set; }

        public int Id_User { get; set; }

        [Required]
        public DateTime dataInicio { get; set; }

        [Required]
        public DateTime dataFim { get; set; }

        [Required]
        public int tamanho { get; set; }

        [Required]
        public int iteracoes { get; set; }        

        //public Bitmap reconstruida { get; set; }        
    }
}
