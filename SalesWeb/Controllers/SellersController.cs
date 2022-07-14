using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWeb.Services;
using SalesWeb.Models;

namespace SalesWeb.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerservice;

        public SellersController(SellerService sellerService)
        {
            _sellerservice = sellerService;
        }

        public IActionResult Index()
        {
            return View(_sellerservice.FindAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] // Indica que é um método do tipo POST.
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            _sellerservice.Insert(seller);
            return RedirectToAction(nameof(Index));
        }
    }
}
