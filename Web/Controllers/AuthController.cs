using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Persistence;
using Web.Models;

namespace Web.Controllers;

public class AuthController : Controller
{
    private readonly IApplicationDbContext _dbContext;

    public AuthController(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid Email or password");
            return View(model);
        }

        var user = await _dbContext.AppUsers.Where(x => x.Email == model.Email 
                                                  && x.Password == model.Password)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid Email or password");
            return View(model);
        }
        
        HttpContext.Session.SetInt32("Id", user.UserId);

        return RedirectToAction("Index", "Home");
    }
}