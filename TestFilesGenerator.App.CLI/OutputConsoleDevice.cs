using TestFilesGenerator.Library;
using static System.Console;

internal static class OutputConsoleDevice
{
    internal static void PrintInformationDelimiter(
        char symbol = '-', byte count = 75) =>
            WriteLine("{0}\n", new string(symbol, count));

    internal static void PrintCustomFileStorageCleaningResult(CustomFileStorageCleaningResult result)
    {
        foreach (string message in result.OutputMessages)
        {
            WriteLine(message);
        }
    }
}
