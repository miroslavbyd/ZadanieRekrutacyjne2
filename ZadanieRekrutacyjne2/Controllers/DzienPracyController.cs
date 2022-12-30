using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZadanieRekrutacyjne2.Services;

namespace ZadanieRekrutacyjne2.Controllers
{
    public class DzienPracyController : Controller
    {
        private readonly IDzienPracyService _dzienPracyService;
        public DzienPracyController(IDzienPracyService dzienPracyService)
        {
            _dzienPracyService = dzienPracyService;
        }
        // GET: DzienPracyController
        public ActionResult Index()
        {
            var data = _dzienPracyService.GetAll();
            return View(data);
        }

        // GET: DzienPracyController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DzienPracyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DzienPracyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DzienPracyController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DzienPracyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DzienPracyController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DzienPracyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
