//-----------------------------------------------------------------------
// <copyright file="CustomFileObject.cs" company="Demo Projects Workshop">
// Copyright (c) Demo Projects Workshop. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TestFilesGenerator.Library;

/// <summary>
/// Represents file system's object which can be copied by a user.
/// </summary>
public class CustomFileObject
{
    private string resultOutputMessage;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFileObject"/> class.
    /// </summary>
    /// <param name="targetFileName">User-defined file name for a target file object.</param>
    /// <param name="sourceFileName">User-defined file name for a source file object.</param>
    public CustomFileObject(string targetFileName, string sourceFileName)
    {
        this.resultOutputMessage = default;

        try
        {
            this.TargetFileInfo = new FileInfo(targetFileName);
            this.SourceFileInfo = new FileInfo(sourceFileName);
        }
        catch (FileNotFoundException)
        {
            this.resultOutputMessage = "[ERROR]: Specified file is not found. The file object cannot be created.";
        }
    }

    /// <summary>
    /// Gets the file path for a source object in the file system.
    /// </summary>
    /// <value>The source file path.</value>
    public FileInfo SourceFileInfo { get; }

    /// <summary>
    /// Gets the file path for a target object in the file system.
    /// </summary>
    /// <value>The target file path.</value>
    public FileInfo TargetFileInfo { get; }

    /// <summary>
    /// Create a target file object from its source file.
    /// </summary>
    /// <returns>Result record with information about made cloning.</returns>
    public CustomFileObjectCreationResult Clone()
    {
        string sourceFile = this.SourceFileInfo.FullName;
        string targetFile = this.TargetFileInfo.FullName;
        string targetName = this.TargetFileInfo.Name;

        if (CustomFileStorage.CanCreateNewID(this))
        {
            File.Copy(sourceFile, targetFile, true);

            CustomFileStorage.CurrentFileObjects.Add(targetFile);
            CustomFileStorage.TotalSizeOfIDs += this.TargetFileInfo.Length;

            this.resultOutputMessage += this.GetCloningResultMessage(targetName);
        }
        else
        {
            this.resultOutputMessage += GetCloningErrorMessage(targetName);
        }

        this.resultOutputMessage +=
            $"\n   [?] Total Size of IDs : {CustomFileStorage.TotalSizeOfIDs} bytes." +
                $"\n   [?] Total Count of IDs : {CustomFileStorage.CurrentFileObjects.Count} files.\n";

        return new CustomFileObjectCreationResult(this.resultOutputMessage);
    }

    private static string GetCloningErrorMessage(string idName) =>
        "[CAUTION]: Cannot generate a new instance of the file. " +
            "There is no enough available space on the storage." +
                $"   [ERROR]: {idName} cannot be created via cloning.";

    private static string GetFileObjectCreationDateTime()
    {
        string sep = " - ";
        string dateSep = ".";
        string timeSep = ".";
        string zero = "0";
        bool ms = true;

        return GetDateAndTime(sep, dateSep, timeSep, ms, zero);
    }

    private static string GetDateAndTime(string sep, string dateSep, string timeSep, bool ms, string zero)
    {
        string datetime, date, time;

        date = GetDate(dateSep, zero);
        time = GetTime(timeSep, ms, zero);

        datetime = date + sep + time;

        return datetime;
    }

    private static string GetDate(string sep, string zero)
    {
        string date, dtYear, dtMonth, dtDay;
        int year, month, day;

        year = DateTime.Now.Year;
        month = DateTime.Now.Month;
        day = DateTime.Now.Day;

        dtYear = GetDateOrTimeWithZero(year, zero);
        dtMonth = GetDateOrTimeWithZero(month, zero);
        dtDay = GetDateOrTimeWithZero(day, zero);

        date = dtYear + sep + dtMonth + sep + dtDay;

        return date;
    }

    private static string GetTime(string sep, bool ms, string zero)
    {
        string time, tmHour, tmMinute, tmSecond;
        int hour, minute, second;

        hour = DateTime.Now.Hour;
        minute = DateTime.Now.Minute;
        second = DateTime.Now.Second;

        tmHour = GetDateOrTimeWithZero(hour, zero);
        tmMinute = GetDateOrTimeWithZero(minute, zero);
        tmSecond = GetDateOrTimeWithZero(second, zero);

        time = tmHour + sep + tmMinute + sep + tmSecond;

        if (ms)
        {
            int millisecond;
            string tmMillisecond;

            millisecond = DateTime.Now.Millisecond;
            tmMillisecond = GetMillisecondWithZero(millisecond, zero);

            time += sep + tmMillisecond;
        }

        return time;
    }

    private static string GetDateOrTimeWithZero(int datetimeValue, string zero)
    {
        return (datetimeValue < 10) ? zero + datetimeValue.ToString() : datetimeValue.ToString();
    }

    private static string GetMillisecondWithZero(int millisecond, string zero)
    {
        if (millisecond < 10)
        {
            return zero + zero + millisecond.ToString();
        }

        if (millisecond < 100)
        {
            return zero + millisecond.ToString();
        }

        return millisecond.ToString();
    }

    private string GetCloningResultMessage(string idName) =>
        $"   [{GetFileObjectCreationDateTime()}]: {idName}\n" +
        $"\n   [!] Available Space: {CustomFileStorage.GetFreeDiskSpaceOnStorage(this)} bytes.\n";
}

public record CustomFileObjectCreationResult
{
    /// <summary>
    /// Gets a result output message.
    /// </summary>
    /// <value>The string with a result message.</value>
    public string OutputMessage { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFileObjectCreationResult"/> class.
    /// </summary>
    /// <param name="message">Result information message to output.</param>
    public CustomFileObjectCreationResult(string message)
    {
        this.OutputMessage = message;
    }
}
