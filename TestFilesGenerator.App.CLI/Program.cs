using TestFilesGenerator.Library;
using static System.Console;


const string appName = "TestFilesGenerator.App.CLI";

BeginRunning(appName);
MediaStorage.InitializeOutputStorage();
var mediabaseService = new MediabaseService(new MediaBase());
mediabaseService.CloneIDs();
EndRunning(appName);


void BeginRunning(string appName)
{
    string dateOfStarting = DateTimeService.GetDateAndTimeDefaultString();
    WriteLine("\n   >>> [ {0} ] {1} : Starting ...\n", dateOfStarting, appName);
    WriteLine("\n   {0} starts to process your task ...\n", appName);
}

void EndRunning(string appName)
{
    WriteLine("{0}\n", GeneratorService.BuildSeparator());
    WriteLine("   [SUCCESS]: {0} finished its work.\n", appName);
    WriteLine("   Would you like to delete the test data object?");

    do
    {
        Write("\n   Please press either 'YES' or 'NO' ");
        WriteLine("to finish the application ([Y(y)/N(n)]):");
    } while (!IsRightUserInput(ReadKey()));

    string dateOfFinishing = DateTimeService.GetDateAndTimeDefaultString();
    WriteLine("\n   >>> [ {0} ] {1} : Finishing ...\n", dateOfFinishing, appName);
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
        MediaStorage.CleanBusyDiskSpace();
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
