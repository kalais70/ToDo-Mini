using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext Context;

        public ToDoController(ToDoContext context)
        {
            this.Context = context;
        }

        // GET /
        public async Task<IActionResult> Index()
        {
            IQueryable<TodoList> items = from i in Context.ToDoList orderby i.Id select i;

            List<TodoList> todoList = await items.ToListAsync();

            return View(todoList);
        }
        //GET / todo / create
        public IActionResult Create() => View();

        //POST /todo /create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoList item)
        {
           if (ModelState.IsValid)
            { 
                Context.Add(item);
                await Context.SaveChangesAsync();

                TempData["Sucess"] = "The item has been added";

                return RedirectToAction("Index");
            }

            return View(item);
        }
        // GET / todo / edit
        public async Task<IActionResult> Edit(int id)
        {
            TodoList item = await Context.ToDoList.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        //POST /todo /edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                Context.Update(item);
                await Context.SaveChangesAsync();

                TempData["Sucess"] = "The item has been updated!";

                return RedirectToAction("Index");
            }

            return View(item);
        }
        // GET / todo / delete
        public async Task<IActionResult> Delete(int id)
        {
            TodoList item = await Context.ToDoList.FindAsync(id);
            if (item == null)
            {
                TempData["Error"] = "The item does not exist!";
            }
            else
            {
                Context.ToDoList.Remove(item);
                await Context.SaveChangesAsync();

                TempData["Sucess"] = "The item has been deleted!";
            }

            return RedirectToAction("index");
        }
    }
}
