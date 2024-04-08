using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using NUnit.Framework;
using RestSharp;
using RestfulApiDev.Item_Models;
using System.Runtime.CompilerServices;

namespace RestfulApiDev
{
    public class RestfulApiDevTests
    {
        private RestClient _client;
        private string _url;
        private string _createdItemId;

        [SetUp]
        public void Setup()
        {
            this._url = "https://api.restful-api.dev";
            this._client = new RestClient(_url);
        }

        [Order(1)]
        [Test]
        public async Task Get_GetAll_ShouldReturnAllItems()
        {
            // Arrange
            var request = new RestRequest("/objects", Method.Get);

            // Act
            var response = await this._client.ExecuteAsync(request);
            var jsonResponse = JsonSerializer.Deserialize<List<Item>>(response.Content);
            var firstItem = jsonResponse.First();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(jsonResponse.Count(), Is.EqualTo(13));
            Assert.That(firstItem.Name, Is.Not.Null);
        }

        [Order(2)]
        [Test]
        public async Task Post_PostRequest_ShouldCreateAnItem_WhenGivenValidData()
        {
            // Arrange
            var request = new RestRequest("/objects", Method.Post);

            var itemToAdd = new Item()
            {
                Name = "Added item",
                ItemData = new ItemData()
                {
                    Year = 2020,
                    Price = 1500m,
                    Color = "blue",
                    Capacity = "250 GB"
                }
            };

            var itemToAddJson = JsonSerializer.Serialize(itemToAdd);
            request.AddJsonBody(itemToAddJson);

            // Act
            var response = await this._client.ExecuteAsync(request);
            var jsonResponse = JsonSerializer.Deserialize<Item>(response.Content);

            _createdItemId = jsonResponse.Id;


            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Request.Timeout, Is.LessThan(2000));
            Assert.That(jsonResponse.Id, Is.Not.Null);

        }

        [Order(3)]
        [Test]
        public async Task Get_GetSingleItem_ShouldReturnItem_WhenGivenValidId()
        {
            // Arrange
            int itemId = 5;

            var request = new RestRequest($"/objects/{itemId}", Method.Get);

            // Act
            var response = await this._client.ExecuteAsync(request);

            var responseJson = JsonSerializer.Deserialize<Item>(response.Content);

            // Assert
            Assert.That(responseJson.Id, Is.EqualTo(itemId.ToString()));

        }

        [Order(4)]
        [Test] 
        public async Task Put_PutRequest_ShouldUpdateWholeItem()
        {
            // Arrange                     

            var itemToAdd = new Item()
            {
                Name = "Added item PUT",
                ItemData = new ItemData()
                {
                    Year = 2220,
                    Price = 5500m,
                    Color = "red",
                    Capacity = "1234 GB",
                    CpuModel = "Intel"
                }
            };

            var request = new RestRequest($"/objects/{_createdItemId}", Method.Put);

            var itemToAddJson = JsonSerializer.Serialize(itemToAdd);
            request.AddJsonBody(itemToAddJson);

            // Act
            var response = await this._client.ExecuteAsync(request);

            var responseJson = JsonSerializer.Deserialize<Item>(response.Content);

            // Asset
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseJson.Name, Is.EqualTo("Added item PUT"));
        }

        [Order(5)]
        [Test]
        public async Task Patch_PatchRequest_ShouldPartiallyUpdateAnItem()
        {
            // Arrange                     

            var itemToUpdate = new Item()
            {
                Name = "new name"
            };

            var itemToAddJson = JsonSerializer.Serialize(itemToUpdate);

            var request = new RestRequest($"/objects/{_createdItemId}", Method.Patch);
            
            request.AddJsonBody(itemToAddJson);

            // Act
            var response = await this._client.ExecuteAsync(request);

            var responseJson = JsonSerializer.Deserialize<Item>(response.Content);

            // Asset
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseJson.Name, Is.EqualTo("new name"));
        }

        [Order(6)]
        [Test]
        public async Task Delete_DeleteRequest_ShouldRemoveItemById()
        {
            // Arrange
            var request = new RestRequest($"/objects/{_createdItemId}");

            // Act
            var response = await this._client.ExecuteAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Order(7)]
        [Test]
        public async Task Delete_DeleteRequest_ShouldReturnErrorMessage()
        {
            // Arrange
            string invalidItemId = "invalidId123";

            var request = new RestRequest($"/objects/{invalidItemId}");
            string errorMessage = $"Oject with id={invalidItemId} was not found.";

            // Act
            var response = await this._client.ExecuteAsync(request);

            var reponseJson = JsonSerializer.Deserialize<Errors>(response.Content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(reponseJson.DeletionError, Is.EqualTo(errorMessage));
        }
    }
}
