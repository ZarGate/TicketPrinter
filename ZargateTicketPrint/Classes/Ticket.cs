using System;

namespace ZargateTicketPrint.Classes
{
    class Ticket
    {
        private readonly int _ticketId;
        private readonly int _rad;
        private readonly int _sete;
        private readonly string _navn;
        private readonly long _refNr;
        private readonly Variants _type;
        private readonly DateTime _arrived;

        public long RefNr
        {
            get { return _refNr; }
        }

        public int TicketId
        {
            get { return _ticketId; }
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

        public DateTime Arrived
        {
            get { return _arrived; }
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
                    case Variants.HELGI:
                        return "HELG M/PC";
                    case Variants.VIP:
                        return "VIP";
                    default:
                        return "NOTHING";
                }
            }
        }

        public Ticket(int ticketId, int rad, int sete, string navn, long refNr, Variants type, DateTime arrived)
        {
            _ticketId = ticketId;
            _rad = rad;
            _sete = sete;
            _navn = navn;
            _type = type;
            _arrived = arrived;
            _refNr = refNr;
        }

        public Ticket(int ticketId, long refNr, Variants type, DateTime arrived)
        {
            _ticketId = ticketId;
            _rad = 0;
            _sete = 0;
            _navn = "";
            _type = type;
            _arrived = arrived;
            _refNr = refNr;
        }

        public enum Variants
        {
            DAGU,
            HELGU,
            HELG,
            GRATIS,
            VIP,
            HELGI
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
                    return Variants.HELGI;
                case "HELGI":
                    return Variants.HELG;
                case "VIP":
                    return Variants.VIP;
                default:
                    return Variants.DAGU;
            }
        }
    }
}
