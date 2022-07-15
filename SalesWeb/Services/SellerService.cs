using System.Collections.Generic;
using System.Linq;
using SalesWeb.Data;
using SalesWeb.Models;
using Microsoft.EntityFrameworkCore;
using SalesWeb.Services.Exceptions;
using System.Threading.Tasks;

namespace SalesWeb.Services
{
    public class SellerService
    {
        private readonly SalesWebContext _context;

        public SellerService(SalesWebContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(s => s.Department).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var seller = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(seller);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller seller)
        {
            var hasAny = await _context.Seller.AnyAsync(s => s.Id == seller.Id);

            if (!hasAny)
                throw new NotFoundException("Id not found!");
            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException er)
            {
                throw new DbConcurrencyException(er.Message);
            }
        }
    }
}
