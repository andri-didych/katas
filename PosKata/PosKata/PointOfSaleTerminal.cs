﻿using System.Collections.Generic;
using System.Linq;

namespace PosKata
{
    public interface IPointOfSaleTerminal
    {
        void SetPricing(IList<UnitPrice> prices, IList<BundlePrice> bundles = null);

        void Scan(string code);

        decimal CalculateTotal();
    }

    public class PointOfSaleTerminal : IPointOfSaleTerminal
    {
        private IDictionary<string, int> _cart;
        private IPriceProcessor _priceProcessor;

        public PointOfSaleTerminal(IPriceProcessor priceProcessor)
        {
            _cart = new Dictionary<string, int>();
            _priceProcessor = priceProcessor;
        }

        public PointOfSaleTerminal(IList<UnitPrice> prices, IList<BundlePrice> bundles = null)
        {
            SetPricing(prices, bundles);
        }

        public void SetPricing(IList<UnitPrice> prices, IList<BundlePrice> bundles = null)
        {
            _cart = new Dictionary<string, int>();
            _priceProcessor = new PriceProcessor(prices, bundles);
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
            var items = new List<ICartItem>();

            foreach (var i in _cart.Keys)
            {
                if (_priceProcessor.CanHaveDiscount(i))
                {
                    items.AddRange(_priceProcessor.GetItemsWithDiscount(i, _cart[i]));
                }
                else
                {
                    items.Add(_priceProcessor.GetItem(i, _cart[i]));
                }
            }

            return items.Sum(o => o.Count * o.Price);
        }
    }
}