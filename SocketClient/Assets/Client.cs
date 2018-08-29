using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class Client  {

    Client c;

    private string ip = "127.0.0.1";
    private int port = 8888;
    private Socket socket;
    private static byte[] buffer= new byte[1024];
    private bool IsConnected;

    public Client()
    {

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void StarConnect()
    {
        IPAddress address = IPAddress.Parse(ip);
        IPEndPoint iPEndPoint = new IPEndPoint(address, port);
        try
        {
            socket.Connect(iPEndPoint);
            IsConnected = true;
            Console.WriteLine("服务器连接成功！");
        }
        catch
        {
           Console.WriteLine("服务器连接失败！");
            IsConnected = false;
            return;
        }

        socket.Receive(buffer);
        ByteBuffer byteBuffer = new ByteBuffer(buffer);
        byteBuffer.ReadShort();
        string s= byteBuffer.ReadString();


    }
    public void Send(string data)
    {
        if (!IsConnected) return;
        try
        {
            ByteBuffer byteBuffer = new ByteBuffer();
            byteBuffer.WriteString(data);

            socket.Send(WriteMessage(byteBuffer.ToBytes()));

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            IsConnected = false;
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

    }
    private  byte[] WriteMessage(byte[] p)
    {
        MemoryStream memory = null;

        using (memory = new MemoryStream())
        {
            memory.Position = 0;
            BinaryWriter writer = new BinaryWriter(memory);
            ushort l = (ushort)p.Length;

            writer.Write(l);
            writer.Write(p);
            writer.Flush();
            return memory.ToArray();
        }
    }

   
}
