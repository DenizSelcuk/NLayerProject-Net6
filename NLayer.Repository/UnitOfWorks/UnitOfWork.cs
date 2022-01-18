using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLayer.Core.UnitOfWorks;

namespace NLayer.Repository.UnitOfWorks
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly AppDbContext _context;

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}