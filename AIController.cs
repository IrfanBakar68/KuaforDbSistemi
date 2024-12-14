using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[Route("AI")]
public class AIController : Controller
{
    private readonly IConfiguration _configuration;

    public AIController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.ErrorMessage = null;
        ViewBag.ResultImageUrl = null;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GenerateHairStyle(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ViewBag.ErrorMessage = "Lütfen bir fotoğraf yükleyin.";
            ViewBag.ResultImageUrl = null;
            return View("Index");
        }

        var apiKey = _configuration["AISettings:ApiKey"];
        var endpoint = _configuration["AISettings:Endpoint"];

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(file.OpenReadStream())
        {
            Headers = { ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType) } // Dinamik Content-Type
        };
        content.Add(fileContent, "file", file.FileName);

        try
        {
            var response = await client.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = $"API Yanıt Hatası: {response.StatusCode} ({response.ReasonPhrase})";
                ViewBag.ResultImageUrl = null;
                return View("Index");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var resultImageUrl = ExtractImageUrlFromResponse(responseContent);

            if (string.IsNullOrEmpty(resultImageUrl))
            {
                ViewBag.ErrorMessage = "Geçerli bir resim URL'si API'den alınamadı.";
                ViewBag.ResultImageUrl = null;
                return View("Index");
            }

            ViewBag.ErrorMessage = null;
            ViewBag.ResultImageUrl = resultImageUrl;
            return View("Result", new { ImageUrl = resultImageUrl });
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
            ViewBag.ResultImageUrl = null;
            return View("Index");
        }
    }

    [HttpPost("SubmitFeedback")]
    public IActionResult SubmitFeedback(string imageUrl, bool feedback)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            return BadRequest(new { Message = "Geçerli bir görsel URL'si gereklidir." });
        }

        Console.WriteLine($"Feedback Received: ImageUrl={imageUrl}, Feedback={feedback}");
        return Ok(new { Message = "Geri bildirim başarıyla kaydedildi." });
    }

    [HttpPost("GetMoreRecommendations")]
    public IActionResult GetMoreRecommendations(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { Message = "Lütfen bir dosya yükleyin." });
        }

        var recommendations = new List<string>
        {
            "https://dummyimage.com/600x400/000/fff&text=Model1",
            "https://dummyimage.com/600x400/000/fff&text=Model2",
            "https://dummyimage.com/600x400/000/fff&text=Model3"
        };

        return Ok(recommendations);
    }

    private string ExtractImageUrlFromResponse(string responseContent)
    {
        try
        {
            using var jsonDoc = JsonDocument.Parse(responseContent);
            if (jsonDoc.RootElement.TryGetProperty("image_url", out var imageUrlElement))
            {
                return imageUrlElement.GetString() ?? string.Empty;
            }
            return string.Empty;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON işleme hatası: {ex.Message}");
            return string.Empty;
        }
    }
}
