using System;

namespace MyApp.Models
{
    // Базовый класс Car с различными модификаторами доступа
    public class Car
    {
        private string make;    // private - доступ только внутри класса
        private string model;   // private - доступ только внутри класса
        public int Year { get; set; }  // public - доступ из любого места

        // internal - доступен в пределах текущей сборки
        internal void SetMakeAndModel(string make, string model)
        {
            this.make = make;
            this.model = model;
        }

        // protected - доступен внутри класса и производных классов
        protected virtual void DisplayInfo()
        {
            Console.WriteLine($"Make: {make}, Model: {model}, Year: {Year}");
        }

        // public метод для вызова protected DisplayInfo извне
        public void ShowInfo()
        {
            DisplayInfo();
        }
    }

    // Класс ElectricCar наследует от Car
    public class ElectricCar : Car
    {
        private double batteryCapacity;  // private - доступ только внутри класса

        // public - доступ из любого места
        public void SetBatteryCapacity(double capacity)
        {
            batteryCapacity = capacity;
        }

        // protected override - переопределение метода с изменением видимости
        protected override void DisplayInfo()
        {
            base.DisplayInfo();  // вызов метода базового класса
            Console.WriteLine($"Battery Capacity: {batteryCapacity} kWh");
        }
    }
}

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Работа с классом Car
            var car = new MyApp.Models.Car();

            // car.make = "Toyota"; // Ошибка - private поле
            // car.model = "Camry"; // Ошибка - private поле
            car.Year = 2020;      // OK - public свойство

            car.SetMakeAndModel("Toyota", "Camry");  // OK - internal метод
            car.ShowInfo();       // Вызов protected метода через public обертку

            // Работа с классом ElectricCar
            var electricCar = new MyApp.Models.ElectricCar();
            electricCar.Year = 2022;
            electricCar.SetMakeAndModel("Tesla", "Model S");
            electricCar.SetBatteryCapacity(100);  // OK - public метод
            electricCar.ShowInfo();

            Console.ReadKey();
        }
    }
}
