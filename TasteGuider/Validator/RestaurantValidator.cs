using TasteGuider.Models;
namespace TasteGuider.Validator
{
    public static class RestaurantValidator
    {
        public static string ReturnErrorsCreate(List<Restaurant> restaurants,Restaurant restaurant)
        {
            string msg = "";
            if (String.IsNullOrWhiteSpace(restaurant.Name) || String.IsNullOrWhiteSpace(restaurant.Description))
            {
                msg += "Попълнете всички полета!\n";
            }
   
            if (restaurants.FirstOrDefault(p => p.Name == restaurant.Name) != null)
            {
                msg += "Това име вече присъства в базата с ресторанти!";
            }
            return msg;
        }
        public static string ReturnErrorsEdit(List<Restaurant> restaurants, Restaurant restaurant, int id)
        {
            string msg = "";
            if (String.IsNullOrWhiteSpace(restaurant.Name) || String.IsNullOrWhiteSpace(restaurant.Description))
            {
                msg += "Попълнете всички полета!\n";
            }

            if (restaurants.FirstOrDefault(p => p.Name == restaurant.Name) != null)
            {
                msg += "Това име вече присъства в базата с ресторанти!";
            }
            if (restaurants.FirstOrDefault(p => p.Name == restaurant.Name && p.Id != id) != null)
            {
                msg += "Това име вече присъства в базата със служители!\n";
            }
            return msg;
        }
    }
}
