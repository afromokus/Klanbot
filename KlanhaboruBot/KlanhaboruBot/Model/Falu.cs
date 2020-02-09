using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlanhaboruBot.Model
{
    class Falu
    {
        string felhNev;
        string jelszo;
        int vilagszam;

        int faluSzam;

        public Falu()
        {

        }

        public Falu(string felhNev, string jelszo, int vilagszam, int faluSzam)
        {
            this.felhNev = felhNev;
            this.jelszo = jelszo;
            this.vilagszam = vilagszam;
            this.faluSzam = faluSzam;
        }

        public override string ToString()
        {
            return felhNev + "\t" + vilagszam + "\t" + faluSzam;
        }

        public string FelhNev { get => felhNev; set => felhNev = value; }
        public string Jelszo { get => jelszo; set => jelszo = value; }
        public int FaluSzam { get => faluSzam; set => faluSzam = value; }
        public int Vilagszam { get => vilagszam; set => vilagszam = value; }
    }
}
