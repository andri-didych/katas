using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace PosKata.Tests
{
    [TestFixture]
    public class PointOfSaleTerminalTests : TestFor<PointOfSaleTerminal>
    {
        [Test]
        public void CalculateTotal_ShouldCallGetItem_For1Item()
        {
            var mock = GetMock<ICartItemBuilder>();
            mock.Setup(o => o.IsSpecial(It.IsAny<string>()))
                .Returns(false);

            Sut.Scan("A");
            Sut.CalculateTotal();

            mock.Verify(o => o.BuildItem("A", 1));
            mock.Verify(o => o.BuidlSpecialItems(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void CalculateTotal_ShouldCallGetItemsWithDiscount_For2Items()
        {
            var mock = GetMock<ICartItemBuilder>();
            mock.Setup(o => o.IsSpecial(It.IsAny<string>()))
                .Returns(true);

            Sut.Scan("A");
            Sut.Scan("A");
            Sut.CalculateTotal();

            mock.Verify(o => o.BuidlSpecialItems("A", 2));
            mock.Verify(o => o.BuildItem(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void CalculateTotal_ShouldCallGetItemAndGetItemsWithDiscount_ForDifferentItems()
        {
            var mock = GetMock<ICartItemBuilder>();
            mock.Setup(o => o.IsSpecial(It.Is<string>(s => s == "A")))
                .Returns(true);
            mock.Setup(o => o.IsSpecial(It.Is<string>(s => s == "B")))
              .Returns(false);

            Sut.Scan("A");
            Sut.Scan("B");
            Sut.Scan("A");
            Sut.CalculateTotal();

            mock.Verify(o => o.BuidlSpecialItems("A", 2));
            mock.Verify(o => o.BuildItem("B", 1));
        }

        [Test]
        public void SetPricing_ShouldResetCart()
        {
            Sut.SetPricing(Get<List<UnitPrice>>());

            Assert.AreEqual(0, Sut.CalculateTotal());
        }
    }
}