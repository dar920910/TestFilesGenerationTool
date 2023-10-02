//-----------------------------------------------------------------------
// <copyright file="CollectionFileObjectNumberIdTest.cs" company="Demo Projects Workshop">
// Copyright (c) Demo Projects Workshop. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TestFilesGenerator.Testing;

using TestFilesGenerator.Library;

/// <summary>
/// Contains unit tests for the CollectionFileObjectNumberId class.
/// </summary>
public class CollectionFileObjectNumberIdTest
{
    /// <summary>
    /// Tests creating file object's name when using a number from the Level #1.
    /// This range contains integers more than 100_000 and less than 1_000_000.
    /// </summary>
    [Test]
    public void GetIdNumber_WhenValueLevel1()
    {
        string idNumber = new CollectionFileObjectNumberId(500_000).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0500000"));
    }

    /// <summary>
    /// Tests creating file object's name when using a number from the Level #2.
    /// This range contains integers more than 10_000 and less than 100_000.
    /// </summary>
    [Test]
    public void GetIdNumber_WhenValueLevel2()
    {
        string idNumber = new CollectionFileObjectNumberId(50_000).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0050000"));
    }

    /// <summary>
    /// Tests creating file object's name when using a number from the Level #3.
    /// This range contains integers more than 1000 and less than 10_000.
    /// </summary>
    [Test]
    public void GetIdNumber_WhenValueLevel3()
    {
        string idNumber = new CollectionFileObjectNumberId(5_000).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0005000"));
    }

    /// <summary>
    /// Tests creating file object's name when using a number from the Level #4.
    /// This range contains integers more than 100 and less than 1000.
    /// </summary>
    [Test]
    public void GetIdNumber_WhenValueLevel4()
    {
        string idNumber = new CollectionFileObjectNumberId(500).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0000500"));
    }

    /// <summary>
    /// Tests creating file object's name when using a number from the Level #5.
    /// This range contains integers more than 10 and less than 100.
    /// </summary>
    [Test]
    public void GetIdNumber_WhenValueLevel5()
    {
        string idNumber = new CollectionFileObjectNumberId(50).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0000050"));
    }

    /// <summary>
    /// Tests creating file object's name when using a number from the Level #6.
    /// This range contains integers more than 0 and less than 10.
    /// </summary>
    [Test]
    public void GetIdNumber_WhenValueLevel6()
    {
        string idNumber = new CollectionFileObjectNumberId(5).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0000005"));
    }

    /// <summary>
    /// Tests creating file object's name when using the maximum available number.
    /// </summary>
    [Test]
    public void GetIdNumber_WhenMaximumAvailableValue()
    {
        string idNumber = new CollectionFileObjectNumberId(999_999).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0999999"));
    }

    /// <summary>
    /// Tests creating file object's name when using the minimum available number.
    /// </summary>
    [Test]
    public void GetIdNumber_WhenMinimumAvailableValue()
    {
        string idNumber = new CollectionFileObjectNumberId(0).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0000000"));
    }
}
