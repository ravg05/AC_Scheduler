using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Experimental.System.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCanada_Framework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Program mynewQueue = new Program();
            Console.WriteLine("Writing");
            mynewQueue.createQueue();
          //  mynewQueue.SendMessage();

            mynewQueue.ReceiveMessage();

            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        public void createQueue()
        {
            if (!MessageQueue.Exists(".\\ScheduleData"))
            {
                // Create the queue if it does not exist.
                MessageQueue myNewPublicQueue =
                    MessageQueue.Create(".\\ScheduleData");

                // Send a message to the queue.
                myNewPublicQueue.Send("My message data.");
            }

            // Create (but do not connect to) a second public queue.
            //if (!MessageQueue.Exists(".\\ScheduleData"))
            //{
              //  MessageQueue.Create(".\\ScheduleData");
            //}

            return;
        }

        public void SendMessage()
        {
            // Connect to a queue on the local computer.
            MessageQueue myQueue = new MessageQueue(".\\ScheduleData");

            if (MessageQueue.Exists(".\\ScheduleData"))
            {
                Console.WriteLine("Exists");
            }
            //Console.WriteLine(myQueue.Transactional);
            // Send a message to the queue.
            //if (myQueue.Transactional == true)
            //{
            //    // Create a transaction.
            //    MessageQueueTransaction myTransaction = new
            //        MessageQueueTransaction();

            //    // Begin the transaction.
            //    myTransaction.Begin();

            //    // Send the message.
            //    myQueue.Send("AC123401DEC2031DEC201 3 5 71450YULYYZ301", myTransaction);
            //    Console.WriteLine("Message sent");
            //    // Commit the transaction.
            //    myTransaction.Commit();
            //}
            //else
            //{
            //    myQueue.Send("My Message Data.");
            //}

            //return;
        }


        public void ReceiveMessage()
        {
            // Connect to the a queue on the local computer.
            MessageQueue myQueue = new MessageQueue(".\\ScheduleData");

            // Set the formatter to indicate body contains an Order.
           // myQueue.Formatter = new XmlMessageFormatter(new Type[]
             //   {typeof(MyProject.Order)});

            try
            {
                // Receive and format the message.
                Message myMessage = myQueue.Receive();
                // Order myOrder = (Order)myMessage.Body;

                Console.WriteLine(myMessage);
                // Display message information.
                //Console.WriteLine("Order ID: " +
                  //  myOrder.orderId.ToString());
                //Console.WriteLine("Sent: " +
                  //  myOrder.orderTime.ToString());
            }

            catch (MessageQueueException)
            {
                // Handle Message Queuing exceptions.
            }

            // Handle invalid serialization format.
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }

            // Catch other exceptions as necessary.

            return;
        }

    }
}
