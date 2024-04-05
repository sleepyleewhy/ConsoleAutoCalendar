using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_feladat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Naptar Naptar1 = new Naptar(new DateTime(2023,1,1));
            Naptar1.Feltoltes();
            Console.WriteLine();
            Naptar1.NaptarBeosztas();
            Console.ReadKey();

        }
        static void Kiiras(Naptar sor)
        {
            Console.WriteLine("A sorban lévő teendők:");
            PrioSor.SorElem  p = sor.prio.fej;
            while (p != null)
            {
                Console.WriteLine($"{(p.tartalom as MindennapiTeendo).Nev}, {(p.tartalom as MindennapiTeendo).Idotartam}, {(p.tartalom as MindennapiTeendo).Prioritas}, {(p.tartalom as MindennapiTeendo).Kezdes}");
                p = p.kovetkezo;
            }
        }
    }
}
