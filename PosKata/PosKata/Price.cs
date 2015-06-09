namespace PosKata
{
    public interface IPrice
    {
        string Code { get; }

        decimal Price { get; }
    }

    public class UnitPrice : IPrice
    {
        public UnitPrice(string code, decimal price)
        {
            Code = code;
            Price = price;
        }

        public string Code { get; private set; }

        public decimal Price { get; private set; }
    }

    public class BundlePrice : IPrice
    {
        public BundlePrice(string code, decimal price, int size)
        {
            Code = code;
            Price = price;
            Size = size;
        }

        public int Size { get; private set; }

        public string Code { get; private set; }

        public decimal Price { get; private set; }
    }
}