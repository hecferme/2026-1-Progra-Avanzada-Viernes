using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyMvcApp.Controllers;

public class ThemesController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiBaseUrl = "http://localhost:5017/api"; // Adjust the port as needed

    public ThemesController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index(string search = "")
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{_apiBaseUrl}/Themes");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var themes = JsonSerializer.Deserialize<List<Theme>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Theme>();

            if (!string.IsNullOrEmpty(search))
            {
                themes = themes.Where(t => t.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewBag.Search = search;
            return View(themes);
        }
        return View(new List<Theme>());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Theme theme)
    {
        var client = _httpClientFactory.CreateClient();
        var json = JsonSerializer.Serialize(theme);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_apiBaseUrl}/Themes", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return View("Error");
    }

    [HttpPost]
    public async Task<IActionResult> Update(Theme theme)
    {
        var client = _httpClientFactory.CreateClient();
        var json = JsonSerializer.Serialize(theme);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"{_apiBaseUrl}/Themes/{theme.Id}", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return View("Error");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.DeleteAsync($"{_apiBaseUrl}/Themes/{id}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return View("Error");
    }
}