
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
    [Route("api/trips/{TripName}/stops")]
    public class StopController:Controller
    {
        private ILogger<StopController> _logger;
        private IWorldRepository _repository;

        public StopController(IWorldRepository repository, ILogger<StopController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var results = _repository.GetTripByName(tripName);
                return Json(true);
            }
            catch (Exception ex)
            {

                _logger.LogError("Can not get stops for this trip", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occurred");
            }            
        }
    }
}
