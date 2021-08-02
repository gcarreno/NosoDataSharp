using System;
using System.IO;
using System.Security.Cryptography;

namespace Noso.Data.Legacy
{
    public class LegacyBlock
    {
        public long Number { get; set; }
        private string hash;
        public string Hash { get => hash; }
        public long TimeStart { get; set; }
        public long TimeEnd { get; set; }
        public int TimeTotal { get; set; }
        public int TimeLast20 { get; set; }
        private int transactions;
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
            this.hash = "";
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

        private void doCalculateHash()
        {
            using (MemoryStream msBlock = new MemoryStream())
            {
                doSaveToStream(msBlock);
                msBlock.Position = 0;
                MD5 md5 = MD5.Create();
                byte[] hashValue = md5.ComputeHash(msBlock);
                this.hash = BitConverter.ToString(hashValue).Replace("-","").ToUpper();
            }
        }

        private void doLoadFromFile(string AFilename)
        {
            if (File.Exists(AFilename))
            {
                using(FileStream fsBlock = File.Open(AFilename, FileMode.Open))
                {
                    doLoadFromStream(fsBlock);
                }
            }
            else
            {
                throw new FileNotFoundException($"File {AFilename} not found.");
            }


        }
        private void doLoadFromStream(Stream AStream)
        {
            BinaryReader brBlock = new BinaryReader(AStream);

            int stringSize;
            char[] stringChars;

            this.Number = brBlock.ReadInt64();
            this.TimeStart = brBlock.ReadInt64();
            this.TimeEnd = brBlock.ReadInt64();
            this.TimeTotal = brBlock.ReadInt32();
            this.TimeLast20 = brBlock.ReadInt32();
            this.transactions = brBlock.ReadInt32();
            this.Difficulty = brBlock.ReadInt32();
            // TargetHash String 32
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
            // Solution String 200
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
            // LastBlockHash String 32
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
            // Miner String 40
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
            doCalculateHash();
        }

        public void doSaveToFile(string AFilename)
        {
            using (FileStream fsBlock = File.Open(AFilename, FileMode.OpenOrCreate))
            {
                this.doSaveToStream(fsBlock);
            }

        }

        private void doSaveToStream(Stream AStream)
        {
            BinaryWriter bwBlock = new BinaryWriter(AStream);

            bwBlock.Write(this.Number);
            bwBlock.Write(this.TimeStart);
            bwBlock.Write(this.TimeEnd);
            bwBlock.Write(this.TimeTotal);
            bwBlock.Write(this.TimeLast20);
            bwBlock.Write(this.transactions);
            bwBlock.Write(this.Difficulty);
            // TargetHash String 32
            bwBlock.Write((byte)this.TargetHash.Length);
            for (int index = 0; index < 32; index++)
            {
                if (index < this.TargetHash.Length)
                {
                    bwBlock.Write(this.TargetHash[index]);
                }
                else
                {
                    bwBlock.Write((byte)0);
                }
            }
            // Solution String 200
            bwBlock.Write((byte)this.Solution.Length);
            for (int index = 0; index < 200; index++)
            {
                if (index < this.Solution.Length)
                {
                    bwBlock.Write(this.Solution[index]);
                }
                else
                {
                    bwBlock.Write((byte)0);
                }
            }
            // LastBlockHash String 32
            bwBlock.Write((byte)this.LastBlockHash.Length);
            for (int index = 0; index < 32; index++)
            {
                if (index < this.LastBlockHash.Length)
                {
                    bwBlock.Write(this.LastBlockHash[index]);
                }
                else
                {
                    bwBlock.Write((byte)0);
                }
            }
            bwBlock.Write(this.NextBlockDifficulty);
            // Miner String 40
            bwBlock.Write((byte)this.Miner.Length);
            for (int index = 0; index < 40; index++)
            {
                if (index < this.Miner.Length)
                {
                    bwBlock.Write(this.Miner[index]);
                }
                else
                {
                    bwBlock.Write((byte)0);
                }
            }
            bwBlock.Write(this.Fee);
            bwBlock.Write(this.Reward);
        }
    }
}
