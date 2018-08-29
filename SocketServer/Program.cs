using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace ConsoleApp1
{
    class Program
    {
        private static byte[] result = new byte[1024];
        private  static  Socket socket = null;
        private static int port = 8888;
        private static string ip = "127.0.0.1";
        static void Main(string[] args)
        {
            IPAddress iPAddress = IPAddress.Parse(ip);
            IPEndPoint endpoint = new IPEndPoint(iPAddress, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endpoint);
            socket.Listen(10);
            Console.WriteLine("启动监听{0}成功", socket.LocalEndPoint.ToString());


            Thread thread = new Thread(ClientConnectListen);
            thread.Start();

            
            Console.ReadLine();
        }

        private static void ClientConnectListen()
        {
            while (true)
            {
                Socket clientSocket= socket.Accept();

                Console.WriteLine("客户端{0}成功连接", clientSocket.RemoteEndPoint.ToString());

                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteString("Server connected");

                clientSocket.Send(WriteMessage(buffer.ToBytes()));

                Thread thread = new Thread(ReceiverClientMessage);
                thread.Start(clientSocket);
            }
        }

        private static void ReceiverClientMessage(object clientSocket)
        {
            Socket cs = (Socket)clientSocket;

            while (true)
            {
                try
                {
                    int len = cs.Receive(result);
                    if (len <= 0)
                    {
                        Console.WriteLine("客户端{0}已关闭！", cs.RemoteEndPoint.ToString());
                        break;
                    }
                    Console.WriteLine("接收客户端{0}消息， 长度为{1}", cs.RemoteEndPoint.ToString(), len);

                    ByteBuffer byteBuffer = new ByteBuffer(result);

                    int relen= byteBuffer.ReadShort();

                    Console.WriteLine(byteBuffer.ReadString());

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    cs.Shutdown(SocketShutdown.Both);
                    cs.Close();
                    break;
                }
            }
        }

        private static byte[] WriteMessage(byte[] p)
        {
            MemoryStream memory = null;

            using (memory = new MemoryStream())
            {
                memory.Position = 0;
                BinaryWriter writer = new BinaryWriter(memory);
                ushort l= (ushort) p.Length;

                writer.Write(l);
                writer.Write(p);
                writer.Flush();
                return memory.ToArray();
            }
        }
    }

   
}
