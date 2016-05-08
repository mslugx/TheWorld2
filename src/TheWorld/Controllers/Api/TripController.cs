using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Models;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    public class TripController :Controller
    {
        private ILogger<TripController> _logger;
        private IWorldRepository _repository;

        public TripController (IWorldRepository repository, ILogger<TripController> Logger)
        {
            _repository = repository;
            _logger = Logger;
        }


        [HttpGet("")]
        public JsonResult Get()
        {
            return Json(AutoMapper.Mapper.Map<IEnumerable<StopViewModel>>(_repository.GetAllTripsWithStops()));
        }

        [HttpPost("")]
        public JsonResult Post([FromBody] TripViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTrip = AutoMapper.Mapper.Map<Trip>(vm);

                    _logger.LogInformation("Attemping to save a new trip");
                    _repository.AddTrip(newTrip);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(AutoMapper.Mapper.Map<TripViewModel>(newTrip));
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new trip", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });

            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "failed", ModelState = ModelState });
        }
    }
}
