using Xunit;
using CronExpressionParser.Models;
using System;

namespace CronExpressionParser.Tests;

public class CronExpressionArgsParserTest
{
    private static string CRON_EXPRESSION_MISSING_ARGUMENT = "*/15 0 1,15 * 1-5";

    private static string CRON_EXPRESSION_INVALID_ARGUMENT = "*/15 0 1,15 * a /usr/bin/find";

    // Note: interesting complexity in example provided as it specifies both 'day of week' AND 'day of month'.
    private const string CRON_EXPRESSION_1 = "*/15 0 1,15 * 1-5 /usr/bin/find";

    private const int CRON_EXPRESSION_1_EXPECTED_ARGUMENTS_LENGTH = 6;

    private const string CRON_EXPRESSION_1_EXPECTED_OUTPUT = "minute        0 15 30 45" +
                                                           "\nhour          0" +
                                                           "\nday of month  1 15" +
                                                           "\nmonth         1 2 3 4 5 6 7 8 9 10 11 12" +
                                                           "\nday of week   1 2 3 4 5" +
                                                           "\ncommand       /usr/bin/find";

    // every 5 minutes between 0 and 30 minutes
    private const string CRON_EXPRESSION_2 = "0-30/5 0 0 * 1-5 /usr/bin/find";

    private const int CRON_EXPRESSION_2_EXPECTED_ARGUMENTS_LENGTH = 6;

    private const string CRON_EXPRESSION_2_EXPECTED_OUTPUT = "minute        0 5 10 15 20 25 30" +
                                                           "\nhour          0" +
                                                           "\nday of month  0" +
                                                           "\nmonth         1 2 3 4 5 6 7 8 9 10 11 12" +
                                                           "\nday of week   1 2 3 4 5" +
                                                           "\ncommand       /usr/bin/find";

    // every 15 minutes starting at 10 minutes past
    private const string CRON_EXPRESSION_3 = "10/15 0 0 * 1-5 /usr/bin/find";

    private const int CRON_EXPRESSION_3_EXPECTED_ARGUMENTS_LENGTH = 6;

    private const string CRON_EXPRESSION_3_EXPECTED_OUTPUT = "minute        10 25 40 55" +
                                                           "\nhour          0" +
                                                           "\nday of month  0" +
                                                           "\nmonth         1 2 3 4 5 6 7 8 9 10 11 12" +
                                                           "\nday of week   1 2 3 4 5" +
                                                           "\ncommand       /usr/bin/find";

    // leave 'day of month' blank
    private const string CRON_EXPRESSION_4 = "*/15 0 ? * 1-5 /usr/bin/find";

    private const int CRON_EXPRESSION_4_EXPECTED_ARGUMENTS_LENGTH = 6;

    private const string CRON_EXPRESSION_4_EXPECTED_OUTPUT = "minute        0 15 30 45" +
                                                           "\nhour          0" +
                                                           "\nday of month  " +
                                                           "\nmonth         1 2 3 4 5 6 7 8 9 10 11 12" +
                                                           "\nday of week   1 2 3 4 5" +
                                                           "\ncommand       /usr/bin/find";

    // every 5 minutes between 0 and 30 minutes, Monday to Thursday between April to September
    private const string CRON_EXPRESSION_5 = "0-30/5 0 0 APR-SEP MON-THU /usr/bin/find";

    private const int CRON_EXPRESSION_5_EXPECTED_ARGUMENTS_LENGTH = 6;

    private const string CRON_EXPRESSION_5_EXPECTED_OUTPUT = "minute        0 5 10 15 20 25 30" +
                                                           "\nhour          0" +
                                                           "\nday of month  0" +
                                                           "\nmonth         4 5 6 7 8 9" +
                                                           "\nday of week   1 2 3 4" +
                                                           "\ncommand       /usr/bin/find";

    // every 5 minutes between 0 and 30 minutes, on Fridays in February
    private const string CRON_EXPRESSION_6 = "0-30/5 0 0 Feb fri /usr/bin/find";

    private const int CRON_EXPRESSION_6_EXPECTED_ARGUMENTS_LENGTH = 6;

    private const string CRON_EXPRESSION_6_EXPECTED_OUTPUT = "minute        0 5 10 15 20 25 30" +
                                                           "\nhour          0" +
                                                           "\nday of month  0" +
                                                           "\nmonth         2" +
                                                           "\nday of week   5" +
                                                           "\ncommand       /usr/bin/find";

    [Fact]
    public void ShouldThrowExceptionForMissingArgument()
    {
        Assert.Throws<Exception>(() => 
        { 
            var cronExpressionArgsParser = new CronExpressionArgsParser(CRON_EXPRESSION_MISSING_ARGUMENT);
        });
    }

    [Fact]
    public void ShouldThrowExceptionForInvalidArgument()
    {
        Assert.Throws<Exception>(() => 
        { 
            var cronExpressionArgsParser = new CronExpressionArgsParser(CRON_EXPRESSION_INVALID_ARGUMENT);
        });
    }

    [Theory,
        InlineData(CRON_EXPRESSION_1, CRON_EXPRESSION_1_EXPECTED_ARGUMENTS_LENGTH, CRON_EXPRESSION_1_EXPECTED_OUTPUT),
        InlineData(CRON_EXPRESSION_2, CRON_EXPRESSION_2_EXPECTED_ARGUMENTS_LENGTH, CRON_EXPRESSION_2_EXPECTED_OUTPUT),
        InlineData(CRON_EXPRESSION_3, CRON_EXPRESSION_3_EXPECTED_ARGUMENTS_LENGTH, CRON_EXPRESSION_3_EXPECTED_OUTPUT),
        InlineData(CRON_EXPRESSION_4, CRON_EXPRESSION_4_EXPECTED_ARGUMENTS_LENGTH, CRON_EXPRESSION_4_EXPECTED_OUTPUT),
        InlineData(CRON_EXPRESSION_5, CRON_EXPRESSION_5_EXPECTED_ARGUMENTS_LENGTH, CRON_EXPRESSION_5_EXPECTED_OUTPUT),
        InlineData(CRON_EXPRESSION_6, CRON_EXPRESSION_6_EXPECTED_ARGUMENTS_LENGTH, CRON_EXPRESSION_6_EXPECTED_OUTPUT)
    ]
    public void ShouldMatchCronExpressionExpectedOutput(
        string cronExpression,
        int cronExpressionExpectedArgumentsLength,
        string cronExpressionExpectedOutput
    )
    {
        var cronExpressionArgsParser = new CronExpressionArgsParser(cronExpression);

        Assert.Equal(cronExpressionExpectedArgumentsLength, cronExpressionArgsParser.Arguments.Length);
        Assert.Equal(cronExpressionExpectedOutput, cronExpressionArgsParser.ToString());
    }
}