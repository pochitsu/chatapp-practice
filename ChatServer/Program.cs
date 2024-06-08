using Chat_app.Net.IO;
using ChatServer.Net.IO;
using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace ChatServer
{
    class Program
    {
        static List<Client> _user;
        static TcpListener _listener;
        static void Main(string[] args)
        {
            _user = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _user.Add(client);

                //Broadcast the connection to everyone on the server.
                BroadcastConnection();
            }

        }
        static void BroadcastConnection()
        {
            foreach (var user in _user)
            {
                foreach (var usr in _user)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(usr.Username);
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketByte());
                }
            }
        }
        public static void BroadcastMessage(string message)
        {
            foreach (var user in _user)
            { 
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketByte());
            }
        } public static void BroadcastDisconnected(string uid)
        {
            var disconnnectedUser = _user.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            _user.Remove(disconnnectedUser);
            foreach (var user in _user)
            { 
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketByte());
              
            }

            BroadcastMessage($"{disconnnectedUser.Username} disconnected.");
        }
    }
}
