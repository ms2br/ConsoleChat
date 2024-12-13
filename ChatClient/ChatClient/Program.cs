using Org.BouncyCastle.Security;
using System.Text;

namespace ChatClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            client.CreateClientAsync().Wait();
        }
    }
}
