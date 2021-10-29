using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectArchitecture.Domain.Service;

namespace ProjectArchitecture.UI.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase {
        private readonly IService _service;

        public ServiceController(IService service) {
            _service = service;
        }

    }
}
