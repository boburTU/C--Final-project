using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynFormEx
{
    class Topping
    {
        // Data
        public int ID { get; set; }         // Unique ID
        public string Name { get; set; }    // Ingredient name
        public double Price { get; set; }   // Ingredient price
        public double TotalPrice { get; set; }
        
        // Constructor 
        public Topping(int id, string name, double price)
        {
            ID = id;
            Name = name;
            Price = price;
        }
        //declare list of type TOppings
        List<Topping> tl;

        //Constructor2
        public Topping ()
        {
            //initialize list by getting the toppings
            tl = ProductDB.GetToppings();
        }

        
        public string ToRecipt()
        {
            string s = "";  //string for storing toppings
            foreach (Topping t in tl) {

               s+= "=>" + t.Name + "\r\t" + t.Price.ToString("c") + "\r\n";
               TotalPrice += t.Price;   //calculate total topping price
            }
            return s;


        }
    }
}
