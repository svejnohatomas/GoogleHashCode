namespace GoogleHashCode2020.Models
{
    public class Book
    {
        public Book(int id, int score)
        {
            this.Id = id;
            this.Score = score;
        }

        public int Id { get; }
        public int Score { get; }
        public bool IsScanned { get; private set; }

        public void Scan() => this.IsScanned = true;
    }
}
