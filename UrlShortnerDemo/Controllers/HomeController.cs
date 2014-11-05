using System;
using System.Web.Mvc;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using UrlShortenerDemo.Models;

namespace UrlShortenerDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UrlModel model)
        {
            model.Key = GenerateKey();
            model.Created = DateTime.UtcNow.ToString("O");
            model.User = "testuser";
            model.ShortenedUrl = GenerateShortUrl(model.Key);

            AddToDynamo(model);

            return View(model);
        }

        private void AddToDynamo(UrlModel model)
        {
            
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            DynamoDBContext context = new DynamoDBContext(client);
            
            context.Save(model);
             
        }

        private string GenerateShortUrl(string key)
        {
            return "http://go.test.queue-it.com/in/" + key;
        }

        private string GenerateKey()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}