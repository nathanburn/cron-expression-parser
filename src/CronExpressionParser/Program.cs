using CronExpressionParser.Models;

namespace CronExpressionParser
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            if (args.Length < 1) 
            {
                throw new Exception($"Failed, please provide a cron argument {CronExpressionArgsParser.CRON_EXPRESSION_EXAMPLE}");
            }
            var cronExpressionArgsParser = new CronExpressionArgsParser(args[0]);
            Console.WriteLine(cronExpressionArgsParser);
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(((Exception)e.ExceptionObject).Message);
            Environment.Exit(0);
        }
    }
}