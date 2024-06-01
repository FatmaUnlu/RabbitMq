//Bağlantı Oluşturma
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;



#region Genel Consumer
ConnectionFactory factory = new();
factory.Uri = new("amqps://rafuibco:VSV6fIH1HgIqGwkfSM-qaAQy5EJCmtBz@moose.rmq.cloudamqp.com/rafuibco");

//Bağlantı Aktifleştime ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();
//Queue Oluturma
//channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true, autoDelete: false,
//    arguments: null); //cunsomerda da kuyruk publisherdaki ile birebir aynı yapılandırmada tanımlanmalıdır.

////Queudan mesaj okuma
//EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
//channel.BasicConsume(queue: "example-queue", autoAck: false, consumer);//2.parametre autoAck: kuyruktan mesaj alındığında o mesajın kuyruktan silinip silinmeyeeğini belirtir.

//consumer.Received += (sender, e) =>
//{
//    //kuyruğa gelen mesaın işlendiği yerdir.
//    //e.body:kuyruktakii mesajın verisinibütünsel olarak getirir.
//    //e.Body.ToArray() veya e.Body.Span kuyruktaki mesajın byte verisini getirir.
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

//    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false); //deliveryTag=>bildirimde bulunacağımız mesaja dair unique bir değeri ifade eder. multiple:false => sadece bu mesaja dair bir onay bildiriminde bulunur.
//                                                                   // channel.BasicAck(deliveryTag:e.DeliveryTag,true); //multiple:true => kendinden önceki ce şu anki mesajları kapsayan bir onay bildiriminde bulunur.

//    //channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: true); //requeue:true => mesaj gönderiminde hata olması durumunda bu mesajın tekrar kuyruğa eklenmesini sağlar

//};

#endregion


#region Direct Exchange Consumer
//ConnectionFactory factory = new ConnectionFactory();
//factory.Uri = new("amqps://owgeouwz:f5iOLQ2cnqyQajvjJqHzDtiOX4Uj1pQw@fish.rmq.cloudamqp.com/owgeouwz");

//using IConnection connection = factory.CreateConnection();
//using IModel channel = connection.CreateModel();
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

//publisher tarafından routing key de bulunan değerdeki kuyruğa gönderilen mesajları kendi oluşturdığumuz kuyruğabyönelndirerek tüketmemiz gerekmektedir. bunun iiçin öncelikle bir kuyruk oluşturulmalıdır. 
var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
    queue: queueName,
     exchange: "direct-exchange-example",
        routingKey: "direct-queue"
    );

EventingBasicConsumer eventingBasicConsumer = new(channel);

channel.BasicConsume(
    queue:queueName,
    autoAck: true, 
    consumer: eventingBasicConsumer);

eventingBasicConsumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
    //channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false); //deliveryTag=>bildirimde bulunacağımız mesaja dair unique bir değeri ifade eder. multiple:false => sadece bu mesaja dair bir onay bildiriminde bulunur.
};
#endregion

Console.Read();