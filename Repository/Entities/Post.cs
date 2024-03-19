namespace Repository.Entities;

public class Post
{
    public int PostId { get; set; }    
    public int AuthorId { get; set; }    
    public int CategoryId { get; set; }    
    public DateTime CreatedDate { get; set; }    
    public DateTime UpdatedDate { get; set; }    
    public string? Title { get; set; }    
    public string? Content { get; set; }    
    public bool PublishStatus { get; set; }    
    
    // Navigation Properties
    public virtual PostCategory? PostCategory { get; set; }
    public virtual AppUser? AppUser { get; set; }
}

