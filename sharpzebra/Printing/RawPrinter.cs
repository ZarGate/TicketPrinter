using System;
using System.Runtime.InteropServices;

namespace Com.SharpZebra.Printing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DOCINFO
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string printerDocumentName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pOutputFile;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string printerDocumentDataType;
    }

    /// <summary>
    /// This class was derived from http://www.codeproject.com/csharp/pclprinting.asp accessing
    /// the native winspool APIs to send raw data to any printer. Slightly renamed some of the variables
    /// so that we can understand it a little bit better.
    /// </summary>
    public class RawPrinter
    {
        [
            DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false,
                CallingConvention = CallingConvention.StdCall)]
        public static extern long OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);

        [
            DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false,
                CallingConvention = CallingConvention.StdCall)]
        public static extern long StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFO pDocInfo);

        [
            DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
        public static extern long StartPagePrinter(IntPtr hPrinter);

        [
            DllImport("winspool.drv", CharSet = CharSet.Ansi, ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
        public static extern long WritePrinter(IntPtr hPrinter, string data, int buf, ref int pcWritten);

        [
            DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
        public static extern long EndPagePrinter(IntPtr hPrinter);

        [
            DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
        public static extern long EndDocPrinter(IntPtr hPrinter);

        [
            DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
        public static extern long ClosePrinter(IntPtr hPrinter);

        public static void SendToPrinter(string printerJobName, string rawStringToSendToThePrinter,
                                         string printerNameAsDescribedByPrintManager)
        {
            IntPtr handleForTheOpenPrinter = new IntPtr();
            DOCINFO documentInformation = new DOCINFO();
            int printerBytesWritten = 0;
            documentInformation.printerDocumentName = printerJobName;
            documentInformation.printerDocumentDataType = "RAW";
            OpenPrinter(printerNameAsDescribedByPrintManager, ref handleForTheOpenPrinter, 0);
            StartDocPrinter(handleForTheOpenPrinter, 1, ref documentInformation);
            StartPagePrinter(handleForTheOpenPrinter);
            WritePrinter(handleForTheOpenPrinter, rawStringToSendToThePrinter, rawStringToSendToThePrinter.Length,
                         ref printerBytesWritten);
            EndPagePrinter(handleForTheOpenPrinter);
            EndDocPrinter(handleForTheOpenPrinter);
            ClosePrinter(handleForTheOpenPrinter);
        }
    }
}