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
    public class HotelReservationInitializer : DropCreateDatabaseAlways<HotelReservationContext>
    {
        protected override void Seed(HotelReservationContext context)
        {
            //Let's initialize the database with some dummy data

            //Services first
            var accessible = new RoomService("Accessible");
            var wifi = new RoomService("Wifi");
            var sat = new RoomService("SatTV");
            var safeLocker = new RoomService("SafeLocker");
            var airConditioning = new RoomService("AirConditioning");
            var bathtub = new RoomService("Bathtub");

            //Some dates
            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);
            var afterTomorrow = DateTime.Today.AddDays(2);

            var hotel1 = new Hotel("Flower Hotel")
                .WithRooms(
                    new Room("Single room Violet", image: "violet")
                        .HasRating(2.4m)
                        .HasBasePrice(56.0m)
                        .HasSpecialPrice(50.0m, from: tomorrow, nights: 3)
                        .WithBeds(BedKind.Single, 1)
                        .WithServices(wifi, airConditioning, accessible)
                        .WithReservation("Michael Knight", today, nights: 1)
                        .WithReservation("Bonnie Barstow", afterTomorrow, nights: 1),

                    new Room("Single room Rose", image: "rose")
                        .HasRating(3.2m)
                        .HasBasePrice(62.0m)
                        .HasSpecialPrice(60.0m, from: afterTomorrow, nights: 5)
                        .WithBeds(BedKind.Single, 1)
                        .WithServices(wifi, airConditioning, safeLocker, sat),

                    new Room("Double room Sunflower", image: "sunflower")
                        .HasRating(3.8m)
                        .HasBasePrice(81.0m)
                        .HasSpecialPrice(70.0m, from: today, nights: 3)
                        .WithBeds(BedKind.Full, 1)
                        .WithServices(wifi, safeLocker, bathtub, accessible)
                        .WithReservation("John Annibal Smith", tomorrow, nights: 2),

                    new Room("Double room Tulip", image: "tulip")
                        .HasRating(3.8m)
                        .HasBasePrice(82.0m)
                        .HasSpecialPrice(60.0m, from: afterTomorrow, nights: 5)
                        .WithBeds(BedKind.Single, 2)
                        .WithServices(sat, bathtub)
                        .WithReservation("Ally McBeal", today, nights: 1),

                    new Room("Double room Daisy", image: "daisy")
                        .HasRating(3.8m)
                        .HasBasePrice(78.0m)
                        .WithBeds(BedKind.King, 1)
                        .WithServices(airConditioning, sat, accessible),

                    new Room("Triple room Dandelion", image: "dandelion")
                        .HasRating(3.3m)
                        .HasBasePrice(92.0m)
                        .HasSpecialPrice(90.0m, from: tomorrow, nights: 2)
                        .WithBeds(BedKind.Single, 3)
                        .WithServices(wifi, safeLocker, bathtub, airConditioning)
                        .WithReservation("Thomas Magnum", afterTomorrow, nights: 1),

                    new Room("Triple room Magnolia", image: "magnolia")
                        .HasRating(4.0m)
                        .HasBasePrice(127.0m)
                        .HasSpecialPrice(100.0m, from: afterTomorrow, nights: 4)
                        .WithBeds(BedKind.Single, 1)
                        .WithBeds(BedKind.Full, 1)
                        .WithServices(sat, accessible)
                        .WithReservation("Fran Fine", afterTomorrow, nights: 2),

                    new Room("Family room Daffodil", image: "daffodil")
                        .HasRating(4.1m)
                        .HasBasePrice(145.0m)
                        .HasSpecialPrice(130.0m, from: today, nights: 3)
                        .WithBeds(BedKind.Single, 2)
                        .WithBeds(BedKind.King, 1)
                        .WithServices(wifi, airConditioning, safeLocker, accessible)
                );

            var hotel2 = new Hotel("Planet Hotel")
                .WithRooms(
                        new Room("Single room Saturn", image: "saturn")
                            .HasRating(3.1m)
                            .HasBasePrice(62.0m)
                            .HasSpecialPrice(56.0m, from: today, nights: 2)
                            .WithBeds(BedKind.Single, 1)
                            .WithServices(airConditioning, safeLocker)
                            .WithReservation("Buffy Summers", today, nights: 1),

                        new Room("Single room Mercury", image: "mercury")
                            .HasRating(3.5m)
                            .HasBasePrice(68.0m)
                            .HasSpecialPrice(60.0m, from: today, nights: 5)
                            .WithBeds(BedKind.Single, 1)
                            .WithServices(wifi, sat, accessible)
                            .WithReservation("Joey Potter", tomorrow, nights: 2),

                        new Room("Double room Neptune", image: "neptune")
                            .HasRating(3.9m)
                            .HasBasePrice(86.0m)
                            .HasSpecialPrice(76.0m, from: tomorrow, nights: 3)
                            .WithBeds(BedKind.Full, 1)
                            .WithServices(bathtub, airConditioning),

                        new Room("Double room Mars", image: "mars")
                            .HasRating(4.0m)
                            .HasBasePrice(90.0m)
                            .HasSpecialPrice(84.0m, from: afterTomorrow, nights: 3)
                            .WithBeds(BedKind.Single, 2)
                            .WithServices(wifi, bathtub, accessible)
                            .WithReservation("Veronica Mars", afterTomorrow, nights: 1),

                        new Room("Triple room Jupiter", image: "jupiter")
                            .HasRating(4.5m)
                            .HasBasePrice(112.0m)
                            .HasSpecialPrice(100.0m, from: today, nights: 3)
                            .WithBeds(BedKind.Single, 3)
                            .WithServices(bathtub, airConditioning, accessible),

                        new Room("Double room Venus", image: "venus")
                            .HasRating(4.1m)
                            .HasBasePrice(122.0m)
                            .WithBeds(BedKind.Full, 1)
                            .WithBeds(BedKind.Single, 2)
                            .WithServices(wifi, safeLocker, airConditioning)
                            .WithReservation("C.J. Parker", afterTomorrow, nights: 2)
                            );


            context.Hotels.Add(hotel1);
            context.Hotels.Add(hotel2);
            context.SaveChanges();
        }
    }
}
