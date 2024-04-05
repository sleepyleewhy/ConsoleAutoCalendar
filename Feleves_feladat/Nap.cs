using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_feladat
{
    internal class Nap
    {
        class NapElem
        {
            public ITeendo tartalom;
            public NapElem kovetkezo;
        }
        public DateTime napeleje;
        public DateTime napvege { get { return napeleje.AddDays(1); } }
        NapElem fej { get; set; }
        public Nap(DateTime eznap)
        {
            this.napeleje = eznap;
            fej = null;
        }
        public bool BennVanE(ITeendo tartalom)
        {
            NapElem p = fej;
            while (p != null && p.tartalom != tartalom)
            {
                p = p.kovetkezo;
            }
            return p != null;
        }
        NapElem Kereses(ITeendo tartalom)
        {
            NapElem p = fej;
            while (p != null && p.tartalom != tartalom)
            {
                p = p.kovetkezo;
            }
            return p;
        }
        public void Torles(ITeendo tartalom)
        {
            NapElem p = fej;
            NapElem e = null;
            while (p != null && p.tartalom != tartalom)
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


        public bool Beszuras(ITeendo feladat)
        {
            NapElem uj = new NapElem();
            uj.tartalom = feladat;
            if (fej == null)
            {
                uj.tartalom.Kezdes = napeleje;
                fej = uj;
                uj.kovetkezo = null;
                return true;
            }
            else
            {
                if (fej.tartalom.Kezdes >= napeleje.AddMinutes(uj.tartalom.Idotartam))
                {
                    uj.kovetkezo = fej;
                    uj.tartalom.Kezdes = napeleje.AddMinutes(uj.tartalom.Idotartam);
                    fej = uj;
                    return true;
                }
                else
                {
                    NapElem p = fej;
                    while (p.kovetkezo != null && p.tartalom.Vege.AddMinutes(uj.tartalom.Idotartam) >= p.kovetkezo.tartalom.Kezdes)
                    {
                        p = p.kovetkezo;
                    }
                    if (p.kovetkezo == null && p.tartalom.Vege.AddMinutes(uj.tartalom.Idotartam) <= napvege)
                    {
                        p.kovetkezo = uj;
                        uj.tartalom.Kezdes = p.tartalom.Vege;
                        uj.kovetkezo = null;
                        return true;
                    }
                    else if (p.kovetkezo != null && p.tartalom.Kezdes.AddMinutes(uj.tartalom.Idotartam) <= p.kovetkezo.tartalom.Kezdes)
                    {
                        uj.kovetkezo = p.kovetkezo;
                        uj.tartalom.Kezdes = p.tartalom.Vege;
                        p.kovetkezo = uj;
                        return true;

                    }
                    else { return false; }
                }

            }
        }
        public bool KotottIdopontBehelyezes(ITeendo tartalom)
        {

            NapElem uj = new NapElem();
            uj.tartalom = tartalom;
            NapElem p = fej;
            NapElem e = null;
            while (p != null && p.tartalom.Kezdes < uj.tartalom.Vege)
            {
                e = p;
                p = p.kovetkezo;
            }
            if (e != null && p!=null && p.tartalom.Kezdes > uj.tartalom.Vege && e.tartalom.Vege < uj.tartalom.Kezdes)
            {
                e.kovetkezo = uj;
                uj.kovetkezo = p;
                return true;
            }
            else if (e == null)
            {
                uj.kovetkezo = fej;
                fej = uj;
                return true;
            }
            else if (p == null && e.tartalom.Vege < uj.tartalom.Kezdes)
            {
                e.kovetkezo = uj;
                uj.kovetkezo = null;
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool Modositas(ITeendo feladat)
        {
            NapElem elem = Kereses(feladat);
            if (elem.kovetkezo != null && elem.tartalom.Vege.AddMinutes(30) <= elem.kovetkezo.tartalom.Kezdes && elem.tartalom.Vege <= elem.tartalom.Hatarido && elem.tartalom.Vege.AddMinutes(30) <= napvege)
            {
                elem.tartalom.Kezdes = elem.tartalom.Kezdes.AddMinutes(30);
                return true;
            }
            else if (elem.kovetkezo == null && elem.tartalom.Vege.AddMinutes(30) <= napvege && elem.tartalom.Vege.AddMinutes(30) <= elem.tartalom.Hatarido)
            {
                elem.tartalom.Kezdes = elem.tartalom.Kezdes.AddMinutes(30);
                return true;
            }
            else if (elem.kovetkezo != null)
            {
                NapElem p = elem.kovetkezo;
                while (elem.tartalom.Hatarido >= p.tartalom.Vege.AddMinutes(elem.tartalom.Idotartam) && p.kovetkezo != null && p.tartalom.Vege.AddMinutes(elem.tartalom.Idotartam) > p.kovetkezo.tartalom.Kezdes)
                {
                    p = p.kovetkezo;
                }
                if (p.kovetkezo == null && p.tartalom.Vege.AddMinutes(elem.tartalom.Idotartam) <= napvege && p.tartalom.Vege.AddMinutes(elem.tartalom.Idotartam) <= elem.tartalom.Hatarido)
                {
                    p.kovetkezo = elem;
                    elem.tartalom.Kezdes = p.tartalom.Vege;
                }
                if (elem.tartalom.Hatarido >= p.tartalom.Vege.AddMinutes(elem.tartalom.Idotartam) && p.kovetkezo != null)
                {
                    elem.kovetkezo = p.kovetkezo;
                    p.kovetkezo = elem;
                    elem.tartalom.Kezdes = p.tartalom.Vege;
                    return true;
                }
                else if (p.kovetkezo != null && elem.tartalom.Hatarido >= p.tartalom.Vege.AddMinutes(elem.tartalom.Idotartam) && p.tartalom.Vege.AddMinutes(elem.tartalom.Idotartam) >= p.kovetkezo.tartalom.Kezdes)
                {
                    if (p.tartalom.Vege.AddMinutes(elem.tartalom.Idotartam) <=  napvege)
                    {
                        p.kovetkezo = elem;
                        elem.tartalom.Kezdes = p.tartalom.Vege.AddMinutes(elem.tartalom.Idotartam);
                        return true;
                    }
                }
                return false;

            }
            return false;
            
        }
        public void FeladatKiiras()
        {
            NapElem p = fej;
            while (p != null)
            {
                Console.WriteLine($"{p.tartalom.Nev}, Kezdés: {p.tartalom.Kezdes} , Vége: {p.tartalom.Vege}");
                p = p.kovetkezo;
            }
        }
    }
}
