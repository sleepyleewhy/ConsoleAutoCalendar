using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_feladat
{
    internal class VisszateroTeendo: ITeendo
    {
        public string Nev { get; }
        public int Prioritas { get; }
        public int Idotartam { get; }
        public int Rendszeresseg { get; }
        public DateTime Kezdes { get; set; }
        public bool KotottIdopont { get; }
        public DateTime Hatarido { get; set;
        }
        public DateTime Meddig { get; }
        public DateTime Vege { get { return Kezdes.AddMinutes(Idotartam); } }
        public VisszateroTeendo(string nev, int idotartam, int prio, int rendszer)
        {
            Nev = nev;
            Idotartam = idotartam;
            Prioritas = prio;
            Rendszeresseg = rendszer;
        }


    }
}
