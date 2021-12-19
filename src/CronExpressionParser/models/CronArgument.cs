using System.Text.RegularExpressions;

namespace CronExpressionParser.Models
{
    public class CronArgument : BaseArgument
    {
        private const string ANY = "*";
        private const string BLANK = "?";
        private const char COMMA = ',';
        private const char EVERY = '/';
        private const char RANGE = '-';

        private static string NUMBER_REGEX = $@"([0-9]+)";
        private static string EVERY_REGEX = $@"(\{ANY}|[0-9]+|[0-9]+{RANGE}[0-9]+){EVERY}([0-9]+)";
        private static string RANGE_REGEX = $@"([0-9]+){RANGE}([0-9]+)";
        private static string VALID_CRON_ARGUMENT_REGEX = $@"\{ANY}|\{BLANK}|[0-9]+|{RANGE_REGEX}|{EVERY_REGEX}";

        public int LowerBoundary;

        public int UpperBoundary;

        public Dictionary<string, int>? StringRepresentations;

        public CronArgument(
            string label,
            int upperBoundary,
            int lowerBoundary = 0,
            Dictionary<string, int>? stringRepresentations = null
        ):base(label)
        {
            this.UpperBoundary = upperBoundary;
            this.LowerBoundary = lowerBoundary;
            this.StringRepresentations = stringRepresentations;
        }

        public override void SetInput(string value)
        {
            // replace string representations e.g. MON-FRI to 1-5
            if (this.StringRepresentations != null
                && value.Length > 0
                && char.IsLetter(value[0])) {
                value = value.ToLower();
                foreach (var stringRepresentation in this.StringRepresentations) {
                    value = value.Replace(stringRepresentation.Key, stringRepresentation.Value.ToString());
                }
            }

            if (!new Regex(VALID_CRON_ARGUMENT_REGEX).IsMatch(value))
            {
                throw new Exception($"Failed, invalid value: {value} for '{Label}'");
            }

            this.Input = value;
        }

        public override string ToString()
        {
            return $"{GetLabelWithWhiteSpaces()}{GetInputExpansion()}";
        }

        private string GetInputExpansion()
        {
            string expansion = Input;

            switch(Input) {
                // e.g. *
                case ANY:
                {
                    expansion = GetRange(LowerBoundary, UpperBoundary);
                    break;
                }
                // e.g. ?
                case BLANK:
                {
                    expansion = string.Empty;
                    break;
                }
                // e.g. 1,15
                case string i when i.Contains(COMMA):
                {
                    expansion = String.Join(' ', Input.Split(COMMA));
                    break;
                }
                // e.g. */15 or 10/15 or 0-30/15
                case string i when new Regex(EVERY_REGEX).IsMatch(i):
                {
                    expansion = GetEveryRegexRange(Input);
                    break;
                }
                // e.g. 0-30
                case string i when new Regex(RANGE_REGEX).IsMatch(i):
                {
                    expansion = GetRegexRange(Input);
                    break;
                }
            }

            return expansion;
        }

        private string GetRange(int start, int end, int increment = 1) {
            string range = string.Empty;
            for(int i = start; i <= end; i += increment) {
                range += $"{i} ";
            }
            return range.Trim();
        }

        private string GetRegexRange(string value, int increment = 1) {
            MatchCollection rangeMatches = Regex.Matches(Input, RANGE_REGEX);
            return GetRange(int.Parse(rangeMatches[0].Groups[1].Value), int.Parse(rangeMatches[0].Groups[2].Value), increment);
        }

        private string GetEveryRegexRange(string value) {
            string everyRegexRange = value;

            MatchCollection everyMatches = Regex.Matches(Input, EVERY_REGEX);
            var firstGroupValue = everyMatches[0].Groups[1].Value;
            var every = int.Parse(everyMatches[0].Groups[2].Value);

            switch(firstGroupValue) {
                case ANY:
                {
                    everyRegexRange = GetRange(LowerBoundary, UpperBoundary, int.Parse(everyMatches[0].Groups[2].Value));
                    break;
                }
                case string i when new Regex(RANGE_REGEX).IsMatch(i):
                {
                    everyRegexRange = GetRegexRange(firstGroupValue, every);
                    break;
                }
                case string i when new Regex(NUMBER_REGEX).IsMatch(i):
                {
                    everyRegexRange = GetRange(int.Parse(firstGroupValue), UpperBoundary, int.Parse(everyMatches[0].Groups[2].Value));
                    break;
                }
            }

            return everyRegexRange;
        }
    }
}