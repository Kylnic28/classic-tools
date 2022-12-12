using System.Runtime.CompilerServices;
using System.Text;

namespace ClassicTools.Tools
{
    public class BinaryHelper
    {
        private FileStream stream;

        private BinaryReader reader;

        private BinaryWriter writer;

        private byte[] buffer;
        public FileStream Stream { get => stream; }
        public BinaryReader Reader { get => reader; set => reader = value; }
        public BinaryWriter Writer { get => writer; set => writer = value; }
        public uint Position { get => (uint)stream.Position; set => stream.Position = value; }

        public BinaryHelper(string fileName)
        {
            buffer = File.ReadAllBytes(fileName);
            stream = new(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }

        private unsafe long SearchPattern(byte[] haystack, byte[] needle)
        {
            fixed (byte* h = haystack) fixed (byte* n = needle)
            {
                for (byte* hNext = h, hEnd = h + haystack.Length + 1 - needle.Length, nEnd = n + needle.Length; hNext < hEnd; hNext++)
                    for (byte* hInc = hNext, nInc = n; *nInc == *hInc; hInc++)
                        if (++nInc == nEnd)
                            return hNext - h;
                return -1;
            }
        }

        public bool HasChunk(string chunkName, out uint position)
        {
            position = 0;

            if (Encoding.ASCII.GetString(buffer).Contains(chunkName))
            {
                position = (uint)SearchPattern(buffer, Encoding.ASCII.GetBytes(chunkName));
                return true;
            }
            else
                return false;
   
        }


    }
}
