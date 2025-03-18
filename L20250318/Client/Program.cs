using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var json = new JObject();
            json.Add("message" , "안녕하세요");

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.26"), 4000);

            serverSocket.Connect(serverEndPoint);

            byte[] buffer;

            
            buffer = Encoding.UTF8.GetBytes(json.ToString());

            
            int SendLength = serverSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);

            
            byte[] buffer2 = new byte[1024];
            
            int recvLength = serverSocket.Receive(buffer2);

            Console.WriteLine(Encoding.UTF8.GetString(buffer2));
            Console.ReadKey();

            serverSocket.Close();

        }
    }
}
