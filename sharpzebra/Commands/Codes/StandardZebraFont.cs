namespace Com.SharpZebra.Commands.Codes
{
    public class StandardZebraFont
    {
        public static readonly StandardZebraFont LARGEST = new StandardZebraFont(5);
        public static readonly StandardZebraFont LARGE = new StandardZebraFont(4);
        public static readonly StandardZebraFont NORMAL = new StandardZebraFont(3);
        public static readonly StandardZebraFont SMALL = new StandardZebraFont(2);
        public static readonly StandardZebraFont SMALLEST = new StandardZebraFont(1);
        private readonly int code;

        private StandardZebraFont(int code)
        {
            this.code = code;
        }

        public override string ToString()
        {
            return code.ToString();
        }
    }
}