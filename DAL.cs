using System;
using System.Data;
using System.Data.SqlClient;
using DataAccessLibrary;
using Modell;
using System.Text.RegularExpressions;

namespace DataAccessLibrary
{
    public class DAL
    {
        static string constr = "data source=NEHA\\SQLEXPRESS;initial catalog=RestDetails;integrated security=True;";
        public void DisplayRestaurantList()
        {
            DataTable dt = ExecuteData("select * from Restaurant");
            if (dt.Rows.Count > 0)
            {
                Console.Write(Environment.NewLine);
                Console.WriteLine("===================================================");
                Console.WriteLine("All Active Restaurants");
                Console.WriteLine("===================================================");

                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine(row["RID"].ToString() + "   " + row["RName"].ToString() + "   " + row["RPhoneNo"].ToString() +"   " + row["RAddress"].ToString() +"   " + row["ROpeningTime"].ToString() +"   "+ row["RClosingTime"].ToString() + "   " + row["RCuisine"].ToString());
                }
                Console.WriteLine("================================================" + Environment.NewLine);
            }
            else
            {
                Console.Write(Environment.NewLine);
                Console.WriteLine("No Restaurant found in database!!!");
                Console.Write(Environment.NewLine);
            }
        }
        public DataTable ExecuteData(string query)
        {
            DataTable result = new DataTable();
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(constr))
                {
                    sqlcon.Open();
                    SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(result);
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
        public void SearchRestaurant(Restaurant restaurant)
        {
            string RName = string.Empty;

            Console.Write("Enter the name of the  Restaurant you want to search: ");
            RName = Console.ReadLine();

            DataTable DT = ExecuteData("select RName, RAddress, RPhoneNo, ROpeningTime, RClosingTime, RCuisine from Restaurant where RName='" + RName + "'");
            if (DT.Rows.Count > 0)
            {
                Console.Write(Environment.NewLine);
                Console.WriteLine("======================================================================");
                Console.WriteLine("=============Your Favourite Restaurant are here========================");
                Console.WriteLine("======================================================================");
                foreach (DataRow row in DT.Rows)
                {
                    Console.WriteLine(row["RName"].ToString() + "  " + row["RAddress"].ToString() + " " + row["RPhoneNo"].ToString() + " " + row["RCuisine"].ToString());
                }
                Console.WriteLine("======================================================================" + Environment.NewLine);
            }
            else
            {
                Console.Write(Environment.NewLine);
                Console.WriteLine("!!!" + RName + " Not Found!!!");
                Console.Write(Environment.NewLine);
            }
        }
        public bool ExecuteCommand(string query)
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(constr))
                {
                    sqlcon.Open();
                    SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                    sqlcmd.ExecuteNonQuery();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                throw;
            }
            return true;
        }
        public Restaurant GetInputFromUser()
        {
            string RID = string.Empty;
            string RName = string.Empty;
             long RPhoneNo;
            string RAddress = string.Empty;
            string ROpeningTime = string.Empty;
            string RClosingTime = string.Empty;
            string RCuisine = string.Empty;

            Console.WriteLine("Add new Restaurant");
            Console.Write("Enter RID: ");
            RID = Console.ReadLine();
            while (string.IsNullOrEmpty(RID))
            {
                Console.WriteLine("You Cannot Have A Blank ID of Restaurant");
                Console.WriteLine("Please Re-enter Restaurant ID:");
                RID = Console.ReadLine();
            }


            Console.Write("Enter RName: ");
            RName = Console.ReadLine();   
            while(string.IsNullOrEmpty(RName))
            {
                Console.WriteLine("You Cannot Have A Blank Name of Restaurant");
                Console.WriteLine("Please Re-enter Restaurant Name:");
                RName = Console.ReadLine();
            }

          //  Console.Write("Enter RPhoneNo: ");
          //  RPhoneNo =(Convert.ToInt64 ( Console.ReadLine()));
            while (true)
            {
                Console.Write("Enter contact of Restaurant: ");
                RPhoneNo = Convert.ToInt64(Console.ReadLine());
                bool status = isValidMobileNumber(RPhoneNo);

                //Validate V = new Validate();
                //Console.WriteLine("{0} a valid mobile number.", isValidMobileNumber(RestPhoneNo) ? "is" : "is not");
                if (status == true)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please Enter Valid Contact No.....");
                    continue;
                }
            }

            Console.Write("Enter RAddress: ");
            RAddress = Console.ReadLine();
            while (string.IsNullOrEmpty(RAddress))
            {
                Console.WriteLine("You Cannot Have A Blank Address of Restaurant");
                Console.WriteLine("Please Re-enter Restaurant's Address:");
                RAddress = Console.ReadLine();
            }

            Console.Write("Enter ROpeningTime in 24 hrs format[hh:mm:ss]: ");
            ROpeningTime = Console.ReadLine();
            while (string.IsNullOrEmpty(ROpeningTime))
            {
                Console.WriteLine("You Cannot Have A blank Opening time of Restaurant");
                Console.WriteLine("Please Re-enter Restaurant's opening time:");
                ROpeningTime = Console.ReadLine();
            }

            Console.Write("Enter RClosingTime in 24 hrs format[hh:mm:ss]: ");
            RClosingTime = Console.ReadLine();
            while (string.IsNullOrEmpty(RClosingTime))
            {
                Console.WriteLine("You Cannot Have A Blank closing time of Restaurant");
                Console.WriteLine("Please Re-enter Restaurant's closing time:");
                RClosingTime = Console.ReadLine();
            }

            Console.Write("Enter RCuisine: ");
            RCuisine = Console.ReadLine();
            while (string.IsNullOrEmpty(RCuisine))
            {
                Console.WriteLine("You Cannot Have A Blank cuisine of Restaurant");
                Console.WriteLine("Please Re-enter Restaurant's cuisine:");
                RCuisine = Console.ReadLine();
            }

            Restaurant restaurant = new Restaurant()
            {
                RID = RID,
                RName = RName,
                RPhoneNo = RPhoneNo,
                RAddress = RAddress,
                ROpeningTime = ROpeningTime,
                RClosingTime = RClosingTime,
                RCuisine = RCuisine

            };
            return restaurant;
        }
        public void AddNewRestaurant(Restaurant restaurant)
        {
          
            ExecuteCommand(String.Format("Insert into Restaurant(RID,RName, RPhoneNo, RAddress, ROpeningTime,RClosingTime,RCuisine) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", restaurant.RID, restaurant.RName, restaurant.RPhoneNo, restaurant.RAddress, restaurant.ROpeningTime, restaurant.RClosingTime, restaurant.RCuisine));
            Console.WriteLine(".................Restaurant SuccessFully Added in  RSystem.........." + Environment.NewLine);
        }
        /*  public void UpdateRestaurant(Restaurant restaurant)
          {
              string RID = string.Empty;
              Console.WriteLine("Edit/update Existing Restaurant:");
              Console.WriteLine("Enter RID of restaurant you want to update:");

              Console.Write("Enter Id of Restaurant: ");
              RID = Console.ReadLine();

              ExecuteCommand(String.Format("Update Restaurant set RName='{1}', RAddress='{2}', RPhoneNo='{3}', RCuisine='{6}' where RId='{0}'", restaurant.RID, restaurant.RName, restaurant.RAddress, restaurant.RPhoneNo, restaurant.RCuisine));
          }
      */
       
        public void UpdateRestaurant(Restaurant restaurant)
        {
            ExecuteCommand(String.Format("UPDATE Restaurant SET RID='{0}', RName='{1}', RPhoneNo='{2}', RAddress='{3}',ROpeningTime='{4}',RClosingTime='{5}', RCuisine='{6}' where RID='{0}'", restaurant.RID, restaurant.RName, restaurant.RAddress, restaurant.RPhoneNo, restaurant.ROpeningTime, restaurant.RClosingTime, restaurant.RCuisine));
            Console.WriteLine(); 
            Console.WriteLine();
            Console.WriteLine("Restaurant has been updated in your list");
        }

        public void DeleteRestaurant(Restaurant restaurant)
        {
            string RID = string.Empty;

            Console.WriteLine("Delete Existing Restaurant ");

            Console.Write("Enter Id of Restaurant: ");
            RID = Console.ReadLine();

            ExecuteCommand(String.Format("Delete from Restaurant where RId = '{0}'", RID));

            Console.WriteLine("..........Restaurant SuccessFully Deleted from RSystem............." + Environment.NewLine);
        }
        public static bool isValidMobileNumber(long inputMobileNumber)
        {
            string strRegex = @"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9] {2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)";


            Regex re = new Regex(strRegex);


            if (re.IsMatch(Convert.ToString(inputMobileNumber)))
            {
                return (true);
            }

            else
            {
                return (false);
            }

        }


    }
}
