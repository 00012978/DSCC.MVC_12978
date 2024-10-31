using DSCC.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DSCC.MVC.Controllers;
public class PetController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl = "http://dscc-api-12978.eu-central-1.elasticbeanstalk.com/api/Pet"; // API link

    public PetController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // GET: PetController
    public async Task<ActionResult> Index()
    {
        var response = await _httpClient.GetAsync(_apiBaseUrl);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var pets = JsonConvert.DeserializeObject<List<Pet>>(jsonResponse);
            return View(pets);
        }
        return View("Error");
    }

    // GET: PetController/Details/5
    public async Task<ActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var pet = JsonConvert.DeserializeObject<Pet>(jsonResponse);
            return View(pet);
        }
        return View("Error");
    }

    // GET: PetController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: PetController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Pet pet)
    {
        try
        {
            var petJson = new StringContent(JsonConvert.SerializeObject(pet), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiBaseUrl, petJson);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Error");
        }
        catch
        {
            return View();
        }
    }

    // GET: PetController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var pet = JsonConvert.DeserializeObject<PetViewModel>(jsonResponse);
                return View(pet);
            }
            else
            {
                ModelState.AddModelError("", $"Error retrieving pet with ID {id}: {response.ReasonPhrase}");
                return View("Error");
            }
        }
        catch (Exception ex)
        {
            // Catch-all for any other exceptions
            ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
            return View("Error");
        }
    }

    // POST: PetController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, PetViewModel pet)
    {
        if (!ModelState.IsValid)
        {
            return View(pet); // Return the view with validation errors
        }

        try
        {
            var petJson = new StringContent(JsonConvert.SerializeObject(pet), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{id}", petJson);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Log the error response
                ModelState.AddModelError("", $"Error updating pet: {response.ReasonPhrase}");
                return View(pet);
            }
        }
        catch (Exception ex)
        {
            // Catch-all for any other exceptions
            ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
            return View(pet);
        }
    }


    // GET: PetController/Delete/5
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var pet = JsonConvert.DeserializeObject<Pet>(jsonResponse);
            return View(pet);
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
