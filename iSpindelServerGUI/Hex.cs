using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSpindelServerGUI
{
    public class ByteBuffPrinter
    {
        public void PrintAscii(byte[] buffer, char endChar)
        {
            Int32 i = 0;
            while (buffer[i] != (byte)endChar)
            {
                Console.Write((char)buffer[i++]);
            }
            Console.Write((char)buffer[i]);
            Console.WriteLine();
        }

        public string AsciiToString(byte[] buffer, char endChar)
        {
            Int32 i = 0;
            String result = "";
            while (buffer[i] != (byte)endChar)
            {
                result += ((char)buffer[i++]);
            }
            result += ((char)buffer[i++]);
            return result;
        }

        static public void PrintAscii(byte[] buffer, Int32 start, Int32 count)
        {
            for (Int32 i = start; i < count + start; i++)
            {
                Console.Write((char)buffer[i]);
            }
            Console.WriteLine();
        }

        static public void PrintHex(byte[] buffer, Int32 start, Int32 count, bool addPrefix = true)
        {
            for (Int32 i = start; i < count + start; i++)
            {
                Console.Write(Byte2Hex(buffer[i], addPrefix));
                if (i < count + start - 1)
                {
                    Console.Write(", ");
                }
            }
            Console.WriteLine();
        }

        static public String Byte2Hex(byte b, bool addPrefix = true)
        {
            String chars = "0123456789abcdef";
            String result = "0x";
            result += chars[(b & 0xf0) >> 4];
            result += chars[b & 0x0f];
            return result;
        }
    }
}
