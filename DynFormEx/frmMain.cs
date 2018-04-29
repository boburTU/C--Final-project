using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DynFormEx
{
    // All non-fixed data for Forms are read from the DB
    // frmMain is instantiated for all forms
    public partial class frmMain : Form
    {
        public bool DEBUG = false;      // DEBUG mode for testing
        public Button btnOper;          // Button on Form to close or perform other Ops
        public Label lblTitle;          // Title of Form
        public Button[] btnArray;       // Button array for Form
        private FormConfigArr frmCfgArr;// Array of configs for all Forms in App
        public int frmCfgArrIdx;        // Index of current Form
        public string frmType;          // Type of Form
        public string frmImage;         // Path of background image on Form

        public Button btnCommit;        // Button on Form to open Receipt form
        public TextBox txtBox;          //textbox for displaying receipt information
        public bool clicked = false;    //to check whether topping were selected or not
        public Label[] lblArray;        //Label array for form



        // Form constructor for a typFrmButton Form
        private frmMain(int instNum, string frmType, string frmImage, FormConfigArr frmCfgArr)
        {
            this.frmCfgArrIdx = instNum;
            this.frmType = frmType;
            this.frmImage = frmImage;
            this.frmCfgArr = frmCfgArr;
            InitializeComponent();
        }


        // Form constructor for a typFrmImage Form
        private frmMain(int instNum, string frmType, FormConfigArr frmCfgArr)
        {
            this.frmCfgArrIdx = instNum;
            this.frmType = frmType;
            this.frmCfgArr = frmCfgArr;
            InitializeComponent();
        }


        // Form constructor for first Form - called from Program.cs
        public frmMain(int instNum, string frmType)
        {
            this.frmCfgArrIdx = instNum;
            this.frmType = frmType;
            InitializeComponent();
        }


        // btnOper event handler
        public void btnOper_Click(object sender, EventArgs e)
        {
            if (btnOper.Text == "DONE")
            {
                //open Menu after user clicks Exit
                Form nf = new frmMain(0, "frmMain");
                nf.ShowDialog();
                // Dispose deletes Form
                //this.Dispose();
                // Close would keep instance of Form
                // This may lead to multiple unneeded copies in memory
                //this.Close();
                Application.Exit();
            }
            else if (btnOper.Text == "Settings")
            {
                // Dispose deletes Form
                this.Dispose();
                // Close would keep instance of Form
                // This may lead to multiple unneeded copies in memory
                //this.Close();
            }
        }


        // btnCommit event handler
        public void btnCommit_Click(object sender, EventArgs e)
        {
            //check to see if btn_array was clicked
            //whether topping was selected or not
            //if not, insert topping with no value
            if (clicked == false)
            {
                int max = ProductDB.SelectMaxOrder();
                ProductDB.InsertToppings(max, 7, 0.0);
            }
            //else open receipt form
            frmMain nf = new frmMain(13, "typFrmReceipt");
            nf.ShowDialog();
        }


        // btnArray[] event handler
        public void btnArray_Click(object sender, EventArgs e)
        {
            //topping selected
            clicked = true;
            // Cast sender as Button
            Button b = (Button)sender;
            // Cast Tag as ButtonConfig
            ButtonConfig bc = (ButtonConfig)b.Tag;


            // Declare reference to a frmMain Form
            frmMain nf = null;
            // Instantiate Form given type of Form
            if (bc.btnCfgTarget == "typFrmButton")
            {
                nf = new frmMain(bc.frmOpenIdx, bc.btnCfgTarget, frmCfgArr);
                // Show and transfer flow of contril to new Form
                nf.ShowDialog();
            }
            else if (bc.btnCfgTarget == "typFrmTopping")
            {
                //insert selected product into DB
                ProductDB.InsertOrder(bc.btnProductID, Convert.ToDouble(bc.btnProductPrice));
                nf = new frmMain(bc.frmOpenIdx, bc.btnCfgTarget, frmCfgArr);
                // Show and transfer flow of contril to new Form
                nf.ShowDialog();
            }
            else if (bc.btnCfgTarget == "typFrmReceipt")
            {
                //insert selected toppings to a product that selected it
                int max = ProductDB.SelectMaxOrder();
                ProductDB.InsertToppings(max, bc.btnToppingID, Convert.ToDouble(bc.btnToppingPrice));
                MessageBox.Show(bc.btnToppingName + " was added to your order.");
            }

        }


        // Form load event handler - Set initial Form
        // and handle subsequent Forms
        private void frmMain_Load(object sender, EventArgs e)
        {
            // If initial Form
            if (frmCfgArrIdx == 0)
            {
                // Instantiate and fill frmCfgArr
                frmCfgArr = new FormConfigArr();
                frmCfgArr.GetFormConfigs();

                // Draw initial Form
                if(frmCfgArr != null && frmCfgArr.Count > 0 && frmCfgArr[0] != null)
                    frmCfgArr[0].ConfigureButtonForm(this);
            }
            else // A subsequent Form
            {
                // If a Button Form
                if (frmType == "typFrmButton")
                {
                    frmCfgArr[frmCfgArrIdx].ConfigureButtonForm(this);
                }
                //else if topping form
                else if (frmType == "typFrmTopping")
                    frmCfgArr[frmCfgArrIdx].ConfigureButtonForm(this);
                
                //else receipt form
                else if (frmCfgArrIdx == 13)
                {
                    frmCfgArr = new FormConfigArr();
                    frmCfgArr.GetFormConfigs();
                    frmCfgArr[13].ConfigureReceiptForm(this);
                }
            }
        }

    }
}
