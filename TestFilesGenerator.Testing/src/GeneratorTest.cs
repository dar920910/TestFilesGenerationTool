using TestFilesGenerator.Library;

namespace TestFilesGenerator.Testing;

public class GeneratorTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetIdNumber_WhenValueLevel1()
    {
        string idNumber = new CollectionFileObjectNumberId(500_000).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0500000"));
    }

    [Test]
    public void GetIdNumber_WhenValueLevel2()
    {
        string idNumber = new CollectionFileObjectNumberId(50_000).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0050000"));
    }

    [Test]
    public void GetIdNumber_WhenValueLevel3()
    {
        string idNumber = new CollectionFileObjectNumberId(5_000).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0005000"));
    }

    [Test]
    public void GetIdNumber_WhenValueLevel4()
    {
        string idNumber = new CollectionFileObjectNumberId(500).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0000500"));
    }

    [Test]
    public void GetIdNumber_WhenValueLevel5()
    {
        string idNumber = new CollectionFileObjectNumberId(50).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0000050"));
    }

    [Test]
    public void GetIdNumber_WhenValueLevel6()
    {
        string idNumber = new CollectionFileObjectNumberId(5).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0000005"));
    }

    [Test]
    public void GetIdNumber_WhenMaximumAvailableValue()
    {
        string idNumber = new CollectionFileObjectNumberId(999_999).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0999999"));
    }

    [Test]
    public void GetIdNumber_WhenMinimumAvailableValue()
    {
        string idNumber = new CollectionFileObjectNumberId(0).Create();
        Assert.That(actual: idNumber, Is.EqualTo("0000000"));
    }
}