using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSZ_Gokart
/* PSZ
 * 2025.09.15
 * Gokart időpontfoglalő - Egyéni kisprojekt
 */
{
    internal class Program
    {
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

        public class Versenyzok
        {
            public string Vezetéknév { get; set; }
            public string Keresztnév { get; set; }
            public DateTime SzületésiIdő { get; set; }
            public bool Felnőtt { get; set; }
            public string ID { get; set; }
            public string Email { get; set; }
            public Versenyzok(string vezeteknev, string keresztnev, DateTime születésiIdő, bool felnőtt, string id, string email)
            {
                Vezetéknév = vezeteknev;
                Keresztnév = keresztnev;
                SzületésiIdő = születésiIdő;
                Felnőtt = (DateTime.Now.Year - SzületésiIdő.Year) >= 18;
                ID = $"GO-{vezeteknev}{keresztnev}-{születésiIdő:yyyyMMdd}";
                Email = $"{vezeteknev}.{keresztnev}@gmail.com";
            }
        }

        public class Szabalyok
        {
            public string Idő { get; set; }
            public string Időtartam { get; set; }
            public string verzenyzoszam { get; set; }
            public Szabalyok(string idő, string időtartam, string verzenyzoszam)
            {
                Idő = "08:00-19:00";
                Időtartam = "60 perc";
                verzenyzoszam = "Min.: 8, Max.: 20";
            }
        }

        // Helper to remove diacritics and trim unwanted characters
        public static string CleanName(string name)
        {
            // Remove apostrophes
            name = name.Replace("'", "");
            // Remove diacritics
            string normalized = name.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char c in normalized)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            // Return trimmed and cleaned string
            return sb.ToString().Normalize(NormalizationForm.FormC).Trim();
        }

        static void Main(string[] args)
        {
            string[] file1 = File.ReadAllLines("C:\\25-26_PSZ_ IKT\\PSZ-Gokart\\bin\\Debug\\vezeteknevek.txt");
            string[] file2 = File.ReadAllLines("C:\\25-26_PSZ_ IKT\\PSZ-Gokart\\bin\\Debug\\keresztnevek.txt");
            Random rnd = new Random();
            int minLength = Math.Min(file1.Length, file2.Length);
            Versenyzok[] versenyzok = new Versenyzok[minLength];
            string email = "";

            for (int i = 0; i < minLength; i++)
            {
                string vezeteknev = CleanName(file1[i].Split(',')[0]);
                string keresztnev = CleanName(file2[i].Split(',')[0]);
                versenyzok[i] = new Versenyzok(
                    vezeteknev,
                    keresztnev,
                    new DateTime(rnd.Next(1950, 2024), rnd.Next(1, 13), rnd.Next(1, 28)),
                    false,
                    "",
                    email
                );
            }

            foreach (var v in versenyzok)
            {
                Console.WriteLine($"{v.Vezetéknév} {v.Keresztnév} {v.SzületésiIdő:yyyy.MM.dd.} {v.Felnőtt} {v.ID} {v.Email}");
            }


            Console.Write("Kérem adja meg az azonosítót: ");
            string input= Console.ReadLine();

            Console.ReadKey();
        }

        public static void IdopontFoglalas(string input)
        {
           
        }
    }
}
