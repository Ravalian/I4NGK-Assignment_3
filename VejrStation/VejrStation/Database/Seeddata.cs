using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VejrStation.Entities;
using static BCrypt.Net.BCrypt;

namespace VejrStation.Database
{
    public static class Seeddata
    {
        public const int BcryptWorkfactor = 10;

        public static void SeedData(MyDBContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Users.Any())
                SeedUsers(context);
            if (!context.Observations.Any())
                SeedObservations(context);
        }

        static void SeedUsers(MyDBContext context)
        {
            context.Users.AddRange(
                new User
                    {
                        FirstName = "Hans",
                        LastName = "Goldengun",
                        Username = "HansG",
                        Password = HashPassword("1234", BcryptWorkfactor)
                    },
                new User
                    {
                        FirstName = "Frans",
                        LastName = "Goldengun",
                        Username = "FransG",
                        Password = HashPassword("1234", BcryptWorkfactor)
                },
                new User
                {
                    FirstName = "Torben",
                    LastName = "Swagger",
                    Username = "Swag",
                    Password = HashPassword("1234", BcryptWorkfactor)
                },
                new User
                {
                    FirstName = "Ulla",
                    LastName = "Knudsen",
                    Username = "Ulla",
                    Password = HashPassword("1234", BcryptWorkfactor)
                },
                new User
                {
                    FirstName = "Johnny",
                    LastName = "Bravo",
                    Username = "Bravo",
                    Password = HashPassword("1234", BcryptWorkfactor)
                }
            );
            context.SaveChanges();
        }

        static void SeedObservations(MyDBContext context)
        {
            context.Observations.AddRange(
                new Observation
                    {
                       DateObserved = DateTime.Now,
                       locationName = "Ebeltoft",
                       locationLat = 56.19,
                       locationLot = 10.67,
                       Temperature = 14.6,
                       Humidity = 65,
                       AirPressure = 1012.8
                    },
                new Observation
                {
                    DateObserved = DateTime.Now.AddHours(1),
                    locationName = "Ebeltoft",
                    locationLat = 56.19,
                    locationLot = 10.67,
                    Temperature = 17.6,
                    Humidity = 55,
                    AirPressure = 1013.8
                },
                new Observation
                {
                    DateObserved = DateTime.Now.AddHours(2),
                    locationName = "Ebeltoft",
                    locationLat = 56.19,
                    locationLot = 10.67,
                    Temperature = 18.1,
                    Humidity = 50,
                    AirPressure = 1013.5
                },
                new Observation
                {
                    DateObserved = DateTime.Now.AddHours(3),
                    locationName = "Ebeltoft",
                    locationLat = 56.19,
                    locationLot = 10.67,
                    Temperature = 17.8,
                    Humidity = 53,
                    AirPressure = 1013.1
                },
                new Observation
                {
                    DateObserved = DateTime.Now.AddDays(1),
                    locationName = "Ebeltoft",
                    locationLat = 56.19,
                    locationLot = 10.67,
                    Temperature = 5.6,
                    Humidity = 85,
                    AirPressure = 1012.5
                },
                new Observation
                {
                    DateObserved = DateTime.Now.AddDays(2),
                    locationName = "Ebeltoft",
                    locationLat = 56.19,
                    locationLot = 10.67,
                    Temperature = 20.6,
                    Humidity = 43,
                    AirPressure = 1012.8
                },
                new Observation
                {
                    DateObserved = DateTime.Now,
                    locationName = "Skagen",
                    locationLat = 57.72,
                    locationLot = 10.58,
                    Temperature = 14.6,
                    Humidity = 65,
                    AirPressure = 1012.8
                },
                new Observation
                {
                    DateObserved = DateTime.Now.AddHours(1),
                    locationName = "Skagen",
                    locationLat = 57.72,
                    locationLot = 10.58,
                    Temperature = 17.6,
                    Humidity = 55,
                    AirPressure = 1013.8
                },
                new Observation
                {
                    DateObserved = DateTime.Now.AddHours(2),
                    locationName = "Skagen",
                    locationLat = 57.72,
                    locationLot = 10.58,
                    Temperature = 18.1,
                    Humidity = 50,
                    AirPressure = 1013.5
                },
                new Observation
                {
                    DateObserved = DateTime.Now.AddHours(3),
                    locationName = "Skagen",
                    locationLat = 57.72,
                    locationLot = 10.58,
                    Temperature = 17.8,
                    Humidity = 53,
                    AirPressure = 1013.1
                },
                new Observation
                {
                    DateObserved = DateTime.Now.AddDays(1),
                    locationName = "Skagen",
                    locationLat = 57.72,
                    locationLot = 10.58,
                    Temperature = 5.6,
                    Humidity = 85,
                    AirPressure = 1012.5
                },
                new Observation
                {
                    DateObserved = DateTime.Now.AddDays(2),
                    locationName = "Skagen",
                    locationLat = 57.72,
                    locationLot = 10.58,
                    Temperature = 20.6,
                    Humidity = 43,
                    AirPressure = 1012.8
                }
                );
            context.SaveChanges();
        }
    }
}
