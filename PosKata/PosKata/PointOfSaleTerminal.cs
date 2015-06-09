using System.Collections.Generic;

namespace PosKata
{
    public interface IPointOfSaleTerminal
    {
        void SetPricing(IList<UnitPrice> prices, IList<BundlePrice> bundles = null);

        void Scan(string code);

        decimal CalculateTotal();

        void ApplyDiscount(DiscountCard discount);

        void CloseSale();
    }

    public class PointOfSaleTerminal : IPointOfSaleTerminal
    {
        private IDictionary<string, int> _cart;
        private ICartItemBuilder _cartItemBuilder;
        private DiscountCard _discount = DiscountCard.None;

        public PointOfSaleTerminal(ICartItemBuilder cartItemBuilder)
        {
            _cart = new Dictionary<string, int>();
            _cartItemBuilder = cartItemBuilder;
        }

        public PointOfSaleTerminal(IList<UnitPrice> prices, IList<BundlePrice> bundles = null)
        {
            SetPricing(prices, bundles);
        }

        public void SetPricing(IList<UnitPrice> prices, IList<BundlePrice> bundles = null)
        {
            _cart = new Dictionary<string, int>();
            _cartItemBuilder = new CartItemBuilder(prices, bundles);
        }

        public void Scan(string code)
        {
            if (_cart.ContainsKey(code))
                _cart[code]++;
            else
            {
                _cart[code] = 1;
            }
        }

        public decimal CalculateTotal()
        {
            var cart = new Cart(BuildCartItems());
            cart.SetDiscount(_discount.Discout);
            return cart.GetPrice();
        }

        public void ApplyDiscount(DiscountCard discount)
        {
            _discount = discount;
        }

        public void CloseSale()
        {
            var cart = new Cart(BuildCartItems());
            cart.SetDiscount(_discount.Discout);
            _discount.AddBalance(cart.GetFullPrice());
        }

        private IList<ICartItem> BuildCartItems()
        {
            var items = new List<ICartItem>();

            foreach (var i in _cart.Keys)
            {
                if (_cartItemBuilder.IsSpecial(i))
                {
                    items.AddRange(_cartItemBuilder.BuidlSpecialItems(i, _cart[i]));
                }
                else
                {
                    items.Add(_cartItemBuilder.BuildItem(i, _cart[i]));
                }
            }
            return items;
        }
    }
}