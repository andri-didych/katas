using System.Collections.Generic;
using System.Linq;

namespace PosKata
{
    public interface IPriceProcessor
    {
        bool CanHaveDiscount(string code);

        IEnumerable<ICartItem> GetItemsWithDiscount(string code, int count);

        ICartItem GetItem(string code, int count);
    }

    public class PriceProcessor : IPriceProcessor
    {
        private readonly IDictionary<string, UnitPrice> _prices;
        private readonly IDictionary<string, BundlePrice> _discounts;

        public PriceProcessor(IList<UnitPrice> prices, IList<BundlePrice> discounts = null)
        {
            _prices = prices.ToDictionary(o => o.Code, o => o);
            _discounts = discounts == null ? new Dictionary<string, BundlePrice>() : discounts.ToDictionary(o => o.Code, o => o);
        }

        public bool CanHaveDiscount(string code)
        {
            return _discounts.ContainsKey(code);
        }

        public IEnumerable<ICartItem> GetItemsWithDiscount(string code, int count)
        {
            var discountPrice = _discounts[code];
            var discountCount = count / discountPrice.Rule;
            if (discountCount > 0)
            {
                yield return new CartItem
                {
                    Count = discountCount,
                    Price = discountPrice.Price
                };
            }

            var itemsCount = count % discountPrice.Rule;
            if (itemsCount > 0)
            {
                yield return new CartItem
                {
                    Count = itemsCount,
                    Price = _prices[code].Price
                };
            }
        }

        public ICartItem GetItem(string code, int count)
        {
            return new CartItem
            {
                Count = count,
                Price = _prices[code].Price
            };
        }
    }
}
