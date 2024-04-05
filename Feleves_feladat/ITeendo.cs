using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_feladat
{
    interface ITeendo
    {
        string Nev { get; }
        int Idotartam { get; }
        int Prioritas { get; }

        DateTime Kezdes { get; set; }

        bool KotottIdopont { get; }
        DateTime Hatarido { get; set; }
        DateTime Vege { get; }

    }
}
