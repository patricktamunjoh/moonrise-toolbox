using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MoonriseGames.Toolbox.Extensions;

namespace MoonriseGames.Toolbox.Security
{
    public class FileValidator
    {
        private const string VALIDATION_FILE_EXTENSION = ".lock";

        private string TargetFilePath { get; }
        private string ValidationFilePath { get; }

        private byte[] Key { get; }

        public FileValidator(string targetFilePath, string encryptionKey)
        {
            TargetFilePath = targetFilePath;
            ValidationFilePath = Path.ChangeExtension(targetFilePath, VALIDATION_FILE_EXTENSION);
            Key = Encoding.UTF8.GetBytes(encryptionKey);
        }

        public void GenerateValidationFile()
        {
            if (File.Exists(TargetFilePath).Not())
                return;

            var content = File.ReadAllBytes(TargetFilePath);

            using var hmac = new HMACSHA256(Key);
            var authenticationCode = hmac.ComputeHash(content);

            File.WriteAllBytes(ValidationFilePath, authenticationCode);
        }

        public void DeleteValidationFile()
        {
            if (File.Exists(ValidationFilePath).Not())
                return;

            File.Delete(ValidationFilePath);
        }

        public bool IsTargetFileValid()
        {
            if (File.Exists(ValidationFilePath).Not() || File.Exists(TargetFilePath).Not())
                return false;

            using var hmac = new HMACSHA256(Key);

            var authenticationCode = File.ReadAllBytes(ValidationFilePath);
            var content = File.ReadAllBytes(TargetFilePath);

            var expected = hmac.ComputeHash(content);

            return expected.SequenceEqual(authenticationCode);
        }
    }
}
