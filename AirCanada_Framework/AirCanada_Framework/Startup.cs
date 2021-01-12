using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Experimental.System.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AirCanada_Framework
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }


        public void ReceiveMessageTransactional()
        {
            // Connect to a transactional queue on the local computer.
            MessageQueue myQueue = new
                MessageQueue(".\\myTransactionalQueue");

            // Set the formatter.
            myQueue.Formatter = new XmlMessageFormatter(new Type[]
                {typeof(String)});

            // Create a transaction.
            MessageQueueTransaction myTransaction = new
                MessageQueueTransaction();

            try
            {
                // Begin the transaction.
                myTransaction.Begin();

                // Receive the message.
                Message myMessage = myQueue.Receive(myTransaction);
                String myOrder = (String)myMessage.Body;

                // Display message information.
                Console.WriteLine(myOrder);

                // Commit the transaction.
                myTransaction.Commit();
            }

            catch (MessageQueueException e)
            {
                // Handle nontransactional queues.
                if (e.MessageQueueErrorCode ==
                    MessageQueueErrorCode.TransactionUsage)
                {
                    Console.WriteLine("Queue is not transactional.");
                }

                // Else catch other sources of MessageQueueException.

                // Roll back the transaction.
                myTransaction.Abort();
            }

            // Catch other exceptions as necessary, such as
            // InvalidOperationException, thrown when the formatter
            // cannot deserialize the message.

            return;
        }

    }
}
