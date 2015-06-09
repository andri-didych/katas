using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PosKata.Tests
{
    public class CartTests : TestFor<Cart>
    {
        [Test]
        public void GetFullPrice_ShouldReturnCorrectResult()
        {
            var items = GetItems();
            Inject(items);

            var result = Sut.GetFullPrice();

            Assert.AreEqual(items.Sum(o => o.FullPrice * o.Count), result);
        }

        [Test]
        public void GetPrice_ShouldReturnCorrectResult()
        {
            var items = GetItems();
            Inject(items);

            var result = Sut.GetPrice();

            Assert.AreEqual(items.Sum(o => o.Price * o.Count), result);
        }

        [Test]
        public void GetPrice_ShouldReturnCorrectResult_WhenDiscountIsSet()
        {
            var items = GetItems();
            Inject(items);

            Sut.SetDiscount(5);
            var result = Sut.GetPrice();

            var usualPricesWithDiscounts = items.Where(o => !o.IsSpecial).Sum(o => o.Price * o.Count * 0.95m);
            var specialPricesWithoutDiscounts = items.Where(o => o.IsSpecial).Sum(o => o.Price * o.Count);
            var total = usualPricesWithDiscounts + specialPricesWithoutDiscounts;
            Assert.AreEqual(total, result);
        }

        private List<ICartItem> GetItems()
        {
            return Get<List<TestCartItem>>().Cast<ICartItem>().ToList();
        }

        internal class TestCartItem : ICartItem
        {
            public int Count { get; set; }
            public decimal Price { get; set; }
            public decimal FullPrice { get; set; }
            public bool IsSpecial { get; set; }
        }
    }
}
