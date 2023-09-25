//-----------------------------------------------------------------------
// <copyright file="CustomFileCollection.cs" company="Demo Projects Workshop">
// Copyright (c) Demo Projects Workshop. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TestFilesGenerator.Library;

using System.Xml.Serialization;

public class CustomFileCollection
{
    public CustomFileCollection()
    {
    }

    public CustomFileCollection(string alias, string source, uint count)
    {
        this.Alias = alias;
        this.SourceFileObject = source;
        this.CountOfObjects = count;
        this.IsRandom = false;
    }

    public CustomFileCollection(string alias, string source, uint count, bool isRandom)
    {
        this.Alias = alias;
        this.SourceFileObject = source;
        this.CountOfObjects = count;
        this.IsRandom = isRandom;
    }

    [XmlAttribute("Alias")]
    public string Alias { get; set; }

    [XmlAttribute("SourceFileObject")]
    public string SourceFileObject { get; set; }

    [XmlAttribute("CountOfIDs")]
    public uint CountOfObjects { get; set; }

    [XmlAttribute("RandomMode")]
    public bool IsRandom { get; set; }

    public List<CustomFileObject> RetrieveFileObjects()
    {
        var fileObjects = new List<CustomFileObject>();
        CustomFileObject[] arrayOfFileObjects = this.GenerateFileObjects();

        foreach (var fileObject in arrayOfFileObjects)
        {
            fileObjects.Add(fileObject);
        }

        return fileObjects;
    }

    private CustomFileObject[] GenerateFileObjects()
    {
        var arrayOfFileObjects = new CustomFileObject[this.CountOfObjects];

        for (uint idCounter = 0; idCounter < arrayOfFileObjects.Length; idCounter++)
        {
            string fileName = this.IsRandom ? this.GenerateRandomIdName() : this.GenerateCustomIdName(idCounter + 1);
            string filePath = this.GenerateTargetIdPath(fileName);
            arrayOfFileObjects[idCounter] = new CustomFileObject(filePath, this.SourceFileObject);
        }

        return arrayOfFileObjects;
    }

    private string GenerateTargetIdPath(string targetIdName)
    {
        string targetDirectory = CustomFileStorage.GetCustomFileCollectionDirectory(this.Alias);
        return Path.Combine(targetDirectory, targetIdName);
    }

    private string GenerateCustomIdName(uint idNumber) =>
        $"{this.Alias}_{new CollectionFileObjectNumberId(idNumber).Create()}{this.GetFileExtensionFromSource()}";

    private string GenerateRandomIdName()
    {
        // File object ID can be consisting from 32 characters.
        // I set the rule that the name has 16 lower and 16 upper case letters.
        return new RandomFileObjectName(16, 16).Create() + this.GetFileExtensionFromSource();
    }

    private string GetFileExtensionFromSource()
    {
        return new FileInfo(this.SourceFileObject).Extension;
    }
}

/// <summary>
/// Represents a numeric ID for a file object from a custom file collection.
/// </summary>
public class CollectionFileObjectNumberId
{
    private const uint ValueLevel1 = 1_000_000;
    private const uint ValueLevel2 = 100_000;
    private const uint ValueLevel3 = 10_000;
    private const uint ValueLevel4 = 1_000;
    private const uint ValueLevel5 = 100;
    private const uint ValueLevel6 = 10;

    private readonly uint fileObjectNumber;

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionFileObjectNumberId"/> class.
    /// </summary>
    /// <param name="number">Number of a file object in a custom file collection.</param>
    public CollectionFileObjectNumberId(uint number)
    {
        this.fileObjectNumber = number;
    }

    /// <summary>
    /// Create a numeric ID for a file object.
    /// </summary>
    /// <returns>Numeric ID of a file object.</returns>
    public string Create()
    {
        string targetNumber;

        if (this.fileObjectNumber < ValueLevel6)
        {
            targetNumber = AddZeroes(6);
        }
        else if (this.fileObjectNumber < ValueLevel5)
        {
            targetNumber = AddZeroes(5);
        }
        else if (this.fileObjectNumber < ValueLevel4)
        {
            targetNumber = AddZeroes(4);
        }
        else if (this.fileObjectNumber < ValueLevel3)
        {
            targetNumber = AddZeroes(3);
        }
        else if (this.fileObjectNumber < ValueLevel2)
        {
            targetNumber = AddZeroes(2);
        }
        else if (this.fileObjectNumber < ValueLevel1)
        {
            targetNumber = AddZeroes(1);
        }
        else
        {
            targetNumber = string.Empty;
        }

        return targetNumber + this.fileObjectNumber.ToString();
    }

