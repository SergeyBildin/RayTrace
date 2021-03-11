using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;



namespace RayTracing
{
    class Program
    {    //-------------------Задаем размеры холста-----------------------------

        public const int Cw = 1920; // Ширина холста в пикселях
        public const int Ch = 1080; // высота холста в пикселях

        //--------------------Определяем положение камеры-----------------------

        public static Vector O = new Vector(480, 270, 3);  // координаты положения камеры

        //--------------------Задаем параметры окна просмотра (ViewPort)-------------------

        public static int d = 831; //расстояние от камеры до плоскости проекции(окна просмотра)  (координатa z)
        public const int Vw = 960; // Ширина рамки(окна просмотра)
        public const int Vh = 540; // высота рамки(окна просмотра)       
        public static Vector D;// V - O - точка на окне просмотра
        

        //--------------------Создаем файл изображения---------------------------

        public static Bitmap Canvas = new Bitmap(Cw, Ch, PixelFormat.Format64bppArgb);

        static void Main(string[] args)
        {
            Console.WriteLine("Rendering...\n");
            string path = @"Out.jpg"; // указываем путь сохранения
            //string TExtpath = @"C:\Users\Sereg\OneDrive\Рабочий стол\ОП\ТП(2 семестр)\Курсовая работа(2 семестр)\Курсовая работа(2 семестр)\Об авторе.txt";
            Spheres[] sphere = new Spheres[6];//Массив Сфер, заданных на сцене
            sphere[0] = new Spheres(200, new Vector(600, 540, 900), Color.Red, 500,0.2);
            sphere[1] = new Spheres(130, new Vector(940, 540, 900), Color.Blue, 500,0.3);
            sphere[2] = new Spheres(20, new Vector(1260, 540, 900), Color.Cyan, 400,0.4);
            sphere[3] = new Spheres(3500, new Vector(960, 4000, 1600), Color.Yellow, 500,0.5);
            sphere[4] = new Spheres(50, new Vector(960, 540, 600), Color.White, 300,0.9);
            sphere[5] = new Spheres(28, new Vector(700, 540, 600), Color.White, 10, 0.9);

            Light[] light = new Light[3];//добавляем источники освещения
            light[0] = new Light("ambient", 0.1);
            light[1] = new Light("point", 0.6, new Vector(760, 340, 600));
            light[2] = new Light("directional", 0.3, new Vector(600, 100, 600));

            render(sphere,light); //рендерим сцену
            Canvas.Save(path, ImageFormat.Jpeg); // сохраняем в файл
            
            //string[]About = File.ReadAllLines(TExtpath,System.Text.Encoding.UTF8);
            Console.WriteLine("Completed!!! Press anykey to exit....");

            /*for(int i = 0;i<About.Length;i++)
            {
                Console.WriteLine(About[i]);
            }*/
            Console.ReadKey();

        }

        static void render(Spheres[] sphere,Light[]light)  //рендеринг нашей сцены
        {

            for (int x = 0; x < Cw; x++)// для каждого пикселя на холсте
            {
                for (int y = 0; y < Ch; y++)
                {
                    D = CanvasToViewport(x, y);// определить координаты в окне просмотра
                    Color color = TraceRay(O, D, sphere, 0.001, 1,3,light);// получить цвет пикселя
                    Canvas.SetPixel(x, y, color);// закрасить соответствующим цветом каждый пиксель
                }
            }

        }
        static (double, double) IntersectRaySphere(Vector O, Vector D, Spheres sphere)  // вычисление точек пересечения
        {
            Vector C = sphere.Center;
            double r = sphere.Radius;
            Vector OC = O - C;                 //задаем уравнение пересеченя сферы и луча
            double k1 = D * D;
            double k2 = 2 * (OC * D);
            double k3 = (OC * OC) - (r * r);

            double Discriminant = (k2 * k2) - (4 * k1 * k3);  //находим точки пересечения

            if (Discriminant < 0) return (int.MaxValue, int.MaxValue);

            double t1 = (-k2 + Math.Sqrt(Discriminant)) / (2 * k1);
            double t2 = (-k2 - Math.Sqrt(Discriminant)) / (2 * k1);
            return (t1, t2);
        }    

