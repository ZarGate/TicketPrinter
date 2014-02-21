namespace Com.SharpZebra.Printing
{
    public interface IRawPrinter
    {
        void Print(string rawData);
        void Print(IZebraCommand command);
    }
}