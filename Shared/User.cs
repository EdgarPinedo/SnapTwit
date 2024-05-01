using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snaptwit.Shared;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(64)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public DateTime JoinedDate { get; set; }

    [StringLength(100)]
    public string? ProfilePicturePath { get; set; }

    [StringLength(100)]
    public string? CoverPicturePath { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Followers> Followers { get; set; } = new List<Followers>();
    public virtual ICollection<Followers> Following { get; set; } = new List<Followers>();
}