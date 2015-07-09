using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framer;

namespace ItemQuote
{
    class ItemQuoteDecoderText : ItemQuoteDecoder
    {
        private Encoding encoding;
        public ItemQuoteDecoderText(string encodingDesc)
        {
            encoding = Encoding.GetEncoding(encodingDesc);
        }

        public ItemQuoteDecoderText() : this(ItemQuoteTextConst.DEFAULT_CHAR_ENC){ }

        public ItemQuote decode(byte[] packet)
        {
            Stream payload = new MemoryStream(packet, 0, packet.Length, false);
            return decode(payload);
        }

        public ItemQuote decode(Stream wire)
        {
            string itemNo, desc, qt, price, flags;

            byte[] space = encoding.GetBytes(" ");
            byte[] newLine = encoding.GetBytes("\n");

            itemNo = encoding.GetString(Framer.Framer.nextToken(wire, space));
            desc = encoding.GetString(Framer.Framer.nextToken(wire, newLine));
            qt = encoding.GetString(Framer.Framer.nextToken(wire, space));
            price = encoding.GetString(Framer.Framer.nextToken(wire, space));
            flags = encoding.GetString(Framer.Framer.nextToken(wire, newLine));

            return new ItemQuote
            {
                ItemNumber = Int64.Parse(itemNo),
                ItemDescription = desc,
                Quantity = Int32.Parse(qt),
                UnitPrice = Int32.Parse(price),
                Discounted = (flags.IndexOf('d') != -1),
                InStock = (flags.IndexOf('s') != -1)
            };
        }
    }
}
