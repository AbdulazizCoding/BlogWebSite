namespace Data.Entities;
public class Channel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public EChannelStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? ImageUrl { get; set; }
    public virtual List<Post>? posts { get; set; }
}