    private static string AddZeroes(int countZeroes)
    {
        string zeroString = string.Empty;

        for (int i = 0; i < countZeroes; i++)
        {
            zeroString += AddZero();
        }

        return zeroString;
    }

    private static string AddZero() => "0";
}

/// <summary>
/// Represents a random name for a file object from a custom file collection.
/// </summary>
public class RandomFileObjectName
{
    private readonly byte nameLength;
    private readonly byte nameLowers;
    private readonly byte nameUppers;

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomFileObjectName"/> class.
    /// </summary>
    /// <param name="lowers">Count of letters in lower-case in the target name.</param>
    /// <param name="uppers">Count of letters in upper-case in the target name.</param>
    public RandomFileObjectName(byte lowers, byte uppers)
    {
        this.nameLength = (byte)(lowers + uppers);
        this.nameLowers = lowers;
        this.nameUppers = uppers;
    }

    /// <summary>
    /// Create a random file name for a file object of a custom file collection.
    /// </summary>
    /// <returns>Random file name of a file object from a custom file collection.</returns>
    public string Create()
    {
        string targetRandomIdName = string.Empty;

        char[] lowersArray = this.GenerateLetters(this.nameLowers, false);
        char[] uppersArray = this.GenerateLetters(this.nameUppers, true);

        List<char> lowersList = this.FillListOfLetters(lowersArray);
        List<char> uppersList = this.FillListOfLetters(uppersArray);

        for (byte i = 0; i < this.nameLength; i++)
        {
            char letter;

            if (this.IsUpperCase())
            {
                letter = this.RetrieveLetterFromList(ref uppersList);
            }
            else
            {
                letter = this.RetrieveLetterFromList(ref lowersList);
            }

            targetRandomIdName += Convert.ToString(letter);
        }

        return targetRandomIdName;
    }

    private bool IsUpperCase()
    {
        // This is logic to choose letter's case by random way.
        int random = new Random().Next(0, 2);

        if (random == 1)
        {
            return true;
        }

        return false;
    }

    private char RetrieveLetterFromList(ref List<char> lettersList)
    {
        char letter;
        byte random;

        try
        {
            random = Convert.ToByte(new Random().Next(0, lettersList.Count));

            // Retrieve a random letter from the list.
            letter = lettersList[(int)random];

            // Remove this retrieved letter from the list.
            lettersList.RemoveAt((int)random);
        }
        catch (ArgumentOutOfRangeException)
        {
            // The following code was added to process the exception throwing.
            random = Convert.ToByte(new Random().Next(0, 2));
            letter = (random == 1) ? '#' : '$';
        }

        return letter;
    }

    private List<char> FillListOfLetters(char[] lettersArray)
    {
        var lettersList = new List<char>(lettersArray.Length);

        foreach (var letter in lettersArray)
        {
            lettersList.Add(letter);
        }

        return lettersList;
    }

    private char[] GenerateLetters(int count, bool isUpper)
    {
        var letters = new char[count];

        if (isUpper)
        {
            this.PopulateUppersArray(ref letters);
        }
        else
        {
            this.PopulateLowersArray(ref letters);
        }

        return letters;
    }

    private void PopulateLowersArray(ref char[] letters)
    {
        for (byte i = 0; i < letters.Length; i++)
        {
            letters[i] = this.GetRandomLowerLetter();
        }
    }

    private void PopulateUppersArray(ref char[] letters)
    {
        for (byte i = 0; i < letters.Length; i++)
        {
            letters[i] = this.GetRandomUpperLetter();
        }
    }

    private char GetRandomLowerLetter()
    {
        // I use ASCII notation for generating random character:
        // 97 integer literal is 'a' letter from ASCII
        // 122 integer literal is 'z' (ASCII)
        int random = new Random().Next(97, 122 + 1);
        return Convert.ToChar(random);
    }

    private char GetRandomUpperLetter()
    {
        // I use ASCII notation for generating random character:
        // 65 integer literal is 'A' letter from ASCII
        // 90 integer literal is 'Z' (ASCII)
        int random = new Random().Next(65, 90 + 1);
        return Convert.ToChar(random);
    }
}
