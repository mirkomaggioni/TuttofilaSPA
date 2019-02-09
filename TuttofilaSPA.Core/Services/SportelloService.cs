using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using TuttofilaSPA.Core.Models;

namespace TuttofilaSPA.Core.Services
{
	public class SportelloService
	{
		private readonly ConnectionFactory _connectionFactory;
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

		public SportelloService(ConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public void ChiamaServizio(Servizio servizio)
		{
			using (var connection = _connectionFactory.CreateConnection(AmqpTcpEndpoints))
			using (var channel = connection.CreateModel())
			{
				channel.ExchangeDeclare(exchange, "direct", false, false, null);
				channel.BasicPublish(exchange, routingKey, true, null, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(servizio)));
			}
		}
	}
}
