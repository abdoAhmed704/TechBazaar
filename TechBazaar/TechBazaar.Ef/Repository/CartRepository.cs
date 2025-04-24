using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Interfaces;
using TechBazaar.Core.Models;

namespace TechBazaar.Ef.Repository
{
    public class CartRepository<T> : BaseRepository<T>, ICartRepository<T> where T : Cart
    {
        private readonly EContext eContext;

        public CartRepository(EContext eContext):base(eContext)
        {
            this.eContext = eContext;
        }
    }
}
