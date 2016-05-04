
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;

namespace TheWorld.Controllers.Api
{
    [Authorize]
    [Route("api/trips/{TripName}/stops")]
    
    public class StopController:Controller
    {
        private CoordService _coordService;
        private ILogger<StopController> _logger;
        private IWorldRepository _repository;

        public StopController(IWorldRepository repository, ILogger<StopController> logger, CoordService coordService)
        {
            _repository = repository;
            _logger = logger;
            _coordService = coordService;
        }

        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var results = _repository.GetTripByName(tripName, User.Identity.Name);

                if (results == null)
                {
                    return Json(null);
                }


                return Json(AutoMapper.Mapper.Map<IEnumerable<StopViewModel>>(results.Stops.OrderBy(s => s.Order)));
            }
            catch (Exception ex)
            {

                _logger.LogError($"Can not get stops for this trip {tripName} ", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occurred");
            }
        }

        public async Task<JsonResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    //map to entity
                    var newStop = AutoMapper.Mapper.Map<Stop>(vm);

                    //looking up geocoordinates
                    var coordresult = await _coordService.Lookup(newStop.Name);

                    if (!coordresult.Success)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        Json(coordresult.Message);
                    }

                    newStop.Longitude = coordresult.Longitude;
                    newStop.Latitude = coordresult.Latitude;
                    //newStop.Longitude = coordresult.Longitude;
                    //save to the database
                    _repository.AddStop(tripName, User.Identity.Name,newStop);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(AutoMapper.Mapper.Map<StopViewModel>(newStop));
                    }

                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Failed to save new stop", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new stop");
        }
    }
}
