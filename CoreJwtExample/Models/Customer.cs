using System.ComponentModel.DataAnnotations;

namespace CoreJwtExample.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        //[Required(ErrorMessage = "Customer name is empty!")]
        public string CustomerName { get; set; }
        //[Required(ErrorMessage = "Role is empty!")]
        public string Role { get; set; }
        public IFormFile Files{ get; set; }
        public byte[] ImgByte{ get; set; }
        public string Message { get; set; }

    }
}
