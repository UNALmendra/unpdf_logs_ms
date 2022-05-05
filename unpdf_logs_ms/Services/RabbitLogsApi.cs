﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using unpdf_logs_ms.Models;

namespace unpdf_logs_ms.Services
{
    public class LogsReceiverService : BackgroundService
    {
        private IServiceProvider _sp;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private readonly LogsService _logsService;

        // initialize the connection, channel and queue 
        // inside the constructor to persist them 
        // for until the service (or the application) runs
        public LogsReceiverService(IServiceProvider sp, LogsService logsService)
        {
            _sp = sp;

            _factory = new ConnectionFactory() { HostName = "host.docker.internal", UserName = "admin", Password = "masterkey" };

            _connection = _factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "logs",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _logsService = logsService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // when the service is stopping
            // dispose these references
            // to prevent leaks
            if (stoppingToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();
                return Task.CompletedTask;
            }

            // create a consumer that listens on the channel (queue)
            var consumer = new EventingBasicConsumer(_channel);

            // handle the Received event on the consumer
            // this is triggered whenever a new message
            // is added to the queue by the producer
            consumer.Received += (model, ea) =>
            {
                // read the message bytes
                var body = ea.Body.ToArray();

                // convert back to the original string
                // {index}|SuperHero{10000+index}|Fly,Eat,Sleep,Manga|1|{DateTime.UtcNow.ToLongDateString()}|0|0
                // is received here
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(" [x] Received {0}", message);


                Task.Run(async () =>
                {
                    // split the incoming message
                    // into chunks which are inserted
                    // into respective columns of the Heroes table

                    var newlog = JsonConvert.DeserializeObject<Log>(message);

                    Console.WriteLine(newlog); 
                    
                    DateTime now = DateTime.Now;

                    newlog.Date = now.Add(new TimeSpan(-5, 0, 0));
                    // BackgroundService is a Singleton service
                    // IHeroesRepository is declared a Scoped service
                    // by definition a Scoped service can't be consumed inside a Singleton
                    // to solve this, we create a custom scope inside the Singleton and 
                    // perform the insertion.
                    await _logsService.CreateAsync(newlog);
                });
            };

            _channel.BasicConsume(queue: "logsq", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
