using COUNTERAPP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace COUNTERAPP.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var counterValues = GetCounterValues();
            var theme = HttpContext.Session.GetString("Theme") ?? "light";

            ViewBag.CounterValues = counterValues;
            ViewBag.Theme = theme;
            return View();
        }

        [HttpPost]
        public IActionResult Increment()
        {
            var counterValues = GetCounterValues();
            int newValue = counterValues.Count > 0 ? counterValues[^1] + 1 : 1;
            counterValues.Add(newValue);
            UpdateSession(counterValues);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Decrement()
        {
            var counterValues = GetCounterValues();
            int newValue = counterValues.Count > 0 ? counterValues[^1] - 1 : -1;
            counterValues.Add(newValue);
            UpdateSession(counterValues);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ToggleTheme()
        {
            var currentTheme = HttpContext.Session.GetString("Theme") ?? "light";
            var newTheme = currentTheme == "light" ? "dark" : "light";
            HttpContext.Session.SetString("Theme", newTheme);
            return RedirectToAction("Index");
        }

        public IActionResult Search()
        {
            var counterValues = GetCounterValues();
            return PartialView("_SearchPartial", counterValues);
        }

        [HttpPost]
        public IActionResult FilterSearch(string searchTerm)
        {
            var counterValues = GetCounterValues();
            var filteredValues = counterValues.Where(v => v.ToString().Contains(searchTerm)).ToList();
            return PartialView("_SearchResultsPartial", filteredValues);
        }

        private List<int> GetCounterValues()
        {
            var sessionData = HttpContext.Session.GetString("CounterValues");
            return sessionData != null ? JsonConvert.DeserializeObject<List<int>>(sessionData) : new List<int>();
        }

        private void UpdateSession(List<int> counterValues)
        {
            HttpContext.Session.SetString("CounterValues", JsonConvert.SerializeObject(counterValues));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
