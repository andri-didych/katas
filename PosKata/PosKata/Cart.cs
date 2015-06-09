using System;
using System.Collections.Generic;
using System.Linq;

namespace PosKata
{
    public class Cart
    {
        private readonly IList<ICartItem> _items;
        private decimal _discount = 1m;

        public Cart(IList<ICartItem> items)
        {
            _items = items;
        }

        public void SetDiscount(int percent)
        {
            if (percent < 0 || percent > 100)
            {
                throw new ArgumentOutOfRangeException("percent");
            }
            _discount = 1m - percent / 100m;
        }

        public decimal GetPrice()
        {
            return _items.Sum(o => o.IsSpecial
                ? o.Count * o.Price
                : o.Count * o.Price * _discount);
        }

        public decimal GetFullPrice()
        {
            return _items.Sum(o => o.Count * o.FullPrice);
        }
    }
}
