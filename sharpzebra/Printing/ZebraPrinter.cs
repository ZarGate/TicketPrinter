namespace Com.SharpZebra.Printing
{
    public class ZebraPrinter : IRawPrinter
    {
        private readonly string jobName;
        private readonly string rawPrinterName;

        public ZebraPrinter(string rawPrinterName) : this(rawPrinterName, "Zebra Printing")
        {
            
        }

        private ZebraPrinter(string rawPrinterName, string jobName)
        { 
            this.rawPrinterName = rawPrinterName;
            this.jobName = jobName;
        }

        public void Print(string rawData)
        {
            RawPrinter.SendToPrinter(jobName, rawData, rawPrinterName);
        }

        public void Print(IZebraCommand command)
        {
            Print(command.ToZebraInstruction());
        }
    }
}