using NUnit.Framework;

public class TestTest
{
    [TestCase(1, 2, 3)]
    [TestCase(5, -3, 2)]
    [TestCase(0, 0, 0)]
    public void Add_VariousInputs_ReturnsCorrectResult(int a, int b, int expected)
    {
        //Calculator calculator = new Calculator();
        int result = 7;
        Assert.AreEqual(expected, result);
    }
}
