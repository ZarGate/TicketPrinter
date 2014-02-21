namespace Com.SharpZebra.Commands.Codes
{
    public class ElementRotation 
    {
        public static ElementRotation NO_ROTATION = new ElementRotation(0);
        public static ElementRotation ROTATE_90_DEGREES = new ElementRotation(1);
        public static ElementRotation ROTATE_180_DEGREES = new ElementRotation(2);
        public static ElementRotation ROTATE_270_DEGREES = new ElementRotation(3);

        private readonly int value;

        private ElementRotation(int value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}