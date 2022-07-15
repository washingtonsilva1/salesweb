using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SalesWeb.Services;
using SalesWeb.Models;
using SalesWeb.Models.ViewModels;
using SalesWeb.Services.Exceptions;
using System.Diagnostics;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            return View(await _sellerService.FindAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            ICollection<Department> departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost] // Indica que é um método do tipo POST.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                ICollection<Department> dps = await _departmentService.FindAllAsync();
                SellerFormViewModel sfm = new SellerFormViewModel { Seller = seller, Departments = dps };
                return View(sfm);
            }

            await _sellerService.InsertAsync(seller);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            var seller = await _sellerService.FindByIdAsync(id.Value);

            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Id not found" });

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sellerService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            var seller = await _sellerService.FindByIdAsync(id.Value);

            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Id not found" });

            return View(seller);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            var seller = await _sellerService.FindByIdAsync(id.Value);

            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Id not found" });

            ICollection<Department> departments = await _departmentService.FindAllAsync();

            SellerFormViewModel sfm = new SellerFormViewModel { Seller = seller, Departments = departments };

            return View(sfm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                ICollection<Department> dps = await _departmentService.FindAllAsync();
                SellerFormViewModel sfm = new SellerFormViewModel { Seller = seller, Departments = dps };
                return View(sfm);
            }

            if (id != seller.Id)
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });

            try
            {
               await _sellerService.UpdateAsync(seller);

                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}
