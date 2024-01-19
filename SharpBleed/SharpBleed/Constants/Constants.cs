using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBleed.Constants
{
    internal static class Packets
    {
        public const string helloHex = @"
        16 03 01 00 6e 01 00 00 6a 03 02 31 cc 98
        c3 c0 de ed 93 77 42 e8 71 e4 87 ed 26 3a 44 20
        ea ac 0e c6 59 c4 7e ba d8 bd 1b 68 da 00 00 12
        c0 0a c0 14 c0 09 c0 13 00 39 00 33 00 35 00 2f
        00 ff 01 00 00 2f 00 0b 00 04 03 00 01 02 00 0a
        00 0c 00 0a 00 1d 00 17 00 1e 00 19 00 18 00 10
        00 0b 00 09 08 68 74 74 70 2f 31 2e 31 00 16 00
        00 00 17 00 00";

        public const string heartBeatHex = @"
        18 03 02 00 03
        01 40 00";
    }
}
