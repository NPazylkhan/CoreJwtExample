using CoreJwtExample.IRepository;
using CoreJwtExample.Models;
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

        [HttpPost]
        [Route("Save")]
        public async Task<ActionResult> Save(Customer customer)
        {
            try
            {
                customer = await _customerRepository.Save(customer);
                if(customer.Message == null)
                {
                    return Ok(customer);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetByCustomerId/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            if(customerId == 0)
            {
                return Ok(new Customer());
            }
            var customer = await _customerRepository.Get(customerId);
            return Ok(customer);
        }

        [HttpDelete]
        [Route("Delete/{customerId}")]
        public async Task<IActionResult> Delete(int customerId)
        {
            try
            {
                Customer customer = new Customer() { CustomerId = customerId };
                string message = await _customerRepository.Delete(customer);
                if(message == "Deleted")
                {
                    return Ok(message);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
