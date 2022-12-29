// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using SquidEyes.Testing;

namespace SquidEyes.UnitTests;

public class TestingFixture : IDisposable
{
    private readonly TestData testData;

    public TestingFixture()
    {
        testData = new TestData();
    }

    public TradeDate TradeDate => testData.TradeDate;

    public TickSet GetTickSet(Symbol symbol) =>
        testData.GetTickSet(symbol);

    public void Dispose()
    {
        testData.Dispose();

        GC.SuppressFinalize(this);
    }
}