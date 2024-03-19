namespace Repository.Entities;

public class PostCategory
{
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    
    // Navigation Properties
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}