using Microsoft.AspNetCore.Mvc.Rendering;

namespace NationalParkWeb.Models.ViewModel
{
    public class TrailVM
    {
        public IEnumerable<SelectListItem> NationalParkList { get; set; }  //Dropdown button contain list of NationalPark
        public Trail Trail { get; set; }//Trail obj
    }
}
