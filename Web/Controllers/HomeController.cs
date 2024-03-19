using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Persistence;
using Web.Models;
using Web.SignalR;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IApplicationDbContext _dbContext;
    private readonly IHubContext<SignalRServer> _signalRHub;

    public HomeController(ILogger<HomeController> logger, IApplicationDbContext dbContext, IHubContext<SignalRServer> signalRHub)
    {
        _logger = logger;
        _dbContext = dbContext;
        _signalRHub = signalRHub;
    }

    public async Task<IActionResult> Index()
    {
        await _signalRHub.Clients.All.SendAsync("LoadProducts");
        return View(await _dbContext.Posts.Include(x => x.PostCategory).Include(x => x.AppUser).ToListAsync());
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        var res = await _dbContext.Posts
            .Include(x => x.AppUser)
            .Include(x => x.PostCategory).ToListAsync();
        return Ok(res);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.PostId == id);

        if (post is null)
        {
            return NotFound();
        }
        
        var categories = _dbContext.PostCategories.ToList();

        ViewBag.Categories = categories;

        return View(post);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        var post = await _dbContext.Posts.Include(x => x.PostCategory)
            .Include(x => x.AppUser)
            .FirstOrDefaultAsync(x => x.PostId == id);

        if (post is null)
        {
            return NotFound();
        }

        return View(post);
    }
    
    public async Task<IActionResult> Create()
    {
        var categories = await _dbContext.PostCategories.ToListAsync();

        ViewBag.Categories = categories;

        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Post post)
    {
        if (!ModelState.IsValid)
        {
            return View(post);
        }
        var authorId = HttpContext.Session.GetInt32("Id");
        post.AuthorId = authorId.GetValueOrDefault();
        post.CreatedDate = DateTime.Now;
        post.UpdatedDate = DateTime.Now;

        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync(cancellationToken: default);
        await _signalRHub.Clients.All.SendAsync("LoadProducts");

        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(Post post)
    {
        if (!ModelState.IsValid)
        {
            return View(post);
        }

        var authorId = HttpContext.Session.GetInt32("Id");

        post.AuthorId = authorId.GetValueOrDefault();
        post.CreatedDate = DateTime.Now;
        post.UpdatedDate = DateTime.Now;

        _dbContext.Posts.Update(post);
        await _dbContext.SaveChangesAsync(cancellationToken: default);
        await _signalRHub.Clients.All.SendAsync("LoadProducts");

        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _dbContext.Posts
            .Include(p => p.PostCategory)
            .FirstOrDefaultAsync(m => m.PostId == id);

        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _dbContext.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync(cancellationToken: default);
        await _signalRHub.Clients.All.SendAsync("LoadProducts");
        return RedirectToAction(nameof(Index));
    }
}