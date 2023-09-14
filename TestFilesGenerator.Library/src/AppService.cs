using static System.Console;

namespace TestFilesGenerator.Library;

public static class AppService
{
        private static string name = "NxMediabaseBuilder";
        private static string dateOfStarting;
        private static string dateOfFinishing;

        public static void Launch()
        {
            BeginRunning();

            MediaStorage.InitializeOutputStorage();

            var mediabaseService = new MediabaseService(new MediaBase());
            mediabaseService.CloneIDs();

            EndRunning();
        }

        private static void Begin()
        {
            dateOfStarting = FixDateAndTime();
            WriteLine("\n   >>> [ {0} ] {1} : Starting ...\n", dateOfStarting, name);
        }

        private static void End()
        {
            dateOfFinishing = FixDateAndTime();
            WriteLine("\n   >>> [ {0} ] {1} : Finishing ...\n", dateOfFinishing, name);
        }

        private static string FixDateAndTime()
        {
            return DateTimeService.GetDateAndTimeDefaultString();
        }

        private static void BeginRunning()
        {
            WriteLine("\n   {0} starts to process your task ...\n", name);
        }
        private static void EndRunning()
        {
            WriteLine("{0}\n", GeneratorService.BuildSeparator());
            WriteLine("   [SUCCESS]: {0} finished its work.\n", name);
            WriteLine("   Would you like to delete the test data object?");

            do
            {
                Write("\n   Please press either 'YES' or 'NO' ");
                WriteLine("to finish the application ([Y(y)/N(n)]):");
            } while (!IsRightUserInput(ReadKey()));
        }

        private static bool IsRightUserInput(ConsoleKeyInfo keyInfo)
        {
            char answer = keyInfo.KeyChar;

            char upperYes = 'Y';
            char lowerYes = 'y';

            char upperNo = 'N';
            char lowerNo = 'n';

            bool answerYes = ( (answer == upperYes) | (answer == lowerYes) );
            bool answerNo = ( (answer == upperNo) | (answer == lowerNo) );

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
}
