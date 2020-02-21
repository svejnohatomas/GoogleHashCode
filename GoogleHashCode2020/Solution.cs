using GoogleHashCode2020.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoogleHashCode2020
{
    public class Solution
    {
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

        private Library[] Libraries { get; set; }
        private int NumberOfDaysForScanning { get; set; }

        private LinkedList<Library> UsedLibraries { get; set; } = new LinkedList<Library>();
        public string LoadPath { get; }
        public string SavePath { get; }

        private void SortLibraries()
        {
            //TODO: Figure out a better solution
            this.Libraries = this.Libraries.OrderBy(x => x.DaysToSignUp).ThenByDescending(x => x.TotalScore).ToArray();
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

                foreach (Library item in this.UsedLibraries.OrderByDescending(x => x.TotalScore))
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
            }
        }
        private void SaveToFile(string path)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                IEnumerable<Library> filteredLibraries = this.UsedLibraries.Where(x => x.SentBooks.Count > 0);

                writer.WriteLine(filteredLibraries.Count());

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
