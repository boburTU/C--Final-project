using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynFormEx
{
    // The ButtonConfig class contains data for a Button on a Form
    class ButtonConfig
    {
        // Path of image on Button
        public string btnCfgImage;
        // Operation to be performed when Button is clicked
        // Values are showForm, commitOrder, addOption
        public string btnCfgOPer;
        // Target Form for operation
        public string btnCfgTarget;
        // Array index of Form opened (-1 for none)
        public int frmOpenIdx;


        //current selected productID
        public int btnProductID;
        //current selected productID
        public decimal btnProductPrice;
        //current selected TOppingID
        public int btnToppingID;
        //current selected ToppingPrice
        public decimal btnToppingPrice;
        //current selected ToppingName
        public string btnToppingName;


        // Constructor for ButtonCfg // FOR FOODS
        public ButtonConfig(string btnCfgImage, string btnCfgOPer, string btnCfgTarget, int frmOpenIdx, int btnProductID, decimal btnProductPrice)
        {
            this.btnCfgImage = btnCfgImage;
            this.btnCfgOPer = btnCfgOPer;
            this.btnCfgTarget = btnCfgTarget;
            this.frmOpenIdx = frmOpenIdx;
            this.btnProductID = btnProductID;
            this.btnProductPrice = btnProductPrice;
        }

        // Constructor for ButtonCfg // FOR TOPPINGS
        public ButtonConfig(string btnCfgImage, string btnCfgOPer, string btnCfgTarget, int frmOpenIdx, int btnToppingID, decimal btnToppingPrice, string btnToppingName)
        {
            this.btnCfgImage = btnCfgImage;
            this.btnCfgOPer = btnCfgOPer;
            this.btnCfgTarget = btnCfgTarget;
            this.frmOpenIdx = frmOpenIdx;
            this.btnToppingID = btnToppingID;
            this.btnToppingPrice = btnToppingPrice;
            this.btnToppingName = btnToppingName;
        }


        // ToString for debugging
        public override string ToString()
        {
            string s = btnCfgImage + "\r\n" +
                btnCfgOPer + "\r\n" +
                btnCfgTarget + "\r\n" +
                frmOpenIdx;
            return s;
        }

    } // End class

} // End namespace
