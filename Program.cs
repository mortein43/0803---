using System;
namespace Bookstore
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public struct Book
    {
        public string Title;
        public string Author;
        public double Price;
        public Book (string title, string author, double price)
        {
            Title = title;
            Author = author;
            Price = price;
        }
    }
    public delegate void ProcessBookDelegate (Book book);
    public delegate double ProcessBookDelegateForPrice(Book book);
    public class BookDB
    {
        public ArrayList list = new ArrayList ();
        
        public void AddBook (string title, string author, double price)
        {
            list.Add (new Book (title, author, price));
        }

        public void ProcessBookDelegate (ProcessBookDelegate processBook)
        {
            foreach (Book book in list)
            {
                processBook (book);
            }
        }
    }
    class BookDBforOrder
    {
        public ArrayList order = new ArrayList();
        public void AddBookOrder(Book b)
        {
            order.Add(b);
        }
        public void ProcessBookDelegate(ProcessBookDelegate processBook)
        {
            foreach (Book book in order)
            {
                processBook(book);
            }
        }
        public double ProcessBookDelegateForPrice(ProcessBookDelegateForPrice processBook)
        {
            double sum = 0;
            foreach (Book book in order)
            {
                sum = sum + book.Price;
            }
            return sum;
        }

    }
}
namespace BookTestClient
{
    using Bookstore;
    using System.Collections;

    class Test
    {
        static int i = 0;
        static int iOrder = 0;
        static void PrintBook (Book b)
        {
            
            Console.WriteLine($"{++i}.Назва - {b.Title}\nАвтор - {b.Author}\nЦiна - {b.Price}\n");
        }
        static void PrintBookForOrder(Book b)
        {
            
            Console.WriteLine($"{++iOrder}.Назва - {b.Title}\nАвтор - {b.Author}\nЦiна - {b.Price}\n");
        }
        static double Price(Book b)
        {
            return b.Price;
        }
        static void AddBooks (BookDB bookDB)
        {
            bookDB.AddBook("Пшениця без куколю", "Iгор Каганець", 400.0);
            bookDB.AddBook("The Unicode Standard 2.0", "The Unicode Consortium", 1085.5);
            bookDB.AddBook("Гаррi Поттер i фiлософський камiнь", "Джоан Роулiнг", 300.0);
            bookDB.AddBook("День незалежностi", "Петро Масляк", 30.0);
        }
        static void Main ()
        {
            BookDB bookDB = new BookDB ();
            AddBooks (bookDB);
            Console.WriteLine("Список книг:");
            bookDB.ProcessBookDelegate (PrintBook);
            Console.WriteLine("Виберiть книги для замовлення:");
            Console.WriteLine("0 - Завершити вибiр");
            int choice = -1;
            BookDBforOrder bookDBOrder = new BookDBforOrder();
            while (choice != 0)
            {
                choice = int.Parse(Console.ReadLine());
                int resChoice = choice - 1;
                if (choice == 0)
                {
                    break;
                }
                bookDBOrder.AddBookOrder((Book)bookDB.list[resChoice]);
            }
            Console.WriteLine("Список книг якi ви замовили:");
            bookDBOrder.ProcessBookDelegate(PrintBookForOrder);
            Console.WriteLine("Загальна вартiсть:");
            Console.Write(bookDBOrder.ProcessBookDelegateForPrice(Price));
            Console.Write(" грн.");

        }
    }
}