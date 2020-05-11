using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VejrStation.Database;
using VejrStation.Entities;

namespace VejrStation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationsController : ControllerBase
    {
        private readonly MyDBContext _context;

        public ObservationsController(MyDBContext context)
        {
            _context = context;
        }

        //get to see all the observations
        [HttpGet]
        [ActionName("AllObservations")]
        public async Task<ActionResult<IEnumerable<Observation>>> GetObservations()
        {
            return await _context.Observations.ToListAsync();
        }

        //Get to see the observation specified by a specific id
        [HttpGet("ObservedById/{id}")]
        [ActionName("ObservedById")]
        public async Task<ActionResult<Observation>> GetObservation(int id)
        {
            var observation = await _context.Observations.FindAsync(id);

            if (observation == null)
            {
                return NotFound();
            }

            return observation;
        }

        //get to see all the observations for a specified date
        [HttpGet("{DateObserved}")]
        [ActionName("DateObserved")]
        public async Task<ActionResult<object>> GetObservationsDate(DateTime date)
        {
            //DateTime tempDate = DateTime.Parse(date.ToString());

            List<Observation> observations = _context.Observations
                .Where(a => a.DateObserved.Date == date.Date)
                .ToList();

            if (observations[0] == null) //If there is no observations on the first element in the array return notfound
            {
                return NotFound();
            }

            return observations;
        }

        //Get to see all observations between a starting point and finish
        [HttpGet("{DateObserved1}/{DateObserved2}")]
        [ActionName("StartStop")]
        public async Task<ActionResult<object>> GetObservationsStartStop(DateTime date1, DateTime date2)
        {
            List<Observation> observations = _context.Observations
                .Where(a => a.DateObserved.Date >= date1.Date && a.DateObserved.Date <= date2.Date)
                .ToList();

            if (observations[0] == null) //If there is no observations on the first element in the array return notfound
            {
                return NotFound();
            }

            return observations;
        }

        //get to see the last 3 observations
        [HttpGet("latest")]
        [ActionName("LatestThree")]
        public async Task<ActionResult<object>> GetObservationsLatest()
        {
            //string temp = latest;

            int maxID = _context.Observations.Max(a => a.ObservationId);

            List<Observation> observations = _context.Observations
                .Where(a => a.ObservationId >= maxID - 2)
                .ToList();

            if (observations[0] == null
            ) //If there is no observations on the first element in the array return notfound
            {
                return NotFound();
            }

            return observations;
        }



        [HttpPost]
        public async Task<ActionResult<Observation>> CreateObservation(Observation observation)
        {
            _context.Observations.Add(observation);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetObservation", new { id = observation.ObservationId }, observation);
        }

    }
}