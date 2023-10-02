//-----------------------------------------------------------------------
// <copyright file="RandomFileObjectName.cs" company="Demo Projects Workshop">
// Copyright (c) Demo Projects Workshop. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Represents a random name for a file object from a custom file collection.
/// </summary>
public class RandomFileObjectName
{
    private const char LowerCaseFirstLetter = 'a';
    private const char UpperCaseFirstLetter = 'A';
    private const char LowerCaseLastLetter = 'z';
    private const char UpperCaseLastLetter = 'Z';

    private static readonly char[] LowerCaseLettersArray;
    private static readonly char[] UpperCaseLettersArray;

    private readonly byte filenameLength;

    static RandomFileObjectName()
    {
        LowerCaseLettersArray = GetAlphabetLettersArray(LowerCaseFirstLetter, LowerCaseLastLetter);
        UpperCaseLettersArray = GetAlphabetLettersArray(UpperCaseFirstLetter, UpperCaseLastLetter);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomFileObjectName"/> class.
    /// </summary>
    /// <param name="length">Length of the target name.</param>
    public RandomFileObjectName(byte length)
    {
        this.filenameLength = length;
    }

    /// <summary>
    /// Create a random file name for a file object of a custom file collection.
    /// </summary>
    /// <returns>Random file name of a file object from a custom file collection.</returns>
    public string Create()
    {
        string targetRandomIdName = string.Empty;

        for (byte i = 0; i < this.filenameLength; i++)
        {
            char letter;

            if (IsUpperCase())
            {
                letter = GetRandomLetterFromArray(UpperCaseLettersArray);
            }
            else
            {
                letter = GetRandomLetterFromArray(LowerCaseLettersArray);
            }

            targetRandomIdName += Convert.ToString(letter);
        }

        return targetRandomIdName;
    }

    private static char[] GetAlphabetLettersArray(char alphabetFirstLetter, char alphabetLastLetter)
    {
        List<char> alphabetLowerCaseLetters = new ();

        for (char alphabetLetter = alphabetFirstLetter; alphabetLetter <= alphabetLastLetter; alphabetLetter++)
        {
            alphabetLowerCaseLetters.Add(alphabetLetter);
        }

        return alphabetLowerCaseLetters.ToArray();
    }

    private static bool IsUpperCase() =>
        new Random().Next(0, 2) == 1;

    private static char GetRandomLetterFromArray(char[] array) =>
        array[new Random().Next(minValue: 0, maxValue: array.Length)];
}
