namespace ZadanieRekrutacyjne2.Models
{
    public class DzienPracyModel
    {
        public String KodPracownika;

        public DateTime Data;

        public TimeSpan? GodzinaWejscia;

        public TimeSpan? GodzinaWyjscia;
        public DzienPracyModel(string kodPracownika, DateTime data, TimeSpan? godzinaWejscia, TimeSpan? godzinaWyjscia)
        {
            KodPracownika = kodPracownika;
            Data = data;
            GodzinaWejscia = godzinaWejscia;
            GodzinaWyjscia = godzinaWyjscia;
        }
        DzienPracyModel()
        {

        }
    }
}
