using System.Drawing;


    public class Spheres
    {
        public Vector Center; // координаты центра

        public double Radius;  // радиус сферы

        public Color Color; // цвет сферы

        public int Specular;  // матовость сферы

        public double Reflective; // способность к отражению
        public Spheres(double R,Vector C,Color Color, int Specular, double Reflective)
        {
            this.Radius = R;
            this.Center = C;
            this.Color = Color;
            this.Specular = Specular;
            this.Reflective = Reflective;
        }

        
    }

