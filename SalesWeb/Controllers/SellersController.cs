using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SalesWeb.Services;
using SalesWeb.Models;
using SalesWeb.Models.ViewModels;
using SalesWeb.Services.Exceptions;

namespace SalesWeb.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            return View(_sellerService.FindAll());
        }

        public IActionResult Create()
        {
            ICollection<Department> departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost] // Indica que é um método do tipo POST.
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var seller = _sellerService.FindById(id.Value);

            if (seller == null)
                return NotFound();

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var seller = _sellerService.FindById(id.Value);

            if (seller == null)
                return NotFound();

            return View(seller);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var seller = _sellerService.FindById(id.Value);

            if (seller == null)
                return NotFound();

            ICollection<Department> departments = _departmentService.FindAll();
            
            SellerFormViewModel sfm = new SellerFormViewModel { Seller = seller, Departments = departments };
            
            return View(sfm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
                return BadRequest();

            try
            {
                _sellerService.Update(seller);

                return RedirectToAction(nameof(Index));
            }
            catch(NotFoundException)
            {
                return BadRequest();
            }
            catch(DbConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}
