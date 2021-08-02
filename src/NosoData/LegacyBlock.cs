using System;
using System.IO;

namespace Noso.Data.Legacy
{
    public class LegacyBlock
    {
        public long Number { get; set; }
        //public string Hash { get; }
        public long TimeStart { get; set; }
        public long TimeEnd { get; set; }
        public int TimeTotal { get; set; }
        public int TimeLast20 { get; set; }
        public int Difficulty { get; set; }
        public string TargetHash { get; set; }
        public string Solution { get; set; }
        public string LastBlockHash { get; set; }
        public int NextBlockDifficulty { get; set; }
        public string Miner { get; set; }
        public long Fee { get; set; }
        public long Reward { get; set; }

        public LegacyBlock()
        {
            this.InitFields();
        }

        public LegacyBlock(string AFilename)
        {
            this.InitFields();
            this.doLoadFromFile(AFilename);
        }

        private void InitFields()
        {
            this.Number = -1;
            //this.Hash = "";
            this.TimeStart = -1;
            this.TimeEnd = -1;
            this.TimeTotal = -1;
            this.TimeLast20 = -1;
            this.Difficulty = -1;
            this.TargetHash = "";
            this.Solution = "";
            this.LastBlockHash ="";
            this.NextBlockDifficulty = -1;
            this.Miner = "";
            this.Fee = 0;
            this.Reward = 0;
        }

        private void doLoadFromFile(string AFilename)
        {
            if (File.Exists(AFilename))
            {
                using (BinaryReader brBlock = new BinaryReader(File.Open(AFilename, FileMode.Open)))
                {
                    this.Number = brBlock.ReadInt64();
                }
            }
        }
    }
}
