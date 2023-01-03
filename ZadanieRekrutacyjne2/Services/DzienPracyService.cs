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
            foreach (String[] line in lista)
            {
                if(line.Length == 5)
                {
                    //sprawdzanie poprawności wpisu
                    if ((line[0] != "") && (line[1] != "") && (line[2] != "") && (line[3] != "") && (line[4] != ""))
                    {
                        rezult = DodajWpis(rezult, line[0], DateTime.Parse(line[1]), TimeSpan.Parse(line[2]), TimeSpan.Parse(line[3]), line[4]);
                    }
                }
                if (line.Length == 4)
                {
                    //sprawdzanie poprawności wpisu
                    if ((line[0] != "") && (line[1] != "") && (line[2] != "") && (line[3] != ""))
                    {
                        rezult = DodajWpis(rezult, line[0], DateTime.Parse(line[1]), TimeSpan.Parse(line[2]), line[3]);
                    }
                }
            }
            return rezult;
        }
        //analiza lini pliku CSV z jednym wpisem czasu pracy
        private List<DzienPracyModel> DodajWpis(List<DzienPracyModel>  rezult, string idPracownika, DateTime data, TimeSpan godzinaWejscia, TimeSpan godzinaWyjscia, string czasPracy)
        {
            rezult.Add(new DzienPracyModel(idPracownika, data, godzinaWejscia, godzinaWyjscia));
            return rezult;
        }
        //analiza lini pliku CSV z dwoma wpisami czasu pracy
        private List<DzienPracyModel> DodajWpis(List<DzienPracyModel> rezult, string idPracownika, DateTime data, TimeSpan godzina, string status)
        {
            //dla pierwszego wpisu do listy
            if (rezult.Count == 0)
            {
                //dla danych wejściowych
                if (status == "WE")
                {
                    //dodaj nowy wpis
                    rezult.Add(new DzienPracyModel(idPracownika, data, godzina, null));
                }
                //dla danych wyjściowych
                if (status == "WY")
                {
                    //dodaj nowy wpis
                    rezult.Add(new DzienPracyModel(idPracownika, data, null, godzina));
                }
            }
            else
                for (int i = rezult.Count - 1; i >= 0; i--)
                {
                    var dzienPracy = rezult[i];
                    //dla danych wejściowych
                    if (status == "WE")
                    {
                        //dodaj nowy wpis
                        rezult.Add(new DzienPracyModel(idPracownika, data, godzina, null));
                        break;
                    }
                    //dla danych wyjściowych
                    if (status == "WY")
                    {
                        //czy wpis istnieje
                        if ((dzienPracy.KodPracownika == idPracownika) && (dzienPracy.Data == data))
                        {
                            //analisa wpisów niepełnych
                            if (dzienPracy.GodzinaWyjscia == null)
                            {
                                dzienPracy.GodzinaWyjscia = godzina;
                                break;
                            }
                        }
                        else
                        {
                            //czy poprzedni dzień ma miepełny wpis wyjściowy
                            if ((dzienPracy.KodPracownika == idPracownika) && (dzienPracy.Data.AddDays(1) == data))
                            {
                                if (dzienPracy.GodzinaWyjscia == null)
                                {
                                    //kończenie nocnej zmiany
                                    dzienPracy.GodzinaWyjscia = godzina;
                                    break;
                                }
                            }
                        }
                    }
                }
            return rezult;
        }
    }
}
