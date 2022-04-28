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
        public static IWebHostEnvironment _webHostEnvironment;
        ICustomerRepository _customerRepository = null;

        public CustomerController(IConfiguration configuration, ICustomerRepository customerRepository, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _customerRepository = customerRepository;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<ActionResult> Save([FromForm] Customer customer)
        {
            try
            {
                /*if (ModelState.IsValid)
                {*/
                    string message = "";
                    var files = customer.Files;
                    customer.Files = null;

                    customer = await _customerRepository.Save(customer);
                    if (customer.CustomerId > 0 && files != null && files.Length > 0)
                    {
                        string path = _webHostEnvironment.WebRootPath + "\\Photos\\";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        string fileName = "CustomerPic" + customer.CustomerId + ".png";
                        if (System.IO.File.Exists(path + fileName))
                        {
                            System.IO.File.Delete(path + fileName);
                        }

                        using (FileStream fileStream = System.IO.File.Create(path + fileName))
                        {
                            files.CopyTo(fileStream);
                            fileStream.Flush();
                            message = "Success";
                        }
                    }
                    else if (customer.CustomerId == 0)
                    {
                        message = "Failed";
                    }
                    else
                    {
                        message = "Success";
                    }

                    if (message == "Success")
                    {
                        return Ok(customer);
                    }
                    else
                    {
                        return StatusCode((int)HttpStatusCode.InternalServerError, message);
                    }
                /*}
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Form is not valid"); ;
                }*/
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
            if (customerId == 0)
            {
                return Ok(new Customer());
            }
            var customer = await _customerRepository.Get(customerId);

            string fileName = "CustomerPic_" + customer.CustomerId + ".png";
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Photos", fileName);
            customer.ImgByte = System.IO.File.ReadAllBytes(path);

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
                if (message == "Deleted")
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
