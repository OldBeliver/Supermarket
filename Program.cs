using System;
using System.Collections.Generic;
using System.Linq;

namespace Supermarket
{
    class Program
    {
        static void Main(string[] args)
        {
            Supermarket supermarket = new Supermarket();

            supermarket.Open();

            Console.ReadKey();
        }
    }

    class Product
    {
        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public int Price { get; private set; }
    }

    class Customer
    {
        private List<Product> _products;

        public Customer(List<Product> products)
        {
            _products = products;

            //Money = GetRandomMoney(); // 2 варианта генерации денег для покупателя
            SetRandomMoney(); // Set - нормальное ли название? или лучше Get?
        }

        public int Money { get; private set; }

        #region
        // 1й вариант определения суммы товара,
        // не нравится, что покупатель определяет сумму товаров
        public int CalculateTotalPrice()
        {
            int totalPrice = 0;

            foreach (Product product in _products)
            {
                totalPrice += product.Price;
            }

            return totalPrice;
        }

        // LINQ 
        public int TotalPrice => _products.Sum(product => product.Price);
        #endregion

        #region 
        // 2й вариант определения суммы товара
        // сообщить количество и по индексу показывать товары
        public int ProductsCount => _products.Count;

        public Product GetProduct(int index)
        {
            Product product = _products[index];
            return product;
        }
        #endregion

        #region
        // 3й вариант определения суммы товара
        // передать список товаров кассиру
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>(_products);

            return products;
        }
        #endregion

        public void ShowProducts()
        {
            //Console.WriteLine($"Товары покупателя:");

            Console.WriteLine($"{new string('-', 40)}"); // является ли 40 магическим?

            foreach (Product product in _products)
            {
                Console.WriteLine($"{product.Name}");
            }

            Console.WriteLine($"{new string('-', 40)}");
        }

        public void Pay(int bill)
        {
            while (bill > Money)
            {
                TakeOffProduct(ref bill); // долго искал, где в задачах по ООП можно применить ref
            }

            Money -= bill;
        }

        private void TakeOffProduct(ref int bill)
        {
            int index = UserUtils.GenerateRandomNumber(0, _products.Count);

            Product product = _products[index];            

            _products.Remove(product);
            
            bill -= product.Price;

            Console.WriteLine($"{product.Name} - убрали из корзины.");
        }        

        #region
        private void SetRandomMoney()
        {
            int minMoney = 25;
            int maxMoney = 100;

            Money = UserUtils.GenerateRandomNumber(minMoney, maxMoney);
        }

        private int GetRandomMoney()
        {
            int minMoney = 25;
            int maxMoney = 100;

            return UserUtils.GenerateRandomNumber(minMoney, maxMoney);
        }
        #endregion
    }

    class Supermarket
    {
        private Queue<Customer> _customers;
        private List<Product> _products;
        private int _money = 0;

        public Supermarket()
        {
            _products = CreateProducts();
            _customers = new Queue<Customer>();
        }

        public void Open()
        {
            CreateQueue(10);

            while (_customers.Count > 0)
            {
                Console.Clear();
                Console.WriteLine($"Дневной оборот супермаркета: {_money}");
                Console.WriteLine($"{new string('-', 40)}");
                Console.WriteLine($"В очереди {_customers.Count} человек");
                Console.WriteLine($"{new string('-', 40)}");

                ServeCustomer();

                Console.ReadKey();
            }

            Console.Clear();
            Console.WriteLine($"Магазин завершил работу с выручкой {_money}");
            Console.ReadKey();
        }

        public void ShowAllProducts()
        {
            int counter = 0;

            foreach (Product product in _products)
            {
                counter++;

                Console.WriteLine($"{counter,2}. {product.Name,-45} {product.Price} монет.");
            }
        }

        public void CreateQueue(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _customers.Enqueue(new Customer(GetRandomProducts()));
            }
        }

        private List<Product> CreateProducts()
        {
            List<Product> products = new List<Product>()
            {
                new Product("Зелье избавления от прыгучести", 15),
                new Product("Порошок для выведения застарелых шуток", 20),
                new Product("Пена для борьбы с горючими проблемами", 64),
                new Product("Флакон для лечения от скуки", 5),
                new Product("Мазь для лечения неважных проблем", 17),
                new Product("Розги для излечения невыносимого характера", 2),
                new Product("Порошок для лечения драчливости", 7),
                new Product("Пилюли от багов в программном обеспечении", 52),
                new Product("Капли от прокрастинации при написании кода", 37),
            };

            return products;
        }

        private List<Product> GetRandomProducts()
        {
            int anyCount = UserUtils.GenerateRandomNumber(1, _products.Count);

            List<Product> products = new List<Product>();

            for (int i = 0; i < anyCount; i++)
            {
                int index = UserUtils.GenerateRandomNumber(0, _products.Count);

                products.Add(_products[index]);
            }

            return products;
        }

        private void ServeCustomer()
        {
            Customer customer = _customers.Dequeue();            
            int bill = customer.TotalPrice;
            Console.WriteLine($"покупатель с чеком : {bill} и деньгами {customer.Money}");
            Console.WriteLine($"{new string('-', 40)}");
            Console.WriteLine($"Товары покупателя:");
            customer.ShowProducts();
            customer.Pay(bill);
            Console.WriteLine($"{new string('-', 40)}");
            Console.WriteLine($"Покупатель ушел с покупками:");
            customer.ShowProducts();
            _money += customer.TotalPrice;
        }
    }

    static class UserUtils // должен ли класс быть статик?
    {
        private static Random _random = new Random();

        public static int GenerateRandomNumber(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        // вариация использования GenerateRandomNumber, чтобы принимал 1 или 2 параметра
        // при этом GenerateRandomNumber становиться приватным

        public static int GetRandomNumber(int max)
        {
            int min = 0;

            return GenerateRandomNumber(min, max);
        }

        public static int GetRandomNumber(int min, int max)
        {
            return GenerateRandomNumber(min, max);
        }
    }
}
