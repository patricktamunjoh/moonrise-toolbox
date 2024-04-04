using System.Collections.Generic;

namespace MoonriseGames.Toolbox.Validation
{
    public class ValidationResult
    {
        public List<Issue> Issues { get; } = new();
        public bool IsSuccess => Issues.Count == 0;

        public class Issue
        {
            public string Path { get; }
            public string Message { get; }

            public Issue(string path, string message)
            {
                Path = path;
                Message = message;
            }
        }

        public void AddIssue(string path, string message) => Issues.Add(new Issue(path, message));
    }
}
