using GoogleHashCode2020.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoogleHashCode2020
{
    public class Solution
    {
        private static object LockObject { get; set; } = new object();
        public static int MaxScore { get; private set; }
        public static int Score { get; private set; }

        public Solution(string loadPath, string savePath)
        {
            this.LoadPath = loadPath;
            this.SavePath = savePath;
        }

        public void Start()
        {
            this.LoadFile(this.LoadPath);
            this.SortLibraries();
            this.Process();
            this.SaveToFile(this.SavePath);
        }

        private Book[] Books { get; set; }
        private Library[] Libraries { get; set; }
        private int NumberOfDaysForScanning { get; set; }

        private LinkedList<Library> UsedLibraries { get; set; } = new LinkedList<Library>();
        public string LoadPath { get; }
        public string SavePath { get; }

        private void SortLibraries()
        {
            //TODO: Figure out a better solution

            // (00 - 11.02 %)
            // this.Libraries = this.Libraries.OrderByDescending(x => x.TotalScore).ToArray();

            // (16 - 24.10%) 
            // this.Libraries = this.Libraries.OrderBy(x => x.DaysToSignUp).ThenByDescending(x => x.TotalScore).ToArray();

            // (17 - 26.02 %)
            this.Libraries = this.Libraries.OrderByDescending(x => x.ScanLimitPerDay / (double) x.DaysToSignUp * x.Books.Count).ToArray();
        }

        private void Process()
        {
            int identity = 0;
            Library libraryInProcess = null;
            int daysRemainingToProcessLibrary = 0;

            for (int i = 0; i < this.NumberOfDaysForScanning; i++)
            {
                if (libraryInProcess != null && daysRemainingToProcessLibrary == 0)
                {
                    this.UsedLibraries.AddLast(libraryInProcess);
                    libraryInProcess = null;
                }

                if (libraryInProcess == null && identity < this.Libraries.Length)
                {
                    libraryInProcess = this.Libraries[identity++];
                    daysRemainingToProcessLibrary = libraryInProcess.DaysToSignUp;
                }

                foreach (Library item in this.UsedLibraries)
                {
                    item.ChooseBooks();
                }

                if (libraryInProcess != null)
                {
                    daysRemainingToProcessLibrary--;
                }
            }
        }

        private void LoadFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string[] firstLine = reader.ReadLine().Split(' ');

                Book[] globalBooks;

                #region Data set info
                int numberOfBooks = Convert.ToInt32(firstLine[0]);
                int numberOfLibraries = Convert.ToInt32(firstLine[1]);
                int numberOfDaysForScanning = Convert.ToInt32(firstLine[2]);

                globalBooks = new Book[numberOfBooks];
                this.Libraries = new Library[numberOfLibraries];
                this.NumberOfDaysForScanning = numberOfDaysForScanning;
                #endregion

                #region Books info
                string[] bookScores = reader.ReadLine().Split(' ');
                for (int i = 0; i < bookScores.Length; i++)
                {
                    int score = Convert.ToInt32(bookScores[i]);
                    globalBooks[i] = new Book(i, score);
                }
                #endregion

                #region Load libraries
                for (int i = 0; i < numberOfLibraries; i++)
                {
                    string[] libraryInfoLine = reader.ReadLine().Split(' ');
                    string[] booksInLibrary = reader.ReadLine().Split(' ');

                    int bookCount = Convert.ToInt32(libraryInfoLine[0]);
                    int days = Convert.ToInt32(libraryInfoLine[1]);
                    int scanLimit = Convert.ToInt32(libraryInfoLine[2]);

                    Library library = new Library(i, bookCount, days, scanLimit);

                    List<Book> libraryBooks = new List<Book>(bookCount);
                    foreach (string item in booksInLibrary)
                    {
                        int id = Convert.ToInt32(item);
                        libraryBooks.Add(globalBooks[id]);
                    }

                    foreach (Book item in libraryBooks.OrderByDescending(x => x.Score))
                    {
                        library.Books.Enqueue(item);
                        library.TotalScore += item.Score;
                    }

                    this.Libraries[i] = library;
                }
                #endregion

                this.Books = globalBooks;
            }
        }
        private void SaveToFile(string path)
        {
            IEnumerable<Library> filteredLibraries = this.UsedLibraries.Where(x => x.SentBooks.Count > 0);
            int numberOfFilteredLibraries = filteredLibraries.Count();

            lock (LockObject)
            {
                Console.WriteLine($"Total number of libraries:    {this.Libraries.Length}");
                Console.WriteLine($"- Number of used libraries:   {this.UsedLibraries.Count} ({this.UsedLibraries.Count / (double)this.Libraries.Length * 100} %)");
                Console.WriteLine($"- Number of really libraries: {numberOfFilteredLibraries} ({numberOfFilteredLibraries / (double)this.Libraries.Length * 100} %)");

                int sentBooksCount = this.UsedLibraries.Sum(x => x.SentBooks.Count);
                Console.WriteLine($"- Total number of books:   {this.Books.Length}");
                Console.WriteLine($"- Number of scanned books: {sentBooksCount} ({sentBooksCount / (double)this.Books.Length * 100} %)");
                int score = this.Books.Where(x => x.IsScanned).Sum(x => x.Score);
                int maxScore = this.Books.Sum(x => x.Score);
                Console.WriteLine($"- Score: {score}/{maxScore} ({score / (double)maxScore*100} %)\n\n");

                MaxScore += maxScore;
                Score += score;
            }

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine(numberOfFilteredLibraries);

                foreach (Library library in filteredLibraries)
                {
                    writer.WriteLine($"{library.Id} {library.SentBooks.Count}");
                    foreach (Book book in library.SentBooks)
                    {
                        writer.Write($"{book.Id} ");
                    }
                    if (library.SentBooks.Count > 0)
                    {
                        writer.WriteLine();
                    }
                }
            }
        }
    }
}
