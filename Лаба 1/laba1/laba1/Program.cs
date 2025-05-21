using System;

namespace AreaCalculatorOverload
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator calculator = new Calculator();

            // Квадрат
            double squareArea = calculator.CalculateArea(2);
            Console.WriteLine($"Площадь квадрата со стороной 2 равна {squareArea}.");

            // Прямоугольник
            double rectangleArea = calculator.CalculateArea(3, 4);
            Console.WriteLine($"Площадь прямоугольника со сторонами 3 и 4 равна {rectangleArea}.");

            // Прямоугольный треугольник
            double triangleArea = calculator.CalculateArea(3.0, 4.0, "triangle");
            Console.WriteLine($"Площадь прямоугольного треугольника с катетами 3 и 4 равна {triangleArea}.");

            // Трапеция
            double trapezoidArea = calculator.CalculateArea(5, 7, 4, "trapezoid");
            Console.WriteLine($"Площадь трапеции с основаниями 5 и 7 и высотой 4 равна {trapezoidArea}.");
        }
    }

    class Calculator
    {
        // Площадь квадрата
        public double CalculateArea(double a)
        {
            return a * a;
        }

        // Площадь прямоугольника
        public double CalculateArea(double a, double b)
        {
            return a * b;
        }

        // Площадь прямоугольного треугольника
        public double CalculateArea(double a, double b, string shapeType)
        {
            if (shapeType.ToLower() == "triangle")
                return a * b / 2;
            else
                throw new ArgumentException("Неизвестный тип фигуры");
        }

        // Площадь трапеции
        public double CalculateArea(double a, double b, double h, string shapeType)
        {
            if (shapeType.ToLower() == "trapezoid")
                return (a + b) * h / 2;
            else
                throw new ArgumentException("Неизвестный тип фигуры");
        }
    }
}