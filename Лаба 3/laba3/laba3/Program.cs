
using System;

//namespace HelloApp
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Person tom = new Employee();
//            Console.ReadKey();
//        }
//    }

//    internal class Person
//    {

//    }
//    public class Employee : Person
//    {

//    }
//

//class Auto // легковой автомобиль
//{
//    public int Seats { get; set; } // количество сидений
//    public Auto(int seats)
//    {
//        Seats = seats;
//    }
//}
//class Truck : Auto // грузовой автомобиль
//{
//    public decimal Capacity { get; set; } // грузоподъемность
//    public Truck(int seats, decimal capacity) : base (seats)
//    {
//        Seats = seats;
//        Capacity = capacity;
//    }
//}
//class Program
//{
//    static void Main(string[] args)
//    {
//        Truck truck = new Truck(2, 1.1m);
//        Console.WriteLine($"Грузовик с грузоподъемностью {truck.Capacity} тонн");
//        Console.ReadKey();
//    }
//}
//class Auto // легковой автомобиль
//{
//    public int Seats { get; set; } // количество сидений
//    public Auto()
//    {
//        Console.WriteLine("Auto has been created");
//    }
//    public Auto(int seats)
//    {
//        Seats = seats;
//    }
//}
//class Truck : Auto // грузовой автомобиль
//{
//    public decimal Capacity { get; set; } // грузоподъемность
//    public Truck(decimal capacity)
//    {
//        Seats = 2;
//        Capacity = capacity;
//        Console.WriteLine("Truck has been created");
//    }
//}
//class Program
//{
//    static void Main(string[] args)
//    {
//        Truck truck = new Truck(1.1m);
//        Console.WriteLine($"Truck with capacity {truck.Capacity}");
//        Console.ReadKey();
//    }
//}
class Person
{
    public string Name { get; set; } = "Ben";

    public Person(string name)
    {
        Name = "Tim";
    }
}

class Employee : Person
{
    public string Company { get; set; }

    public Employee(string name, string company)
    : base("Bob")
    {
        Company = company;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Employee emp = new Employee("Tom", "Microsoft") { Name = "Sam" };

        Console.WriteLine(emp.Name); // Ben Tim Bob Tom Sam
        Console.ReadKey();
    }
}



