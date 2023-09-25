//-----------------------------------------------------------------------
// <copyright file="CustomFileObject.cs" company="DemoProjectsWorkshop">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace TestFilesGenerator.Library;

public class CustomFileObject
{
    private string resultOutputMessage;

    public FileInfo SourceFileInfo { get; }

    public FileInfo TargetFileInfo { get; }

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
    /// Create a target file object from its source file.
    /// </summary>
    /// <returns>Result record with information about made cloning.</returns>
    public CustomFileObjectCreationResult Clone()
    {
        string sourceFile = this.SourceFileInfo.FullName;
        string targetFile = this.TargetFileInfo.FullName;
        string targetName = this.TargetFileInfo.Name;

        if (FileDriveManager.CanCreateNewID(this))
        {
            File.Copy(sourceFile, targetFile, true);

            FileDriveManager.CurrentFileObjects.Add(targetFile);
            FileDriveManager.TotalSizeOfIDs += this.TargetFileInfo.Length;

            this.resultOutputMessage += this.GetCloningResultMessage(targetName);
        }
        else
        {
            this.resultOutputMessage += GetCloningErrorMessage(targetName);
        }

        FileDriveManager.ShowStorageStatus();

        return new CustomFileObjectCreationResult(this.resultOutputMessage);
    }

    private static string GetCloningErrorMessage(string idName) =>
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
        $"   [!] Available Space: {FileDriveManager.GetFreeDiskSpaceOnStorage(this)} bytes.";
}

public record CustomFileObjectCreationResult
{
    public string OutputMessage { get; }
    public CustomFileObjectCreationResult(string message)
    {
        this.OutputMessage = message;
    }
}
