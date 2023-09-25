using static System.Console;

internal static class OutputConsoleDevice
{
    internal static void PrintInformationDelimiter(
        char symbol = '-', byte count = 75) =>
            WriteLine("{0}\n", new string(symbol, count));
}
