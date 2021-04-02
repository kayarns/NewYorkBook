using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace NewYorkBook 
{
    
    public class Function
    {

        public static readonly HttpClient client = new HttpClient();
        public async Task<ExpandoObject> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            string list = "";
            Dictionary<string, string> dict = (Dictionary<string, string>)input.QueryStringParameters;
            dict.TryGetValue("list", out list);

            HttpResponseMessage response = await client.GetAsync("https://api.nytimes.com/svc/books/v3/lists/current/" + list + ".json?api-key=AYXPL5EqnRKllAnMhpMFwVlk8D0pEKAW");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();


            Document myDoc = Document.FromJson(responseBody);
            dynamic objX = JsonConvert.DeserializeObject<ExpandoObject>(myDoc.ToJson());
            return objX;
        }
    }
}
