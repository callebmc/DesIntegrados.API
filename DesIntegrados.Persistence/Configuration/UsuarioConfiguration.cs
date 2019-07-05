using DesIntegrados.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesIntegrados.Persistence.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            //ID
            builder.HasKey(p => p.Id);

            //NOME
            builder.Property(p => p.Nome).IsRequired(true).HasMaxLength(255);

            //EMAIL
            builder.Property(p => p.Email).IsRequired(true);

            //Numero de Iteracoes
            builder.Property(p => p.iteracoes).IsRequired(true);

            //Data Final da execucao
            builder.Property(p => p.dataFim).IsRequired(true);

            //Data inicial da execucao
            builder.Property(p => p.dataInicio).IsRequired(true);

            //Tamanho da imagem
            builder.Property(p => p.tamanho).IsRequired(true);

            //Senha
            builder.Property(p => p.Senha).IsRequired(true);

            //Imagem
            //builder.Property(p => p.Imagem).IsRequired(false);
        }
    }
}
