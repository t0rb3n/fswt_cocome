using Grpc.Core;

namespace CashDesk;

class Program
{
    static void Main(string[] args)
    {
        Channel channel = new Channel("127.0.0.1:30051", ChannelCredentials.Insecure);

        var client = new Enterprise.EnterpriseClient(channel);

        var reply = client.GetProductWithStockItem(new Barcode { Code = 1233 });
        Console.WriteLine("Greeting: " + reply.Message);

        channel.ShutdownAsync().Wait();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}