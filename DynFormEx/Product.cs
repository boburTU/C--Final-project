using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynFormEx
{
    class Product
    {
        // Data
        public int ID { get; set; }         // Unique ID
        public string Name { get; set; }    // Product name
        public double Price { get; set; }   // Product price


        // Constructor
        public Product(int id, string name, double price)
        {
            ID = id;
            Name = name;
            Price = price;
        }
        //constructor 2
        public Product(Product p)
        {
            
            ID = p.ID;
            Name = p.Name;
            Price = p.Price;

        }
        //print the vales of product
        public string ToRecipt()
        {
            return "Order # " + ID + "\r\n" + Name + "\r\t" + Price.ToString("c");
        }

    }
}
