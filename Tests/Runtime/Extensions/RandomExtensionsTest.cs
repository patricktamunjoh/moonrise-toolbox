using System;
using System.Linq;
using MoonriseGames.Toolbox.Extensions;
using Moq;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Extensions
{
    public class RandomExtensionsTest
    {
        [Test]
        public void ShouldSucceedCheckForProbabilityOne()
        {
            for (var i = 0; i < 100; i++)
                Assert.True(1f.Check());
        }

        [Test]
        public void ShouldFailCheckForProbabilityZero()
        {
            for (var i = 0; i < 100; i++)
                Assert.False(0f.Check());
        }

        [Test]
        public void ShouldSucceedCheckForRandomLeqProbability()
        {
            var random = new Mock<System.Random>();
            random.Setup(x => x.NextDouble()).Returns(0.5f);

            Assert.False(0.3f.Check(random.Object));
            Assert.True(0.5f.Check(random.Object));
            Assert.True(0.6f.Check(random.Object));
        }

        [Test]
        public void ShouldCheckUsingSystemRandom()
        {
            var random = new Mock<Random>();
            random.Setup(x => x.NextDouble()).Returns(0);

            0.3f.Check(random.Object);
            random.Verify(x => x.NextDouble());
        }

        [Test]
        public void ShouldReturnDefaultWhenSamplingNullArray()
        {
            Assert.AreEqual(default(int), (null as int[]).Sample());
            Assert.AreEqual(default(string), (null as string[]).Sample());
            Assert.AreEqual(default, (null as object[]).Sample());
        }

        [Test]
        public void ShouldReturnDefaultWhenSamplingEmptyArray()
        {
            Assert.AreEqual(default(int), Array.Empty<int>().Sample());
            Assert.AreEqual(default(string), Array.Empty<string>().Sample());
            Assert.AreEqual(default, Array.Empty<object>().Sample());
        }

        [Test]
        public void ShouldReturnDefaultWhenSamplingWeightedValueFromEmptyArray()
        {
            Assert.AreEqual(default(int), Array.Empty<int>().Sample(Array.Empty<float>()));
            Assert.AreEqual(default(string), Array.Empty<string>().Sample(Array.Empty<float>()));
            Assert.AreEqual(default, Array.Empty<object>().Sample(Array.Empty<float>()));
        }

        [Test]
        public void ShouldReturnDefaultWhenSamplingWeightedValueFromNullArray()
        {
            Assert.AreEqual(default(int), (null as int[]).Sample(Array.Empty<float>()));
            Assert.AreEqual(default(string), (null as string[]).Sample(Array.Empty<float>()));
            Assert.AreEqual(default, (null as object[]).Sample(Array.Empty<float>()));
        }

        [Test]
        public void ShouldThrowWhenSamplingWeightedValueWithInvalidWeights()
        {
            Assert.Throws<ArgumentException>(() => new[] { 1 }.Sample(new[] { 0f, 1f }));
            Assert.Throws<ArgumentException>(() => new[] { 1 }.Sample(null as float[]));
        }

        [Test]
        public void ShouldSampleValueFromArray()
        {
            var array = Enumerable.Range(0, 10).ToArray();
            for (var i = 0; i < 10; i++)
                Assert.Less(array.Sample(), 10);
        }

        [Test]
        public void ShouldSampleUsingSystemRandom()
        {
            var random = new Mock<Random>();
            random.Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(0);

            var array = Enumerable.Range(0, 10).ToArray();
            array.Sample(random.Object);

            random.Verify(x => x.Next(0, array.Length));
        }

        [Test]
        public void ShouldSampleWeightedValueFromArray()
        {
            var array = Enumerable.Range(0, 5).ToArray();

            Assert.AreEqual(0, array.Sample(new[] { 0f, 0f, 0f, 0f, 0f }));
            Assert.AreEqual(1, array.Sample(new[] { 0f, 1f, 0f, 0f, 0f }));
            Assert.AreEqual(2, array.Sample(new[] { 0f, 0f, 9f, 0f, 0f }));
        }

        [Test]
        public void ShouldSampleWeightedValueUsingSystemRandom()
        {
            var random = new Mock<Random>();
            random.Setup(x => x.NextDouble()).Returns(0);

            Enumerable.Range(0, 5).ToArray().Sample(new[] { 1f, 2f, 3f, 4f, 5f }, random.Object);

            random.Verify(x => x.NextDouble());
        }

        [Test]
        public void ShouldShuffleArray()
        {
            var array = Enumerable.Range(0, 100).ToArray();
            Assert.False(Enumerable.Range(0, 100).SequenceEqual(array.Shuffled()));
        }

        [Test]
        public void ShouldShuffleArrayUsingSystemRandom()
        {
            var random = new Mock<Random>();
            random.Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(0);

            var array = Enumerable.Range(0, 100).ToArray();
            Assert.False(Enumerable.Range(0, 100).SequenceEqual(array.Shuffled(random.Object)));

            random.Verify(x => x.Next(It.IsAny<int>(), It.IsAny<int>()));
        }
    }
}
