using ABCHardwareWebApplication.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Data;

namespace ABCHardwareWebApplication.TechnicalServices
{
    public class Sales
    {
        private string? _connectionString;   // the ? helps to remove the nullable reference on the _connectionString
        // private string _connectionString;  

        public Sales()
        {
            //constructor logic
            ConfigurationBuilder DatabaseBuilder = new();
            DatabaseBuilder.SetBasePath(Directory.GetCurrentDirectory());
            DatabaseBuilder.AddJsonFile("appsettings.json");
            IConfiguration DatabaseConfiguration = DatabaseBuilder.Build();
            _connectionString = DatabaseConfiguration.GetConnectionString("myConnectionString");  //to remove the error'
            //_connectionString = DatabaseUserConfiguration.GetConnectionString("VarBAIS3150")!;  //null forgiving operator  - !
        }

        public int AddSales(Sale mySale)
        {
            int saleNumber = 0;
            //conect
            SqlConnection MyDataSource = new();
            MyDataSource.ConnectionString = _connectionString;
            MyDataSource.Open();

            //SqlCpmmand
            SqlCommand MyCommand = new()
            {
                Connection = MyDataSource,
                CommandType = CommandType.StoredProcedure,
                CommandText = "ABCHardware.AddSale"
            };

            //SqlParameter
            SqlParameter MyInputParameter ;
            MyInputParameter  = new()
            {
                ParameterName = "@Salesperson",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = mySale.Salesperson
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@CustomerID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = mySale.CustomerID
            };
            MyCommand.Parameters.Add(MyInputParameter );


            MyInputParameter  = new()
            {
                ParameterName = "@SubTotal",
                SqlDbType = SqlDbType.Decimal,
                Direction = ParameterDirection.Input,
                Value = mySale.SubTotal
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@GST",
                SqlDbType = SqlDbType.Decimal,
                Direction = ParameterDirection.Input,
                Value = mySale.GST
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@SaleTotal",
                SqlDbType = SqlDbType.Decimal,
                Direction = ParameterDirection.Input,
                Value = mySale.SubTotal
            };
            MyCommand.Parameters.Add(MyInputParameter );



            SqlParameter MyOutputParameter;

            MyOutputParameter = new()
            {
                ParameterName = "@NewSaleNumber",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            MyCommand.Parameters.Add(MyOutputParameter);

            MyCommand.ExecuteNonQuery();
            saleNumber = Convert.ToInt32(MyOutputParameter.Value);
            MyDataSource.Close();
            
            return saleNumber;
        }
    }
}
