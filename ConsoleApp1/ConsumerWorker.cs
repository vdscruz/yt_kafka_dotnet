using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ConsumerWorker : BackgroundService
    {
        private readonly ILogger<ConsumerWorker> _log;
        private readonly IConfiguration _config;
        private readonly string _host;
        private readonly string _topic;

        public ConsumerWorker(ILogger<ConsumerWorker> log, IConfiguration config)
        {
            _log = log;
            _config = config;
            _host = _config.GetSection("Kafka:Host").Value;
            _topic = _config.GetSection("Kafka:Topic").Value;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _host,
                GroupId = $"{_topic}-group-0",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(_topic);
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = consumer.Consume(stoppingToken);
                        _log.LogInformation($"Key: {cr.Message.Key} | Value: {cr.Message.Value}");
                    }
                    catch(OperationCanceledException oce)
                    {
                        continue;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
