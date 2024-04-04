using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Utilities;
using UnityEngine;

namespace MoonriseGames.Toolbox.Localization
{
    internal class LanguageFileReader
    {
        private char Delimiter { get; }
        private char Escape { get; }
        private char Comment { get; }

        public LanguageFileReader(char delimiter = ',', char escape = '"', char comment = '#')
        {
            Delimiter = delimiter;
            Escape = escape;
            Comment = comment;
        }

        public TranslationMap<string> ReadTranslationMap(TextAsset[] files)
        {
            Dictionary<string, TranslationSet<string>> sets = new();
            SystemLanguage[] languages = null;

            foreach (var set in files.SelectMany(ReadTranslationSets))
            {
                if (sets.ContainsKey(set.Key))
                    throw new ArgumentException($"Language files contains duplicate key {set.Key}");

                if (languages != null && languages.SequenceEqual(set.Languages).Not())
                    throw new ArgumentException($"Language files headers do not match");

                sets[set.Key] = set;
                languages ??= set.Languages;
            }

            return new TranslationMap<string>(sets.Values.ToArray());
        }

        private IEnumerable<TranslationSet<string>> ReadTranslationSets(TextAsset file)
        {
            var lines = GetContentLinesFromTextAsset(file);
            var languages = ReadLanguagesFromHeader(lines[0]);

            foreach (var line in lines.Skip(1))
            {
                var set = null as TranslationSet<string>;

                try
                {
                    var segments = ReadTextSegments(line);
                    set = GetTranslationSetFromSegments(segments, languages);
                }
                catch (ArgumentException e)
                {
                    throw new ArgumentException($"Language file parsing failed at \"{line}\"\n{e.Message}");
                }

                yield return set;
            }
        }

        private string[] GetContentLinesFromTextAsset(TextAsset file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var lines = file
                .text.Split('\n')
                .Select(x => x.Trim())
                .Where(x => x.StartsWith(Comment).Not())
                .Where(x => string.IsNullOrEmpty(x).Not())
                .ToArray();

            if (lines.Length < 2)
                throw new ArgumentException("Language file is empty");

            return lines;
        }

        private SystemLanguage[] ReadLanguagesFromHeader(string header)
        {
            if (header.Contains(Comment.ToString()))
                throw new ArgumentException($"Language file header contains illegal symbol \"{Comment}\"");

            if (header.Contains(Escape.ToString()))
                throw new ArgumentException($"Language file header contains illegal symbol \"{Escape}\"");

            var headers = header.Split(Delimiter).Select(x => x.Trim()).ToArray();

            if (headers.Length < 2)
                throw new Exception("Language file header is missing language definitions");

            return headers.Skip(1).Select(GetSystemLanguageFromName).ToArray();
        }

        private TranslationSet<string> GetTranslationSetFromSegments(string[] segments, SystemLanguage[] languages)
        {
            if (languages.Length != segments.Length - 1)
                throw new ArgumentException("Line content does not match file header");

            var values = segments.Skip(1).Zip(languages, (s, language) => new TranslationValue<string>(language, GetFormattedValue(s)));
            return new TranslationSet<string>(segments[0], values.ToArray());
        }

        private string[] ReadTextSegments(string line)
        {
            var segments = new List<string>(new[] { "" });
            var isEscaped = false;

            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == Delimiter && !isEscaped)
                {
                    segments.Add("");
                    continue;
                }

                if (line[i] == Escape && isEscaped && i + 1 < line.Length && line[i + 1] == Escape)
                {
                    i++;
                    segments[^1] += Escape;
                    continue;
                }

                if (line[i] == Escape)
                {
                    isEscaped = !isEscaped;
                    continue;
                }

                segments[^1] += line[i];
            }

            if (string.IsNullOrEmpty(segments[^1]))
                segments.RemoveAt(segments.Count - 1);

            if (isEscaped)
                throw new ArgumentException($"Escape sequence not terminated");

            return segments.Select(x => x.Trim()).ToArray();
        }

        private string GetFormattedValue(string value)
        {
            const string newLinePattern = @"(?<!\\)\\n";
            value = Regex.Replace(value, newLinePattern, "\n");
            return value;
        }

        private SystemLanguage GetSystemLanguageFromName(string name)
        {
            foreach (var value in TypeUtility.GetEnumValues<SystemLanguage>())
                if (string.Equals(value.ToString(), name, StringComparison.CurrentCultureIgnoreCase))
                    return value;

            throw new Exception($"No SystemLanguage found for query {name}");
        }
    }
}
