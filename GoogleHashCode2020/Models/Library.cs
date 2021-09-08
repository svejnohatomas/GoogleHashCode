using System;
using System.Collections.Generic;
using System.Linq;

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
        public int TotalScore { get; set; }

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

                this.TotalScore -= book.Score;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDay">An Id of a day when we start the sign-up process.</param>
        /// <returns></returns>
        public int NumberOfBooksWeCanRegisterFromDay(int daysToRegister, int startDay = 0)
        {
            int remainingDays = daysToRegister - startDay - this.DaysToSignUp;

            return Math.Min(remainingDays * this.ScanLimitPerDay, this.Books.Count);
        }
        public int ScoreOfBooksWeCanRegisterFromDay(int daysToRegister, int startDay = 0)
        {
            int numberOfBooks = this.NumberOfBooksWeCanRegisterFromDay(daysToRegister, startDay);

            // TODO: Change Data Structure to List and use index to indicate the current position
            return this.Books.Take(numberOfBooks).Sum(x => x.Score);
        }
    }
}
