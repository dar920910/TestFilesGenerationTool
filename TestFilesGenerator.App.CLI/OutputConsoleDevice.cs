//-----------------------------------------------------------------------
// <copyright file="OutputConsoleDevice.cs" company="Demo Projects Workshop">
// Copyright (c) Demo Projects Workshop. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using TestFilesGenerator.Library;
using static System.Console;

/// <summary>
/// Represents static methods for console output of the program.
/// </summary>
internal static class OutputConsoleDevice
{
    /// <summary>
    /// Print a delimiter string consisting of an arbitrary count of symbols.
    /// </summary>
    /// <param name="symbol">User-defined symbol for a delimiter.</param>
    /// <param name="count">User-defined count of symbols for a delimiter.</param>
    internal static void PrintInformationDelimiter(
        char symbol = '-', byte count = 75) =>
            WriteLine("{0}\n", new string(symbol, count));

    /// <summary>
    /// Print a record matching to an instance of the CustomFileStorageCleaningResult type.
    /// </summary>
    /// <param name="result">Instance of the CustomFileStorageCleaningResult record.</param>
    internal static void PrintCustomFileStorageCleaningResult(CustomFileStorageCleaningResult result)
    {
        foreach (string message in result.OutputMessages)
        {
            WriteLine(message);
        }
    }
}
