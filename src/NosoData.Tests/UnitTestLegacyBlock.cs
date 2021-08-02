using System;
using Xunit;
using Noso.Data.Legacy;

namespace Noso.Data.Tests
{
    public class UnitTestLegacyBlock
    {
        private void CheckFieldsCreate(LegacyBlock block)
        {
            Assert.Equal(-1, block.Number);
            Assert.Equal("", block.Hash);
            Assert.Equal(-1, block.TimeStart);
            Assert.Equal(-1, block.TimeEnd);
            Assert.Equal(-1, block.TimeTotal);
            Assert.Equal(-1, block.TimeLast20);
            Assert.Equal(-1, block.Difficulty);
            Assert.Equal("", block.TargetHash);
            Assert.Equal("", block.Solution);
            Assert.Equal("", block.LastBlockHash);
            Assert.Equal(-1, block.NextBlockDifficulty);
            Assert.Equal("", block.Miner);
            Assert.Equal(0, block.Fee);
            Assert.Equal(0, block.Reward);
        }

        private void CheckFieldsBlockZero(LegacyBlock block)
        {
            Assert.Equal(0, block.Number);
            Assert.Equal("4E8A4743AA6083F3833DDA1216FE3717", block.Hash);
            Assert.Equal(1531896783, block.TimeStart);
            Assert.Equal(1615132800, block.TimeEnd);
            Assert.Equal(83236017, block.TimeTotal);
            Assert.Equal(600, block.TimeLast20);
            Assert.Equal(60, block.Difficulty);
            Assert.Equal("", block.TargetHash);
            Assert.Equal("", block.Solution);
            Assert.Equal("NOSO GENESYS BLOCK", block.LastBlockHash);
            Assert.Equal(60, block.NextBlockDifficulty);
            Assert.Equal("N4PeJyqj8diSXnfhxSQdLpo8ddXTaGd", block.Miner);
            Assert.Equal(0, block.Fee);
            Assert.Equal(1030390730000, block.Reward);
        }
        
        [Fact]
        public void TestCreate()
        {
            var block = new LegacyBlock();
            CheckFieldsCreate(block);
        }
        
        [Fact]
        public void TestBlockZero()
        {
            var block = new LegacyBlock(@"../../../../../test-data/NOSODATA/BLOCKS/0.blk");
            CheckFieldsBlockZero(block);
        }
    }
}
