using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWeb.Services;

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
    }
}
