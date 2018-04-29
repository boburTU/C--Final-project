using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DynFormEx
{
    // The FormConfig class contains data and methods to draw a Form
    class FormConfig
    {
        // Form data read from DB
        public int frmIdx;          // The unique ID for the Form in app abd DB
        private string frmTitle;    // The title shown on the Form (none for none)
        private string frmImage;    // The path of the image to be displayed for background
        public string frmType;      // The type of Form (typFrmButton or TypeFrmImage fir this app)
        private string imgDir;      // The directory (folder) that contains the app images

        // Button data
        public ButtonConfig[] btnConfig;    // Array fo Buttons for this Form
        public int numButtons;              // Number of Butons on Form
        public int maxBtnRows = 3;          // Max rows of Buttons on Form
        public int maxBtnCols = 4;          // Max cols of Buttons on Form

        // Calculated measurements
        private int frmWidth;       // Maximized width of Form
        private int frmHeight;      // Maximized height of Form
        private int btnWidth;       // Calculated width of a Button wrt the Form
        private int btnHeight;      // Calculated height of a Button wrt the Form
        private int btnStartPosX;   // Starting X position on Form of first Button
        private int btnStartPosY;   // Starting Y position on Form of first Button
        private int btnOffsetX;     // Spacing X between Buttons
        private int btnOffsetY;     // Spacing Y between Buttons


        public LabelConfig[] lblConfig;     //Array for Labels for this form

        // Constructor for FormConfig
        public FormConfig(int frmIdx, string frmTitle, string frmImage, string frmType, string imgDir)
        {
            this.frmIdx = frmIdx;
            this.frmTitle = frmTitle;
            this.frmImage = frmImage;
            this.frmType = frmType;
            this.imgDir = imgDir;
        }


        // Draw a Form to print receipt
        public void ConfigureReceiptForm(frmMain fm)
        {
            // Set Form Properties
            // Maximize the Form
            fm.WindowState = FormWindowState.Maximized;
            // Remove the Form control box
            fm.ControlBox = false;
            // Remove border around Form
            fm.FormBorderStyle = FormBorderStyle.None;


            // Get width and height of maximized Form
            frmWidth = fm.Size.Width;
            frmHeight = fm.Size.Height;


            //set background color
            fm.BackColor = Color.LightGoldenrodYellow;

            // Instantiate Form Button and Label
            fm.btnOper = new Button();
            fm.lblTitle = new Label();

            fm.btnOper.Text = "DONE";
            // Set location of Button on Form
            fm.btnOper.Location = new System.Drawing.Point(frmWidth/2, frmHeight - 50);
            // Event handler wiring
            fm.btnOper.Click += new System.EventHandler(fm.btnOper_Click);
            // Add Button to Form
            fm.Controls.Add(fm.btnOper);

            // Put title Label on Form
            // Set position of Label
            // Note that (0, 0) is the upper left of Form
            int thisLblPosX = frmWidth / 2 - 250;
            int thisLblPosY = 75;
            // Set Label Name
            fm.lblTitle.Name = "lblTitle";
            // Set Label Text
            fm.lblTitle.Text = frmTitle;
            // Set Label Font color
            fm.lblTitle.ForeColor = Color.Black;
            // Set Label background color
            fm.lblTitle.BackColor = Color.AntiqueWhite;
            // Set Label size
            fm.lblTitle.Size = new System.Drawing.Size(500, 50);
            // Set position of text in Label
            fm.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // St Font family, point and style
            fm.lblTitle.Font = new Font("Arial", 24, FontStyle.Bold);
            // Set Label location
            fm.lblTitle.Location = new System.Drawing.Point(thisLblPosX, thisLblPosY);
            // Add Label to Form
            fm.Controls.Add(fm.lblTitle);


            fm.txtBox = new TextBox();

            fm.txtBox.Multiline = true;
            fm.txtBox.ReadOnly = true; fm.txtBox.BorderStyle = BorderStyle.Fixed3D;
            fm.txtBox.Name = "txtRecipt";
            fm.txtBox.ForeColor = Color.Black;
            fm.txtBox.BackColor = Color.AntiqueWhite;
            fm.txtBox.Size = new System.Drawing.Size(500, ((frmHeight*3)/4));
            fm.txtBox.Font = new Font("Arial", 24, FontStyle.Bold);
            fm.txtBox.Location = new System.Drawing.Point(thisLblPosX, thisLblPosY +54);
            fm.Controls.Add(fm.txtBox);
            Product p = new Product(ProductDB.GetProducts()); //get the current product from DB
            Topping t = new Topping();  //get the selected toppings for the above selected product from DB
            fm.txtBox.Text = p.ToRecipt() + "\r\n"+ t.ToRecipt() + "\r\nTotal price: " + 
                (p.Price + t.TotalPrice ).ToString("c"); 
        }



        public void ConfigureButtonForm(frmMain fm)
        {

            // Instantiate Form Button and Label
            fm.btnOper = new Button();
            fm.lblTitle = new Label();

            // Set Form Properties - These are fixed
            // Maximize the Form
            fm.WindowState = FormWindowState.Maximized;
            // Remove the Form control box
            fm.ControlBox = false;
            // Remove border around Form
            fm.FormBorderStyle = FormBorderStyle.None;
            string path = imgDir + frmImage;
            // Read image from file
            Image image1 = Image.FromFile(path, true);
            // Set Image as background
            fm.BackgroundImage = image1;
            // Stretch image to fit background size
            fm.BackgroundImageLayout = ImageLayout.Stretch;
            // Get width and height of maximized Form
            frmWidth = fm.Size.Width;
            frmHeight = fm.Size.Height;

            // Get sizes and offsets - These are calculated from Form width and height
            btnWidth = (int)(frmWidth / 6.5);
            btnOffsetX = (int)(btnWidth * 0.5);
            btnStartPosX = btnOffsetX;
            btnHeight = (int)(frmHeight / 7.0);
            btnOffsetY = (int)(btnHeight * 1.75);
            btnStartPosY = btnOffsetY;

            // Create exit Button if in DEBUG mode
            if (fm.DEBUG)
            {
                // Set Text property
                fm.btnOper.Text = "Exit";
                // Set location of Button on Form
                fm.btnOper.Location = new System.Drawing.Point(frmWidth - 80, frmHeight - 30);
                // Event handler wiring
                fm.btnOper.Click += new System.EventHandler(fm.btnOper_Click);
                // Add Button to Form
                fm.Controls.Add(fm.btnOper);
            }
            // Else put Admin Button on Form
            else
            {
                fm.btnOper.Text = "Settings";
                // Set location of Button on Form
                fm.btnOper.Location = new System.Drawing.Point(frmWidth - 80, frmHeight - 40);
                // Event handler wiring
                fm.btnOper.Click += new System.EventHandler(fm.btnOper_Click);
                // Add Button to Form
                fm.Controls.Add(fm.btnOper);
            }



            if (fm.frmType == "typFrmTopping")
            {
                fm.btnCommit = new Button();
                fm.btnCommit.Text = "Commit";
                fm.btnCommit.Location = new System.Drawing.Point(frmWidth - 200, frmHeight - 40);
                // Event handler wiring
                fm.btnCommit.Click += new System.EventHandler(fm.btnCommit_Click);
                // Add Button to Form
                fm.Controls.Add(fm.btnCommit);
            }



            // Put title Label on Form
            // Set position of Label
            // Note that (0, 0) is the upper left of Form
            int thisLblPosX = frmWidth / 2 - 250;
            int thisLblPosY = 75;
            // Set Label Name
            fm.lblTitle.Name = "lblTitle";
            // Set Label Text
            fm.lblTitle.Text = frmTitle;
            // Set Label Font color
            fm.lblTitle.ForeColor = Color.Orange;
            // Set Label background color
            fm.lblTitle.BackColor = Color.LightGoldenrodYellow;
            // Set Label size
            fm.lblTitle.Size = new System.Drawing.Size(500, 50);
            // Set position of text in Label
            fm.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // St Font family, point and style
            fm.lblTitle.Font = new Font("Arial", 24, FontStyle.Bold);
            // Set Label location
            fm.lblTitle.Location = new System.Drawing.Point(thisLblPosX, thisLblPosY);
            // Add Label to Form
            fm.Controls.Add(fm.lblTitle);

            // Put Buttons on form
            // Init Button count
            int btnCount = 0;
            // Position of current Button on Form
            int thisBtnPosX = btnStartPosX;
            int thisBtnPosY = btnStartPosY;
            // Instantiate the array of Buttons for this Form
            fm.btnArray = new Button[maxBtnRows * maxBtnCols];  fm.lblArray = new Label[maxBtnRows * maxBtnCols];
            // Loop through rows of Buttons
            for (int i = 0; i < maxBtnRows; i++)
            {
                // Get start position for row
                thisBtnPosX = btnStartPosX;
                // Loop through cols
                for (int j = 0; j < maxBtnCols; j++)
                {
                    // Get DB data until all Buttons are added
                    if (btnCount < numButtons)
                    {
                        // Instantiate a Button
                        fm.btnArray[btnCount] = new Button();
                        //Instantiate a Label
                        fm.lblArray[btnCount] = new Label();
                        // Set Label Name
                        fm.lblArray[btnCount].Name = lblConfig[btnCount].labelName + " - " + lblConfig[btnCount].lblprice.ToString("c");
                        // Set Size of Button
                        fm.btnArray[btnCount].Size = new System.Drawing.Size(btnWidth, btnHeight);
                        // Set Size of Label
                        fm.lblArray[btnCount].Size = new System.Drawing.Size(btnWidth, btnHeight / 4);
                        // Set Location of Button
                        fm.btnArray[btnCount].Location = new System.Drawing.Point(thisBtnPosX, thisBtnPosY);
                        // Set Location of Label
                        fm.lblArray[btnCount].Location = new System.Drawing.Point(thisBtnPosX, thisBtnPosY + 110);
                        //allign label text to center
                        fm.btnArray[btnCount].TextAlign = ContentAlignment.BottomCenter;
                        // Set Text for Label
                        fm.lblArray[btnCount].Text = fm.lblArray[btnCount].Name;
                        // Set Font color of Button
                        fm.btnArray[btnCount].ForeColor = Color.AntiqueWhite;
                        // Set back color of button
                        fm.btnArray[btnCount].BackColor = Color.LightGoldenrodYellow;
                        // Set font color of label
                        fm.lblArray[btnCount].ForeColor = Color.Red;
                        // Set back color of label
                        fm.lblArray[btnCount].BackColor = Color.Yellow;
                        // Read image from file
                        image1 = Image.FromFile(btnConfig[btnCount].btnCfgImage, true);
                        //allign label text to center
                        fm.lblArray[btnCount].TextAlign = ContentAlignment.MiddleCenter;
                        // Set Image as background image
                        fm.btnArray[btnCount].BackgroundImage = image1;
                        // Stretch image to fit background size
                        fm.btnArray[btnCount].BackgroundImageLayout = ImageLayout.Stretch;
                        // Copy btnConfig to this Buttons Tag for future reference
                        fm.btnArray[btnCount].Tag = btnConfig[btnCount];
                        // Event handler wiring
                        fm.btnArray[btnCount].Click += new System.EventHandler(fm.btnArray_Click);
                        // Add Button to Form
                        fm.Controls.Add(fm.btnArray[btnCount]);
                        // Add Label to Form
                        fm.Controls.Add(fm.lblArray[btnCount]);
                        // Increment count
                        btnCount++;
                    }
                    // Advance to next Button position in X
                    thisBtnPosX += btnOffsetX + btnWidth;
                }
                // Advance to next Button position in Y
                thisBtnPosY += btnOffsetY;
            }
        }


        // ToString to print Form data
        public override string ToString()
        {
            string s = frmIdx + " ";
            s += frmTitle + " ";
            s += frmImage + " ";
            s += frmType + " ";
            s += imgDir + " ";
            s += numButtons;
            return s;
        }

    } // End class

} // End namespace
