using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ode.Core;
using Ode.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        private readonly IRestaurantData restaurantData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public Restaurant Restaurant { get; set; }

        public IEnumerable<SelectListItem> Cuisines { get; set; }

        //CONSTRUCTOR INJECTS DATA SERVICE
        public EditModel(IRestaurantData restaurantData,
                            IHtmlHelper htmlHelper)
        {
            this.restaurantData = restaurantData;
            this.htmlHelper = htmlHelper;
        }
        //ON GET METHOD ACCEPTS ID FROM URL --SEE CSHTML @PAGE
        public IActionResult OnGet(int? restaurantId)
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            if (restaurantId.HasValue)
            {
                Restaurant = restaurantData.GetById(restaurantId.Value);
            }
            else
            {
                Restaurant = new Restaurant();
            }
            if(Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
        //ON POST METHOD FOR UPDATING DATA
        public IActionResult OnPost()
        {
            //WILL PERFORM VALIDATION CHECKS BASED ON RESTAURANT CLASS
            if (!ModelState.IsValid)
            {
                Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
                return Page();

            }//IS THIS A NEW RESTAURANT OR UPDATE?
            if(Restaurant.Id > 0)
            {
                restaurantData.Update(Restaurant);
            }
            else
            {
                restaurantData.Add(Restaurant);
            }
            //COMMIT CHANGES
            restaurantData.Commit();
            //TEMP DATA WORKS FOR NEXT PAGE REQUEST ONLY..THEN IT GONE
            TempData["Message"] = "Restaurant saved!";
            return RedirectToPage("./Detail", new {restaurantId = Restaurant.Id });


        }
    }
}
