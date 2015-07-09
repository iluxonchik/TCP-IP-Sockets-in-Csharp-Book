using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemQuote
{
    public class ItemQuoteBinConst
    {
        public static readonly string DEFAULT_CHAR_ENC = "ascii";
        public static readonly byte DISCOUNT_FLAG = 1 << 7;
        public static readonly byte IN_STOCK_FLAG = 1 << 0;
        public static readonly int MAX_DESC_LEN = 255;
        public static readonly int MAX_WIRE_LEN = 1024;
    }
}
