using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CinemaConsole.Data.Entities;

namespace CinemaConsole.Data.IEntityTypeConfiguration
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Login)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(c => c.Login)
                   .IsUnique();

            builder.Property(c => c.PasswordHash)
                   .IsRequired();
        }
    }
}

