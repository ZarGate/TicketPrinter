using Com.SharpZebra;
using Com.SharpZebra.Commands;
using Com.SharpZebra.Commands.Codes;

namespace ZargateTicketPrint.ZebraHelpers
{
    internal class LabelBuilder
    {
        private readonly ZebraCommands commands = new ZebraCommands();
        private Command commandToExecute;
        private object value;

        public LabelBuilder Barcode(object inputValue)
        {
            return StoreValues(BarcodeCommand, inputValue);
        }

        private LabelBuilder StoreValues(Command delegateCommand, object inputValue)
        {
            value = inputValue;
            commandToExecute = delegateCommand;
            return this;
        }

        public LabelBuilder At(int x, int y)
        {
            commands.Add(commandToExecute(value, x, y));
            commandToExecute = null;
            value = null;
            return this;
        }

        public LabelBuilder Text(object inputValue)
        {
            return StoreValues(StandardText, inputValue);
        }

        public LabelBuilder LargeText(object inputValue)
        {
            return StoreValues(LargeText, inputValue);
        }

        public LabelBuilder Image(object inputValue)
        {
            return StoreValues(ImageCommand, inputValue);
        }

        private IZebraCommand ImageCommand(object imageStream, int x, int y)
        {
            return new GraphicZebraCommand((System.IO.Stream)imageStream, x, y);
        }

        private IZebraCommand BarcodeCommand(object barcode, int x, int y)
        {
            return ZebraCommands.BarCodeCommand(x, y, ElementRotation.ROTATE_90_DEGREES, 1, 2, 4, 50, true, (string)barcode);
        }

        private IZebraCommand StandardText(object text, int x, int y)
        {
            return ZebraCommands.TextCommand(x, y, ElementRotation.ROTATE_90_DEGREES, StandardZebraFont.LARGE, 1, 1,
                                             false, (string)text);
        }

        private IZebraCommand LargeText(object text, int x, int y)
        {
            return ZebraCommands.TextCommand(x, y, ElementRotation.ROTATE_90_DEGREES, StandardZebraFont.LARGEST, 1, 1,
                                             false, (string)text);
        }

        public string ToZebraInstruction()
        {
            return commands.ToZebraInstruction();
        }

        public LabelBuilder Border(int startX, int startY, int endX, int endY)
        {
            commands.Add(ZebraCommands.DrawBox(startX, startY, 2, endX, endY));
            return this;
        }

        #region Nested type: Command

        private delegate IZebraCommand Command(object value, int x, int y);

        #endregion
    }
}