using MoonriseGames.Toolbox.Extensions;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class ReflectionExtensionsTest
    {
        [Test]
        public void ShouldSetNonPublicField()
        {
            var sample = new Sample();
            sample.SetNonPublicField("example value");
            Assert.AreEqual("example value", sample.Value);
        }

        [Test]
        public void ShouldHandleMissingNonPublicField()
        {
            var sample = new Sample();
            sample.SetNonPublicField(12);
            sample.SetNonPublicField("missing field", 12);
        }

        [Test]
        public void ShouldSetNonPublicFieldByName()
        {
            var sample = new Sample();
            sample.SetNonPublicField("value", "example value");
            Assert.AreEqual("example value", sample.Value);
        }

        private class Sample
        {
            private string value;
            public string Value => value;
        }
    }
}
