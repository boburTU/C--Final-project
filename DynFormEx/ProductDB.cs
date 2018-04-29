using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace DynFormEx
{
    class ProductDB
    {

        public static OleDbConnection GetConnection()
        {
            OleDbConnection conn = null;
            string connStr = File.ReadAllText(@"..\..\DBConnStr.txt", Encoding.UTF8);
            conn = new OleDbConnection(connStr);
            return conn;
        }


        // Execute a non-query on DB
        public static int ExeNonQuery(string sqlString)
        {
            int records = 0;
            OleDbConnection connection = GetConnection();
            OleDbCommand command = new OleDbCommand(sqlString, connection);
            try
            {
                connection.Open();
                records = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                records = -1;
            }
            finally
            {
                connection.Close();
            }
            return records;
        }

        // Add a transaction to DB
        public static int InsertOrder(int productID, double productPrice)
        {
            string sqlString = "INSERT INTO tblOrders (fldProductID, fldPrice) " +
                "VALUES  (" + productID + ", " + productPrice + ");";
            //MessageBox.Show(sqlString);
            int records = ExeNonQuery(sqlString);
            return records;
        }


        // Get max transaction id
        // Query a Product from DB
        public static int SelectMaxOrder()
        {
            int max = 0;
            OleDbConnection connection = GetConnection();
            string sqlString = "SELECT MAX(fldID) FROM tblOrders;";
            //MessageBox.Show(sqlString);
            OleDbCommand command = new OleDbCommand(sqlString, connection);
            try
            {
                connection.Open();
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    max = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                max = -1;
            }
            finally
            {
                connection.Close();
            }
            return max;
        }


        // Add a line item to DB
        // Insert a Product record
        public static int InsertToppings(int orderID, int ingredientID, double ingredientPrice)
        {
            string sqlString = "INSERT INTO tblOrderToppings (fldOrdersID, fldToppingsID, fldToppingPrice) " +
                "VALUES  (" + orderID + ", " + ingredientID + ", " + ingredientPrice + ");";
            //MessageBox.Show(sqlString);
            int records = ExeNonQuery(sqlString);
            return records;
        }

        // Clear tblOrder and tblOrderToppings in DB
        public static void clearOrders()
        {
            string sqlString = "DELETE * FROM  tblOrders";
            //MessageBox.Show(sqlString);
            ExeNonQuery(sqlString);
            
        }


        public static string[] getFormData()
        {
            string[] sArr = null;
            return sArr;
        }


        // Get the current product from DB
        public static Product GetProducts()
        {
            // Declare a Product and initialize to null pointer
            Product p = null;

            // Declare and get connection
            OleDbConnection conn = GetConnection();

            // Declare a string to contain the sql statement
            string sqlStr = "SELECT tblOrders.fldID, tblButtons.fldName, tblOrders.fldPrice  " +
                "FROM tblButtons INNER JOIN tblOrders ON tblButtons.fldID = tblOrders.fldProductID " +
                "WHERE tblOrders.fldID = " + SelectMaxOrder();

            // Show sql string in message box
            //MessageBox.Show(sqlStr); // For debugging

            // Declare and instantiate a command
            OleDbCommand command = new OleDbCommand(sqlStr, conn);

            // Read all Products from DB in try-catch block
            // Try
            try
            {
                // Open connection
                conn.Open();
                // Execute the command reader
                OleDbDataReader reader = command.ExecuteReader();
                // Loop to get all records and copy to List of Product
                while (reader.Read())
                {
                    // Instantiate Product with constructor, passing data from reader
                     p = new Product(Convert.ToInt32(reader[0]), reader[1].ToString(), Convert.ToDouble(reader[2]));
                }
            }
            // Catch
            catch (Exception ex)
            {
                // Catch exception
                MessageBox.Show("Exception: " + ex.ToString());
                // Set List of Product to null pointer
                p = null;
            }
            // Finally
            finally
            {
                // If connection not null close it
                if (conn != null)
                {
                    conn.Close();
                }
            }
            // Return Product
            return p;
        }



        // Get a list of all Toppings from DB
        public static List<Topping> GetToppings()
        {
            // Declare a List of Toppings and initialize to null pointer
            List < Topping > tl= null;

            // Declare and get connection
            OleDbConnection conn = GetConnection();

            // Declare a string to contain the sql statement
            string sqlStr = "SELECT tblOrderToppings.fldOrdersID, tblToppings.fldName, tblToppings.fldPrice " +
            "FROM tblToppings INNER JOIN tblOrderToppings ON tblToppings.fldID = tblOrderToppings.fldToppingsID " +
            "WHERE tblOrderToppings.fldOrdersID = " + SelectMaxOrder() + ";";


            // Show sql string in message box
            //MessageBox.Show(sqlStr); // For debugging

            // Declare and instantiate a command
            OleDbCommand command = new OleDbCommand(sqlStr, conn);

            // Read all Products from DB in try-catch block
            // Try
            try
            {
                // Instantiate List of Toppings
                tl = new List<Topping>();
                // Open connection
                conn.Open();
                // Execute the command reader
                OleDbDataReader reader = command.ExecuteReader();
                // Loop to get all records and copy to List of Product
                while (reader.Read())
                {
                    // Instantiate Topping with constructor, passing data from reader
                    Topping t = new Topping(Convert.ToInt32(reader[0]), reader[1].ToString(), Convert.ToDouble(reader[2]));
                    
                    // Add Topping to List of Topping
                    tl.Add(t);

                }
            }
            // Catch
            catch (Exception ex)
            {
                // Catch exception
                MessageBox.Show("Exception: " + ex.ToString());
                // Set List of Product to null pointer
                tl = null;
            }
            // Finally
            finally
            {
                // If connection not null close it
                if (conn != null)
                {
                    conn.Close();
                }
            }
            // Return Topping list
            return tl;
        }

    }
}
