using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private string errorString;


        public string ErrorString
        {
            get { return errorString; }
            set
            {
                errorString = value;
                NotifyPropertyChanged("ErrorString");
            }
        }

        private void NotifyPropertyChanged(string v)
        {
            throw new NotImplementedException();
        }

        public MyTelnetClient()
        {
            
            this.tcpclnt = new TcpClient();
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void connect(string ip, int port)
        {
            try
            {
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect(ip, port);

                Console.WriteLine("Connected");
                //tcpclnt.ReceiveTimeout = 10000;
                this.stream = tcpclnt.GetStream();

            }
            catch (SocketException er)
            {
                throw er;

                
            }


            //catch (ArgumentNullException e)
            //{
            //    Console.WriteLine("ArgumentNullException: {0}", e);
            //    ErrorString = "Exception: " + e;
            //}
            //catch (SocketException e)
            //{
            //    Console.WriteLine("SocketException: {0}", e);
            //    ErrorString = "Exception: " + e;
            //}

        }

        public void disconnect()
        {
            stream.Close();
            tcpclnt.Close();
        }

        public string read()
        {

            byte[] buffer = new byte[tcpclnt.ReceiveBufferSize];
            try
            {
                //---read incoming stream---
                int bytesRead = stream.Read(buffer, 0, tcpclnt.ReceiveBufferSize);

                //---convert the data received into a string---
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                return dataReceived;
            }
            catch(IOException e)
            {
                
                throw e;
            }
           
        }

        public void write(string command)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(command);




            try
            {
                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                //Console.WriteLine("Sent: {0}", command);
            }
            catch (IOException e)
            {
                throw e;

            }
            catch (NullReferenceException e)
            {
                throw e;

            }
        }


    }
}
