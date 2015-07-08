using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ItemQuote
{
    public class ItemQuoteDecoderBin : ItemQuoteDecoder
    {
        public Encoding encoding;

        public ItemQuoteDecoderBin() : this(ItemQuoteBinConst.DEFAULT_CHAR_ENC) { }

        public ItemQuoteDecoderBin(string encodingDesc)
        {
            encoding = Encoding.GetEncoding(encodingDesc);
        }
        public ItemQuote decode(byte[] packet)
        {
            Stream payload = new MemoryStream(packet, 0, packet.Length, false);
            return decode(payload);
        }

        public ItemQuote decode(Stream wire)
        {
            BinaryReader src = new BinaryReader(new BufferedStream(wire));

            long itemNum = IPAddress.NetworkToHostOrder(src.ReadInt64());
            int qt = IPAddress.NetworkToHostOrder(src.ReadInt32());
            int unitPrice = IPAddress.NetworkToHostOrder(src.ReadInt32());
            byte flags = src.ReadByte();

            int strLen = src.Read();
            if (strLen == -1)
            {
                throw new EndOfStreamException("EOS");
            }
            byte[] strBuff = new byte[strLen];
            src.Read(strBuff, 0, strLen);
            string itemDesc = encoding.GetString(strBuff);

            return new ItemQuote(itemNum, itemDesc, qt, unitPrice, ((flags & ItemQuoteBinConst.DISCOUNT_FLAG) == ItemQuoteBinConst.DISCOUNT_FLAG),
                ((flags & ItemQuoteBinConst.IN_STOCK_FLAG) == ItemQuoteBinConst.IN_STOCK_FLAG));
        }
    }
}
