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
    }
}
