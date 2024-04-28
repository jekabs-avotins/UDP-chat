using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UDPServer
{
    private const int ListenPort = 11000;
    private static List<IPEndPoint> clientEndpoints = new List<IPEndPoint>();

    static void Main()
    {
        UdpClient listener = new UdpClient(ListenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, ListenPort);

        try
        {
            while (true)
            {
                Console.WriteLine("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

                // Register client if new
                if (!clientEndpoints.Contains(groupEP))
                {
                    clientEndpoints.Add(new IPEndPoint(groupEP.Address, groupEP.Port));
                    Console.WriteLine($"New client added: {groupEP}");
                }

                Console.WriteLine($"Received broadcast from {groupEP} :");
                Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");

                // Broadcast message to all known clients
                foreach (var clientEP in clientEndpoints)
                {
                    listener.Send(bytes, bytes.Length, clientEP);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            listener.Close();
        }
    }
}

