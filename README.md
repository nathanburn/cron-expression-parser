# Cron Expression Parser by Nathan Burn

## Pre-requistites

- Install [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Run Example

For example, the following input argument:
```
cd src/CronExpressionParser
dotnet run "*/15 0 1,15 * 1-5 /usr/bin/find"
```

Should yield the following output:
```
minute        0 15 30 45
hour          0
day of month  1 15
month         1 2 3 4 5 6 7 8 9 10 11 12
day of week   1 2 3 4 5
command       /usr/bin/find
```

## Run Tests

```
cd test/CronExpressionParser.Tests
dotnet test
```