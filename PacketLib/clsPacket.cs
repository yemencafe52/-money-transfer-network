using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLib
{

    public enum CommandType : byte
    {
        Command1 =1,
        Command2,
        Command3,
        Command4,
        Command5,
        Command6,
        Command7,
        Command8,
        Command9,
        Command10
    }

    public class Packet
    {
        CommandType cmd;
        byte[] buffer;

        public Packet(CommandType cmd)
        {
            this.cmd = cmd;
            this.buffer = new byte[0];
        }

        public Packet(CommandType cmd,byte[] ar)
        {
            this.cmd = cmd;
            this.buffer = new byte[ar.Length];
            Array.Copy(ar, 0, this.buffer, 0, ar.Length);
        }

        public Packet (byte[] ar)
        {
            this.cmd = (CommandType)ar[0];
            this.buffer = new byte[ar.Length-1];
            Array.Copy(ar, 1, this.buffer, 0, ar.Length-1);
        }

        public byte[] ToBytes()
        {
            byte[] res = new byte[this.buffer.Length + 1];
            res[0] = (byte)this.cmd;
         
            Array.Copy(this.buffer, 0, res, 1, this.buffer.Length);

            return res;
        }

        public CommandType GetCommand()
        {
            return this.cmd;
        }

        public byte[] GetBuffer()
        {
            return this.buffer;
        }

    }
}
