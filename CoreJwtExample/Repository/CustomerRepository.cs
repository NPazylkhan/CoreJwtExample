using CoreJwtExample.Common;
using CoreJwtExample.IRepository;
using CoreJwtExample.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace CoreJwtExample.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        string _connectionString = "";
        Customer _customer = new Customer();
        List<Customer> _customersList = new List<Customer>();

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("StoreDb");
        }

        public async Task<string> Delete(Customer obj)
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
                    var customer = await connection.QueryAsync<User>("SP_Customer",
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

        public async Task<Customer> Get(int objId)
        {
            _customer = new Customer();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var customers = await connection.QueryAsync<Customer>(string.Format(@"select * from Customer where CustomerId={0}", objId));
                if (customers != null && customers.Count() > 0)
                {
                    _customer = customers.SingleOrDefault();
                }
                return _customer;
            }
        }

        public async Task<List<Customer>> Gets1()
        {
            _customersList = new List<Customer>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var customersList = await connection.QueryAsync<Customer>("select * from Customer");
                if (customersList != null && customersList.Count() > 0)
                {
                    _customersList = customersList.ToList();
                }
                return _customersList;
            }
        }

        public async Task<List<Customer>> Gets2()
        {
            _customersList = new List<Customer>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var customersList = await connection.QueryAsync<Customer>("select * from Customer");
                if (customersList != null && customersList.Count() > 0)
                {
                    _customersList = customersList.ToList();
                }
                return _customersList;
            }
        }

        public async Task<Customer> Save(Customer obj)
        {
            _customer = new Customer();
            try
            {
                int operationType = Convert.ToInt32(obj.CustomerId == 0 ? OperationType.Insert : OperationType.Update);

                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    var customers = await connection.QueryAsync<Customer>("SP_Customer",
                         this.SetParameters(obj, operationType),
                         commandType: CommandType.StoredProcedure);
                    if (customers != null && customers.Count() > 0)
                    {
                        _customer = customers.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                _customer = new Customer();
                _customer.Message = ex.Message;
            }
            return _customer;
        }

        private DynamicParameters SetParameters(Customer _customer, int _operationType)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CustomerId", _customer.CustomerId);
            parameters.Add("@CustomerName", _customer.CustomerName);
            parameters.Add("@Role", _customer.Role);
            parameters.Add("@OperationType", _operationType);
            return parameters;

        }
    }
}
