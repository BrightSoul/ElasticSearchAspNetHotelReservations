using AspNetElasticSearchDemo.Models;
using AspNetElasticSearchDemo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Services
{
    public class HotelReservationContext : DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>().HasMany(hotel => hotel.Rooms).WithRequired(room => room.Hotel).HasForeignKey(room => room.HotelId);
            modelBuilder.Entity<Room>().HasMany(room => room.Services).WithMany();
            modelBuilder.Entity<Room>().HasMany(room => room.Beds).WithMany();
            modelBuilder.Entity<Room>().HasMany(room => room.SpecialPrices).WithRequired(specialPrice => specialPrice.Room);
            modelBuilder.Entity<Room>().HasMany(room => room.Reservations).WithRequired(reservation => reservation.Room);
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<SpecialPrice> SpecialPrices { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
