using Amazon.DynamoDBv2.DataModel;

namespace UrlShortenerDemo.Models
{
    [DynamoDBTable("Urls")]
    public class UrlModel
    {
        [DynamoDBHashKey]
        public string Key { get; set; }
        [DynamoDBProperty]
        public string Url { get; set; }
        [DynamoDBProperty]
        public string Created { get; set; }
        [DynamoDBProperty]
        public string User { get; set; }
        [DynamoDBProperty]
        public string ShortenedUrl { get; set; }
    }
}