using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snaptwit.Shared;
    
public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(240)]
    public string Content { get; set; } = string.Empty;

    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    [ForeignKey("Post")]
    public int PostId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [ForeignKey("Reply")]
    public int? CommentId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Post Post { get; set; } = null!;
    public virtual Comment Reply { get; set; } = null!;
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
}