using System.Collections.Generic;
using System.Linq;

namespace PosKata
{
    public interface ICartItemBuilder
    {
        bool IsSpecial(string code);

        IEnumerable<ICartItem> BuidlSpecialItems(string code, int count);

        ICartItem BuildItem(string code, int count);
    }

    public class CartItemBuilder : ICartItemBuilder
    {
        private readonly IDictionary<string, UnitPrice> _prices;
        private readonly IDictionary<string, BundlePrice> _specials;

        public CartItemBuilder(IList<UnitPrice> prices, IList<BundlePrice> specials = null)
        {
            _prices = prices.ToDictionary(o => o.Code, o => o);
            _specials = specials == null ? new Dictionary<string, BundlePrice>() : specials.ToDictionary(o => o.Code, o => o);
        }

        public bool IsSpecial(string code)
        {
            return _specials.ContainsKey(code);
        }

        public IEnumerable<ICartItem> BuidlSpecialItems(string code, int count)
        {
            var bundle = _specials[code];
            var bundleCount = count / bundle.Size;
            if (bundleCount > 0)
            {
                var fullPrice = GetPrice(code) * bundle.Size;
                yield return new BundleCartItem(bundleCount, bundle.Price, fullPrice);
            }

            var itemsCount = count % bundle.Size;
            if (itemsCount > 0)
            {
                yield return BuildItem(code, itemsCount);
            }
        }

        public ICartItem BuildItem(string code, int count)
        {
            var price = GetPrice(code);
            return new CartItem(count, price);
        }

        private decimal GetPrice(string code)
        {
            return _prices[code].Price;
        }
    }
}
