using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PosKata.Tests
{
    public class CartItemBuilderTests : TestFor<CartItemBuilder>
    {
        [Test]
        public void IsSpecial_ShouldReturnFalse_WhenOnlyUnitPriceAvaildalbe()
        {
            Inject(new List<UnitPrice> { new UnitPrice("A", 1.00m) });

            Assert.IsFalse(Sut.IsSpecial("A"));
        }

        [Test]
        public void BuildItem_ShouldReturnCartItem()
        {
            Inject(new List<UnitPrice> { new UnitPrice("A", 7.77m) });

            var result = Sut.BuildItem("A", 5);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(7.77m, result.Price);
        }

        [Test]
        public void BuildItem_ShouldReturnCartItem_WithCorrectFullPrice()
        {
            Inject(new List<UnitPrice> { new UnitPrice("A", 7.77m) });

            var result = Sut.BuildItem("A", 5);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(7.77m, result.FullPrice);
        }

        [Test]
        public void BuildItem_ShouldReturnCartItem_WithIsSpecialFalse()
        {
            Inject(new List<UnitPrice> { new UnitPrice("A", 7.77m) });

            var result = Sut.BuildItem("A", 5);
            Assert.AreEqual(5, result.Count);
            Assert.That(result.IsSpecial == false);
        }

        [Test]
        public void BuidlSpecialItems_ShouldReturnCartItems_WhenNotEnoughForBundle()
        {
            Inject(new List<UnitPrice> { new UnitPrice("A", 1.00m) });
            Inject(new List<BundlePrice> { new BundlePrice("A", 3.00m, 4) });

            var results = Sut.BuidlSpecialItems("A", 3).ToList();

            Assert.AreEqual(1, results.Count);
            Assert.True(results.Any(o => o.Price == 1.00m && o.Count == 3));
        }

        [Test]
        public void BuidlSpecialItems_ShouldReturnCartItemsInBundle_WhenEnoughForBundle()
        {
            Inject(new List<UnitPrice> { new UnitPrice("A", 1.00m) });
            Inject(new List<BundlePrice> { new BundlePrice("A", 3.00m, 4) });

            var results = Sut.BuidlSpecialItems("A", 4).ToList();

            Assert.AreEqual(1, results.Count);
            Assert.True(results.Any(o => o.Price == 3.00m && o.Count == 1));
        }

        [Test]
        public void BuidlSpecialItems_ShouldReturnCartItemsInBundle_WithCorrectFullPrice_WhenEnoughForBundle()
        {
            Inject(new List<UnitPrice> { new UnitPrice("A", 1.00m) });
            Inject(new List<BundlePrice> { new BundlePrice("A", 3.00m, 4) });

            var results = Sut.BuidlSpecialItems("A", 4).ToList();

            Assert.AreEqual(1, results.Count);
            Assert.That(results.Any(o => o.FullPrice == 4.00m));
        }

        [Test]
        public void BuidlSpecialItems_ShouldReturnCartItemsInBundle_WithIsSpecialTrue_WhenEnoughForBundle()
        {
            Inject(new List<UnitPrice> { new UnitPrice("A", 1.00m) });
            Inject(new List<BundlePrice> { new BundlePrice("A", 3.00m, 4) });

            var results = Sut.BuidlSpecialItems("A", 4).ToList();

            Assert.AreEqual(1, results.Count);
            Assert.That(results.Any(o => o.IsSpecial));
        }

        [Test]
        public void BuidlSpecialItems_ShouldReturnCartItemsAndBundles_WhenMoreThanBundle()
        {
            Inject(new List<UnitPrice> { new UnitPrice("A", 1.00m) });
            Inject(new List<BundlePrice> { new BundlePrice("A", 3.00m, 4) });

            var results = Sut.BuidlSpecialItems("A", 11).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.True(results.Any(o => o.Price == 3.00m && o.Count == 2));
            Assert.True(results.Any(o => o.Price == 1.00m && o.Count == 3));
        }

        [Test]
        public void IsSpecial_ShouldReturnTrue_WhenBundlePriceAvaildalbe()
        {
            Inject(new List<BundlePrice> { new BundlePrice("A", 1.00m, 5) });

            Assert.IsTrue(Sut.IsSpecial("A"));
        }
    }
}
