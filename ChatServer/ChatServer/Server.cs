using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    internal class Server
    {
        static ConcurrentBag<TcpClient> clients = new();

        public async Task CreateServerAsync()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 13000);
            tcpListener.Start();
            Console.WriteLine("Sunucu Dinlemeye Basladi");

            while (true)
            {
                TcpClient tcpClient = await tcpListener.
                    AcceptTcpClientAsync();
                clients.Add(tcpClient);
                Console.WriteLine("Yeni bir istemci bağlandı.");
                Task.Run(() => HandleClientAsync(tcpClient));
            }

        }


        public async Task HandleClientAsync(TcpClient tcp)
        {
            NetworkStream stream = tcp.GetStream();
            byte[] buffer = new byte[256];
            int bytesRead;

            try
            {
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    await Console.Out.WriteLineAsync($"Alinan : {message}");
                    Task.Run(() => MessageSharerAsync(tcp, buffer));
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"{ex.Message}");
            }
            finally
            {
                clients.TryTake(out tcp);
                tcp.Close();
                stream.Close();
            }
        }

        public async Task MessageSharerAsync(TcpClient tcp, byte[] buffer)
        {
            foreach (TcpClient client in clients)
            {
                if (client == tcp)
                    continue;
                NetworkStream stream = client.GetStream();
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}
