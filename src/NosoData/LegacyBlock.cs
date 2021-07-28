namespace Noso.Data.Legacy
{
    public class LegacyBlock
    {
        public int Number { get; set; }
        public string Hash { get; }

        public LegacyBlock() {
            this.Number = -1;
            this.Hash = "";
        }
    }
}
