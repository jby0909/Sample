using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Message
    {
        public string message;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, 4000);

            listenSocket.Bind(listenEndPoint);

            listenSocket.Listen(10);

            Socket clientSocket = listenSocket.Accept();

            //[][] [][][][][][]

            //패킷 길이 받기(header)
            byte[] headerBuffer = new byte[2];
            int RecvLength = clientSocket.Receive(headerBuffer, 2, SocketFlags.None);
            //헤더에서 byte로 받아온 '길이'에 대한 정보를 short타입으로 변환
            short packetlength = BitConverter.ToInt16(headerBuffer, 0);
            //헤더에 '길이'라는 데이터는 숫자이기 때문에 NetWorkOrder로 byte order를 바꿔서 받아야 함
            packetlength = IPAddress.NetworkToHostOrder(packetlength);


            //[][][][][]
            //실제 패킷(header 길이 만큼)
            byte[] dataBuffer = new byte[4096];
            RecvLength = clientSocket.Receive(dataBuffer, packetlength, SocketFlags.None);

            string JsonString = Encoding.UTF8.GetString(dataBuffer);

            Console.WriteLine(JsonString);

            //Custom 패킷 만들기
            //다시 전송 메세지
            string message = "{ \"message\" : \"클라이언트 받고 서버꺼 추가.\"}";
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
            //'길이'라는 자료는 ushort이므로 NetworkOrder로 byte order를 바꿔서 보내야 함
            ushort length = (ushort)IPAddress.HostToNetworkOrder((short)messageBuffer.Length);

            //길이  자료
            //[][] [][][][][][][][]
            headerBuffer = BitConverter.GetBytes(length);

            //[][][][][][][][][][][]
            byte[] packetBuffer = new byte[headerBuffer.Length + messageBuffer.Length];

            Buffer.BlockCopy(headerBuffer, 0, packetBuffer, 0, headerBuffer.Length);
            Buffer.BlockCopy(messageBuffer, 0, packetBuffer, headerBuffer.Length, messageBuffer.Length);

            int SendLength = clientSocket.Send(packetBuffer, packetBuffer.Length, SocketFlags.None);


            clientSocket.Close();
            listenSocket.Close();
        }
    }
    }
}
