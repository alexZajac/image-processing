using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_final
{
    public class Pixel
    {
        private byte red;
        private byte green;
        private byte blue; // espace RGB
        private double hue;
        private double saturation;
        private double valeur; // Espace HSV


        public Pixel(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            hue = 0; // On détermine les valeurs de H, S et V en fonction de la couleur du pixel
            double min = Math.Min(Math.Min(red, green), blue);
            valeur = Math.Max(Math.Max(red, green), blue);
            double Delta = valeur - min;
            
            if (valeur == 0) { saturation = 0; }
            else { saturation = Delta / valeur; }

            if (saturation != 0)
            {
                if (red == valeur) { hue = ((green - blue) / Delta) % 6;  }

                else if (green == valeur) { hue = 2 + (blue - red) / Delta; }

                else if (blue == valeur) { hue = 4 + (red - green) / Delta; }

                hue *= 60;

                if (hue < 0.0) { hue += 360; }
            }
            valeur /= 255;
        }

        public byte Red
        {
            get { return red; }
            set { red = value; }
        }

        public byte Green
        {
            get { return green; }
            set { green = value; }
        }

        public byte Blue
        {
            get { return blue; }
            set { blue = value; }
        }

        public double Hue
        {
            get { return hue; }
            set { hue = value; }
        }

        public double Saturation
        {
            get { return saturation; }
            set { saturation = value; }
        }

        public double Value
        {
            get { return valeur; }
            set { valeur = value; }
        }

    }
}
