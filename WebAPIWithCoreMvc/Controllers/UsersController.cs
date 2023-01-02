using Entities.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIWithCoreMvc.Controllers
{
    public class UsersController : Controller
    {
        #region Defines
        private readonly HttpClient _httpClient;


        private string url = "https://localhost:7176/api/";
        #endregion Defines

        #region Constructor
        public UsersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        #endregion Constructor

        public async Task<IActionResult> Index()
        {
            var users = await _httpClient.GetFromJsonAsync<List<UserDetailDto>>(url+"Users/GetList");
            return View(users);
        }
    }
}
