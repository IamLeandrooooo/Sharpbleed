using System.Net.Sockets;
using SharpBleed.Constants;

namespace SharpBleed.Helper
{
    internal static class HeartbleedHelper
    {
        public static byte[] ReceiveBytes(TcpClient client, int count)
        {
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[count];
            int bytesRead = 0;
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                bytesRead = stream.Read(buffer, totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    throw new EndOfStreamException("End of stream reached.");
                }
                totalBytesRead += bytesRead;
            }

            return buffer;
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "").Replace("\n", "").Replace("\r", "");

            if (hexString.Length % 2 != 0)
            {
                hexString = "0" + hexString;
            }

            byte[] binaryData = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                string hexPair = hexString.Substring(i, 2);
                byte byteValue = Convert.ToByte(hexPair, 16);
                binaryData[i / 2] = byteValue;
            }

            return binaryData;
        }

        public static void HexDump(byte[] s, string outputPath)
        {
            using (StreamWriter fileStream = new StreamWriter(outputPath))
            {
                for (int b = 0; b < s.Length; b += 16)
                {
                    byte[] lin = new byte[Math.Min(16, s.Length - b)];
                    Array.Copy(s, b, lin, 0, lin.Length);

                    string hxdat = string.Join(' ', lin.Select(c => $"{c:X2}"));
                    string pdat = new string(lin.Select(c => (c >= 32 && c <= 126) ? (char)c : '.').ToArray());

                    Console.WriteLine($"{b:X4}: {hxdat,-48} {pdat}");
                    fileStream.WriteLine($"{b:X4}: {hxdat,-48} {pdat}");
                }
            }

            Console.WriteLine();
        }

        public static (byte, ushort, ushort) UnpackHeader(byte[] header)
        {
            byte content_type = header[0];
            ushort version = BitConverter.ToUInt16(new byte[] { header[2], header[1] }, 0); // Convert to little-endian
            ushort length = BitConverter.ToUInt16(new byte[] { header[4], header[3] }, 0); // Convert to little-endian
            return (content_type, version, length);
        }

    }
}
