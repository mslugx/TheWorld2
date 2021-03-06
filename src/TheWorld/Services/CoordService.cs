<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
=======
﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
>>>>>>> 036c636763d4c51ac646a581213af65261fc34b3

namespace TheWorld.Services
{
    public class CoordService
    {
        private ILogger<CoordService> _logger;
<<<<<<< HEAD
=======

>>>>>>> 036c636763d4c51ac646a581213af65261fc34b3
        public CoordService(ILogger<CoordService> logger)
        {
            _logger = logger;
        }

<<<<<<< HEAD
        public CoordServiceResult Lookup(string location)
        {

        }
=======
        public async Task<CoordServiceResult> Lookup(string location)
        {
            var result = new CoordServiceResult()
            {
                Success = false,
                Message = "Undetermined failure while looking up coordinates"

            };

            //lookup coordinates
            var encodeName = WebUtility.UrlEncode(location);
            var bingKey = Startup.configuration["AppSettings:bingKey"];
            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodeName}&key={bingKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            // Read out the results
            // Fragile, might need to change if the Bing API changes
            var results = Newtonsoft.Json.Linq.JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];
            if (!resources.HasValues)
            {
                result.Message = $"Could not find '{location}' as a location";
            }
            else
            {
                var confidence = (string)resources[0]["confidence"];
                if (confidence != "High")
                {
                    result.Message = $"Could not find a confident match for '{location}' as a location";
                }
                else
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double)coords[0];
                    result.Longitude = (double)coords[1];
                    result.Success = true;
                    result.Message = "Success";
                }
            }

            return result;
        }

>>>>>>> 036c636763d4c51ac646a581213af65261fc34b3
    }
}
