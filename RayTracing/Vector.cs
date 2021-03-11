using System;




    /// <summary>Трассируемый луч</summary>

    
    public class Vector 
    {
        private double x;
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        private double y;
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        private double z;
        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        // Конструктор для 3-мерных векторов
        public Vector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        // конструктор для двухмерных векторов
        public Vector(double x,double y)
        {
            this.X = x;
            this.Y = y;
        }
        // ДЛИНА ВЕКТОРА                                        
        // Свойство
        private double length;
        public double Length
        {
            get
            {
                return Math.Round(Math.Sqrt(X * X + Y * Y + Z * Z), 4);
            }
            set
            {
                length = value;
            }
        }
        //УГОЛ МЕЖДУ ДВУМЯ ВЕКТОРАМИ                 
        public double Angle(Vector v)
        {
            double cos = Math.Round((X * v.X + Y * v.Y + Z * v.Z) / (this.Length * v.Length), 4);
            return cos;
        }
        // Сложение векторов               
        // Переопределение операции +
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        // Вычитание векторов       
        // Переопределение операции -
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        // Умножение вектора на число        
        // Переопределение операции *
        public static Vector operator *(double digit, Vector v1)
        {
            return new Vector(digit * v1.X, digit * v1.Y, digit * v1.Z);
        }
        // Скалярное умножение векторов        
        // Переопределение операции *
        public static double operator *(Vector v1, Vector v2)
        {
            return Math.Round(v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z);
        }
        // Векторное умножение векторов       
        public static Vector Multiplicate(Vector v1, Vector v2)
        {
            return new Vector((v1.y * v2.Z - v1.z * v2.Y), (-1) * (v1.x * v2.Z - v1.z * v2.X), (v1.x * v2.Y - v1.y * v2.X));
        }
        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", X, Y, Z);
        }
        
        
    }


