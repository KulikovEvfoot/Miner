using NUnit.Framework;
using Services.Currency.Runtime;

namespace Services.Currency.Tests
{
    public class CurrencyControllerTests
    {
        private ICurrencyController m_CurrencyControllerMock;
        private CurrencyObserverMock m_CurrencyObserverMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }

        [SetUp]
        public void Setup()
        {
            m_CurrencyControllerMock = new CurrencyControllerMock();
            m_CurrencyObserverMock = new CurrencyObserverMock();
            m_CurrencyControllerMock.EventProducer.Attach(m_CurrencyObserverMock);
        }

        [Test]
        public void ShouldAdd_10_Value()
        {
            var value = 10;
            var result = m_CurrencyControllerMock.AddValue(value);
            
            Assert.IsTrue(result);
            Assert.AreEqual(m_CurrencyControllerMock.GetValue(), value);
            Assert.AreEqual(m_CurrencyObserverMock.Value, value);
        }
        
        [Test]
        public void ShouldAdd_MaxValue()
        {
            var longMaxValue = long.MaxValue;
            var firstAdd = m_CurrencyControllerMock.AddValue(longMaxValue);
            var secondAdd = m_CurrencyControllerMock.AddValue(longMaxValue);
            
            Assert.IsTrue(firstAdd);
            Assert.IsTrue(secondAdd);
            Assert.AreEqual(m_CurrencyControllerMock.GetValue(), longMaxValue);
            Assert.AreEqual(m_CurrencyObserverMock.Value, longMaxValue);
        }     
        
        [Test]
        public void ShouldNotAdd_NegativeValue()
        {
            var value = -10;
            long defaultLong = default;
            var result = m_CurrencyControllerMock.AddValue(value);
            
            Assert.IsFalse(result);
            Assert.AreEqual(m_CurrencyControllerMock.GetValue(), defaultLong);
            Assert.AreEqual(m_CurrencyObserverMock.Value, defaultLong);
        }
        
        [Test]
        public void ShouldNotSub_BiggerThanDataValue()
        {
            var startValue = 100;
            var firstSub = 70;
            var secondSub = 80;
            m_CurrencyControllerMock.AddValue(startValue);
            var canSubFirst = m_CurrencyControllerMock.CanSub(firstSub);
            var firstSubResult = m_CurrencyControllerMock.SubtractValue(firstSub);
            var canSubSecond = m_CurrencyControllerMock.CanSub(secondSub);
            var secondSubResult = m_CurrencyControllerMock.SubtractValue(secondSub);
            
            Assert.IsTrue(canSubFirst);
            Assert.IsTrue(firstSubResult);
            Assert.AreEqual(m_CurrencyControllerMock.GetValue(), startValue - firstSub);
            Assert.AreEqual(m_CurrencyObserverMock.Value, startValue - firstSub);

            var currentValue = m_CurrencyControllerMock.GetValue();
            Assert.IsFalse(canSubSecond);
            Assert.IsFalse(secondSubResult);
            Assert.AreEqual(m_CurrencyControllerMock.GetValue(), currentValue);
            Assert.AreEqual(m_CurrencyObserverMock.Value, currentValue);
        }
        
        [Test]
        public void ShouldNotSub_NegativeValue()
        {
            var value = -10;
            long defaultLong = default;
            var result = m_CurrencyControllerMock.SubtractValue(value);
            
            Assert.IsFalse(result);
            Assert.AreEqual(m_CurrencyControllerMock.GetValue(), defaultLong);
            Assert.AreEqual(m_CurrencyObserverMock.Value, defaultLong);
        }
    }
}