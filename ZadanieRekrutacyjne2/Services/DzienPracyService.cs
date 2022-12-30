using ZadanieRekrutacyjne2.Models;

namespace ZadanieRekrutacyjne2.Services
{
    public class DzienPracyService : IDzienPracyService
    {
        public IEnumerable<DzienPracyModel> GetAll()
        {
            List < DzienPracyModel > lista = new List<DzienPracyModel>();
            var pliki = OdczytListyPlików();
            foreach (String p in pliki)
            {
                lista.AddRange(Konwersja(OdczytPliku(p)));
            }
            return lista;
        }
        private string[] OdczytListyPlików()
        {
            //ścieżka dostępu do folderu z przetwarzanymi plikami
            String sciezka = Path.Combine(Directory.GetCurrentDirectory(), "Dane");
            string[] pliki = Directory.GetFiles(sciezka);
            return pliki;
        }
        private List<String[]> OdczytPliku(String plik)
        {
            List<String[]> result = new List<String[]>();
            StreamReader plikReader = new StreamReader(plik);
            if (plikReader != null)
            {
                int line_number = 1;//Jeżleli w pierwszym wierszu są nazwy kolumn to ten wiersz musimy opuścić
                while (!plikReader.EndOfStream)
                {
                    var line = plikReader.ReadLine();
                    if (line_number > 0)
                    {
                        var values = line.Split(';');
                        result.Add(values);
                    }
                    line_number++;
                }
                plikReader.Close();
            }
            return result;
        }
        private List<DzienPracyModel> Konwersja(List<String[]> lista)
        {
            List<DzienPracyModel> rezult = new List<DzienPracyModel>();
            foreach (String[] p in lista)
            {
                if(p.Length == 5)
                {
                    //sprrawdzanie poprawności wpisu
                    if ((p[0] != "") && (p[1] != "") && (p[2] != "") && (p[3] != "") && (p[4] != ""))
                    {
                        var a = p[0];
                        var b = DateTime.Parse(p[1]);
                        var c = TimeSpan.Parse(p[2]);
                        var d = TimeSpan.Parse(p[3]);
                        var e = p[4];
                        rezult.Add(new DzienPracyModel(a, b, c, d));
                    }
                }
                if (p.Length == 4)
                {
                    //sprrawdzanie poprawności wpisu
                    if ((p[0]!="") && (p[1] != "") && (p[2] != "") && (p[3] != ""))
                    {
                        var a = p[0];
                        var b = DateTime.Parse(p[1]);
                        var c = TimeSpan.Parse(p[2]);
                        var d = p[3];
                        //dla pierwszego wpisu do listy
                        if (rezult.Count == 0)
                        {
                            //dla danych wejściowych
                            if (d == "WE")
                            {
                                //dodaj nowy wpis
                                rezult.Add(new DzienPracyModel(a, b, c, null));
                            }
                            //dla danych wyjściowych
                            if (d == "WY")
                            {
                                //dodaj nowy wpis
                                rezult.Add(new DzienPracyModel(a, b, null, c));
                            }
                        }
                        else
                        for(int i = rezult.Count-1; i >= 0; i--)
                        {
                            var dzienPracy = rezult[i];
                            //dla danych wejściowych
                            if (d == "WE")
                            {
                                //czy wpis istnieje
                                if ((dzienPracy.KodPracownika == a) && (dzienPracy.Data == b))
                                {
                                    //dodaj nowy wpis
                                    rezult.Add(new DzienPracyModel(a, b, c, null));
                                    break;
                                }
                                else
                                {
                                    //dodaj nowy wpis
                                    rezult.Add(new DzienPracyModel(a, b, c, null));
                                    break;
                                }
                            }
                            //dla danych wyjściowych
                            if (d == "WY")
                            {
                                //czy wpis istnieje
                                if ((dzienPracy.KodPracownika == a) && (dzienPracy.Data == b))
                                {
                                    //analisa wpisów niepełnych
                                    if (dzienPracy.GodzinaWyjscia == null)
                                    {
                                        dzienPracy.GodzinaWyjscia = c;
                                        break;
                                    }
                                }
                                else
                                {
                                    //czy poprzedni dzień ma miepełny wpis wyjściowy
                                    if ((dzienPracy.KodPracownika == a) && (dzienPracy.Data.AddDays(1) == b))
                                    {
                                        if(dzienPracy.GodzinaWyjscia == null)
                                        {
                                            //kończenie nocnej zmiany
                                            dzienPracy.GodzinaWyjscia = c;
                                            break;
                                        }
                                    }
                                }
                            }
                            //czy wpis istnieje
                            if ((dzienPracy.KodPracownika == a) && (dzienPracy.Data == b))
                            {
                                //analisa wpisów niepełnych
                                if((dzienPracy.GodzinaWejscia == null)||(dzienPracy.GodzinaWyjscia == null))
                                {

                                }
                            }
                        }
                    }
                }
            }
            return rezult;
        }
    }
}
