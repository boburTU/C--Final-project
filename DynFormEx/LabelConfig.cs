using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynFormEx
{
    class LabelConfig
    {
        // name for label
        public string labelName;
        // Operation to be performed when Button is clicked
        // Values are showForm, commitOrder, addOption
        public string labelCfgOPer;
        // Target Form for operation
        public string labelCfgTarget;
        // Array index of Form opened (-1 for none)
        public int frmOpenIdx;
        //price for product or topping
        public decimal lblprice;

        // Constructor for ButtonCfg
        public LabelConfig(string labelName, string labelCfgOPer, string labelCfgTarget, int frmOpenIdx, decimal lblprice)
        {
            this.labelName = labelName;
            this.labelCfgOPer = labelCfgOPer;
            this.labelCfgTarget = labelCfgTarget;
            this.frmOpenIdx = frmOpenIdx;
            this.lblprice = lblprice;
        }


        // ToString for debugging
        public override string ToString()
        {
            string s = labelName + "\r\n" +
                labelCfgOPer + "\r\n" +
                labelCfgTarget + "\r\n" +
                frmOpenIdx;
            return s;
        }
    }
}
