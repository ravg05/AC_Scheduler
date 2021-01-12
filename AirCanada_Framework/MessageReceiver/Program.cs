using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.Remoting.Channels.Ipc;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


//Interprocess communication
namespace MessageReceiver
{
    class Program
    {
        static void Main(string[] args)
        {            
            Messaging.createQueue();

            insertTextFile("New_Schedule.txt");

            while (true)
            {
                String receivedMessage = Messaging.ReceiveMessage();

                if (receivedMessage != "")
                {
                    insertToJson(receivedMessage);
                }
            }
        }

        //Previous implementation of inserting into a temp db to share data
        public static void inserToDB(string message)
        {
            String connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Flights;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(connectionString);

            String FlightNo = message.Substring(0, 6);
            String PeriodOperation = message.Substring(6, 14);
            String DaysofOperation = message.Substring(15, 7);
            String DepartureTime = message.Substring(23, 3);
            String OriginStation = message.Substring(27, 3);
            String DestStation = message.Substring(31, 2);
            String Aircraft = message.Substring(34, 2);

            var cmd = new SqlCommand(@"Insert into flights (FlightNO, PeriodOperation, DaysOperation, DepartureTime, OriginStation, DestStation, Aircraft)
                VALUES(@flightNo, @PeriodOperation, @DayofOp, @depTime, @originSta, @destSta, @Aircraft)", conn);

            cmd.Parameters.AddWithValue("@flightNo", FlightNo);
            cmd.Parameters.AddWithValue("@PeriodOperation", PeriodOperation);
            cmd.Parameters.AddWithValue("@DayofOp", DaysofOperation);
            cmd.Parameters.AddWithValue("@depTime", DepartureTime);
            cmd.Parameters.AddWithValue("@originSta", OriginStation);
            cmd.Parameters.AddWithValue("@destSta", DestStation);
            cmd.Parameters.AddWithValue("@Aircraft", Aircraft);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        //Inserts data from *Get Request* 
        public static void insertTextFile(string textFilePath)
        {
            string[] lines = File.ReadAllLines(textFilePath);
            foreach(string line in lines)
            {
                insertToJson(line);
            }
        }

        //Adds each message to json file containing flight info
        public static void insertToJson(string message)
        {            
            string startupPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName, "flights.json");


            String FlightNo = message.Substring(0, 6);
            String PeriodOperation = message.Substring(6, 14);
            String DaysofOperation = message.Substring(20, 7);
            String DepartureTime = message.Substring(27, 4);
            String OriginStation = message.Substring(31, 3);
            String DestStation = message.Substring(34, 3);
            String Aircraft = message.Substring(37, 3);

            JObject flight = new JObject();

            flight["flightNo"] = FlightNo;
            flight["PeriodOfOperation"] = PeriodOperation;
            flight["DaysofOperation"] = DaysofOperation;
            flight["DepartureTime"] = DepartureTime;
            flight["OriginSta"] = OriginStation;
            flight["DestStation"] = DestStation;
            flight["Aircraft"] = Aircraft;

            Console.WriteLine(flight.ToString());

            
            if (File.Exists(startupPath))
            {
                var jsondata = System.IO.File.ReadAllText(startupPath);

                var data = JArray.Parse(jsondata);

                bool flightFound = false;
                foreach(var flight_val in data)
                {
                    if(flight_val["flightNo"].ToString() == FlightNo)
                    {
                        data.Remove(flight_val);
                        data.Add(flight);
                        flightFound = true;
                        break;
                    }
                }

                if(flightFound == false)
                {
                    data.Add(flight);
                }

                var jsonRes = JsonConvert.SerializeObject(data, Formatting.Indented);
                
                File.WriteAllText(startupPath, jsonRes);
            }
            else
            {
                var val = new JArray();
                val.Add(flight);
                File.WriteAllText(startupPath, JsonConvert.SerializeObject(val));
            }
        }
    }
}
