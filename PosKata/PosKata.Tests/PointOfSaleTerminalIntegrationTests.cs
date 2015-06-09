using System.Collections.Generic;
using NUnit.Framework;

namespace PosKata.Tests
{
    [TestFixture]
    public class PointOfSaleTerminalIntegrationTests : TestFor<PointOfSaleTerminal>
    {
        public override void RunBeforeEachTest()
        {
            base.RunBeforeEachTest();
            var discounts = new List<BundlePrice>
            {
                new BundlePrice("A", 3.00m, size: 3),
                new BundlePrice("C", 5.00m, size: 6)
            };

            var prices = new List<UnitPrice>
            {
                new UnitPrice("A", 1.25m),
                new UnitPrice("B", 4.25m),
                new UnitPrice("C", 1.00m),
                new UnitPrice("D", 0.75m)
            };
            Inject(new PointOfSaleTerminal(prices, discounts));
        }

        [Test]
        [TestCase("", 0.00)]
        [TestCase("A", 1.25)]
        [TestCase("AA", 2.50)]
        [TestCase("AAA", 3.00)]
        [TestCase("AAAAAA", 6.00)]
        [TestCase("BBB", 12.75)]
        [TestCase("AD", 2.00)]
        [TestCase("AACADCCABCACACCA", 5 + 1 + 4.25 + 3 * 2 + 1.25 + 0.75)]

        [TestCase("ABCD", 7.25)]
        [TestCase("ABCDABA", 13.25)]
        [TestCase("CCCCCCC", 6.00)]
        public void FullTest(string items, decimal expected)
        {
            RunTest(items, expected);
        }

        private void RunTest(string items, decimal expected)
        {
            ScanItems(items);

            var actual = Sut.CalculateTotal();

            Assert.AreEqual(expected, actual);
        }

        private void ScanItems(string items)
        {
            foreach (var item in items)
            {
                Sut.Scan(item.ToString());
            }
        }

        [Test]
        public void CloseSale_ShouldIncreaseDiscountCardBalance_ForFullPrice()
        {
            Sut.ApplyDiscount(DiscountCard.Andri);
            ScanItems("AAA");

            Sut.CloseSale();

            Assert.AreEqual(3.75m, DiscountCard.Andri.Balance);
        }

        [Test]
        public void CalculateTotal_ShouldUseDiscountCard_WhenApplied()
        {
            Sut.ApplyDiscount(DiscountCard.Andri);
            ScanItems("AACAA"); // 3.00 + 1.25 + 1.00 | -10%  | -0.225

            var actual = Sut.CalculateTotal();

            //TODO: introduce 'Money' domain object for proper rounding handling
            Assert.AreEqual(5.25m - 0.225m, actual);
        }
    }
}