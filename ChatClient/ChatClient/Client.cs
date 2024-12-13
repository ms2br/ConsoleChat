using Org.BouncyCastle.Security;
using System;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    public class Client
    {
        public Encryption Encryption { get; set; } = new Encryption();

        public async Task CreateClientAsync()
        {
            TcpClient client = new TcpClient("127.0.0.1", 13000);
            NetworkStream stream = client.GetStream();
            string message;
            Task.Run(() => ReceiveDataAsync(client));
            while (!string.IsNullOrWhiteSpace(message = Console.ReadLine()))
            {
                byte[] buffer = Encryption.CombineEncryptMessage(message);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            client.Close();
        }

        public async Task ReceiveDataAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];
            int bytesRead;
            try
            {
                while ((bytesRead = stream.ReadAsync(buffer, 0, buffer.Length).Result) != 0)
                {                    
                    string message = Encoding.ASCII.GetString(Encryption.DistributedEncryptMessage(ExtractNonZeroBytes(buffer)));
                    await Console.Out.WriteLineAsync($"Message : {message}");
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        public byte[] ExtractNonZeroBytes(byte[] buffer)
        {
            Array.Reverse(buffer);
            var index = Array.FindIndex(buffer, x => x > 0);
            byte[] purifiedBuffer = new byte[buffer.Length - index];
            Array.Copy(buffer, index, purifiedBuffer, 0, purifiedBuffer.Length);
            Array.Reverse(purifiedBuffer);
            return purifiedBuffer;
        }
    }
}
