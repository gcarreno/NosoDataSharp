using System;
using Xunit;
using Noso.Data.Legacy;

namespace Noso.Data.Tests
{
    public class UnitTestLegacyBlock
    {
        [Fact]
        public void TestCreate()
        {
            var block = new LegacyBlock();

            Assert.Equal(-1, block.Number);
            Assert.Equal("", block.Hash);
        }
    }
}
