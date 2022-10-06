using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers;

public class DishesController : Controller
{
    private MyContext _context;

    public DishesController(MyContext context)
    {
        _context = context;
    }
    
    [HttpGet("/dashboard")]
    public IActionResult Dashboard(){
        // Get all Dishes
        List<Dish> AllDishes = _context.Dishes.ToList();
        return View("Dashboard", AllDishes);
    }

    [HttpGet("/dishes/new")]
    public IActionResult New(){
        return View();
    }

    [HttpPost("/dishes/new")]
    public IActionResult CreateDish(Dish newDish)
    {
        if(ModelState.IsValid){
            // We can take the Dish object created from a form submission
            // And pass this object to the .Add() method
            _context.Add(newDish);
            // OR _context.Dishes.Add(newDish);
            _context.SaveChanges();
            // Other code
            return View("Dashboard");
        }
        //return RedirectToAction("New");
        return View("New");
    }

    [HttpGet("/dishes/{DishId}")]
    public IActionResult One(int DishId)
    {
        
        Dish? oneDish = _context.Dishes.FirstOrDefault(dish => dish.DishId == DishId);
        // Other code
        if(oneDish == null){
            return View("Dashboard");
        }
        return View("One", oneDish);
    }

    [HttpGet("/dishes/{DishId}/edit")]
    public IActionResult Update(int DishId){
        Dish? editDish = _context.Dishes.FirstOrDefault(dish => dish.DishId == DishId);
        // Other code
        if(editDish == null){
            return View("Dashboard");
        }
        return View("Update", editDish);
    }

    [HttpPost("/dishes/{DishId}/edit")]
    public IActionResult UpdateDish(int DishId, Dish editedDish)
    {
        if(ModelState.IsValid){
            // We must first Query for a single Dish from our Context object to track changes.
            Dish RetrievedDish = _context.Dishes
                .FirstOrDefault(Dish => Dish.DishId == DishId);

            // Then we may modify properties of this tracked model object
            RetrievedDish.ChefName = editedDish.ChefName;
            RetrievedDish.DishName = editedDish.DishName;
            RetrievedDish.Calories = editedDish.Calories;
            RetrievedDish.Tastiness = editedDish.Tastiness;
            RetrievedDish.Description = editedDish.Description;
            RetrievedDish.UpdatedAt = DateTime.Now;
            
            // Finally, .SaveChanges() will update the DB with these new values
            _context.SaveChanges();
            
            // Other code
            return RedirectToAction("Dashboard");
        }
        return View("Update", DishId);
    }

    [HttpGet("delete/{DishId}")]
    public IActionResult DeleteDish(int DishId)
    {
        // Like Update, we will need to query for a single Dish from our Context object
        Dish RetrievedDish = _context.Dishes
            .SingleOrDefault(Dish => Dish.DishId == DishId);
        
        // Then pass the object we queried for to .Remove() on Dishs
        _context.Dishes.Remove(RetrievedDish);
        
        // Finally, .SaveChanges() will remove the corresponding row representing this Dish from DB 
        _context.SaveChanges();
        // Other code
        return RedirectToAction("Dashboard");
    }
}