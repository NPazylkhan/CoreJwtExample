using CoreJwtExample.Common;
using CoreJwtExample.IRepository;
using CoreJwtExample.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace CoreJwtExample.Repository
{
    public class UserRepository : IUserRepository
    {
        string _connectionString = "";
        User _user = new User();
        List<User> _usersList = new List<User>();

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("StoreDb");
        }

        public async Task<string> Delete(User obj)
        {
            string message = "";
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    var user = await connection.QueryAsync<User>("SP_User",
                        this.SetParameters(obj, (int)OperationType.Delete),
                        commandType: CommandType.StoredProcedure);
                    message = "Deleted";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        public async Task<User> Get(int objId)
        {
            _user = new User();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var users = await connection.QueryAsync<User>(string.Format(@"select * from User where UserId={0}", objId));
                if (users != null && users.Count() > 0)
                {
                    _user = users.SingleOrDefault();
                }
                return _user;
            }
        }

        public async Task<List<User>> GetAll()
        {
            _usersList = new List<User>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var users = await connection.QueryAsync<User>("select * from User");
                if (users != null && users.Count() > 0)
                {
                    _usersList = users.ToList();
                }
                return _usersList;
            }
        }

        public async Task<User> GetByUserNamePassword(User user)
        {
            _user = new User();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var users = await connection.QueryAsync<User>(string.Format(@"select * from [User] where UserName='{0}' and Password = '{1}'", user.UserName, user.Password));
                if (users != null && users.Count() > 0)
                {
                    _user = users.SingleOrDefault();
                }
                return _user;
            }
        }

        public async Task<User> Save(User obj)
        {
            _user = new User();
            try
            {
                int operationType = Convert.ToInt32(obj.UserId == 0 ? OperationType.Insert : OperationType.Update);

                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    var users = await connection.QueryAsync<User>("SP_User",
                         this.SetParameters(obj, operationType),
                         commandType: CommandType.StoredProcedure);
                    if (users != null && users.Count() > 0)
                    {
                        _user = users.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                _user = new User();
                _user.Message = ex.Message;
            }
            return _user;
        }

        private DynamicParameters SetParameters(User _user, int _operationType)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", _user.UserId);
            parameters.Add("@UserName", _user.UserName);
            parameters.Add("@Email", _user.Email);
            parameters.Add("@Password", _user.Password);
            parameters.Add("@OperationType", _operationType);
            return parameters;

        }
    }
}
