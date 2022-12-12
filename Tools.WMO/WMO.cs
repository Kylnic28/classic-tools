using ClassicTools.Tools;

namespace Tools.WMO
{
    public class WMO
    {
        private string filename;
        private BinaryHelper io;
        public WMO(string fileName)
        {
            filename = fileName;
            io = new BinaryHelper(fileName);
        }

        /// <summary>
        /// Fix water for Vanilla WMO.
        /// </summary>
        public void ApplyFixes()
        {
            #region Water fixes

            uint mogpPosition;

            if (io.HasChunk("PGOM", out mogpPosition))
            {
                Console.WriteLine("(Chunk=MOGP) Applying fixes ...");

                io.Position = 0x48;
                io.Writer.Write(0x0F); // 15 
            }
            #endregion

            #region Render fixes

            uint mobaPosition;
            if (io.HasChunk("ABOM", out mobaPosition))
            {
                Console.WriteLine("(Chunk=MOBA) Applying fixes ...");

                io.Position = mobaPosition + 4;
                uint data = io.Reader.ReadUInt32() / 24;
                io.Position = 0x40;
                io.Writer.Write(data);

            }
            #endregion

            #region Textures Fixes

            uint momtPosition;
            if (io.HasChunk("TMOM", out momtPosition))
            {
                Console.WriteLine("(Chunk=MOMT) Applying fixes ...");

                io.Position = momtPosition + 0x04; // chunk size

                uint count = io.Reader.ReadUInt32() / 0x40; // chunk content

                for (int i = 0; i < count; i++)
                {
                    io.Position = momtPosition + 8;

                    if (io.Reader.ReadUInt32() >= 0x06) // Classic and TBC supports only values lower or equal than 5.
                        io.Writer.Write(0);

                    io.Position += 0x38;
                }


            }

            #endregion

            #region Lightning fixes

            // TODO

            #endregion

        }
    }
}

