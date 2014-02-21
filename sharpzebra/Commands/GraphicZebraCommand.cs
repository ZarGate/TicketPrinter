using System.IO;

namespace Com.SharpZebra.Commands
{
    public class GraphicZebraCommand : IZebraCommand
    {
        private static readonly string PRINTER_IMAGE_REFERENCE = "AN_IMAGE";
        private readonly string deleteStoreAndPrintCommand;


        private static char[] FromBytesToChars(byte[] fileContents)
        {
            char[] bytesAsChar = new char[fileContents.Length];
            for (int i = 0; i < fileContents.Length; i++)
            {
                bytesAsChar[i] = (char)fileContents[i];
            }

            return bytesAsChar;
        }

        private static string ImageFrom(string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open);
            string fileContents = ImageFrom(stream);
            stream.Close();
            return fileContents;
        }

        public GraphicZebraCommand(Stream fileStream, int xPositionInDots, int yPositionInDots) : this(xPositionInDots, yPositionInDots, ImageFrom(fileStream))
        {
            
        }

        /* Note that we can't simply read in the string from the file. We need to read it in byte by byte
         * converting it into a char, that is then sent to the printer as a string. Without this, the 
         * zebra printer does not appear to be able to print the graphic at all.
         */
        private static string ImageFrom(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            byte[] fileContents = binaryReader.ReadBytes((int)stream.Length);
            binaryReader.Close();
            return new string(FromBytesToChars(fileContents));
        }

        public GraphicZebraCommand(string filename, int xPositionInDots, int yPositionInDots) : this(xPositionInDots, yPositionInDots, ImageFrom(filename))
        {
        }

        private GraphicZebraCommand(int xPositionInDots, int yPositionInDots, string pcxFileReadInAsBytesButRepresentedAsAString)
        {
            deleteStoreAndPrintCommand =
                string.Format("GK\"{0}\"\nGM\"{0}\"{3}\n{4}\nGG{1},{2},\"{0}\"", PRINTER_IMAGE_REFERENCE,
                              xPositionInDots, yPositionInDots, pcxFileReadInAsBytesButRepresentedAsAString.Length,
                              pcxFileReadInAsBytesButRepresentedAsAString);

        }

        public override string ToString()
        {
            return deleteStoreAndPrintCommand;
        }

        public string ToZebraInstruction()
        {
            return ToString();
        }
    }
}
