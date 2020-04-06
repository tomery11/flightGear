using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace flightGear.models
{
    class MyTelnetClient : ITelnetClient
    {
        TcpClient tcpclnt;
        NetworkStream stream;
        const int PORT_NO = 5402;
        const string SERVER_IP = "localhost";

        public MyTelnetClient()
        {
            Console.WriteLine("MytelnetClient constructor");
            this.tcpclnt = new TcpClient();
            //this.stream = tcpclnt.GetStream();
        }



        public void connect(string ip, int port)
        {
            try
            {
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect(SERVER_IP, PORT_NO);
                Console.WriteLine("Connected");
            }
            
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public void disconnect()
        {
            stream.Close();
            tcpclnt.Close();
        }

        public string read()
        {
            

            // Buffer to store the response bytes.
            Byte[] data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);
            return responseData;
        }

        public void write(string command)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(command);


            this.stream = tcpclnt.GetStream();

            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();



            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", command);
        }




    }
}
