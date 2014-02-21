namespace Com.SharpZebra.Commands
{
    public class ConstantCommands : IZebraCommand
    {
        public static readonly ConstantCommands START_NEW_PRINT_JOB = new ConstantCommands("\nN");
        public static readonly ConstantCommands PRINT_ONE_COPY = new ConstantCommands("P1");

        private readonly string commandString;

        public ConstantCommands(string commandString)
        {
            this.commandString = commandString;
        }

        public override string ToString()
        {
            return commandString;
        }


        public string ToZebraInstruction()
        {
            return ToString();
        }
    }
}