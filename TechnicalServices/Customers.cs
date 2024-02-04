using ABCHardwareWebApplication.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;

namespace ABCHardwareWebApplication.TechnicalServices
{
    public class Customers
    {
        private string? _connectionString;   // the ? helps to remove the nullable reference on the _connectionString
        // private string _connectionString;  

        public Customers()
        {
            //constructor logic
            ConfigurationBuilder DatabaseBuilder = new();
            DatabaseBuilder.SetBasePath(Directory.GetCurrentDirectory());
            DatabaseBuilder.AddJsonFile("appsettings.json");
            IConfiguration DatabaseConfiguration = DatabaseBuilder.Build();
            _connectionString = DatabaseConfiguration.GetConnectionString("myConnectionString");  //to remove the error'
            //_connectionString = DatabaseUserConfiguration.GetConnectionString("VarBAIS3150")!;  //null forgiving operator  - !
        }

        public bool AddCustomer(Customer aCustomer)
        {
            bool Success = true;

            //conect
            SqlConnection MyDataSource = new();
            MyDataSource.ConnectionString = _connectionString;
            MyDataSource.Open();

            //SqlCpmmand
            SqlCommand MyCommand = new()
            {
                Connection = MyDataSource,
                CommandType = CommandType.StoredProcedure,
                CommandText = "ABCHardware.AddCustomer"
            };
            
            //SqlParameter
            SqlParameter MyInputParameter ;
            MyInputParameter  = new()
            {
                ParameterName = "@FirstName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.FirstName
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@LastName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.LastName
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@Address",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.Address
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.City
            };
            MyCommand.Parameters.Add(MyInputParameter );


            MyInputParameter  = new()
            {
                ParameterName = "@Province",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.Province
            };
            MyCommand.Parameters.Add(MyInputParameter );



            MyInputParameter  = new()
            {
                ParameterName = "@PostalCode",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.PostalCode
            };
            MyCommand.Parameters.Add(MyInputParameter );

            

            MyCommand.ExecuteNonQuery();
            MyDataSource.Close();
            Success = true;


            return Success;

        }


        public Customer GetCustomer(string customerID)
        {
            Customer existingCustomer= new();

            existingCustomer = null;
            //conect
            SqlConnection MyDataSource = new();
            MyDataSource.ConnectionString = _connectionString;
            MyDataSource.Open();

            //SqlCpmmand
            SqlCommand MyCommand = new()
            {
                Connection = MyDataSource,
                CommandType = CommandType.StoredProcedure,
                CommandText = "ABCHardware.FindCustomer"
            };

            //SqlParameter
            SqlParameter MyInputParameter ;
            MyInputParameter  = new()
            {
                ParameterName = "@CustomerID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = customerID
            };
            MyCommand.Parameters.Add(MyInputParameter );

            //DataReader
            SqlDataReader MyDataReader;
            MyDataReader = MyCommand.ExecuteReader();


            if (MyDataReader.HasRows)
            {
                MyDataReader.Read(); // Move to the first row

                existingCustomer = new()
                {

                    CustomerID= (int)MyDataReader["CustomerID"],
                    FirstName = (string)MyDataReader["FirstName"],
                    LastName = (string)MyDataReader["LastName"],
                    Address = (string)MyDataReader["Address"],
                    City = (string)MyDataReader["City"],
                    Province = (string)MyDataReader["Province"],
                    PostalCode = (string)MyDataReader["PostalCode"]
                };
            };
            MyDataReader.Close();
            MyDataSource.Close();
            return existingCustomer;

        }

        public bool UpdateCustomer(Customer aCustomer)
        {
            bool Success = true;

            //conect
            SqlConnection MyDataSource = new();
            MyDataSource.ConnectionString = _connectionString;
            MyDataSource.Open();

            //SqlCpmmand
            SqlCommand MyCommand = new()
            {
                Connection = MyDataSource,
                CommandType = CommandType.StoredProcedure,
                CommandText = "ABCHardware.UpdateCustomer"
            };

            //SqlParameter
            SqlParameter MyInputParameter ;
            MyInputParameter  = new()
            {
                ParameterName = "@CustomerID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = aCustomer.CustomerID
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@FirstName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.FirstName
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@LastName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.LastName
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@Address",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.Address
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.City
            };
            MyCommand.Parameters.Add(MyInputParameter );


            MyInputParameter  = new()
            {
                ParameterName = "@Province",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.Province
            };
            MyCommand.Parameters.Add(MyInputParameter );



            MyInputParameter  = new()
            {
                ParameterName = "@PostalCode",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = aCustomer.PostalCode
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyCommand.ExecuteNonQuery();
            MyDataSource.Close();
            Success = true;

            return Success;
        }
        public bool DeleteCustomer(string customerID)
        {
            bool Success = true;

            //conect
            SqlConnection MyDataSource = new();
            MyDataSource.ConnectionString = _connectionString;
            MyDataSource.Open();

            //SqlCpmmand
            SqlCommand MyCommand = new()
            {
                Connection = MyDataSource,
                CommandType = CommandType.StoredProcedure,
                CommandText = "ABCHardware.DeleteCustomer"
            };

            //SqlParameter
            SqlParameter MyInputParameter ;
            MyInputParameter  = new()
            {
                ParameterName = "@CustomerID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = customerID
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyCommand.ExecuteNonQuery();
            MyDataSource.Close();
            Success = true;


            return Success;

        }


    }
}
//AddCustomer(aCustomer);