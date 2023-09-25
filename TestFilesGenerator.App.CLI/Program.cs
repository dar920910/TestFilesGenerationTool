//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Demo Projects Workshop">
// Copyright (c) Demo Projects Workshop. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Diagnostics;
using TestFilesGenerator.Library;
using static System.Console;

const string appName = "TestFilesGenerator.App.CLI";
Stopwatch programExecutionStopwatch = new ();

BeginRunning(appName, programExecutionStopwatch);

WriteLine("   >>> Initializing the output storage for IDs ...\n");
CustomFileStorage userFileStorage = new ();
List<CustomFileObject> userFileObjects = new ();

foreach (CustomFileCollection fileCollection in userFileStorage.UserFileCollections)
{
    List<CustomFileObject> collectionFileObjects = fileCollection.RetrieveFileObjects();

    foreach (var fileObject in collectionFileObjects)
    {
        userFileObjects.Add(fileObject);
    }
}

foreach (var fileObject in userFileObjects)
{
    CustomFileObjectCreationResult result = fileObject.Clone();
    OutputConsoleDevice.PrintInformationDelimiter();
    WriteLine(result.OutputMessage);
}

EndRunning(appName, programExecutionStopwatch);

void BeginRunning(string appName, Stopwatch stopwatch)
{
    stopwatch.Start();

    WriteLine("\n   >>> [ {0} ] {1} : Starting ...\n", DateTime.Now, appName);
    WriteLine("\n   {0} starts to process your task ...\n", appName);
}

void EndRunning(string appName, Stopwatch stopwatch)
{
    OutputConsoleDevice.PrintInformationDelimiter();
    WriteLine("   [SUCCESS]: {0} finished its work.\n", appName);
    WriteLine("   Would you like to delete the test data object?");

    do
    {
        Write("\n   Please press either 'YES' or 'NO' ");
        WriteLine("to finish the application ([Y(y)/N(n)]):");
    }
    while (!IsRightUserInput(ReadKey()));

    WriteLine("\n   >>> [ {0} ] {1} : Finishing ...\n", DateTime.Now, appName);

    if (stopwatch.IsRunning)
    {
        stopwatch.Stop();

        WriteLine($"\n[INFO] Elapsed Program's Execution Time: {stopwatch.Elapsed}\n");
    }
}

bool IsRightUserInput(ConsoleKeyInfo keyInfo)
{
    char answer = keyInfo.KeyChar;

    char upperYes = 'Y';
    char lowerYes = 'y';

    char upperNo = 'N';
    char lowerNo = 'n';

    bool answerYes = (answer == upperYes) | (answer == lowerYes);
    bool answerNo = (answer == upperNo) | (answer == lowerNo);

    if (answerYes)
    {
        CustomFileStorageCleaningResult result = CustomFileStorage.CleanBusyDiskSpace();
        OutputConsoleDevice.PrintCustomFileStorageCleaningResult(result);
        return true;
    }

    if (answerNo)
    {
        WriteLine("\n   [WARNING]: Please note the test data object keeps to locate on the storage!");
        WriteLine();
        return true;
    }

    Write("\n   [ERROR]: You have to select either 'YES' or 'NO'");
    WriteLine(" answer to quit from the app ([Y(y)/N(n)]).");
    WriteLine();

    return false;
}
