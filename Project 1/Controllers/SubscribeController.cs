using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_1.Domain.Models;
using Project_1.Application.Services.Base;

namespace Project_1.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly ISubscribeService _subscribeService;

        public SubscriptionsController(
            ISubscribeService subscribeService)
        {
            _subscribeService = subscribeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_subscribeService.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateDropdowns();

            return View(new Subscribe());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Subscribe subscription)
        {
            RemoveNavigationValidation();

            if (ModelState.IsValid)
            {
                _subscribeService.Create(subscription);

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(
                subscription.MemberId,
                subscription.PlanId,
                subscription.EmployeeId);

            return View(subscription);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subscription = _subscribeService.GetById(id);

            if (subscription == null)
            {
                return NotFound();
            }

            PopulateDropdowns(
                subscription.MemberId,
                subscription.PlanId,
                subscription.EmployeeId);

            return View(subscription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Subscribe subscription)
        {
            RemoveNavigationValidation();

            if (ModelState.IsValid)
            {
                if (!_subscribeService.Update(subscription))
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(
                subscription.MemberId,
                subscription.PlanId,
                subscription.EmployeeId);

            return View(subscription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (!_subscribeService.Delete(id))
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        private void RemoveNavigationValidation()
        {
            // الفورم يرسل أرقام العلاقات فقط.
            ModelState.Remove(nameof(Subscribe.Member));
            ModelState.Remove(nameof(Subscribe.Plan));
            ModelState.Remove(nameof(Subscribe.Employee));
        }

        private void PopulateDropdowns(
            int? memberId = null,
            int? planId = null,
            int? employeeId = null)
        {
            ViewBag.Members = new SelectList(
                _subscribeService.GetAllMembers(),
                "Id",
                "Name",
                memberId);

            ViewBag.Plans = new SelectList(
                _subscribeService.GetAllPlans(),
                "Id",
                "Name",
                planId);

            ViewBag.Employees = new SelectList(
                _subscribeService.GetAllEmployees(),
                "Id",
                "Name",
                employeeId);
        }
    }
}