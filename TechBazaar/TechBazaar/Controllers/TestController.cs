using Microsoft.AspNetCore.Mvc;
using TechBazaar.Models;
using TechBazaar.Repository.Base;

namespace TechBazaar.Controllers
{
    public class TestController : Controller
    {
        private readonly IUnitOfWork unitContext;

        public TestController(IUnitOfWork unitContext)
        {
            this.unitContext = unitContext;
        }
        public IActionResult Index()
        {

            return Ok(unitContext.Category.FindAll());
        }
        public IActionResult Addone()
        {
            var category = new Category();
            category.Name = "new two";
            category.Desc = " new";
            unitContext.Category.Add(category);
            unitContext.Category.SaveChanges();
            return Content("category have been added");
        }
        public IActionResult Selectone()
        {

            return Ok(unitContext.Category.SelectOne(c => c.Name == "new two"));
        }
    }
}
