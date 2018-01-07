using System;
using PeckingOrder;
using System.Collections.Generic;
using NUnit.Framework;

namespace PeckingOrder.Tests
{
    [TestFixture]
    public class Tests
    {
        [TestCase("City", false)]
        [TestCase("State", true)]
        [TestCase("Country", false)]
        [TestCase("Default", true)]
        public void Given_ValidSingleItem_When_ResolvingSettings_Then_ReturnSetting(string key, bool value)
        {
            Sorter<string, int> target = buildTarget();

            var isNewYearsAHoliday = new List<(string mode, bool value)>()
            {
                (key,value)
            };

            var actual = target.Resolve(isNewYearsAHoliday);

            Assert.That(actual == value);
        }

        [TestCase("Potatoes", false)]
        public void Given_InvalidSingleItem_When_ResolvingSettings_Then_ThrowArgumentOutOfRangeException(string key, bool value)
        {
            Sorter<string, int> target = buildTarget();

            var isNewYearsAHoliday = new List<(string mode, bool value)>()
            {
                (key,value)
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => target.Resolve(isNewYearsAHoliday));
        }

        [Test]
        public void Given_ValidCollection_When_ResolvingSettings_Then_ReturnSetting()
        {
            Sorter<string, int> target = buildTarget();

            var locationNames = new List<(string mode, string value)>()
            {
                ("Country","United States of America"),
                ("City","Las Vegas"),
                ("State","Nevada")
            };

            var actual = target.Resolve(locationNames);

            Assert.That(actual == "Las Vegas");
        }

        [Test]
        public void Given_CollectionWithInvalidEntry_When_ResolvingSettings_Then_ThrowArgumentOutOfRangeException()
        {
            Sorter<string, int> target = buildTarget();

            var locationNames = new List<(string mode, string value)>()
            {
                ("Country","United States of America"),
                ("City","Las Vegas"),
                ("Street","123 Main Street")
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => target.Resolve(locationNames));
        }

        [Test]
        public void Given_NullCollection_When_ResolvingSettings_Then_ThrowArgumentNullException()
        {
            Sorter<string, int> target = buildTarget();

            Assert.Throws<ArgumentNullException>(() => target.Resolve<(string, int)>(null));
        }

        [Test]
        public void Given_EmptyCollection_When_ResolvingSettings_Then_ThrowArgumentException()
        {
            Sorter<string, int> target = buildTarget();

            var locationNames = new List<(string mode, string value)>()
            {

            };

            Assert.Throws<ArgumentException>(() => target.Resolve(locationNames));
        }

        [Test]
        public void Given_NullOrderAndValidCollection_When_ResolvingSettings_Then_ThrowInvalidOperationException()
        {
            Sorter<string, int> target = new Sorter<string, int>();

            target.Order = null;

            var locationNames = new List<(string mode, string value)>()
            {
                ("Country","United States of America"),
                ("City","Las Vegas"),
                ("State","Nevada")
            };

            Assert.Throws<InvalidOperationException>(() => target.Resolve<(string, int)>(null));
        }

        [Test]
        public void Given_EmptyOrderAndValidCollection_When_ResolvingSettings_Then_ThrowArgumentNullException()
        {
            Sorter<string, int> target = new Sorter<string, int>();

            var locationNames = new List<(string mode, string value)>()
            {
                ("Country","United States of America"),
                ("City","Las Vegas"),
                ("State","Nevada")
            };

            Assert.Throws<ArgumentNullException>(() => target.Resolve<(string, int)>(null));
        }

        private Sorter<string, int> buildTarget()
        {
            Sorter<string, int> target = new Sorter<string, int>();

            target.Order.Add("City", 100);
            target.Order.Add("State", 200);
            target.Order.Add("Country", 300);
            target.Order.Add("Default", int.MaxValue);

            return target;
        }
    }
}
