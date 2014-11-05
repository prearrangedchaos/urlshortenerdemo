using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using UrlShortenerDemo.Models;

namespace UrlShortenerDemo.Controllers
{
    public class InController : Controller
    {
        
        public ActionResult Index(string key)
        {
            
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            DynamoDBContext context = new DynamoDBContext(client);

            var model = LoadModel(key, context);

            if (model == null)
                return Redirect("/");

            LogRedirect(key, client);

            return Redirect(model.Url);
        }

        private static UrlModel LoadModel(string key, DynamoDBContext context)
        {
            UrlModel model = context.Load<UrlModel>(key);
            return model;
        }

        private void LogRedirect(string key, AmazonDynamoDBClient client)
        {
            string userMetadata = this.Request.UserAgent;

            client.UpdateItem(new UpdateItemRequest(
                "Urls",
                new Dictionary<string, AttributeValue>() {{"Key", new AttributeValue() {S = key}}},
                new Dictionary<string, AttributeValueUpdate>()
                {
                    {
                        "Redirected", new AttributeValueUpdate(new AttributeValue()
                        {
                            L = new List<AttributeValue>()
                            {
                                new AttributeValue
                                {
                                    M = new Dictionary<string, AttributeValue>()
                                    {
                                        {"RedirectTime", new AttributeValue() {S = DateTime.UtcNow.ToString("O")}},
                                        {"UserAgent", new AttributeValue() {S = userMetadata}}
                                    }
                                }
                            }
                        },
                        AttributeAction.ADD)
                    }
                })
            );
        }
     
    }
}