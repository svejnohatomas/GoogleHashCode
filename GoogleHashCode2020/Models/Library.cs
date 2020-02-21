using System.Collections.Generic;

namespace GoogleHashCode2020.Models
{
    public class Library
    {
        public Library(int id, int numberOfBooks, int daysToSignUp, int scanLimitPerDay)
        {
            this.Id = id;
            this.Books = new Queue<Book>(numberOfBooks);
            this.DaysToSignUp = daysToSignUp;
            this.ScanLimitPerDay = scanLimitPerDay;
        }

        public int Id { get; set; }
        public int DaysToSignUp { get; set; }
        public int ScanLimitPerDay { get; set; }
        public Queue<Book> Books { get; private set; }

        public Queue<Book> SentBooks { get; private set; } = new Queue<Book>();

        public void ChooseBooks()
        {
            int chosenBooksCount = 0;
            while (chosenBooksCount < this.ScanLimitPerDay && this.Books.Count > 0)
            {
                Book book = this.Books.Dequeue();

                if (book.IsScanned == false)
                {
                    book.Scan();
                    this.SentBooks.Enqueue(book);
                    chosenBooksCount++;
                }
            }
        }
    }
}
