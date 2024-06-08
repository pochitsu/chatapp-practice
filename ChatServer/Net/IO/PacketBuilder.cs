using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;


namespace Chat_app.Net.IO
{
    class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }
        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg)
        {
            byte[] buff = BitConverter.GetBytes(msg.Length);
            var msgLength = msg.Length;
            _ms.Write(buff, 0, buff.Length);
            _ms.Write(Encoding.ASCII.GetBytes(msg), 0, msg.Length);
        }

        public byte[] GetPacketByte()
        {
            return _ms.ToArray();
        }
    }
}
