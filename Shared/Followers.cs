using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snaptwit.Shared;

public class Followers
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int FollowingUserId { get; set; }

    [Required]
    public int FollowedUserId { get; set; }

    public virtual User FollowingUser { get; set; } = null!;
    public virtual User FollowedUser { get; set; } = null!;
}