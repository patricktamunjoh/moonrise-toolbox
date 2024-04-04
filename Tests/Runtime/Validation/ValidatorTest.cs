using System;
using System.Collections.Generic;
using MoonriseGames.Toolbox.Constants;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Validation;
using NUnit.Framework;
using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Validation
{
    public class ValidatorTest
    {
        [Test]
        public void ShouldBeSuccessfulForValidFields()
        {
            var sut = new Validator();
            var sample = new Sample();

            sut.Validate(sample);

            Assert.True(sut.Result.IsSuccess);
        }

        [Test]
        public void ShouldBeSuccessfulForIgnoredFields()
        {
            var sut = new Validator();
            var sample = new ValidSample();

            sut.Validate(sample);

            Assert.True(sut.Result.IsSuccess);
        }

        [Test]
        public void ShouldBeSuccessfulForIgnoredClass()
        {
            var sut = new Validator();
            var sample = new NoValidationSample();

            sut.Validate(sample);

            Assert.True(sut.Result.IsSuccess);
        }

        [Test]
        public void ShouldBeSuccessfulForIgnoredTags()
        {
            var sut = new Validator();
            var sample = new GameObject().AddComponent<InvalidSampleBehaviour>();

            sample.tag = Tags.NO_VALIDATION;
            sut.Validate(sample);

            Assert.True(sut.Result.IsSuccess);
        }

        [Test]
        public void ShouldNotBeSuccessfulForMissingField()
        {
            var sut = new Validator();
            var sample = new Sample { SerializedField = null };

            sut.Validate(sample);

            Assert.False(sut.Result.IsSuccess);
            Assert.AreEqual(1, sut.Result.Issues.Count);
            Assert.AreEqual($"{nameof(Sample)}/{nameof(sample.SerializedField)}", sut.Result.Issues[0].Path);
        }

        [Test]
        public void ShouldNotBeSuccessfulForMissingCollection()
        {
            var sut = new Validator();
            var sample = new Sample { SerializedArray = null };

            sut.Validate(sample);

            Assert.False(sut.Result.IsSuccess);
            Assert.AreEqual(1, sut.Result.Issues.Count);
            Assert.AreEqual($"{nameof(Sample)}/{nameof(sample.SerializedArray)}", sut.Result.Issues[0].Path);
        }

        [Test]
        public void ShouldNotBeSuccessfulForMissingArrayItem()
        {
            var sut = new Validator();
            var sample = new Sample { SerializedArray = new object[1] };

            sut.Validate(sample);

            Assert.False(sut.Result.IsSuccess);
            Assert.AreEqual(1, sut.Result.Issues.Count);
            Assert.AreEqual($"{nameof(Sample)}/{nameof(sample.SerializedArray)}/[0]", sut.Result.Issues[0].Path);
        }

        [Test]
        public void ShouldNotBeSuccessfulForMissingListItem()
        {
            var sut = new Validator();
            var sample = new Sample { SerializedList = new List<object>(new object[1]) };

            sut.Validate(sample);

            Assert.False(sut.Result.IsSuccess);
            Assert.AreEqual(1, sut.Result.Issues.Count);
            Assert.AreEqual($"{nameof(Sample)}/{nameof(sample.SerializedList)}/[0]", sut.Result.Issues[0].Path);
        }

        [Test]
        public void ShouldNotBeSuccessfulForInvalidObject()
        {
            var sut = new Validator();
            var sample = new InvalidSample();

            sut.Validate(sample);

            Assert.False(sut.Result.IsSuccess);
            Assert.AreEqual(1, sut.Result.Issues.Count);
            Assert.AreEqual($"{nameof(InvalidSample)}", sut.Result.Issues[0].Path);
        }

        [Test]
        public void ShouldNotBeSuccessfulForInvalidBehaviour()
        {
            var sut = new Validator();
            var sample = new GameObject().AddComponent<InvalidSampleBehaviour>();

            sut.Validate(sample);

            Assert.False(sut.Result.IsSuccess);
            Assert.AreEqual(1, sut.Result.Issues.Count);
            Assert.AreEqual($"{nameof(InvalidSampleBehaviour)}", sut.Result.Issues[0].Path);
        }

        [Test]
        public void ShouldNotBeSuccessfulForInvalidObjectInField()
        {
            var sut = new Validator();
            var sample = new Sample { SerializedField = new InvalidSample() };

            sut.Validate(sample);

            Assert.False(sut.Result.IsSuccess);
            Assert.AreEqual(1, sut.Result.Issues.Count);
            Assert.AreEqual($"{nameof(Sample)}/{nameof(sample.SerializedField)}", sut.Result.Issues[0].Path);
        }

        [Test]
        public void ShouldNotBeSuccessfulForInvalidObjectInArray()
        {
            var sut = new Validator();
            var sample = new Sample { SerializedArray = new[] { new InvalidSample() } };

            sut.Validate(sample);

            Assert.False(sut.Result.IsSuccess);
            Assert.AreEqual(1, sut.Result.Issues.Count);
            Assert.AreEqual($"{nameof(Sample)}/{nameof(sample.SerializedArray)}/[0]", sut.Result.Issues[0].Path);
        }

        [Test]
        public void ShouldNotBeSuccessfulForInvalidObjectInList()
        {
            var sut = new Validator();
            var sample = new Sample { SerializedList = new List<object>(new[] { new InvalidSample() }) };

            sut.Validate(sample);

            Assert.False(sut.Result.IsSuccess);
            Assert.AreEqual(1, sut.Result.Issues.Count);
            Assert.AreEqual($"{nameof(Sample)}/{nameof(sample.SerializedList)}/[0]", sut.Result.Issues[0].Path);
        }

        [Test]
        public void ShouldFindAllIssues()
        {
            var sut = new Validator();
            var sample = new Sample { SerializedArray = new object[2], SerializedField = null };

            sut.Validate(sample);

            Assert.AreEqual(3, sut.Result.Issues.Count);
        }

        [Test]
        public void ShouldValidateObjectOnlyOnce()
        {
            var sut = new Validator();
            var sample = new Sample();

            sut.Validate(sample);

            sample.IsValid = false;
            sut.Validate(sample);

            Assert.True(sut.Result.IsSuccess);
        }

        private class Sample : IValidateable
        {
            public bool IsValid { get; set; } = true;

            [SerializeField]
            public object SerializedField = "value";

            [SerializeField]
            public object[] SerializedArray = Array.Empty<object>();

            [SerializeField]
            public List<object> SerializedList = new();

            public void Validate()
            {
                if (IsValid.Not())
                    throw new ValidationException("invalid");
            }
        }

        private class ValidSample : IValidateable
        {
            private GameObject _nonSerializedField = null;

            [NoValidation]
            [SerializeField]
            private GameObject _serializedFieldWithNoValidation = null;

            [Optional]
            [SerializeField]
            private GameObject _serializedFieldWithOptional = null;

            [SerializeField]
            private HashSet<object> _enumerableWithInvalidValue = new(new[] { new InvalidSample() });

            public void Validate() { }
        }

        private class InvalidSample : IValidateable
        {
            public void Validate()
            {
                throw new ValidationException("invalid");
            }
        }

        private class InvalidSampleBehaviour : MonoBehaviour, IValidateable
        {
            public void Validate()
            {
                throw new ValidationException("invalid");
            }
        }

        [NoValidation]
        private class NoValidationSample : IValidateable
        {
            [SerializeField]
            private GameObject value = null;

            public void Validate()
            {
                throw new ValidationException("invalid");
            }
        }
    }
}
