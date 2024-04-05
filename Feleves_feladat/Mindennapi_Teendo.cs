using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_feladat
{
    internal class MindennapiTeendo : ITeendo
    {
        public string Nev { get; }
        public int Idotartam { get; }
        public int Prioritas { get; }

        public DateTime Kezdes { get; set; }
        public DateTime Hatarido { get; set;
        }
        public bool KotottIdopont { get; set; }
        public DateTime Vege { get { return Kezdes.AddMinutes(Idotartam); } }
        public MindennapiTeendo(string nev, int idotartam, int prioritas)
        {

            Nev = nev;
            Idotartam = idotartam;
            Prioritas = prioritas;
            KotottIdopont = false;

        }
        public MindennapiTeendo(string nev, int idotartam, int prioritas, DateTime kotottIdopont)
        {
            Nev = nev;
            Idotartam = idotartam;
            Prioritas = prioritas*10;
            Kezdes = kotottIdopont;
            KotottIdopont = true;
        }
    }
}
