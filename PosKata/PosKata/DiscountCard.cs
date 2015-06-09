namespace PosKata
{
    public class DiscountCard
    {
        public static readonly DiscountCard None = new DiscountCard("None", 0);
        public static readonly DiscountCard Andri = new DiscountCard("Andri", 10);

        private readonly string _name;
        private readonly int _discountPercent;
        private decimal _balance;

        public DiscountCard(string name, int discountPercent)
        {
            _balance = decimal.Zero;
            _name = name;
            _discountPercent = discountPercent;
        }

        public void AddBalance(decimal amount)
        {
            _balance += amount;
        }

        public int Discout { get { return _discountPercent; } }

        public decimal Balance { get { return _balance; } }

        public override string ToString()
        {
            return string.Format("{0}'s {1}% discount card with {2:C} balance", _name, _discountPercent, _balance);
        }
    }
}
