using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
//using System.Windows.Forms;
using System.IO;
using System.Windows.Forms;

namespace DynFormEx
{

    // Array of FormConfig for project
    class FormConfigArr : List<FormConfig>
    {

        private static string imgDir;

        // Constructor for FormConfigArr
        public FormConfigArr()
        {
            // Get Image dir from DB
            GetImgDir();
        }


        // Get Image dir from DB
        public static void GetImgDir()
        {
            // DB objects
            OleDbConnection conn = null;
            OleDbCommand comm = null;
            string sqlStr;
            // Query image dir from DB
            try
            {
                conn = GetConnection();
                conn.Open();
                sqlStr = "SELECT fldImgDir " +
                    "FROM tblApplication " +
                    "WHERE fldID = 0";
                comm = new OleDbCommand(sqlStr, conn);
                OleDbDataReader reader = comm.ExecuteReader(
                    System.Data.CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    // Note that Access uses index of data given its
                    // location in forst line of sqlStr
                    imgDir = reader.GetString(0);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception: " + ex.ToString());
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }


        // Get Form config data from DB
        // This reads all the Form config data from DB
        public void GetFormConfigs()
        {
            // Number of Buttons on current Form
            int numButtons;
            // DB objects
            OleDbConnection conn = null;
            OleDbCommand comm = null;
            string sqlStr;
            // Read all Forms from DB
            try
            {
                conn = GetConnection();
                conn.Open();
                sqlStr = "SELECT fldID, fldFrmTitle, fldFrmImage, fldFrmType " +
                    "FROM tblForms";
                comm = new OleDbCommand(sqlStr, conn);
                OleDbDataReader reader = comm.ExecuteReader(
                    System.Data.CommandBehavior.SingleResult);
                while (reader.Read())
                {
                    // Use FormConfig constructor to instantiate Form
                    FormConfig cf = new FormConfig(reader.GetInt32(0),
                        reader.GetString(1), reader.GetString(2),
                        reader.GetString(3), imgDir);
                    // Add form to this List
                    this.Add(cf);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception: " + ex.ToString());
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            // Add Bottons to Forms
            for (int i = 0; i < this.Count; i++)
            {
                // Init num Buttons to zero
                numButtons = 0;
                // The first type of Form has Buttons that cause some action when clicked
                if (this[i].frmType == "typFrmButton" )
                {
                    // Instantiate a ButtonConfig array with max rows x max cols elements
                    this[i].btnConfig = new ButtonConfig[this[i].maxBtnRows * this[i].maxBtnCols];
                    // Instantiate a LabelConfig array with max rows x max cols elements
                    this[i].lblConfig = new LabelConfig[this[i].maxBtnRows * this[i].maxBtnCols];

                    // Read Buttons for this Form from DB
                    try
                    {
                        conn = GetConnection();
                        conn.Open();
                        sqlStr = "SELECT fldBtnCfgImage, fldBtnCfgOper, fldBtnCfgTarget, fldFrmOpenIdx, fldID, fldPrice, fldName " +
                            "FROM tblButtons " +
                            "WHERE fldFormID = " + this[i].frmIdx;
                        //MessageBox.Show(sqlStr);
                        comm = new OleDbCommand(sqlStr, conn);
                        OleDbDataReader reader = comm.ExecuteReader(
                            System.Data.CommandBehavior.SingleResult);
                        while (reader.Read())
                        {
                            // Instantiate a Button with constructor
                            this[i].btnConfig[numButtons] = new ButtonConfig(imgDir + reader.GetString(0),
                                reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetDecimal(5));
                            // Instantiate a Label with constructor
                            this[i].lblConfig[numButtons] = new LabelConfig(reader.GetString(6),
                                reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetDecimal(5));
                            numButtons++;
                        }
                        // Record current Forms number of Buttons
                        this[i].numButtons = numButtons;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Exception: " + ex.ToString());
                    }
                    finally
                    {
                        if (conn != null)
                            conn.Close();
                    }
                }
                
                else if (this[i].frmType == "typFrmTopping")
                {
                    
                    // Instantiate a ButtonConfig array with max rows x max cols elements
                    this[i].btnConfig = new ButtonConfig[this[i].maxBtnRows * this[i].maxBtnCols];

                    // Instantiate a LabelConfig array with max rows x max cols elements
                    this[i].lblConfig = new LabelConfig[this[i].maxBtnRows * this[i].maxBtnCols];
                    

                    // Read Buttons for this Form from DB
                    try
                    {
                        conn = GetConnection();
                        conn.Open();
                        sqlStr = "SELECT fldBtnCfgImage, fldBtnCfgOper, fldBtnCfgTarget, fldFrmOpenIdx, fldID, fldPrice, fldName " +
                            "FROM tblToppings " +
                            "WHERE fldFormID = " + this[i].frmIdx;
                        //MessageBox.Show(sqlStr);
                        comm = new OleDbCommand(sqlStr, conn);
                        OleDbDataReader reader = comm.ExecuteReader(
                            System.Data.CommandBehavior.SingleResult);
                        while (reader.Read())
                        {
                            // Instantiate a Button with constructor
                            this[i].btnConfig[numButtons] = new ButtonConfig(imgDir + reader.GetString(0),
                                reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetDecimal(5), reader.GetString(6));
                            // Instantiate a Label with constructor
                            this[i].lblConfig[numButtons] = new LabelConfig(reader.GetString(6),
                                reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetDecimal(5));

                            numButtons++;
                        }
                        // Record current Forms number of Buttons
                        this[i].numButtons = numButtons;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Exception: " + ex.ToString());
                    }
                    finally
                    {
                        if (conn != null)
                            conn.Close();
                    }
                }

            }

        }


        // Get a connection to DB
        public static OleDbConnection GetConnection()
        {
            OleDbConnection conn = null;
            string connStr = File.ReadAllText(@"..\..\DBConnStr.txt", Encoding.UTF8);
            conn = new OleDbConnection(connStr);
            return conn;
        }




    }
}
