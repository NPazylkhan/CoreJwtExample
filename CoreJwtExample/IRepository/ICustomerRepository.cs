using CoreJwtExample.Models;

namespace CoreJwtExample.IRepository
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> Gets1();
        Task<List<Customer>> Gets2();
    }
}
