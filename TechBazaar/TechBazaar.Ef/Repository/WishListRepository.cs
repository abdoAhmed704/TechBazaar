using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Interfaces;
using TechBazaar.Core.Models;

namespace TechBazaar.Ef.Repository
{
    public class WishListRepository<T> : BaseRepository<T>, IWishListRepository<T> where T : WishList
    {
        private readonly EContext eContext;
        public WishListRepository(EContext eContext) : base(eContext)
        {
            this.eContext = eContext;
        }
    }
}
