using System;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace PosKata.Tests
{
    [TestFixture]
    public class TestFor<TSut> where TSut : class
    {
        private TSut _sut;

        public TSut Sut
        {
            get { return _sut ?? (_sut = _fixture.Create<TSut>()); }
        }

        private IFixture _fixture;

        [SetUp]
        public virtual void RunBeforeEachTest()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
            _sut = null;
        }

        public void Inject<TInject>(TInject instance)
        {
            _fixture.Inject(instance);
        }

        public Mock<TMock> GetMock<TMock>() where TMock : class
        {
            return _fixture.Freeze<Mock<TMock>>();
        }

        public T Get<T>(Action<T> action = null) where T : class
        {
            var result = _fixture.Create<T>();
            if (action != null)
                action(result);
            return result;
        }
    }
}
