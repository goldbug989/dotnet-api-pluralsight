using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Ode.Core;
using Ode.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        //OUTPUT MODELS TO BIND TO
        private readonly IConfiguration config;
        private readonly IRestaurantData restaurantData;

        public string message { get; set; }
        public IEnumerable<Restaurant> Restaurants { get; set; }

        //PROPERTIES CAN BE USED AS INPUT MODEL AND OUTPUT MODEL
        //USING BIND PROPERTY by default ONLY POST SO ADD GET
        [BindProperty(SupportsGet =true)]
        public string SearchTerm { get; set; }

        //SERVICE INJECTION TO GET DATA 
        public ListModel(IConfiguration config,IRestaurantData restaurantData)
        {
            this.config = config;
            this.restaurantData = restaurantData;
        }


        //MODEL BINDING FINDS SEARCHTERM INSIDE OF MARKUP BY NAME="SEARCHTERM"
        public void OnGet()
        {
            //ACCESS DATA FROM APPSETTING.JSON TO DISPLAY ON RAZOR PAGE
            message = config["Message"];
            //ANOTHER EXAMPLE ACCESS CONNECTION STRING--NOTE CONFIG IN STARTUP.CS
            //message = config.GetConnectionString("OdeToFoodDb");
            Restaurants = restaurantData.GetRestaurantsByName(SearchTerm);
        }
    }
}
