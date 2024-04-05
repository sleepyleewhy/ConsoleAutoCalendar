using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_feladat
{
    internal class EgyszeriTeendo  :ITeendo
    {
        public string Nev { get; }
        public int Idotartam { get; }
        public int Prioritas { get; }

        public bool KotottIdopont { get; }
        public DateTime Vege { get { return Kezdes.AddMinutes(Idotartam); } }
        public DateTime Kezdes { get; set; }
        public DateTime Hatarido { get; set; }

        public EgyszeriTeendo(string nev, int idotartam, int prioritas, bool kotott, DateTime idopont)
        {
            Nev = nev;
            Idotartam = idotartam;
            Prioritas = prioritas;

            if (!kotott)
            {
                Hatarido = idopont;
            }
            else
            {
                Kezdes = idopont;
                Prioritas *= 10;
                KotottIdopont = true;

            }
        }

    }
}
