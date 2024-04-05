using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_feladat
{
    internal class PrioSor
    {
        public class SorElem
        {
            public ITeendo tartalom;
            public SorElem kovetkezo;

        }
        public SorElem fej { get;private set; }
        public PrioSor()
        {
            fej = null;
        }
        public void Torles(string torlendo)
        {
            SorElem p = fej;
            SorElem e = null;
            while (p != null && !p.tartalom.Nev.Equals(torlendo))
            {
                e = p;
                p = p.kovetkezo;
            }
            if (p != null)
            {
                if (e == null)
                {
                    fej = p.kovetkezo;
                }
                else
                {
                    e.kovetkezo = p.kovetkezo;
                }
            }
            else
            {
                throw new NincsIlyenElemException();
            }
        }
        public void Beszuras(ITeendo beszurando)
        {
            SorElem uj = new SorElem();
            uj.tartalom = beszurando;
            SorElem p = fej;
            SorElem e = null;
            while (p != null && p.tartalom.Prioritas > uj.tartalom.Prioritas)
            {
                e = p;
                p = p.kovetkezo;
            }
            if (e == null)
            {
                uj.kovetkezo = fej;
                fej = uj;
            }
            else
            {
                uj.kovetkezo = p;
                e.kovetkezo = uj;
            }

        }
    }
}