        static (Spheres, double) ClosestIntersection(Vector O, Vector D, Spheres[] sphere, double Tmin, double Tmax)
        {
            Spheres ClosestSphere = new Spheres(0, new Vector(0, 0, 0), Color.Empty, 0,0);
            double closestT = int.MaxValue;

            for (int i = 0; i < 6; i++)
            {
                var tuple = IntersectRaySphere(O, D, sphere[i]);
                double t1 = tuple.Item1;
                double t2 = tuple.Item2;

                if (t1 > Tmin & t1 < Tmax & t1 < closestT)
                {
                    closestT = t1;
                    ClosestSphere = sphere[i];

                }
                if (t2 > 0 & t2 < Tmax & t2 < closestT)
                {
                    closestT = t2;
                    ClosestSphere = sphere[i];
                }
            }

            return (ClosestSphere, closestT);
        }  //вычисление ближайшей точки пересечения
        static Vector CanvasToViewport(int x, int y)  //переход от координат холста к координатам окна просмотра
        {
            Vector D = new Vector(((x * Vw) / Cw), ((y * Vh) / Ch), d);
            return D;
        }
        static Vector ReflectRay(Vector R, Vector N)
            {
            return 2 * (N * R) * N - R;
            }  // вычисление отраженного луча
        static Color TraceRay(Vector O, Vector D,Spheres[] sphere,double Tmin,double Tmax, double RecursionDepth,Light[]light)  // вычисление точек пересечения лучей и сфер
        {
            Tmin = 0.001;     // диапазон значений точек,удовлетворяющих решению
            Tmax = int.MaxValue;
            var tuple = ClosestIntersection(O, D, sphere,Tmin,Tmax);
            Spheres ClosestSphere = tuple.Item1;
            double closestT = tuple.Item2;            
            if (ClosestSphere.Radius == 0) return Color.Empty;
            Vector P = O + closestT * D;
            Vector N = P - ClosestSphere.Center;
            N = (1 / N.Length) * N;
            Vector V = -1 * D;
            Vector ReturnedColor = new Vector(ClosestSphere.Color.R * ComputingLight(P,N,V,ClosestSphere.Specular,sphere,light), ClosestSphere.Color.G *ComputingLight(P,N,V,ClosestSphere.Specular,sphere,light), ClosestSphere.Color.B * ComputingLight(P,N,V,ClosestSphere.Specular,sphere,light));
            double Alpha = ClosestSphere.Color.A;
            Alpha = Alpha * ComputingLight(P, N,V,ClosestSphere.Specular,sphere,light);
            if (Alpha > 255) Alpha = 255;
            if (ReturnedColor.X > 255) ReturnedColor.X = 255;
            if (ReturnedColor.Y > 255) ReturnedColor.Y = 255;
            if (ReturnedColor.Z > 255) ReturnedColor.Z = 255;
            Color LightedColor = Color.FromArgb((int)Alpha,(int)ReturnedColor.X, (int)ReturnedColor.Y, (int)ReturnedColor.Z);

            double r = ClosestSphere.Reflective;
            if (RecursionDepth <= 0 | r <= 0) return LightedColor;

            Vector R = ReflectRay(V, N);
            Color ReflectedColor = TraceRay(P, R, sphere, 0.001, int.MaxValue, RecursionDepth - 1,light);
            Vector LColor = new Vector(LightedColor.R*(1-r), LightedColor.G*(1-r), LightedColor.B*(1-r));
            double ALColor = LightedColor.A;
            ALColor = ALColor * (1 - r);
            LightedColor = Color.FromArgb((int)ALColor,(int) LColor.X, (int)LColor.Y,(int) LColor.Z);
            ReflectedColor = Color.FromArgb((int)(ReflectedColor.A * r), (int)(ReflectedColor.R * r), (int)(ReflectedColor.G * r), (int)(ReflectedColor.B * r));
            Vector FinalyColor = new Vector(LightedColor.R+ReflectedColor.R,LightedColor.G+ReflectedColor.G,LightedColor.B+ReflectedColor.B);
            double AFinalyColor = ALColor + ReflectedColor.A;
            LightedColor = Color.FromArgb((int)AFinalyColor, (int)FinalyColor.X, (int)FinalyColor.Y, (int)FinalyColor.Z);
            return LightedColor;         
        }
        static double ComputingLight(Vector P,Vector N, Vector V, int s,Spheres[] sphere,Light[] light)
        {
            
            double k = 0.0;  // общая интенсивность освещения
            Vector L;
            Vector R;
            double NxL;
            double RxV;
            double TMax;

            for (int i = 0;i<3;i++)
            {
                if (light[i].Type == "ambient") k += light[i].Intensity;
                else
                {
                    if (light[i].Type == "point")
                    {
                        L = light[i].Position - P;
                        TMax = 1;
                    }
                    else
                    {
                        L = light[i].Position;
                        TMax = int.MaxValue;
                    }

                    var tuple = ClosestIntersection(P, L,sphere,0.001,TMax); //проверка теней
                    Spheres ShadowSphere = tuple.Item1;
                    double ShadowT = tuple.Item2;
                    if (ShadowSphere.Radius != 0) continue;
                    NxL = N * L;

                    if (NxL > 0) k += light[i].Intensity * NxL / (N.Length * L.Length); //диффузность

                    if(s!=-1) // зеркальность
                    {
                        R = 2 * NxL * N - L;
                        RxV = R * V;
                        if (RxV > 0) k += light[i].Intensity * Math.Pow(RxV / (R.Length * V.Length), s);
                    }
                }

            }
            return k;
        }  // обработка света,теней и отражения

    } 

    
}

