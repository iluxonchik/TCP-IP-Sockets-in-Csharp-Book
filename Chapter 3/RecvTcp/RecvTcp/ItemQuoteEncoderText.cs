using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemQuote
{
    public class ItemQuoteEncoderText : ItemQuoteEncoder
    {
        public Encoding encoding; // character encoding

        public ItemQuoteEncoderText() : this(ItemQuoteTextConst.DEFAULT_CHAR_ENC) { }

        public ItemQuoteEncoderText(string encodingDesc)
        {
            this.encoding = Encoding.GetEncoding(encodingDesc);
        }
        public byte[] encode(ItemQuote item)
        {
            string encodedStr = item.ItemNumber + " ";
            if (item.ItemDescription.IndexOf('\n') != -1)
                throw new IOException("Invalid description: contains newline.");
            encodedStr += item.ItemDescription + " ";
            encodedStr += item.Quantity + " ";
            encodedStr += item.UnitPrice + " ";

            if (item.Discounted)
                encodedStr += "d";
            if (item.InStock)
                encodedStr += "s";
            encodedStr += "\n";


            if (encodedStr.Length > ItemQuoteTextConst.MAX_WIRE_LENGTH)
                throw new IOException("Encoded length too long. MAX ALLOWED: {0}", ItemQuoteTextConst.MAX_WIRE_LENGTH);

            return Encoding.ASCII.GetBytes(encodedStr);

        }
    }
}
