using CoreJwtExample.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoreJwtExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerController : ControllerBase
    {
        private IConfiguration _configuration;
        ICustomerRepository _customerRepository = null;

        public CustomerController(IConfiguration configuration, ICustomerRepository customerRepository)
        {
            _configuration = configuration;
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [Route("Gets1")]
        public async Task<IActionResult> Gets1()
        {
            var list = await _customerRepository.Gets1();
            return Ok(list);
        }

        [HttpGet]
        [Route("Gets2")]
        public async Task<IActionResult> Gets2()
        {
            var list = await _customerRepository.Gets2();
            return Ok(list);
        }
    }
}
