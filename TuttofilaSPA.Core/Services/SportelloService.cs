using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TuttofilaSPA.Core.Models;

namespace TuttofilaSPA.Core.Services
{
	public class SportelloService
	{
		private readonly ConcurrentDictionary<string, List<Servizio>> ServiziChiamati = new ConcurrentDictionary<string, List<Servizio>>();
		private readonly ConnectionFactory ConnectionFactory;
		private readonly List<AmqpTcpEndpoint> AmqpTcpEndpoints = new List<AmqpTcpEndpoint>()
			{
				new AmqpTcpEndpoint()
					{
						HostName = ConfigurationManager.AppSettings["RabbitMQHostname1"]
					},
					new AmqpTcpEndpoint()
					{
						HostName = ConfigurationManager.AppSettings["RabbitMQHostname2"]
					}
			};
		private readonly string exchange = "tuttofila";
		private readonly string routingKey = "chiamate";
		private readonly string queue = "ha.chiamate";

		public SportelloService(ConnectionFactory connectionFactory)
		{
			ConnectionFactory = connectionFactory;
			Consuma();
		}

		public void Pubblica(Servizio servizio)
		{
			using (var connection = ConnectionFactory.CreateConnection(AmqpTcpEndpoints))
			using (var channel = connection.CreateModel())
			{
				channel.ExchangeDeclare(exchange, "direct", false, false, null);
				channel.BasicPublish(exchange, routingKey, true, null, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(servizio)));
			}
		}

		public void Consuma()
		{
			using (var connection = ConnectionFactory.CreateConnection(AmqpTcpEndpoints))
			using (var channel = connection.CreateModel())
			{
				channel.ExchangeDeclare(exchange, "direct", false, false);
				channel.QueueDeclare(queue, false, false, false, null);
				channel.QueueBind(queue, exchange, routingKey);
				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, result) =>
				{
					try
					{
						channel.BasicAck(result.DeliveryTag, false);
						var servizio = JsonConvert.DeserializeObject<Servizio>(Encoding.UTF8.GetString(result.Body));
						var serviziChiamatiSala = ServiziChiamati.GetOrAdd(servizio.SalaId.ToString(), new List<Servizio>());
						serviziChiamatiSala.Add(servizio);
						ServiziChiamati.AddOrUpdate(servizio.SalaId.ToString(), serviziChiamatiSala, (_, __) => serviziChiamatiSala);
					}
					catch (Exception)
					{
						throw;
					}
				};

				channel.BasicConsume(queue, false, consumer);
			}
		}
	}
}
