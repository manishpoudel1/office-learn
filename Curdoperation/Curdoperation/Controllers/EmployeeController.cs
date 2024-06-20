using Curdoperation.DAL;
using Curdoperation.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Curdoperation.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly Employee_DAL _dal;

        public EmployeeController(Employee_DAL dal)
        {
            _dal = dal;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                employees = _dal.GetAll();
            }
            catch (Exception ex)
            {
                // Handle exception (optional: log the exception)
                TempData["errorMessage"] = $"Error: {ex.Message}";
            }
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee em)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model Data Is invalid";
                    return View(em); // Return the view with the model to show validation errors
                }

                bool result = _dal.Insert(em);

                if (!result)
                {
                    TempData["errorMessage"] = "Unable to save the data";
                    return View(em); // Return the view with the model
                }
                TempData["successMessage"] = "Data is saved";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle exception (optional: log the exception)
                TempData["errorMessage"] = $"Error: {ex.Message}";
                return View(em);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _dal.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View(model);
                }

                bool result = _dal.Update(model);

                if (!result)
                {
                    TempData["errorMessage"] = "Unable to save the data";
                    return View(model);
                }

                TempData["successMessage"] = "Data is saved";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Error: {ex.Message}";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool result = _dal.Delete(id);

                if (!result)
                {
                    TempData["errorMessage"] = "Unable to delete the data";
                    var employee = _dal.GetById(id); // Ensure the model is repopulated for the view
                    return View("Delete", employee); // Return to the Delete view with the model
                }

                TempData["successMessage"] = "Data is deleted";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Error: {ex.Message}";
                var employee = _dal.GetById(id); // Ensure the model is repopulated for the view
                return View("Delete", employee); // Return to the Delete view with the model
            }
        }

    }
}

    
