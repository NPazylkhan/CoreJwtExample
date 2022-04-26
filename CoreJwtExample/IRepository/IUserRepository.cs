using CoreJwtExample.Models;

namespace CoreJwtExample.IRepository
{
    public interface IUserRepository
    {
        Task<User> Save(User obj);
        Task<User> Get(int objId);
        Task<List<User>> GetAll();
        Task<User> GetByUserNamePassword(User user);
        Task<string> Delete(User obj);

    }
}
