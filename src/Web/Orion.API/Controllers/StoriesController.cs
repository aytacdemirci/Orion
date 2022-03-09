using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orion.API.Controllers.SeedWork;
using Orion.Application.StoryAppLayer.StoryUseCases.CreateStory;
using Orion.Application.StoryAppLayer.StoryUseCases.DeleteStory;
using Orion.Application.StoryAppLayer.StoryUseCases.GetStories;
using Orion.Application.StoryAppLayer.StoryUseCases.GetStoryById;
using Orion.Application.StoryAppLayer.StoryUseCases.UpdateStory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.API.Controllers
{
    public class StoriesController : OrionBaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetStoriesQuery query)
        {
            var stories = await Mediator.Send(query);
            return Ok(stories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            var story = await Mediator.Send(new GetStoryByIdQuery { Id = id });
            return Ok(story);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateStoryCommand command)
        {
            var story =  await Mediator.Send(command);
            return CreatedAtAction("Get", new { id = story.Id });
        }

        [HttpPut("{id}")]//stories/id
        public async Task<IActionResult> Put([FromRoute]Guid id, [FromBody] UpdateStoryCommand command)
        {
            if(id != command.Id)
            {
                return BadRequest();
            }

            var story = await Mediator.Send(command);
            return Ok(story);
        }

        [HttpDelete("{id}")] // stories/id
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var story = await Mediator.Send(new DeleteStoryCommand { Id = id });
            return Ok(story);
        }
    }
}
