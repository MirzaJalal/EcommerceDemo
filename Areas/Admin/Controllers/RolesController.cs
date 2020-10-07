using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chondok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var role = _roleManager.Roles.ToList();
            ViewBag.Roles = role;
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name) //this name variable must be same as "name=name" in the create view
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(role.Name);

            if(isExist == true)
            {
                ViewBag.message = "This role is already exist";
                ViewBag.name = name;
                return View();
            }

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                TempData["save"] = "New Role created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if(role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name) //this name variable must be same as "name=name" in the create view
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;

            var isExist = await _roleManager.RoleExistsAsync(role.Name);

            if (isExist == true)
            {
                ViewBag.message = "This role is already exist";
                ViewBag.name = name;
                return View();
            }

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                TempData["save"] = "Role has been Updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
