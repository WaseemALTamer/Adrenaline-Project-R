using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private TcpListener tcpListener;
        private static List<TcpClient> clients = new List<TcpClient>();

        public string[] GamesIPAndPort;
        public string[] GamesAvailableName;

        public void Start(){
            GamesIPAndPort = new string[1024];
            
            //Testers
            //GamesIPAndPort[0] = "192.168.1.173:5005";
            //GamesIPAndPort[1] = "192.168.1.173:5001";
            //GamesIPAndPort[2] = "192.168.1.173:5002";
            //GamesIPAndPort[3] = "192.168.1.173:5003";

            tcpListener = new TcpListener(IPAddress.Any, 3003);
            tcpListener.Start();
            Console.WriteLine("TCP Server is listening on port " + 3003);
            System.Threading.Thread thread = new System.Threading.Thread(AcceptClients);
            thread.Start();
            while (true) {
                Thread.Sleep(5000);
                CheckGames();
            }
        }
        void CheckGames() {
            for (int i = 0; i < GamesIPAndPort.Length; i++)
            {
                if (GamesIPAndPort[i] != null && GamesIPAndPort[i] != "Local") {
                    Console.WriteLine("Once");
                    string[] parts = GamesIPAndPort[i].Split(':');
                    try
                    {
                        using (TcpClient client = new TcpClient()){
                            Console.WriteLine(parts[0] + ":" +int.Parse(parts[1]));
                            client.Connect(parts[0], int.Parse(parts[1]));
                            Console.WriteLine($"IP Address {GamesIPAndPort[i]} is reachable on port {GamesIPAndPort[i]}");
                            client.Close();
                        }
                    }
                    catch (Exception e){
                        //needs more information for it to do that but eh lets keep it to this for now
                        if (GamesIPAndPort[i] != "Local"){
                            GamesIPAndPort[i] = null;
                        }
                    }
                }
            }
        }





        async void AcceptClients(){
            while (true){
                TcpClient client = await tcpListener.AcceptTcpClientAsync();
                clients.Add(client);

                _ = HandleClientAsync(client);
            }
        }

        async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream()){
                    byte[] data = Encoding.UTF8.GetBytes(GetGamesMessage());
                    await stream.WriteAsync(data, 0, data.Length);
                    Console.WriteLine("Sent message to client: " + GetGamesMessage());
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0){
                        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Received message from client: " + receivedMessage);
                        
                        
                        //i need to increase the recived message time proccessing so it does not overflow the server with sending too many messages at the same time;
                        if (receivedMessage.Length > 1024) {
                            receivedMessage = "";
                        }


                        // Grap Clinet message and decode it with excution to it needs more commands later on for game rooms;
                        if (receivedMessage == "Request") {
                            data = Encoding.UTF8.GetBytes(GetGamesMessage());
                            await stream.WriteAsync(data, 0, data.Length);
                            Console.WriteLine("Sent message to client: " + GetGamesMessage());
                        }

                        if (receivedMessage.Substring(0, Math.Min(receivedMessage.Length, 6)) == "Store:") {
                            StoreGame(receivedMessage.Substring(6));
                            Console.WriteLine($"Stored ({receivedMessage.Substring(6)})");
                        }
                    }
                }
            }
            catch (Exception ex){
                Console.WriteLine("Error handling client: " + ex.Message);
            }
            finally
            {
                client.Close();
                clients.Remove(client);
            }
        }

        string GetGamesMessage(){
            string message = "@";
            for (int i = 0; i < GamesIPAndPort.Length; i++){
                if (GamesIPAndPort[i] != null){
                    message += ($"{GamesIPAndPort[i]}@");
                }
            }
            return message.ToString();
        }

        void StoreGame(string IPAndPort){
            for (int i = 0; i < GamesIPAndPort.Length; i++){
                if (GamesIPAndPort[i] == null){
                    GamesIPAndPort[i] = IPAndPort;
                    return;
                }
            }
        }

        class Program{
            static void Main(string[] args){
                Server server = new Server();
                server.Start();
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}