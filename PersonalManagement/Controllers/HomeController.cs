using System.Web.Mvc;
using PersonalManagement.ViewModels;
using PersonalManagement.Model;
using PersonalManagement.DAL;

namespace PersonalManagement.Controllers
{
    public class HomeController : Controller
    {
        private IPersonRepository _repository;

        public HomeController()
        {
            _repository = new PersonRepository();
        }
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            model.PersonCollection = _repository.GetAllPerson();

            return View(model);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Person person)
        {
            _repository.AddPerson(person);

            return Redirect("/Home/Index");
        }
        public ActionResult Delete(string id)
        {
            _repository.DeletePerson(id);

            return Redirect("/Home/Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}