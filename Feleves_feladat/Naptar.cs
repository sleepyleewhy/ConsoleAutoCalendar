using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feleves_feladat
{
    internal class Naptar
    {

        public PrioSor prio = new PrioSor();
        public List<string> ProcessedFiles = new List<string>();
        public List<Nap> napok = new List<Nap>();
        public List<MindennapiTeendo> MindenNapos = new List<MindennapiTeendo>();
        DateTime EddigInicializalt;
        public DateTime JelenlegiNap;
        private DateTime MaxHatarido;
        public List<ITeendo> PrioList = new List<ITeendo>();

        public Naptar(DateTime elsonap)
        {
            napok.Add(new Nap(elsonap));
            EddigInicializalt = elsonap.AddDays(-1);
        }

        public void Feltoltes()
        {
            string filename = FajlKereses(@"C:\Users\endll\Óbudai Onedrive\OneDrive - Óbudai egyetem\Egyetem\2. félév\SZTF2\Feleves feladat\Feleves_feladat\Feleves_feladat\Bemenet", "teendok");
            

            string[] osszessor = File.ReadAllLines(filename);
            foreach (var sor in osszessor)
            {
                string[] feladat= sor.Split(';');
                if (feladat[0].ToUpper() == "M")
                {
                    if (feladat.Length == 4)
                    {
                        MindenNapos.Add(new MindennapiTeendo(feladat[1], int.Parse(feladat[2]), int.Parse(feladat[3])));
                    }
                    else if (feladat.Length == 5)
                    {
                        MindenNapos.Add(new MindennapiTeendo(feladat[1], int.Parse(feladat[2]), int.Parse(feladat[3]), DateTime.Parse(feladat[4])));
                    }
                    else
                    {
                        throw new HibasBemenetException();
                    }
                }
                else if (feladat[0].ToUpper() == "E")
                {
                    PrioListaBeszuras(new EgyszeriTeendo(feladat[1], int.Parse(feladat[2]), int.Parse(feladat[3]), bool.Parse(feladat[4]), DateTime.Parse(feladat[5])));
                }
                else if (feladat[0].ToUpper() == "V")
                { 
                        VisszateroTeendo teendo = new VisszateroTeendo(feladat[1], int.Parse(feladat[2]), int.Parse(feladat[3]), int.Parse(feladat[4]));
                        GregorianCalendar calendar = new GregorianCalendar();
                        CalendarWeekRule rule = CalendarWeekRule.FirstDay;
                        DayOfWeek FirstDayOfWeek = DayOfWeek.Monday;
                        int jelenlegihet = calendar.GetWeekOfYear(JelenlegiNap, rule, FirstDayOfWeek);
                        DateTime Vasarnap = JelenlegiNap;
                    while (Vasarnap.DayOfWeek != DayOfWeek.Sunday)
                    {
                        Vasarnap.AddDays(1);
                    }
                        int meddighet = calendar.GetWeekOfYear(teendo.Meddig, rule, FirstDayOfWeek);
                    while (jelenlegihet != meddighet)
                    {
                        for (int i = 0; i < teendo.Rendszeresseg; i++)
                        {
                            PrioListaBeszuras(new AlapTeendo(teendo.Nev, teendo.Idotartam, teendo.Prioritas,"H", Vasarnap));
                        }
                        jelenlegihet++;
                    }
                }
            }
            

        }
        public string FajlKereses(string path, string searchkey)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Contains(searchkey) && !ProcessedFiles.Contains(file))
                {
                    ProcessedFiles.Add(file);
                    return file;
                    
                }
                
            }
            throw new NincsUjFileException();

        }
       void PrioListaBeszuras(ITeendo tartalom)
        {
            if (PrioList.Count == 0)
            {
                PrioList.Add(tartalom);
            }
            else
            {
                int i = 0;
                while (i < PrioList.Count && PrioList[i].Prioritas > tartalom.Prioritas)
                {
                    i++;
                }
                if (i >= PrioList.Count)
                {
                    PrioList.Insert(i, tartalom);
                }
                else
                {
                    while (i < PrioList.Count && PrioList[i].Prioritas == tartalom.Prioritas && PrioList[i].Hatarido < tartalom.Hatarido)
                    {
                        i++;
                    }
                    PrioList.Insert(i, tartalom);
                }          
               
            }
        }




        public void Backtrack_2(int szint, ref bool van)
        {
            bool kotottproba = false;
            while (!van && ((!PrioList[szint].KotottIdopont && PrioList[szint].Hatarido >= PrioList[szint].Vege) || !kotottproba))
            {
                Console.Clear();
                Console.WriteLine(szint);
                if (PrioList[szint].KotottIdopont)
                {
                    int j = 0;
                    while (PrioList[szint].Kezdes.Day != napok[j].napeleje.Day)
                    {
                        j++;
                    }
                    if (napok[j].KotottIdopontBehelyezes(PrioList[szint]))
                    {
                        if (szint == PrioList.Count-1)
                        {
                            van = true;
                        }
                        else
                        {
                            Backtrack_2(szint + 1, ref van); 
                        }
                    }
                    kotottproba = true;
                }
                else
                {
                    int j = 0;
                    bool bennvane = napok[j].BennVanE(PrioList[szint]);
                    while (napok.Count > j && napok[j].napvege <= PrioList[szint].Hatarido && !bennvane)
                    {
                        j++;
                        if (napok.Count > j)
                        {
                            bennvane = napok[j].BennVanE(PrioList[szint]);
                        }
                    }
                    if (bennvane && napok[j].napvege <= PrioList[szint].Hatarido && napok[j].Modositas(PrioList[szint]))
                    {
                        if (szint == PrioList.Count-1)
                        {
                            van = true;
                        }
                        else
                        {
                            Backtrack_2(szint + 1, ref van);
                        }
                    }
                    else if (!bennvane)
                    {
                        int k = 0;
                        bool beszurte = false;
                        while (napok.Count > k && napok[k].napvege <= PrioList[szint].Hatarido && !beszurte)
                        {
                            if (PrioList[szint].Kezdes > new DateTime(1000,1,1) && PrioList[szint].Kezdes >= napok[k].napeleje && PrioList[szint].Kezdes < napok[k].napvege)
                            {
                                beszurte = napok[k].Beszuras(PrioList[szint]);
                            }
                            else if (PrioList[szint].Kezdes == new DateTime(1,1,1))
                            {
                                beszurte = napok[k].Beszuras(PrioList[szint]);
                            }
                            k++;
                        }
                        k--;
                        if (napok[k].napvege <= PrioList[szint].Hatarido && beszurte)
                        { 
                            if (szint == PrioList.Count-1)
                            {
                                van = true;
                            }
                            else
                            {
                                Backtrack_2(szint + 1, ref van);
                            }
                        }
                        else if (PrioList[szint].Vege.AddMinutes(10) > PrioList[szint].Hatarido)
                        {
                            PrioList[szint].Hatarido = PrioList[szint].Hatarido.AddDays(1);
                            if (PrioList[szint].Hatarido > MaxHatarido)
                            {
                                NaptarInicializalas();
                            }
                            Backtrack_2(szint, ref van);
                        }
                        else if (!beszurte)
                        {
                            break;
                        }
                    }
                    else if (bennvane)
                    {
                        napok[j].Torles(PrioList[szint]);
                        break;
                    }
                }
                
            }
        }

        public void NaptarBeosztas()
        {
            NaptarInicializalas();
            bool van = false;
            Backtrack_2(0, ref van);
            if (van)
            {
                NaptarKiiras();
            }
            else
            {
                Console.WriteLine("Nem sikerült a beosztás");
            }
        }




       

        public void NaptarInicializalas()
        {
            DateTime MaxHatarido = new DateTime(1, 1, 1);
            if (PrioList != null)
            {
                foreach (ITeendo item in PrioList)
                {
                    if (item.Hatarido > MaxHatarido)
                    {
                        MaxHatarido = item.Hatarido;

                    }
                }
                this.MaxHatarido = MaxHatarido.AddDays(1);
            }       
            while (EddigInicializalt < MaxHatarido)
            {

                EddigInicializalt = EddigInicializalt.AddDays(1);
                napok.Add(new Nap(EddigInicializalt));
                foreach (MindennapiTeendo item in MindenNapos)
                {
                    if (item.KotottIdopont)
                    {
                        PrioListaBeszuras(new AlapTeendo(item.Nev, item.Idotartam, item.Prioritas, "K", new DateTime(EddigInicializalt.Year, EddigInicializalt.Month, EddigInicializalt.Day, item.Kezdes.Hour, item.Kezdes.Minute, 0)));
                    }
                    else
                    {
                        
                        PrioListaBeszuras(new AlapTeendo(item.Nev, item.Idotartam, item.Prioritas, "H", EddigInicializalt.AddDays(1), EddigInicializalt));
                    }
                }
            }


            
        }
        public void NaptarKiiras()
        {
            foreach (Nap item in napok)
            {
                Console.WriteLine(item.napeleje.Day);
                Console.WriteLine();
                item.FeladatKiiras();
            }
        }

    }

    
}
