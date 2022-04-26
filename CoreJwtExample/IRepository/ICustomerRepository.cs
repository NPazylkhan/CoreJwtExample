using CoreJwtExample.Models;

namespace CoreJwtExample.IRepository
{
    public interface ICustomerRepository
    {
        Task<Customer> Save(Customer obj);
        Task<Customer> Get(int objId);
        Task<string> Delete(Customer obj);
        Task<List<Customer>> Gets1();
        Task<List<Customer>> Gets2();
    }
}
