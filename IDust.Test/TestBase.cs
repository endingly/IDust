using Xunit.Abstractions;

namespace IDust.Test;

public class TestBase
{
    protected readonly ITestOutputHelper Output;

    public TestBase(ITestOutputHelper tempOutput)
    {
        Output = tempOutput;
    }
}