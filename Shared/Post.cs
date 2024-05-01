using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snaptwit.Shared;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [StringLength(240)]
    public string Content { get; set; } = string.Empty;

    public int? FileType { get; set; }

    [StringLength(100)]
    public string? FilePath { get; set; }

    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}