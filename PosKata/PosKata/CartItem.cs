namespace PosKata
{
    public interface ICartItem
    {
        int Count { get; }

        decimal Price { get; }

        decimal FullPrice { get; }

        bool IsSpecial { get; }
    }

    public class CartItem : ICartItem
    {
        public CartItem(int count, decimal price)
        {
            Count = count;
            Price = price;
        }

        public int Count { get; private set; }

        public decimal Price { get; private set; }

        public decimal FullPrice
        {
            get { return Price; }
        }

        public bool IsSpecial
        {
            get { return false; }
        }
    }

    public class BundleCartItem : ICartItem
    {
        public BundleCartItem(int count, decimal price,
            decimal fullPrice, bool isSpecial = true)
        {
            Count = count;
            Price = price;
            FullPrice = fullPrice;
            IsSpecial = isSpecial;
        }

        public int Count { get; private set; }

        public decimal Price { get; private set; }

        public decimal FullPrice { get; private set; }

        public bool IsSpecial { get; private set; }
    }
}