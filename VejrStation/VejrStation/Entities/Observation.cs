using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace VejrStation.Entities
{
    public class Observation
    {
        public int ObservationId { get; set; }
        public DateTime DateObserved { get; set; }   //The date and time the observation were made
        public string locationName { get; set; }     //The location name for the observation
        public double locationLat { get; set; }      //gps coordinates Latitude
        public double locationLot { get; set; }      //gps coordinates Longitude
        public double Temperature { get; set; }      //should have 1 decimals accuracy
        public int Humidity { get; set; }            //Humidity in integers
        public double AirPressure { get; set; }      //Should have 1 decimals accuracy
    }
}
