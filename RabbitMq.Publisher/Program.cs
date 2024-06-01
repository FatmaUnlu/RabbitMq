using RabbitMQ.Client;
using System.Text;




#region Genel Publisher
//Bağlantı Oluşturma

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://rafuibco:VSV6fIH1HgIqGwkfSM-qaAQy5EJCmtBz@moose.rmq.cloudamqp.com/rafuibco");

//Bağlantıyı aktifleştirme ve kanal açma

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();
////Queue Oluşturma
//channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true, autoDelete: false,
//    arguments: null); // exclusive: birden fazla channel tarafından bu kuyrukta işlem yaabiliyor olacaka mesaj gönderme. durable:true => kuyruğu kalıcı hale getirmek için(rabbitmq sunucusunda bir sorun olmasına karşın)

////Queue ya mesaj gönderme
////rabbitmq kuyruğa atacağı mesajları byte türünden kabul etmektedir.

////byte[] message = Encoding.UTF8.GetBytes("Merhaba");
////channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message); // default exchang e(exchange:"") direct exchange' e karşılık gelir.


//IBasicProperties properties = channel.CreateBasicProperties();
//properties.Persistent = true; //mesajları kalıcı hale getirmek için(rabbitmq sunucusunda bir sorun olmasına karşın)

//for (int i = 0; i < 100; i++)
//{
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba");
//    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties: properties);
//}

#endregion

#region Direct Exchange Publisher

//ConnectionFactory factory = new ConnectionFactory();
//factory.Uri = new("amqps://owgeouwz:f5iOLQ2cnqyQajvjJqHzDtiOX4Uj1pQw@fish.rmq.cloudamqp.com/owgeouwz");

//using IConnection connection = factory.CreateConnection();
//using IModel channel = connection.CreateModel();
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
    Console.Write("Mesaj : ");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
        exchange: "direct-exchange-example",
        routingKey: "direct-queue",
        body: byteMessage
        );
}

#endregion
Console.Read();
