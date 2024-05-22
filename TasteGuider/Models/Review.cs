using System;
using System.Collections.Generic;

namespace TasteGuider.Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? RestaurantId { get; set; }
        public string? ReviewText { get; set; }

        public virtual Restaurant? Restaurant { get; set; }
        public virtual User? User { get; set; }
    }
}
