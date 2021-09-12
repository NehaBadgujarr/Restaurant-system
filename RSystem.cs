using System;
using System.Data;
using System.Data.SqlClient;
using DataAccessLibrary;
using Modell;

namespace RSystem
{
    class RSystem
    {
        static void Main(string[] args)
        {

            Console.WriteLine("===================RESTAURANT SYSTEMS======================================");
             DAL c = new DAL();
             c.DisplayRestaurantList();


             Restaurant restaurant = c.GetInputFromUser();

                 c.AddNewRestaurant(restaurant);
                   c.DisplayRestaurantList();

                   c.SearchRestaurant(restaurant);

                   c.DeleteRestaurant(restaurant);
                   c.DisplayRestaurantList();
         
        
                c.UpdateRestaurant(restaurant);
                c.DisplayRestaurantList();
           
           
        }
    }
}
