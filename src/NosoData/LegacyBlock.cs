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
            InitFields();
        }

        public LegacyBlock(string AFilename)
        {
            InitFields();
            doLoadFromFile(AFilename);
        }

        private void InitFields()
        {
            Number = -1;
            hash = "";
            TimeStart = -1;
            TimeEnd = -1;
            TimeTotal = -1;
            TimeLast20 = -1;
            Difficulty = -1;
            TargetHash = "";
            Solution = "";
            LastBlockHash ="";
            NextBlockDifficulty = -1;
            Miner = "";
            Fee = 0;
            Reward = 0;
        }

        private void doCalculateHash()
        {
            using (MemoryStream msBlock = new MemoryStream())
            {
                doSaveToStream(msBlock);
                msBlock.Position = 0;
                MD5 md5 = MD5.Create();
                byte[] hashValue = md5.ComputeHash(msBlock);
                hash = BitConverter.ToString(hashValue).Replace("-","").ToUpper();
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

            Number = brBlock.ReadInt64();
            TimeStart = brBlock.ReadInt64();
            TimeEnd = brBlock.ReadInt64();
            TimeTotal = brBlock.ReadInt32();
            TimeLast20 = brBlock.ReadInt32();
            transactions = brBlock.ReadInt32();
            Difficulty = brBlock.ReadInt32();
            // TargetHash String 32
            stringSize = brBlock.ReadByte();
            if (stringSize == 0)
            {
                TargetHash = "";
                stringChars = brBlock.ReadChars(32);
            }
            else
            {
                stringChars = brBlock.ReadChars(32);
                TargetHash = "";
                for (int index = 0; index < stringSize; index++)
                {
                    TargetHash += stringChars[index];
                }
            }
            // Solution String 200
            stringSize = brBlock.ReadByte();
            if (stringSize == 0)
            {
                Solution = "";
                stringChars = brBlock.ReadChars(200);
            }
            else
            {
                stringChars = brBlock.ReadChars(200);
                Solution = "";
                for (int index = 0; index < stringSize; index++)
                {
                    Solution += stringChars[index];
                }
            }
            // LastBlockHash String 32
            stringSize = brBlock.ReadByte();
            if (stringSize == 0)
            {
                LastBlockHash = "";
                stringChars = brBlock.ReadChars(32);
            }
            else
            {
                stringChars = brBlock.ReadChars(32);
                LastBlockHash = "";
                for (int index = 0; index < stringSize; index++)
                {
                    LastBlockHash += stringChars[index];
                }
            }
            NextBlockDifficulty = brBlock.ReadInt32();
            // Miner String 40
            stringSize = brBlock.ReadByte();
            if (stringSize == 0)
            {
                Miner = "";
                stringChars = brBlock.ReadChars(40);
            }
            else
            {
                stringChars = brBlock.ReadChars(40);
                Miner = "";
                for (int index = 0; index < stringSize; index++)
                {
                    Miner += stringChars[index];
                }
            }
            Fee = brBlock.ReadInt64();
            Reward = brBlock.ReadInt64();
            doCalculateHash();
        }

        public void doSaveToFile(string AFilename)
        {
            using (FileStream fsBlock = File.Open(AFilename, FileMode.OpenOrCreate))
            {
                doSaveToStream(fsBlock);
            }

        }

        private void doSaveToStream(Stream AStream)
        {
            BinaryWriter bwBlock = new BinaryWriter(AStream);

            bwBlock.Write(Number);
            bwBlock.Write(TimeStart);
            bwBlock.Write(TimeEnd);
            bwBlock.Write(TimeTotal);
            bwBlock.Write(TimeLast20);
            bwBlock.Write(transactions);
            bwBlock.Write(Difficulty);
            // TargetHash String 32
            bwBlock.Write((byte)TargetHash.Length);
            for (int index = 0; index < 32; index++)
            {
                if (index < TargetHash.Length)
                {
                    bwBlock.Write(TargetHash[index]);
                }
                else
                {
                    bwBlock.Write((byte)0);
                }
            }
            // Solution String 200
            bwBlock.Write((byte)Solution.Length);
            for (int index = 0; index < 200; index++)
            {
                if (index < Solution.Length)
                {
                    bwBlock.Write(Solution[index]);
                }
                else
                {
                    bwBlock.Write((byte)0);
                }
            }
            // LastBlockHash String 32
            bwBlock.Write((byte)LastBlockHash.Length);
            for (int index = 0; index < 32; index++)
            {
                if (index < LastBlockHash.Length)
                {
                    bwBlock.Write(LastBlockHash[index]);
                }
                else
                {
                    bwBlock.Write((byte)0);
                }
            }
            bwBlock.Write(NextBlockDifficulty);
            // Miner String 40
            bwBlock.Write((byte)Miner.Length);
            for (int index = 0; index < 40; index++)
            {
                if (index < Miner.Length)
                {
                    bwBlock.Write(Miner[index]);
                }
                else
                {
                    bwBlock.Write((byte)0);
                }
            }
            bwBlock.Write(Fee);
            bwBlock.Write(Reward);
        }
    }
}
