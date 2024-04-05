using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_feladat
{
    internal class AlapTeendo : ITeendo
    {
        public string Nev { get; }
        public int Prioritas { get; }
        public int Idotartam { get; }
        public DateTime Kezdes { get; set; }
        public DateTime Hatarido { get; set; }
        public bool KotottIdopont { get; }
        public DateTime Vege { get { return Kezdes.AddMinutes(Idotartam); } }
        public AlapTeendo(string nev, int idotartam, int prio)
        {
            Nev = nev;
            Idotartam = idotartam;
            Prioritas = prio;
        }
        public AlapTeendo(string nev, int idotartam, int prio, string melyik, DateTime idopont)
        {
            Nev = nev;
            Idotartam = idotartam;
            Prioritas = prio;
            if (melyik == "H")
            {
                Hatarido = idopont;
            }
            else if (melyik == "K")
            {
                Kezdes = idopont;
                Prioritas *= 10;
            }
            
           
        }
        public AlapTeendo(string nev , int idotartam, int prio, string melyik, DateTime idopont, DateTime kezdes)
        {
            Nev = nev;
            Idotartam = idotartam;
            Prioritas = prio;
            if (melyik == "H")
            {
                Hatarido = idopont;
                Kezdes = kezdes;
            }
            else if (melyik == "K")
            {
                Kezdes = kezdes;
                Prioritas *= 10;
            }

        }
    }
}