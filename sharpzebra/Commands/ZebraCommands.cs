using System.Collections.Generic;
using System.Text;
using Com.SharpZebra.Commands;
using Com.SharpZebra.Commands.Codes;

namespace Com.SharpZebra
{
    public class ZebraCommands : IZebraCommand
    {
        private IList<IZebraCommand> zebraCommands = new List<IZebraCommand>();
        
        public ZebraCommands()
        {
            zebraCommands.Add(ConstantCommands.START_NEW_PRINT_JOB);
        }

        public void Add(IZebraCommand zebraCommand)
        {
            zebraCommands.Add(zebraCommand);
        }

        public string ToZebraInstruction()
        {
            EndZebraCommands();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (IZebraCommand command in zebraCommands)
            {
                stringBuilder.AppendLine(command.ToZebraInstruction());
            }
            return stringBuilder.ToString();
        }

        private void EndZebraCommands()
        {
            if (zebraCommands[zebraCommands.Count - 1]
                != ConstantCommands.PRINT_ONE_COPY)
            {
                zebraCommands.Add(ConstantCommands.PRINT_ONE_COPY);
            }
        }

        public static IZebraCommand BarCodeCommand(int horizontalStartPositionInDots, int verticalStartPositionInDots,
                                                   ElementRotation rotation, int barcodeSelection,
                                                   int narrowBarWidthInDots, int wideBarWidthInDots,
                                                   int barcodeHeightInDots, bool printHumanReadableCode,
                                                   string barcodeData)
        {
            string encodedPrintHumanReadableCode = printHumanReadableCode ? "B" : "N";
            return new ConstantCommands(
                string.Format("B{0},{1},{2},{3},{4},{5},{6},{7},\"{8}\"", horizontalStartPositionInDots,
                              verticalStartPositionInDots, rotation, barcodeSelection, narrowBarWidthInDots,
                              wideBarWidthInDots, barcodeHeightInDots, encodedPrintHumanReadableCode,
                              barcodeData));
        }

        public static IZebraCommand TextCommand(int horizontalStartPositionInDots, int verticalStartPositionInDots,
                                                ElementRotation rotation, StandardZebraFont zebraTextFont,
                                                int horizontalMultiplier,
                                                int verticalMultiplier, bool isReverse, string text)
        {
            string normalOrReverseString = isReverse ? "R" : "N";
            string command = string.Format("A{0},{1},{2},{3},{4},{5},{6},\"{7}\"", horizontalStartPositionInDots,
                                           verticalStartPositionInDots, rotation, zebraTextFont, horizontalMultiplier,
                                           verticalMultiplier, normalOrReverseString, text);
            return new ConstantCommands(command);
        }

        public static IZebraCommand BlackLine(int horizontalStartPositionInDots, int verticalStartPositionInDots,
                                              int horizontalLengthInDots, int verticalLengthInDots)
        {
            return LineDraw("LO", horizontalStartPositionInDots, verticalStartPositionInDots, horizontalLengthInDots,
                            verticalLengthInDots);
        }

        private static IZebraCommand LineDraw(string lineDrawCode, int horizontalStartPositionInDots,
                                              int verticalStartPositionInDots, int horizontalLengthInDots,
                                              int verticalLengthInDots)
        {
            string command =
                string.Format("{0}{1},{2},{3},{4}", lineDrawCode, horizontalStartPositionInDots,
                              verticalStartPositionInDots,
                              horizontalLengthInDots, verticalLengthInDots);
            return new ConstantCommands(command);
        }

        public static IZebraCommand WhiteLine(int horizontalStartPositionInDots, int verticalStartPositionInDots,
                                              int horizontalLengthInDots, int verticalLengthInDots)
        {
            return LineDraw("LW", horizontalStartPositionInDots, verticalStartPositionInDots, horizontalLengthInDots,
                            verticalLengthInDots);
        }

        public static IZebraCommand DiagonalLine(int horizontalStartPositionInDots, int verticalStartPositionInDots,
                                                 int lineThicknessInDots, int horizontalEndPositionInDots, int verticalEndPositionInDots)
        {
            string command =
                string.Format("LS{0},{1},{2},{3},{4}", horizontalStartPositionInDots, verticalStartPositionInDots,
                              lineThicknessInDots, horizontalEndPositionInDots, verticalEndPositionInDots);
            return new ConstantCommands(command);
        }

        public static IZebraCommand DrawBox(int horizontalStartPositionInDots, int verticalStartPositionInDots,
                                            int lineThicknessInDots, int horizontalEndPositionInDots, int verticalEndPositionInDots)
        {
            string command =
                string.Format("X{0},{1},{2},{3},{4}", horizontalStartPositionInDots, verticalStartPositionInDots,
                              lineThicknessInDots, horizontalEndPositionInDots, verticalEndPositionInDots);
            return new ConstantCommands(command);
        }
    }
}