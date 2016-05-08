
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Models;
<<<<<<< HEAD
using AutoMapper;
=======
using TheWorld.Services;
>>>>>>> 036c636763d4c51ac646a581213af65261fc34b3

namespace TheWorld.Controllers.Api
{
    [Authorize]
    [Route("api/trips/{TripName}/stops")]
<<<<<<< HEAD
    public class StopController : Controller
=======
    
    public class StopController:Controller
>>>>>>> 036c636763d4c51ac646a581213af65261fc34b3
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
<<<<<<< HEAD
                var results = _repository.GetTripByName(tripName);
=======
                var results = _repository.GetTripByName(tripName, User.Identity.Name);
>>>>>>> 036c636763d4c51ac646a581213af65261fc34b3

                if (results == null)
                {
                    return Json(null);
                }


<<<<<<< HEAD

=======
>>>>>>> 036c636763d4c51ac646a581213af65261fc34b3
                return Json(AutoMapper.Mapper.Map<IEnumerable<StopViewModel>>(results.Stops.OrderBy(s => s.Order)));
            }
            catch (Exception ex)
            {

                _logger.LogError($"Can not get stops for this trip {tripName} ", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occurred");
            }
        }

<<<<<<< HEAD

        public JsonResult Post(string tripName, [FromBody] StopViewModel vm)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    // map to the enitty
                    var newStop = Mapper.Map <Stop>(vm);


                    // looking up geocoordinates
                    

                    //save to the database

                    _repository.AddStop(tripName,newStop);
=======
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
>>>>>>> 036c636763d4c51ac646a581213af65261fc34b3

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
<<<<<<< HEAD
                        return Json(Mapper.Map<StopViewModel>(newStop));
                    }
=======
                        return Json(AutoMapper.Mapper.Map<StopViewModel>(newStop));
                    }

>>>>>>> 036c636763d4c51ac646a581213af65261fc34b3
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Failed to save new stop", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
<<<<<<< HEAD
                return Json("Failed to save new stop");
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new stop");

=======
                return null;
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new stop");
>>>>>>> 036c636763d4c51ac646a581213af65261fc34b3
        }
    }
}
