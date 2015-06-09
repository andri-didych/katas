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
            var mock = GetMock<IPriceProcessor>();
            mock.Setup(o => o.CanHaveDiscount(It.IsAny<string>()))
                .Returns(false);

            Sut.Scan("A");
            Sut.CalculateTotal();

            mock.Verify(o => o.GetItem("A", 1));
            mock.Verify(o => o.GetItemsWithDiscount(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Test]

        public void CalculateTotal_ShouldCallGetItemsWithDiscount_For2Items()
        {
            var mock = GetMock<IPriceProcessor>();
            mock.Setup(o => o.CanHaveDiscount(It.IsAny<string>()))
                .Returns(true);

            Sut.Scan("A");
            Sut.Scan("A");
            Sut.CalculateTotal();

            mock.Verify(o => o.GetItemsWithDiscount("A", 2));
            mock.Verify(o => o.GetItem(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Test]

        public void CalculateTotal_ShouldCallGetItemAndGetItemsWithDiscount_ForDifferentItems()
        {
            var mock = GetMock<IPriceProcessor>();
            mock.Setup(o => o.CanHaveDiscount(It.Is<string>(s => s == "A")))
                .Returns(true);
            mock.Setup(o => o.CanHaveDiscount(It.Is<string>(s => s == "B")))
              .Returns(false);

            Sut.Scan("A");
            Sut.Scan("B");
            Sut.Scan("A");
            Sut.CalculateTotal();

            mock.Verify(o => o.GetItemsWithDiscount("A", 2));
            mock.Verify(o => o.GetItem("B", 1));
        }

        [Test]
        public void SetPrising_ShouldResetCart()
        {
            Sut.SetPricing(Get<List<UnitPrice>>());

            Assert.AreEqual(0, Sut.CalculateTotal());
        }
    }
}