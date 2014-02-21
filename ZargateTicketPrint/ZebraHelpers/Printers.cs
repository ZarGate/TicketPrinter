using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace ZargateTicketPrint.ZebraHelpers
{
    internal static class Printers
    {
        public static IList<string> InstalledPrinters()
        {
            return ToSortedStringArray(PrinterSettings.InstalledPrinters);
        }

        public static string DefaultZebraPrinter()
        {
            foreach (string printer in InstalledPrinters())
            {
                if (!string.IsNullOrEmpty(printer)
                    && printer.ToUpper().Contains("2824"))
                {
                    Printer.Default.PrinterName1 = printer;
                    return printer;
                }
            }
            return null;
        }

        private static IList<string> ToSortedStringArray(IEnumerable printers)
        {
            var stringList = new List<string>();
            foreach (string printer in printers)
            {
                stringList.Add(printer);
            }
            stringList.Sort(StringComparer.Ordinal);
            return stringList;
        }
    }
}