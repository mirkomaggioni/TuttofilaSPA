using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TuttofilaSPA.Core.BusinessObjects;
using TuttofilaSPA.Core.Models;

namespace TuttofilaSPA.Core.Services
{
	public class SportelloService
	{
		private readonly ConcurrentDictionary<string, List<ChiamataSportello>> ServiziChiamati = new ConcurrentDictionary<string, List<ChiamataSportello>>();
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
				channel.BasicPublish(exchange, routingKey, true, null, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new ChiamataSportello(servizio))));
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
						var chiamataSportello = JsonConvert.DeserializeObject<ChiamataSportello>(Encoding.UTF8.GetString(result.Body));
						var serviziChiamatiSala = ServiziChiamati.GetOrAdd(chiamataSportello.SalaId.ToString(), new List<ChiamataSportello>());
						serviziChiamatiSala.Add(chiamataSportello);
						ServiziChiamati.AddOrUpdate(chiamataSportello.SalaId.ToString(), serviziChiamatiSala, (_, __) => serviziChiamatiSala);
					}
					catch (Exception)
					{
						throw;
					}
				};

				channel.BasicConsume(queue, false, consumer);
			}
		}

		public List<ChiamataSportello> RestituisciServiziChiamati(Guid salaId)
		{
			var chiamateSportello = ServiziChiamati.GetOrAdd(salaId.ToString(), new List<ChiamataSportello>());
			ServiziChiamati.AddOrUpdate(salaId.ToString(), new List<ChiamataSportello>(), (_, __) => new List<ChiamataSportello>());
			return chiamateSportello.OrderByDescending(cs => cs.Data).ToList();
		}
	}
}
