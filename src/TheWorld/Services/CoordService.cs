using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace TheWorld.Services
{
    public class CoordService
    {
        private ILogger<CoordService> _logger;
        public CoordService(ILogger<CoordService> logger)
        {
            _logger = logger;
        }

        public CoordServiceResult Lookup(string location)
        {

        }
    }
}
