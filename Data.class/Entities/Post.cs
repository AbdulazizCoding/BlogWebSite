namespace Data.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public EPostStatus Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public List<string>? ImageUrl { get; set; }
    public virtual Channel? Channel { get; set; }
}