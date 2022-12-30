using ZadanieRekrutacyjne2.Models;

namespace ZadanieRekrutacyjne2.Services
{
    public interface IDzienPracyService
    {
        public IEnumerable<DzienPracyModel> GetAll();
    }
}
