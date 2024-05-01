using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snaptwit.Shared;

public class Like
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Post")]
    public int? PostId { get; set; }

    [ForeignKey("Comment")]
    public int? CommentId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Post Post { get; set; } = null!;
    public virtual Comment Comment { get; set; } = null!;
}