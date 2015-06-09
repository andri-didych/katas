using System.Collections.Generic;
using NUnit.Framework;

namespace PosKata.Tests
{
    [TestFixture]
    public class PointOfSaleTerminalIntegrationTests
    {
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

        private static void RunTest(string items, decimal expected)
        {
            var discounts = new List<BundlePrice>
            {
                new BundlePrice("A", 3.00m, rule: 3),
                new BundlePrice("C", 5.00m, rule: 6)
            };

            var prices = new List<UnitPrice>
            {
                new UnitPrice("A", 1.25m),
                new UnitPrice("B", 4.25m),
                new UnitPrice("C", 1.00m),
                new UnitPrice("D", 0.75m)
            };

            var pos = new PointOfSaleTerminal(prices, discounts);

            foreach (var item in items)
            {
                pos.Scan(item.ToString());
            }

            var actual = pos.CalculateTotal();

            Assert.AreEqual(expected, actual);
        }
    }
}