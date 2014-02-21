using System;
using System.Reflection;
using System.Text;

namespace ZargateTicketPrint.ZebraHelpers
{
    internal class ZargateLabel
    {
        private readonly string commandString;
        private static Encoding Cp850Encoding = Encoding.GetEncoding(850);
        private static Encoding AnsiEncoding = Encoding.GetEncoding(1252);
        private static Encoding UnicodeEncoding = Encoding.UTF8;
        private static Encoding AsciiEncoding = Encoding.ASCII;


        public ZargateLabel(string name, int row, int seat, string type, string barcode)
        {
            name = name.ToUpper().Replace("Ø", "O").Replace("Æ", "AE").Replace("Å", "A");
            commandString = new LabelBuilder()
                .Barcode(barcode).At(210, 375)
                .Image(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("ZargateTicketPrint.zargate_main_logo.pcx"))
                .At(200, 250)
                .Text(name).At(220, 700)
                .Text("PLASS: " + row + "-" + seat).At(195, 700)
                .LargeText(type.ToUpper()).At(300, 770)
                .Barcode(barcode).At(210, 1250)
                .Image(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("ZargateTicketPrint.zargate_main_logo.pcx"))
                .At(200, 1125).ToZebraInstruction();
        }

        public ZargateLabel(string type, string barcode)
        {
            string date = DateTime.Now.ToLongTimeString();
            string validUntilDate = DateTime.Now.Hour > 12
                               ? (DateTime.Today + TimeSpan.FromDays(1)).ToShortDateString()
                               : (DateTime.Today).ToShortDateString();

            if (type.ToUpper().Contains("DAG"))
            {
                commandString = new LabelBuilder()
                    .Barcode(barcode).At(210, 375)
                    .Image(
                        Assembly.GetExecutingAssembly().GetManifestResourceStream(
                            "ZargateTicketPrint.zargate_main_logo.pcx"))
                    .At(200, 250)
                    .Text("KJOPT: " + date.ToUpper()).At(220, 700)
                    .Text("GYLDIG TIL: " + validUntilDate.ToUpper() + " 18:00").At(195, 700)
                    .LargeText(type.ToUpper()).At(300, 770)
                    .Barcode(barcode).At(210, 1250)
                    .Image(
                        Assembly.GetExecutingAssembly().GetManifestResourceStream(
                            "ZargateTicketPrint.zargate_main_logo.pcx"))
                    .At(200, 1125).ToZebraInstruction();
            }
            else
            {
                commandString = new LabelBuilder()
                    .Barcode(barcode).At(210, 375)
                    .Image(
                        Assembly.GetExecutingAssembly().GetManifestResourceStream(
                            "ZargateTicketPrint.zargate_main_logo.pcx"))
                    .At(200, 250)
                    .LargeText(type.ToUpper()).At(300, 770)
                    .Barcode(barcode).At(210, 1250)
                    .Image(
                        Assembly.GetExecutingAssembly().GetManifestResourceStream(
                            "ZargateTicketPrint.zargate_main_logo.pcx"))
                    .At(200, 1125).ToZebraInstruction();
            }
        }

        public string Convert(Encoding sourceEncoding, Encoding targetEncoding, string value)
        {
            string reEncodedString = null;

            byte[] sourceBytes = sourceEncoding.GetBytes(value);
            byte[] targetBytes = Encoding.Convert(sourceEncoding, targetEncoding, sourceBytes);
            reEncodedString = sourceEncoding.GetString(targetBytes);

            return reEncodedString;
        }

        public string ToZebraInstruction()
        {
            return commandString;
        }
    }
}