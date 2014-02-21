namespace ZargateTicketPrint.Classes
{
    class Ticket
    {
        private readonly int _ticketId;
        private readonly int _userId;
        private readonly int _rad;
        private readonly int _sete;
        private readonly string _navn;
        private readonly int _refNr;

        public int RefNr
        {
            get { return _refNr; }
        }

        private readonly Variants _type;

        public int TicketId
        {
            get { return _ticketId; }
        }

        public int UserId
        {
            get { return _userId; }
        }

        public int Rad
        {
            get { return _rad; }
        }

        public int Sete
        {
            get { return _sete; }
        }

        public string Navn
        {
            get { return _navn; }
        }

        public Variants Type
        {
            get { return _type; }
        }

        public string TypeString
        {
            get
            {
                switch (_type)
                {
                    case Variants.DAGU:
                        return "DAGSPASS";
                    case Variants.GRATIS:
                        return "GRATIS";
                    case Variants.HELGU:
                        return "HELG U/PC";
                    case Variants.HELG:
                        return "HELG M/PC";
                    case Variants.VIP:
                        return "VIP";
                    default:
                        return "NOTHING";
                }
            }
        }

        public Ticket(int ticketId, int userId, int rad, int sete, string navn, int refNr, Variants type)
        {
            _ticketId = ticketId;
            _userId = userId;
            _rad = rad;
            _sete = sete;
            _navn = navn;
            _type = type;
            _refNr = refNr;
        }

        public Ticket(int ticketId, int refNr, Variants type)
        {
            _ticketId = ticketId;
            _userId = 0;
            _rad = 0;
            _sete = 0;
            _navn = "";
            _type = type;
            _refNr = refNr;
        }

        public enum Variants
        {
            DAGU,
            HELGU,
            HELG,
            GRATIS,
            VIP
        }

        public static Variants ParseTypeToVariant (string variant)
        {
            switch (variant.ToUpper().Trim())
            {
                case "DAGU":
                    return Variants.DAGU;
                case "GRATIS":
                    return Variants.GRATIS;
                case "HELGU":
                    return Variants.HELGU;
                case "HELG":
                    return Variants.HELG;
                case "VIP":
                    return Variants.VIP;
                default:
                    return Variants.DAGU;
            }
        }
    }
}
