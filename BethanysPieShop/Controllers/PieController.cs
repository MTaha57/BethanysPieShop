using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
        }

        //public ViewResult List()
        //{
        //    var pieListViewModel = new PiesListViewModel
        //    {
        //        Pies = _pieRepository.AllPies,
        //        CurrentCategory = "Cheese Cake"
        //    };

        //    return View(pieListViewModel);
        //}

        public IActionResult Details(int id)
        {
            var pie = _pieRepository.GetPieById(id);
            if (pie == null)
                return NotFound();

            return View(pie);
        }

        public ViewResult List(string category)
        {
            
            IEnumerable<Pie> pies;
            string CurrentCategory;
            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.AllPies.OrderBy(p => p.PieId);
                CurrentCategory = "All Pies";
            }
            else
            {
                pies = _pieRepository.AllPies
                                .Where(p => p.Category.CategoryName == category)
                                .OrderBy(p => p.PieId);

                var categoryObj = _categoryRepository.AllCategories.FirstOrDefault(c => c.CategoryName == category);
                CurrentCategory = categoryObj.CategoryName;
            }

            var piesListViewModel = new PiesListViewModel
            {
                Pies = pies,
                CurrentCategory = category
            };

            return View(piesListViewModel);
        }


       
    }
}
