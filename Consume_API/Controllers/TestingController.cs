using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Test_API.Models;

namespace Consume_API.Controllers
{
    public class TestingController : Controller
    {
        private string url = "https://localhost:7079/";
        public IActionResult Index()
        {
            List<Customer> data = new List<Customer>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage responseMessage = client.GetAsync("api/Customer/GetAllCustomer").Result;
                    client.Dispose();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        string strData = responseMessage.Content.ReadAsStringAsync().Result;
                        data = JsonConvert.DeserializeObject<List<Customer>>(strData);
                    }
                    else
                    {
                        TempData["errorMessage"] = $"{responseMessage.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View(data);
        }

        public IActionResult AddCustomer()
        {
            var cust = new Customer();
            return View(cust);
        }

        [HttpPost]
        public IActionResult AddCustomer(Customer model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(url);
                        var data = JsonConvert.SerializeObject(model);
                        var contentData = new StringContent(data, Encoding.UTF8, "application/json");
                        if (model.Id == 0)
                        {
                            HttpResponseMessage response = client.PostAsync("api/Customer/AddCustomer", contentData).Result;
                            TempData["message"] = response.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            HttpResponseMessage response = client.PutAsync("api/Customer/UpdateCustomer", contentData).Result;
                            TempData["message"] = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "ModelState is not Valid");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = client.DeleteAsync("api/Customer/DeleteCustomer/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["message"] = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    TempData["message"] = $"{response.ReasonPhrase}";
                }
                return RedirectToAction("Index");
            }
        }

        public IActionResult Edit(int id)
        {
            Customer cust = new Customer();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("api/Customer/GetCustomerById/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string strData = response.Content.ReadAsStringAsync().Result;
                    cust = System.Text.Json.JsonSerializer.Deserialize<Customer>(strData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    TempData["message"] = $"{response.ReasonPhrase}";
                }
            }
            return View("AddCustomer", cust);
        }
    }
}
