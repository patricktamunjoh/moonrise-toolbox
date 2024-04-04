using System.IO;
using MoonriseGames.Toolbox.Security;
using NUnit.Framework;

namespace MoonriseGames.Toolbox.Tests.Security
{
    public class FileValidatorTest
    {
        private string FilePath { get; set; }

        [SetUp]
        public void Setup()
        {
            FilePath = Path.GetTempFileName();
            File.WriteAllText(FilePath, "this is an example");
        }

        [Test]
        public void ShouldBeValidAfterGeneration()
        {
            var sut = new FileValidator(FilePath, "key");
            sut.GenerateValidationFile();

            Assert.True(sut.IsTargetFileValid());
        }

        [Test]
        public void ShouldBeValidWithEqualKey()
        {
            var sut01 = new FileValidator(FilePath, "key");
            var sut02 = new FileValidator(FilePath, "key");

            sut01.GenerateValidationFile();

            Assert.True(sut02.IsTargetFileValid());
        }

        [Test]
        public void ShouldBeInvalidWithDifferentKey()
        {
            var sut01 = new FileValidator(FilePath, "key");
            var sut02 = new FileValidator(FilePath, "different key");

            sut01.GenerateValidationFile();

            Assert.False(sut02.IsTargetFileValid());
        }

        [Test]
        public void ShouldGenerateValidationFileRelativeToTargetFile()
        {
            var sut = new FileValidator(FilePath, "key");
            sut.GenerateValidationFile();

            Assert.True(File.Exists(Path.ChangeExtension(FilePath, ".lock")));
        }

        [Test]
        public void ShouldBeInvalidBeforeGeneration()
        {
            var sut = new FileValidator(FilePath, "key");
            Assert.False(sut.IsTargetFileValid());
        }

        [Test]
        public void ShouldBeInvalidAfterDeletion()
        {
            var sut = new FileValidator(FilePath, "key");

            sut.GenerateValidationFile();
            sut.DeleteValidationFile();

            Assert.False(sut.IsTargetFileValid());
        }

        [Test]
        public void ShouldBeInvalidForMissingTarget()
        {
            var sut = new FileValidator(FilePath, "key");

            sut.GenerateValidationFile();
            File.Delete(FilePath);

            Assert.False(sut.IsTargetFileValid());
        }

        [Test]
        public void ShouldBeInvalidAfterTargetModification()
        {
            var sut = new FileValidator(FilePath, "key");

            sut.GenerateValidationFile();
            File.WriteAllText(FilePath, "this is something else");

            Assert.False(sut.IsTargetFileValid());
        }

        [Test]
        public void ShouldBeInvalidAfterLockModification()
        {
            var sut = new FileValidator(FilePath, "key");

            sut.GenerateValidationFile();
            File.WriteAllText(Path.ChangeExtension(FilePath, ".lock"), "invalid");

            Assert.False(sut.IsTargetFileValid());
        }
    }
}
