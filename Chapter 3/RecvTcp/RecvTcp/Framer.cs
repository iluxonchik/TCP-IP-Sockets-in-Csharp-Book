using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framer
{
    public class Framer
    {
        public static byte[] nextToken(Stream input, byte[] delimiter)
        {
            int nextByte;

            // If the stream has already enede, return null
            if ((nextByte = input.ReadByte()) == -1)
            {
                return null;
            }

            MemoryStream tokenBuffer = new MemoryStream();

            do
            {
                tokenBuffer.WriteByte((byte)nextByte);
                byte[] currentToken = tokenBuffer.ToArray();

                if (endsWith(currentToken, delimiter))
                {
                    int tokenLength = currentToken.Length - delimiter.Length;
                    byte[] token = new byte[tokenLength];
                    Array.Copy(currentToken, 0, token, 0, tokenLength);
                    return token;
                }
            } while ((nextByte = input.ReadByte()) != -1);

            return tokenBuffer.ToArray();
        }

        private static bool endsWith(byte[] value, byte[] suffix)
        {
            if (value.Length < suffix.Length)
                return false;

            for (int offset = 1; offset <= suffix.Length; offset++)
            {
                if (value[value.Length - offset] != suffix[suffix.Length - offset])
                { 
                    return false;
                }
            }

            return true;
        }
    }
}
