using System;
using System.Collections.Generic;

namespace TasteGuider.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public byte[]? RestaurantImage { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
