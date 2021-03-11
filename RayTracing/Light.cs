


    public class Light
    {
        public double Intensity;

        public Vector Position;

        public Vector Direction;

        public string Type;

        public Light(string Type,double Intensity)
        {
            this.Type = Type;
            this.Intensity = Intensity;
        }

        public Light(string Type, double Intensity, Vector Position)
        {
            this.Type = Type;
            this.Intensity = Intensity;
            this.Position = Position;
        }

      
    }

