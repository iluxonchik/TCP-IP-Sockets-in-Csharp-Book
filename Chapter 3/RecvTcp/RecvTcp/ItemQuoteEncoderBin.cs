using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ItemQuote
{
    class ItemQuoteEncoderBin : ItemQuoteEncoder
    {
        public Encoding encoding;

        public ItemQuoteEncoderBin() : this(ItemQuoteBinConst.DEFAULT_CHAR_ENC) { }

        public ItemQuoteEncoderBin(string encodingDesc)
        {
            encoding = Encoding.GetEncoding(encodingDesc);
        }

        public byte[] encode(ItemQuote item)
        {
            MemoryStream mem = new MemoryStream();
            BinaryWriter output = new BinaryWriter(new BufferedStream(mem));

            output.Write(IPAddress.HostToNetworkOrder(item.ItemNumber));
            output.Write(IPAddress.HostToNetworkOrder(item.Quantity));
            output.Write(IPAddress.HostToNetworkOrder(item.UnitPrice));

            byte flags = 0;
            if (item.Discounted)
                flags |= ItemQuoteBinConst.DISCOUNT_FLAG;
            if (item.InStock)
                flags |= ItemQuoteBinConst.IN_STOCK_FLAG;
            output.Write(flags);

            byte[] encodedDesc = encoding.GetBytes(item.ItemDescription);
            if (encodedDesc.Length > ItemQuoteBinConst.MAX_DESC_LEN)
                throw new IOException("Item description too long.");
            output.Write((byte)encodedDesc.Length);
            output.Write(encodedDesc);

            output.Flush();

            return mem.ToArray();
        }
    }
}
