﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.Interfaces
{
    public interface IWishListRepository<T> : IBaseRepository<T> where T : WishList
    {
        Task<IEnumerable<int>> GetWishListProductIds();
        Task<int> GetTotalItemInWishList();
    }
}
