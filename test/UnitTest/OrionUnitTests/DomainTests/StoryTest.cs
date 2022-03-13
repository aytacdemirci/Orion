using FluentAssertions;
using Orion.Domain.SeedWork;
using Orion.Domain.StoryDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrionUnitTests.DomainTests
{
   
    public class StoryTest
    {
        [Fact]
        public void CreateStory_IfBadWordIsNotPassed_ShouldCreateStoryObject()
        {
            //arrance 
            string text = "this is a good text";

            //act
            var story = Story.Create(text);

            //assert
            story.Should().NotBeNull();
            story.Text.Should().Be(text);
        }

        [Fact]
        public void CreateStory_IfBadWordIdPassed_ShouldThrowException()
        {
            //arrange
            string storyText = "this is a badword3";
            
            // act
            Action act = () => Story.Create(storyText); 
            act.Should().Throw<BusinessRuleValidationException>()
                .Where(ex => ex.Errors.First().Equals(ErrorMessages.DetectedBadWordsInText));    
            

        }
    }
}
