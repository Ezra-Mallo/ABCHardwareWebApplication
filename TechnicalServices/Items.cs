using ABCHardwareWebApplication.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ABCHardwareWebApplication.TechnicalServices
{
    public class Items
    {
        private string? _connectionString;   // the ? helps to remove the nullable reference on the _connectionString
        // private string _connectionString;  

        public Items()
        {
            //constructor logic
            ConfigurationBuilder DatabaseBuilder = new();
            DatabaseBuilder.SetBasePath(Directory.GetCurrentDirectory());
            DatabaseBuilder.AddJsonFile("appsettings.json");
            IConfiguration DatabaseConfiguration = DatabaseBuilder.Build();
            _connectionString = DatabaseConfiguration.GetConnectionString("myConnectionString");  //to remove the error'
            //_connectionString = DatabaseUserConfiguration.GetConnectionString("VarBAIS3150")!;  //null forgiving operator  - !
        }

        public bool AddItem(Item anItem)
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
                CommandText = "ABCHardware.AddItem"
            };

            //SqlParameter
            SqlParameter MyInputParameter ;
            MyInputParameter = new()
            {
                ParameterName = "@ItemCode",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = anItem.ItemCode
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@Description",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = anItem.Description
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@UnitPrice",
                SqlDbType = SqlDbType.Decimal,
                Direction = ParameterDirection.Input,
                Value = anItem.UnitPrice
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@StockBal",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = anItem.StockBal
            };
            MyCommand.Parameters.Add(MyInputParameter );


            MyInputParameter  = new()
            {
                ParameterName = "@StockFlag",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = anItem.StockFlag
            };
            MyCommand.Parameters.Add(MyInputParameter );



            MyCommand.ExecuteNonQuery();
            MyDataSource.Close();
            Success = true;

            
            return Success;

        }

        public bool UpdateItem(Item anItem)
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
                CommandText = "ABCHardware.UpdateItem"
            };

            //SqlParameter
            SqlParameter MyInputParameter ;
            MyInputParameter  = new()
            {
                ParameterName = "@ItemCode",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = anItem.ItemCode
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@Description",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = anItem.Description
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@UnitPrice",
                SqlDbType = SqlDbType.Decimal,
                Direction = ParameterDirection.Input,
                Value = anItem.UnitPrice
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyInputParameter  = new()
            {
                ParameterName = "@StockBal",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = anItem.StockBal
            };
            MyCommand.Parameters.Add(MyInputParameter );


            MyInputParameter  = new()
            {
                ParameterName = "@StockFlag",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = anItem.StockFlag
            };
            MyCommand.Parameters.Add(MyInputParameter );



            MyCommand.ExecuteNonQuery();
            MyDataSource.Close();
            Success = true;


            return Success;

        }

        public bool UpdateItemQty(string itemCode, int stockBal)
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
                CommandText = "ABCHardware.UpdateItemQty"
            };

            //SqlParameter
            SqlParameter MyInputParameter ;
            MyInputParameter  = new()
            {
                ParameterName = "@ItemCode",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = itemCode
            };
            MyCommand.Parameters.Add(MyInputParameter );
           

            MyInputParameter  = new()
            {
                ParameterName = "@StockBal",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = stockBal
            };
            MyCommand.Parameters.Add(MyInputParameter );



            MyCommand.ExecuteNonQuery();
            MyDataSource.Close();
            Success = true;


            return Success;

        }
        public Item GetItem(string itemCode)
        {
            Item existingItem=new();

            existingItem = null;
            //conect
            SqlConnection MyDataSource = new();
            MyDataSource.ConnectionString = _connectionString;
            MyDataSource.Open();

            //SqlCpmmand
            SqlCommand MyCommand = new()
            {
                Connection = MyDataSource,
                CommandType = CommandType.StoredProcedure,
                CommandText = "ABCHardware.FindItem"
            };

            //SqlParameter
            SqlParameter MyInputParameter ;
            MyInputParameter  = new()
            {
                ParameterName = "@ItemCode",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = itemCode
            };
            MyCommand.Parameters.Add(MyInputParameter );

                //DataReader
            SqlDataReader MyDataReader;
            MyDataReader = MyCommand.ExecuteReader();
            

            if (MyDataReader.HasRows)
            {
                MyDataReader.Read(); // Move to the first row

                existingItem = new()
                {
                    ItemCode= (string)MyDataReader["ItemCode"],
                    Description = (string)MyDataReader["Description"],
                    UnitPrice = (decimal)MyDataReader["UnitPrice"],
                    StockBal = (int)MyDataReader["StockBal"],
                    StockFlag = (bool)MyDataReader["StockFlag"]
                };
            };
            MyDataReader.Close();
            MyDataSource.Close();
            return existingItem;

        }

        public bool RemoveItem(string itemCode)
        {
            bool Success=false;

            //conect
            SqlConnection MyDataSource = new();
            MyDataSource.ConnectionString = _connectionString;
            MyDataSource.Open();

            //SqlCpmmand
            SqlCommand MyCommand = new()
            {
                Connection = MyDataSource,
                CommandType = CommandType.StoredProcedure,
                CommandText = "ABCHardware.DeleteItem"
            };

            //SqlParameter
            SqlParameter MyInputParameter ;
            MyInputParameter  = new()
            {
                ParameterName = "@ItemCode",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = itemCode
            };
            MyCommand.Parameters.Add(MyInputParameter );

            MyCommand.ExecuteNonQuery();
            
            MyDataSource.Close();
            Success = true;
            return Success;
        }
    }
}
