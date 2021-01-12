using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace MessageReceiver
{
    public class Messaging
    {
        //Generates a private queue and sends a message
        public static void createQueue()
        {
            if (!MessageQueue.Exists(".\\Private$\\SchedulingToolQueue"))
            {
                // Create the queue if it does not exist.
                MessageQueue privateQueue =
                    MessageQueue.Create(".\\Private$\\SchedulingToolQueue");

                // Send a message to the queue.
                privateQueue.Send("AC123401DEC2031DEC201 3 5 71450YULYYZ301");
            }
            return;
        }

        //Function to send a message to private queue
        public static void SendMessage()
        {
            if (!MessageQueue.Exists(".\\Private$\\SchedulingToolQueue"))
            {
                MessageQueue myQueue = new MessageQueue(".\\Private$\\SchedulingToolQueue");
                myQueue.Send("AC123401DEC2031DEC201 3 5 71450YULYYZ301");
            }

            return;
        }

        //Receives message and pops from queue
        public static String ReceiveMessage()
        {

            String message_received = "";
            // Connect to the a queue on the local computer.
            MessageQueue myQueue = new MessageQueue(".\\Private$\\SchedulingToolQueue");

            List<string> messageList = new List<string>();
            try
            {
                //Get all messages in Queue
                // Message[] myMessage = myQueue.GetAllMessages();

                //Pop top message and queue and return it
                Message newMessage = myQueue.Receive(new
                    TimeSpan(0, 0, 3));
                newMessage.Formatter = new XmlMessageFormatter(new string[] { "System.String, mscorlib" });
                Console.WriteLine(newMessage.Body.ToString());
                message_received = newMessage.Body.ToString();
            }

            catch (MessageQueueException e)
            {
                // Handle Message Queuing exceptions.
                Console.WriteLine(e.Message.ToString());
            }

            return message_received;
        }
    }

    
}
