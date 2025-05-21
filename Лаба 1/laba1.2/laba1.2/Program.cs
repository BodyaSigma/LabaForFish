using System;

namespace AreaCalculatorPolymorphism
{
    class Program
    {
        static void Main(string[] args)
        {
            Shape[] shapes = new Shape[]
            {
                new Square(2),
                new Rectangle(3, 4),
                new RightTriangle(3, 4),
                new Trapezoid(5, 7, 4)
            };

            foreach (var shape in shapes)
            {
                Console.WriteLine(shape.GetAreaDescription());
            }
        }
    }

    abstract class Shape
    {
        public abstract double CalculateArea();
        public abstract string GetAreaDescription();
    }

    class Square : Shape
    {
        private double side;

        public Square(double a)
        {
            side = a;
        }

        public override double CalculateArea()
        {
            return side * side;
        }

        public override string GetAreaDescription()
        {
            return $"Площадь квадрата со стороной {side} равна {CalculateArea()}.";
        }
    }

    class Rectangle : Shape
    {
        private double length;
        private double width;

        public Rectangle(double a, double b)
        {
            length = a;
            width = b;
        }

        public override double CalculateArea()
        {
            return length * width;
        }

        public override string GetAreaDescription()
        {
            return $"Площадь прямоугольника со сторонами {length} и {width} равна {CalculateArea()}.";
        }
    }

    class RightTriangle : Shape
    {
        private double leg1;
        private double leg2;

        public RightTriangle(double a, double b)
        {
            leg1 = a;
            leg2 = b;
        }

        public override double CalculateArea()
        {
            return leg1 * leg2 / 2;
        }

        public override string GetAreaDescription()
        {
            return $"Площадь прямоугольного треугольника с катетами {leg1} и {leg2} равна {CalculateArea()}.";
        }
    }

    class Trapezoid : Shape
    {
        private double base1;
        private double base2;
        private double height;

        public Trapezoid(double a, double b, double h)
        {
            base1 = a;
            base2 = b;
            height = h;
        }

        public override double CalculateArea()
        {
            return (base1 + base2) * height / 2;
        }

        public override string GetAreaDescription()
        {
            return $"Площадь трапеции с основаниями {base1} и {base2} и высотой {height} равна {CalculateArea()}.";
        }
    }
}