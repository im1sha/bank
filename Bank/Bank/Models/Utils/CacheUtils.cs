using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public static class CacheUtils
    {
        //public static void DetachAllEntities(this DbContext dbContext)
        //{         

        //    var changedEntriesCopy = dbContext.ChangeTracker.Entries()
        //        .Where(e => e.State == EntityState.Added ||
        //                    e.State == EntityState.Modified ||
        //                    e.State == EntityState.Deleted)
        //        .ToList();

        //    foreach (var entry in changedEntriesCopy)
        //        entry.State = EntityState.Detached;
        //}
    }
}
