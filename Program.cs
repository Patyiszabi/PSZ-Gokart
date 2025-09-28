using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;


namespace PSZ_Gokart
/* PSZ
 * 2025.09.15
 * Gokart időpontfoglaló - Egyéni kisprojekt
 */
{
    internal class Program
    {

        public class FoglalasAdat
        {
            public string FoglaloID { get; set; }
            public int JelenlegiLetszam { get; set; }

            public FoglalasAdat(string foglaloId, int kezdetiLetszam)
            {
                FoglaloID = foglaloId;
                JelenlegiLetszam = kezdetiLetszam;
            }
        }
       
        public class Versenyzok
        {
            public string Vezetéknév { get; set; }
            public string Keresztnév { get; set; }
            public string TeljesNev => $"{Vezetéknév} {Keresztnév}";
            public DateTime SzületésiIdő { get; set; }
            public bool Felnőtt { get; set; }
            public string ID { get; set; }
            public string Email { get; set; }

            public Versenyzok(string vezeteknev, string keresztnev, DateTime születésiIdő)
            {
                Vezetéknév = vezeteknev;
                Keresztnév = keresztnev;
                SzületésiIdő = születésiIdő;

                int age = DateTime.Today.Year - SzületésiIdő.Year;
                if (SzületésiIdő.Date > DateTime.Today.AddYears(-age)) age--;
                Felnőtt = age >= 18;

                ID = $"GO-{ForId(Vezetéknév)}{ForId(Keresztnév)}-{születésiIdő:yyyyMMdd}";
                Email = $"{ForId(Vezetéknév)}.{ForId(Keresztnév)}@gmail.com".ToLowerInvariant();
            }
        }
        public class GokartAdatok
        {
            public string Név { get; set; }
            public string Telefonszám { get; set; }
            public string Cim { get; set; }
            public string Webdomain { get; set; }
            public GokartAdatok(string név, string telefonszám, string cim, string webdomain)
            {
                Név = "Patyi Fasza Gokartja";
                Telefonszám = "+36 70 799 0721";
                Cim = "2890 Tata, Kocsi utca 7.";
                Webdomain = "patyi-fasza-gokartja.hu";
            }
        }


        static Dictionary<DateTime, FoglalasAdat> Foglalasok = new Dictionary<DateTime, FoglalasAdat>();
        static Versenyzok[] versenyzok;

        static void Main(string[] args)
        {
            Random rnd = new Random();
            string vezetekFile = File.ReadAllText("vezeteknevek.txt");
            string keresztFile = File.ReadAllText("keresztnevek.txt");

            string[] vezeteknevek = vezetekFile.Split(',')
                                       .Select(n => n.Trim(' ', '\'', '\r', '\n'))
                                       .Where(n => !string.IsNullOrWhiteSpace(n))
                                       .ToArray();

            string[] keresztnevek = keresztFile.Split(',')
                                                 .Select(n => n.Trim(' ', '\'', '\r', '\n'))
                                                 .Where(n => !string.IsNullOrWhiteSpace(n))
                                                 .ToArray();

            int db = rnd.Next(1, 151);
            versenyzok = new Versenyzok[db];

            for (int i = 0; i < db; i++)
            {
                string vez = vezeteknevek[rnd.Next(vezeteknevek.Length)];
                string ker = keresztnevek[rnd.Next(keresztnevek.Length)];
                DateTime szuletesi = new DateTime(rnd.Next(1950, 2015),
                                                 rnd.Next(1, 13),
                                                 rnd.Next(1, 29));

                versenyzok[i] = new Versenyzok(vez, ker, szuletesi);
            }

            bool running = true;
            MessageBox.Show("Üdvözöljük a 'Patyi-fasza gokartja' időpontfoglaló programjában!");
            while (running)
            {
                Console.Clear();
                Console.WriteLine("--- 'Patyi-fasza gokartja' időpontfoglaló program ---");
                Console.WriteLine("1 - Versenyzők listája");
                Console.WriteLine("2 - Időpontok megjelenítése");
                Console.WriteLine("3 - Foglalás / Átfoglalás / Csatlakozás");
                Console.WriteLine("4 - Kilépés");
                Console.Write("\nVálasszon egy opciót (1-4): ");

                string valasztas = Console.ReadLine();
                switch (valasztas)
                {
                    case "1":
                        ListaVersenyzok();
                        break;
                    case "2":
                        IdopontokMegjelenitese();
                        break;
                    case "3":
                        Console.Clear();
                        Console.Write("Kérem adja meg a versenyző ID-ját: ");

                        string inputId = Console.ReadLine();
                        Versenyzok found = null;
                        foreach (var v in versenyzok)
                        {
                            if (v.ID == inputId)
                            {
                                found = v;
                                break;
                            }
                        }

                        if (found != null)
                        {
                            IdopontFoglalas(found.ID);
                        }
                        else
                        {
                            Console.WriteLine("Nincs ilyen azonosító.");
                            Console.WriteLine("Nyomjon Entert a főmenübe való visszatéréshez...");
                            Console.ReadLine();
                        }
                        break;
                    case "4":
                        running = false;
                        Console.WriteLine("Kilépés...");
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás! Kérjük, adjon meg egy számot (1-4).");
                        Console.WriteLine("Nyomjon Entert a folytatáshoz...");
                        Console.ReadLine();
                        break;
                }
            }
            if (!running)
            {
                MessageBox.Show("Köszönjük szépen, hogy alkalmazásunkat használta!");
            }
        }

        public static void ListaVersenyzok()
        {
            Console.Clear();
            Console.WriteLine("--- Versenyzők listája ---");
            Console.WriteLine($"\nEnnyi verzenyző van beregisztrálva: {versenyzok.Length}");
            Console.WriteLine("\nA versenyzők adatai az alábbiak:\n");
            foreach (var v in versenyzok)
            {
                Console.WriteLine($"{v.ID} | {v.TeljesNev} | {v.SzületésiIdő:yyyy.MM.dd} | Felnőtt: {v.Felnőtt} | {v.Email}");
            }

            Console.WriteLine("\nNyomjon Entert a főmenübe való visszatéréshez...");
            Console.ReadLine();
        }

        public static void IdopontokMegjelenitese()
        {
            Console.Clear();
            Console.WriteLine("--- Időpontok megjelenítése ---");
            Console.WriteLine("Telítettség jelölése: ZÖLD (Szabad), SÁRGA (Részben foglalt), PIROS (Teljesen foglalt / 20 fő)");

            DateTime start = DateTime.Today;
            DateTime end = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));

            for (DateTime nap = start; nap <= end; nap = nap.AddDays(1))
            {
                Console.WriteLine($"\n{nap:MM.dd.} ({nap:dddd}):");
                for (int ora = 8; ora <= 18; ora++)
                {
                    DateTime slot = new DateTime(nap.Year, nap.Month, nap.Day, ora, 0, 0);

                    if (Foglalasok.ContainsKey(slot))
                    {
                        FoglalasAdat foglalas = Foglalasok[slot];

                        if (foglalas.JelenlegiLetszam >= 20)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"  {ora}:00-{ora + 1}:00 FOGLALT (TELT HÁZ - 20 fő): ");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write($"  {ora}:00-{ora + 1}:00 FOGLALT ({foglalas.JelenlegiLetszam} fő / MAX 20): ");
                        }

                        string foglaloNev = foglalas.FoglaloID;

                        Versenyzok v = null;
                        foreach (var versenyzo in versenyzok)
                        {
                            if (versenyzo.ID == foglalas.FoglaloID)
                            {
                                v = versenyzo;
                                break;
                            }
                        }

                        if (v != null)
                        {
                            foglaloNev = v.TeljesNev;
                        }

                        Console.WriteLine($"Eredeti foglaló: {foglaloNev}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"  {ora}:00-{ora + 1}:00 SZABAD");
                    }
                    Console.ResetColor();
                }
            }

            Console.WriteLine("\nNyomjon Entert a főmenübe való visszatéréshez...");
            Console.ReadLine();
        }

        public static void CsatlakozasFoglalasAzon(string versenyzoID)
        {
            Console.Clear();
            Console.WriteLine("--- Csatlakozás meglévő foglaláshoz ---");
            Console.WriteLine("Kérjük válasszon időpontot az 'Időpontok megjelenítése' menüpontban látottak alapján.");

            Console.Write("Adja meg a napot (MM.dd): ");
            string napInput = Console.ReadLine();
            Console.Write("Adja meg a kezdő órát (8-18): ");

            int ora;
            while (!int.TryParse(Console.ReadLine(), out ora) || ora < 8 || ora > 18)
            {
                Console.WriteLine("Érvénytelen óra. Kérjük adjon meg 8 és 18 közötti egész számot.");
                Console.Write("Adja meg a kezdő órát (8-18): ");
            }


            DateTime datum;
            try
            {
                datum = DateTime.ParseExact(napInput, "MM.dd", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nÉrvénytelen dátum formátum. Kérjük térjen vissza a menübe.");
                Console.WriteLine("Nyomjon Entert a főmenübe való visszatéréshez...");
                Console.ReadLine();
                return;
            }

            DateTime slot = new DateTime(DateTime.Today.Year, datum.Month, datum.Day, ora, 0, 0);

            if (Foglalasok.ContainsKey(slot))
            {
                FoglalasAdat foglalas = Foglalasok[slot];
                int szabadHely = 20 - foglalas.JelenlegiLetszam;

                if (szabadHely > 0)
                {
                    Console.WriteLine($"\nA(z) {slot:MM.dd. HH}:00 időpont szabad helyeinek száma: {szabadHely} fő.");

                    int csatlakozokSzama;
                    do
                    {
                        Console.Write($"Hány fővel szeretne csatlakozni (Max. {szabadHely} fő): ");
                        if (int.TryParse(Console.ReadLine(), out csatlakozokSzama) && csatlakozokSzama > 0 && csatlakozokSzama <= szabadHely)
                        {
                            break;
                        }
                        Console.WriteLine($"Érvénytelen létszám. Kérjük adjon meg 1 és {szabadHely} közötti számot.");
                    } while (true);

                    foglalas.JelenlegiLetszam += csatlakozokSzama;
                    Console.WriteLine($"\nSikeresen csatlakozott {csatlakozokSzama} fővel a(z) {slot:MM.dd. HH}:00 időponthoz!");
                }
                else
                {
                    Console.WriteLine("\nSajnáljuk, de ez az időpont betelt (20 fő).");
                }
            }
            else
            {
                Console.WriteLine("\nHiba: A megadott időpont még nincs lefoglalva, vagy érvénytelen dátum/óra.");
            }

            Console.WriteLine("Nyomjon Entert a főmenübe való visszatéréshez...");
            Console.ReadLine();
        }

        public static void IdopontFoglalas(string versenyzoID)
        {
            List<DateTime> jelenlegi = new List<DateTime>();
            foreach (KeyValuePair<DateTime, FoglalasAdat> kv in Foglalasok)
            {
                if (kv.Value.FoglaloID == versenyzoID)
                {
                    jelenlegi.Add(kv.Key);
                }
            }

            if (jelenlegi.Count > 0)
            {
                Console.WriteLine("A versenyzőnek van meglévő foglalása:");
                foreach (var d in jelenlegi) Console.WriteLine($" - {d:MM.dd. HH}:00");
                Console.Write("Törli a meglévő foglalást? (i/n): ");
                if (Console.ReadLine()?.ToLower() == "i")
                {
                    foreach (var d in jelenlegi)
                    {
                        Foglalasok.Remove(d);
                    }
                    Console.WriteLine("Meglévő foglalások törölve.");
                }
                else
                {
                   
                }
            }

            Console.WriteLine("Válasszon: Új foglalás létrehozása (U) vagy Csatlakozás meglévőhöz (C)?");
            Console.Write("Választás (U/C): ");
            string choice = Console.ReadLine()?.ToUpper();

            if (choice == "C")
            {
                CsatlakozasFoglalasAzon(versenyzoID);
                return;
            }

            Console.WriteLine("\n--- Új foglalás létrehozása ---");

            int letszam;
            do
            {
                Console.Write("Adja meg a létszámot (minimum 8, maximum 20 fő): ");
                if (int.TryParse(Console.ReadLine(), out letszam) && letszam >= 8 && letszam <= 20)
                {
                    break;
                }
                Console.WriteLine("Sajnálatos módon minimum 8, maximum 20 személyt tudunk fogadni.");
            } while (true);

            Console.Write("Adja meg a napot (MM.dd): ");
            string napInput = Console.ReadLine();
            Console.Write("Adja meg a kezdő órát (8-18): ");

            int ora;
            while (!int.TryParse(Console.ReadLine(), out ora) || ora < 8 || ora > 18)
            {
                Console.WriteLine("Érvénytelen óra. Kérjük adjon meg 8 és 18 közötti egész számot.");
                Console.Write("Adja meg a kezdő órát (8-18): ");
            }

            Console.Write("Adja meg 1 vagy 2 órát szeretne gokartozni: ");

            int tartam;
            while (!int.TryParse(Console.ReadLine(), out tartam) || (tartam != 1 && tartam != 2))
            {
                Console.WriteLine("Érvénytelen időtartam. Kérjük adjon meg 1 vagy 2 órát.");
                Console.Write("Adja meg 1 vagy 2 órát szeretne gokartozni: ");
            }


            DateTime datum;
            try
            {
                datum = DateTime.ParseExact(napInput, "MM.dd", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nÉrvénytelen dátum formátum. Kérjük térjen vissza a menübe.");
                Console.WriteLine("Nyomjon Entert a főmenübe való visszatéréshez...");
                Console.ReadLine();
                return;
            }


            bool sikeresFoglalas = true;
            for (int i = 0; i < tartam; i++)
            {
                DateTime slot = new DateTime(DateTime.Today.Year, datum.Month, datum.Day, ora + i, 0, 0);

                if (Foglalasok.ContainsKey(slot))
                {
                    Console.WriteLine($"\nHIBA: A(z) {slot:MM.dd. HH}:00 időpont már foglalt. Az új foglalás sikertelen.");
                    sikeresFoglalas = false;
                    break;
                }

                Foglalasok[slot] = new FoglalasAdat(versenyzoID, letszam);
            }

            if (sikeresFoglalas)
            {
                Console.WriteLine("\nFoglalás rögzítve!\nMostmár mehet a menet!");
            }
            Console.WriteLine("Nyomjon Entert a főmenübe való visszatéréshez...");
            Console.ReadLine();

        }

        public static string CleanName(string name)
        {
            string normalized = name.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char c in normalized)
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string ForId(string name)
        {
            var clean = CleanName(name);
            return new string(clean.Where(char.IsLetterOrDigit).ToArray()).ToLower();
        }
    }
}