using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_1.Models;
using Project_1.Services.Base;

namespace Project_1.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_memberService.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            SelectListForPlans();

            return View(new Member());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Member member)
        {
            if (ModelState.IsValid)
            {
                _memberService.Create(member);

                return RedirectToAction(nameof(Index));
            }

            SelectListForPlans(member.PlanId);

            return View(member);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var member = _memberService.GetById(id);

            if (member == null)
            {
                return NotFound();
            }

            SelectListForPlans(member.PlanId);

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                if (!_memberService.Update(member))
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            SelectListForPlans(member.PlanId);

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (!_memberService.Delete(id))
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        private void SelectListForPlans(int? planId = null)
        {
            ViewBag.Plans = new SelectList(
                _memberService.GetAllPlans(),
                "Id",
                "Name",
                planId);
        }
    }
}