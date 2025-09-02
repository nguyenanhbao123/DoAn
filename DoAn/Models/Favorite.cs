using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models
{
    public partial class Favorite
    {
        public string FavoriteId { get; set; } = null!;

        [ForeignKey("UserId")]
        public string UserId { get; set; } = null!;

        public string? ProductId { get; set; }

        public virtual Product? Product { get; set; }

        public virtual IdentityUser? User { get; set; }
    }
}
