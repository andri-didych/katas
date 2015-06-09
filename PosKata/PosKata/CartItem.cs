namespace PosKata
{
    public interface ICartItem
    {
        decimal Price { get; }

        int Count { get; }
    }

    public class CartItem : ICartItem
    {
        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}
