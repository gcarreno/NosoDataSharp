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
                    int transactions;
                    int stringSize;
                    char[] stringChars;

                    this.Number = brBlock.ReadInt64();
                    this.TimeStart = brBlock.ReadInt64();
                    this.TimeEnd = brBlock.ReadInt64();
                    this.TimeTotal = brBlock.ReadInt32();
                    this.TimeLast20 = brBlock.ReadInt32();
                    
                    transactions = brBlock.ReadInt32();

                    this.Difficulty = brBlock.ReadInt32();

                    stringSize = brBlock.ReadByte();
                    if (stringSize == 0)
                    {
                        this.TargetHash = "";
                        stringChars = brBlock.ReadChars(32);
                    }
                    else
                    {
                        stringChars = brBlock.ReadChars(32);
                        this.TargetHash = "";
                        for (int index = 0; index < stringSize; index++)
                        {
                            this.TargetHash += stringChars[index];
                        }
                    }
                    
                    stringSize = brBlock.ReadByte();
                    if (stringSize == 0)
                    {
                        this.Solution = "";
                        stringChars = brBlock.ReadChars(200);
                    }
                    else
                    {
                        stringChars = brBlock.ReadChars(200);
                        this.Solution = "";
                        for (int index = 0; index < stringSize; index++)
                        {
                            this.Solution += stringChars[index];
                        }
                    }
                    
                    stringSize = brBlock.ReadByte();
                    if (stringSize == 0)
                    {
                        this.LastBlockHash = "";
                        stringChars = brBlock.ReadChars(32);
                    }
                    else
                    {
                        stringChars = brBlock.ReadChars(32);
                        this.LastBlockHash = "";
                        for (int index = 0; index < stringSize; index++)
                        {
                            this.LastBlockHash += stringChars[index];
                        }
                    }
                    
                    this.NextBlockDifficulty = brBlock.ReadInt32();
                    
                    stringSize = brBlock.ReadByte();
                    if (stringSize == 0)
                    {
                        this.Miner = "";
                        stringChars = brBlock.ReadChars(40);
                    }
                    else
                    {
                        stringChars = brBlock.ReadChars(40);
                        this.Miner = "";
                        for (int index = 0; index < stringSize; index++)
                        {
                            this.Miner += stringChars[index];
                        }
                    }
                    
                    this.Fee = brBlock.ReadInt64();
                    this.Reward = brBlock.ReadInt64();
                }
            }
            else
            {
                throw new FileNotFoundException($"File {AFilename} not found.");
            }

        }
    }
}
