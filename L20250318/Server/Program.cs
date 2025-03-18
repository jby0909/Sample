using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.26"), 4000);
            listenSocket.Bind(listenEndPoint);

            listenSocket.Listen(10);

            while(true)
            {
                Socket clientSocket = listenSocket.Accept();

                byte[] buffer = new byte[1024];
                int recvLength = clientSocket.Receive(buffer);

                Console.WriteLine(Encoding.UTF8.GetString(buffer));

                if(recvLength <= 0)
                {
                    break;
                }

                var recvJson = new JObject();
                recvJson.Add("message", "안녕하세요");
                if (Encoding.UTF8.GetString(buffer).CompareTo(recvJson.ToString()) == 0)
                {
                    var json = new JObject();
                    json.Add("message", "반가워요");
                    buffer = Encoding.UTF8.GetBytes(json.ToString());

                    int sendLength = clientSocket.Send(buffer);
                    if (sendLength <= 0)
                    {
                        break;
                    }
                }
                


                clientSocket.Close();
            }
            listenSocket.Close();
        }
    }
}
