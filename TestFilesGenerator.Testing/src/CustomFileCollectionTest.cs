//-----------------------------------------------------------------------
// <copyright file="CustomFileCollectionTest.cs" company="Demo Projects Workshop">
// Copyright (c) Demo Projects Workshop. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TestFilesGenerator.Testing;

using TestFilesGenerator.Library;

/// <summary>
/// Contains unit tests for the CustomFileCollection class.
/// </summary>
public class CustomFileCollectionTest
{
    /// <summary>
    /// Tests retrieving names of file objects by collection's alias, source, and count.
    /// This test method is intended to explore the unit test when debugging session.
    /// </summary>
    [Test]
    public void RetrieveFileObjectNamesTestCase_1()
    {
        List<string> actualFileObjectNames = CustomFileCollection.RetrieveFileObjectNames(
                alias: "Document", source: "file.md", count: 5);

        List<string> expectedFileObjectNames = new ()
        {
            "Document_0000001.md",
            "Document_0000002.md",
            "Document_0000003.md",
            "Document_0000004.md",
            "Document_0000005.md",
        };

        Assert.That(actual: actualFileObjectNames, expression: Is.EqualTo(expectedFileObjectNames));
    }

    /// <summary>
    /// Tests retrieving names of file objects by collection's alias, source, and count.
    /// This test method is intended to demonstrate more possible examples of the test.
    /// </summary>
    [Test]
    public void RetrieveFileObjectNamesTestCase_2()
    {
        Assert.Multiple(() =>
        {
            Assert.That(
            actual: CustomFileCollection.RetrieveFileObjectNames(
                alias: "TextFile", source: "file.txt", count: 2),
            expression: Is.EqualTo(new List<string>()
            {
                "TextFile_0000001.txt",
                "TextFile_0000002.txt",
            }));

            Assert.That(
                actual: CustomFileCollection.RetrieveFileObjectNames(
                    alias: "SourceCode", source: "source.cs", count: 4),
                expression: Is.EqualTo(new List<string>()
                {
                    "SourceCode_0000001.cs",
                    "SourceCode_0000002.cs",
                    "SourceCode_0000003.cs",
                    "SourceCode_0000004.cs",
                }));

            Assert.That(
                actual: CustomFileCollection.RetrieveFileObjectNames(
                    alias: "WebPage", source: "index.html", count: 6),
                expression: Is.EqualTo(new List<string>()
                {
                    "WebPage_0000001.html",
                    "WebPage_0000002.html",
                    "WebPage_0000003.html",
                    "WebPage_0000004.html",
                    "WebPage_0000005.html",
                    "WebPage_0000006.html",
                }));

            Assert.That(
                actual: CustomFileCollection.RetrieveFileObjectNames(
                    alias: "Image", source: "index.png", count: 8),
                expression: Is.EqualTo(new List<string>()
                {
                    "Image_0000001.png",
                    "Image_0000002.png",
                    "Image_0000003.png",
                    "Image_0000004.png",
                    "Image_0000005.png",
                    "Image_0000006.png",
                    "Image_0000007.png",
                    "Image_0000008.png",
                }));
        });
    }
}
