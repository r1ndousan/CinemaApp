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
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.StartTime)
                   .IsRequired();

            builder.Property(s => s.MovieTitle)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(s => s.AvailableSeats)
                   .IsRequired();
        }
    }
}
