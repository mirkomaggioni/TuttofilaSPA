using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using TuttofilaSPA.Core.BusinessObjects;
using TuttofilaSPA.Core.Models;

namespace TuttofilaSPA.Core.Services
{
	public class SportelloService
	{
		public ConcurrentDictionary<string, List<ChiamataSportello>> ServiziChiamati = new ConcurrentDictionary<string, List<ChiamataSportello>>();

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
		IConnection Connection;
		IModel Channel;
		private readonly string exchange = "corsoAngular.tuttofila";
		private readonly string routingKey = "chiamate";
		private readonly string queue = $"corsoAngular.{Dns.GetHostName()}.tuttofila.chiamate";

		public SportelloService(ConnectionFactory connectionFactory)
		{
			ConnectionFactory = connectionFactory;
			Connection = ConnectionFactory.CreateConnection(AmqpTcpEndpoints);
			Channel = Connection.CreateModel();
			Consuma();
		}

		public void Pubblica(Servizio servizio)
		{
			Channel.ExchangeDeclare(exchange, "direct", false, false, null);
			Channel.BasicPublish(exchange, routingKey, true, null, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new ChiamataSportello(servizio))));
		}

		public void Consuma()
		{
			Channel.ExchangeDeclare(exchange, "direct", false, false);
			Channel.QueueDeclare(queue, false, false, false, null);
			Channel.QueueBind(queue, exchange, routingKey);
			var consumer = new EventingBasicConsumer(Channel);
			consumer.Received += (model, result) =>
			{
				try
				{
					Channel.BasicAck(result.DeliveryTag, false);
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

			Channel.BasicConsume(queue, false, consumer);
		}

		public List<ChiamataSportello> RestituisciServiziChiamati(Guid salaId)
		{
			var chiamateSportello = ServiziChiamati.GetOrAdd(salaId.ToString(), new List<ChiamataSportello>());
			ServiziChiamati.AddOrUpdate(salaId.ToString(), new List<ChiamataSportello>(), (_, __) => new List<ChiamataSportello>());
			return chiamateSportello.OrderByDescending(cs => cs.Data).ToList();
		}
	}
}
