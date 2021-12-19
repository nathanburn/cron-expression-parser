namespace CronExpressionParser.Models
{
    public class CronExpressionArgsParser
    {
        private const int CRON_EXPRESSION_ARGS_LENGTH = 6;
        private static Dictionary<string, int> CRON_EXPRESSION_DAYS_OF_WEEK = new Dictionary<string, int> { 
            { "sun", 0 }, { "mon", 1 }, { "tue", 2 }, { "wed", 3 }, { "thu", 4 }, { "fri", 5 }, { "sat", 6 },
        };
        public static string CRON_EXPRESSION_EXAMPLE = "e.g. \"*/15 0 1,15 * 1-5 /usr/bin/find\"";
        private static Dictionary<string, int> CRON_EXPRESSION_MONTHS = new Dictionary<string, int> { 
            { "jan", 1 }, { "feb", 2 }, { "mar", 3 }, { "apr", 4 }, { "may", 5 }, { "jun", 6 },
            { "jul", 7 }, { "aug", 8 }, { "sep", 9 }, { "oct", 10 }, { "nov", 11 }, { "dec", 12 },
        };

        public String[] Arguments;
        public List<BaseArgument> CronArguments = new List<BaseArgument>
        {
            new CronArgument("minute", 59),
            new CronArgument("hour", 23),
            new CronArgument("day of month", 31, 1),
            new CronArgument("month", 12, 1, CRON_EXPRESSION_MONTHS),
            new CronArgument("day of week", 6, 0, CRON_EXPRESSION_DAYS_OF_WEEK),
            new BaseArgument("command")
        };

        public CronExpressionArgsParser(String args)
        {
            Arguments = args.Split(" ");

            if (Arguments.Length != CRON_EXPRESSION_ARGS_LENGTH) 
            {
                throw new Exception($"Failed, please provide {CRON_EXPRESSION_ARGS_LENGTH} cron arguments {CRON_EXPRESSION_EXAMPLE}");
            }

            for (int i = 0; i < Arguments.Length; i++) {
                CronArguments[i].SetInput(Arguments[i]);
            }
        }

        public override string ToString()
        {
            return String.Join("\n", CronArguments);
        }
    }
}