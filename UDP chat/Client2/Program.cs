using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class UDPClient
{
    private const int ListenPort = 11000;
    private const string ServerIP = "127.0.0.1";  // Server IP Address

    static void Main()
    {
        UdpClient client = new UdpClient();
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ServerIP), ListenPort); // endpoint where server is listening
        client.Connect(ep);

        // Start the thread for receiving messages
        Thread receiveThread = new Thread(() => ReceiveMessages(client));
        receiveThread.Start();

        // Send messages to the server
        try
        {
            while (true)
            {
                string message = Console.ReadLine();
                byte[] bytes = Encoding.ASCII.GetBytes(message);
                client.Send(bytes, bytes.Length);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
            {
            client.Close();
        }
    }

    static void ReceiveMessages(UdpClient client)
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        while (true)
        {
            try
            {
                byte[] data = client.Receive(ref remoteEP);
                string message = Encoding.ASCII.GetString(data);
                Console.WriteLine($"Received: {message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
        }
    }
}

