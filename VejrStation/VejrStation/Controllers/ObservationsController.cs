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
        public async Task<ActionResult<IEnumerable<Observation>>> GetObservations()
        {
            return await _context.Observations.ToListAsync();
        }

        //Get to see the oberservation specified by a specific id
        [HttpGet("{id}")]
        public async Task<ActionResult<Observation>> GetObservation(long id)
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
        public async Task<ActionResult<object>> GetObservationsDate(DateTime date)
        {
            List<Observation> observations = _context.Observations
                .Where(a => a.DateObserved.Date == date.Date)
                .ToList();

            if (observations[0] == null) //If there is no observations on the first element in the array return notfound
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

            return CreatedAtRoute("GetObservation", new { id = observation.Id }, observation);
        }

    }
}