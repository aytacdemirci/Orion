using Microsoft.AspNetCore.Mvc.Testing;
using Orion.Application.StoryAppLayer.DTOs;
using Orion.Application.StoryAppLayer.UseCases.StoryUseCases.CreateStory;
using Orion.Application.StoryAppLayer.UseCases.StoryUseCases.UpdateStory;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrionIntegrationTests.ApiTests
{
    public class StoriesApiTests
    {
        protected readonly HttpClient httpClient;
        public StoriesApiTests()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            HttpClient httpClient = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task Manage_Stories()
        {
            //---- Create-----------
            //Arrange 
            string url = "/api/stories";
            var inputStory = new CreateStoryCommand { Text = "Integration Test" };

            //Act
            var postResponse = await httpClient.PostAsJsonAsync(url, inputStory);

            //Assert
            postResponse.EnsureSuccessStatusCode();
            var insertedStory = await postResponse.Content.ReadFromJsonAsync<StoryDto>();
            insertedStory.Text.ShouldBe(inputStory.Text);



            //----- Read All --------
            //Act
            var stories = await httpClient.GetFromJsonAsync<IList<StoryDto>>(url);

            //Assert
            var recentlyInsertedStory = stories.First(x => x.Id == insertedStory.Id);
            recentlyInsertedStory.ShouldBeEquivalentTo(insertedStory);


            //----ReadById--------
            //Arrange
            var id = insertedStory.Id;
            var urlWithId = $"{url}/{id.ToString()}";

            //Act
            var fetchedStory = await httpClient.GetFromJsonAsync<StoryDto>(urlWithId);

            //Assert
            insertedStory.ShouldBeEquivalentTo(fetchedStory);


            //------Update--------
            //Arrange
            var storyToUpdate = new UpdateStoryCommand { Id = fetchedStory.Id, Text = "Integration Test-Update" };

            //Act
            var updateResponse = await httpClient.PostAsJsonAsync(urlWithId, storyToUpdate);

            //Assert
            updateResponse.EnsureSuccessStatusCode();
            var updatedStory = await updateResponse.Content.ReadFromJsonAsync<StoryDto>();
            storyToUpdate.Id.ShouldBe(updatedStory.Id);
            storyToUpdate.Text.ShouldBe(updatedStory.Text);


            //------Delete------
            //Act
            var deleteResponse = await httpClient.DeleteAsync(urlWithId);

            //Assert
            deleteResponse.EnsureSuccessStatusCode();
            var deletedStory = await deleteResponse.Content.ReadFromJsonAsync<StoryDto>();
            deletedStory.ShouldBeEquivalentTo(updatedStory);


        }
    }
}
