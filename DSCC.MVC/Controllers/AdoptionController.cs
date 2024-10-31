using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSCC.MVC.Controllers;

using DSCC.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class AdoptionController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl = "http://dscc-api-12978.eu-central-1.elasticbeanstalk.com/api/Adoption";

    public AdoptionController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // GET: AdoptionController
    public async Task<ActionResult> Index()
    {
        var response = await _httpClient.GetAsync(_apiBaseUrl);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var adoptions = JsonConvert.DeserializeObject<List<Adoption>>(jsonResponse);
            return View(adoptions);
        }
        return View("Error");
    }

    // GET: AdoptionController/Details/5
    public async Task<ActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var adoption = JsonConvert.DeserializeObject<Adoption>(jsonResponse);
            return View(adoption);
        }
        return View("Error");
    }

    // GET: AdoptionController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: AdoptionController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(AdoptionViewModel adoption)
    {
        try
        {
            var jsonContent = JsonConvert.SerializeObject(adoption);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiBaseUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        catch
        {
            return View("Error");
        }
        return View(adoption);
    }

    // GET: AdoptionController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var adoption = JsonConvert.DeserializeObject<AdoptionViewModel>(jsonResponse);
            return View(adoption);
        }
        return View("Error");
    }

    // POST: AdoptionController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, AdoptionViewModel adoption)
    {
        try
        {
            var jsonContent = JsonConvert.SerializeObject(adoption);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        catch
        {
            return View("Error");
        }
        return View(adoption);
    }

    // GET: AdoptionController/Delete/5
    public async Task<ActionResult> Delete(int id)
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var adoption = JsonConvert.DeserializeObject<Adoption>(jsonResponse);
            return View(adoption);
        }
        return View("Error");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> OnDelete(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View("Error");
            }
        }
        catch (Exception ex)
        {
            return View("Error");
        }
    }
}