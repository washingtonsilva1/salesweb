using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWeb.Data;
using SalesWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace SalesWeb.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebContext _context;

        public SalesRecordService(SalesWebContext context) {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = _context.SalesRecord.Select(s => s);

            if (minDate.HasValue)
                result = result.Where(s => s.Date >= minDate.Value);

            if (maxDate.HasValue)
                result = result.Where(s => s.Date <= maxDate.Value);

            return await result
                .Include(s => s.Seller)
                .Include(s => s.Seller.Department)
                .OrderByDescending(s => s.Date)
                .ToListAsync();
        }
    }
}
