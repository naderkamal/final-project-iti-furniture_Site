using AdminDashboardMVC.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AdminDashboardMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        HttpClient client = new HttpClient();
        string EndPoint = "http://localhost:5277/api/Role";
        public async Task<IActionResult> index()
        {
            List<RoleDTO> roles = await client.GetFromJsonAsync<List<RoleDTO>>(EndPoint);
            return View(roles);
        }

        //Details
        public async Task<IActionResult> Details(int? id)
        {

            RoleDTO role = await client.GetFromJsonAsync<RoleDTO>(EndPoint + "/" + id.ToString());

            return View(role);
        }
        //Create

        //public async Task<ActionResult> Create()
        //{
        //    ViewData["Users_IDs"] = new SelectList(await client.GetFromJsonAsync<List<User>>(EndPoint), "Id", "Name");
        //    return View("Create", new Role());
        //}

        //[HttpPost]
        //public async Task<ActionResult> Create(Role role)
        //{
        //    HttpResponseMessage response = await client.PostAsJsonAsync<Role>(EndPoint, role);
        //    ViewBag.location = response.Headers.Location;
        //    if (response.StatusCode == System.Net.HttpStatusCode.Created)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        ViewData["Users_IDs"] = new SelectList(await client.GetFromJsonAsync<List<User>>(EndPoint), "Id", "Name");
        //        return View("Create", new Role());
        //    }
        //}


        //////////////////////update
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    Role role = await client.GetFromJsonAsync<Role>(EndPoint + "/" + id.ToString());

        //    ViewData["Users_IDs"] = new SelectList(await client.GetFromJsonAsync<List<User>>(EndPoint), "Id", "Name");
        //    return View("Edit", role);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Edit(Role role)
        //{
        //    HttpResponseMessage response = await client.PutAsJsonAsync<Role>(EndPoint + "/" + role.Id, role);
        //    ViewBag.location = response.Headers.Location;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        ViewData["Users_IDs"] = new SelectList(await client.GetFromJsonAsync<List<User>>(EndPoint), "Id", "Name");
        //        return View("Edit", role);
        //    }
        //}

    }
}
