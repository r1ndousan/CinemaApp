using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CinemaConsole.Data.Entities;

namespace CinemaConsole.Data.IEntityTypeConfiguration
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.SeatsBooked)
                   .IsRequired();

            builder.Property(b => b.BookingTime)
                   .IsRequired();

            // связи
            builder.HasOne(b => b.Client)
                   .WithMany()   // у Client пока нет навиг. свойства Bookings
                   .HasForeignKey(b => b.ClientId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Session)
                   .WithMany()   // у Session пока нет навиг. свойства Bookings
                   .HasForeignKey(b => b.SessionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
