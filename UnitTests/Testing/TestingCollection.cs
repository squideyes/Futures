// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using Xunit;

namespace SquidEyes.UnitTests;

[CollectionDefinition(nameof(TestingCollection))]
public class TestingCollection : ICollectionFixture<TestingFixture>
{
}