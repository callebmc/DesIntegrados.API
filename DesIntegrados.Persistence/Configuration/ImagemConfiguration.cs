using System;
using System.Collections.Generic;
using System.Text;
using DesIntegrados.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesIntegrados.Persistence.Configuration
{
    public class ImagemConfiguration : IEntityTypeConfiguration<Imagem>
    {
        public void Configure(EntityTypeBuilder<Imagem> builder)
        {
            //ID da imagem
            builder.HasKey(p => p.id_imagem);

            //ID do usuario que criou a imagem
            builder.Property(p => p.Id_User).IsRequired(true);

            //Numero de iterações
            builder.Property(p => p.iteracoes).IsRequired(true);

            //Data inicio
            builder.Property(p => p.dataFim).IsRequired(true);

            //Data fim
            builder.Property(p => p.dataInicio).IsRequired(true);

            //Tamanho da imagem
            builder.Property(p => p.tamanho).IsRequired(true);

            //Imagem
            //builder.Property(p => p.reconstruida).IsRequired(true);
        }
    }
}
