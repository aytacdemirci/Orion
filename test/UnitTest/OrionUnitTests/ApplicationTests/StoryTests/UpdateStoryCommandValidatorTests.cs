using Bogus;
using FluentAssertions;
using FluentValidation.TestHelper;
using Orion.Application.StoryAppLayer.StoryUseCases.UpdateStory;
using Orion.Application.StoryAppLayer.UseCases.StoryUseCases.UpdateStory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrionUnitTests.ApplicationTests.StoryTests
{
    public class UpdateStoryCommandValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public void UpdateStoryCommandValidator_IfTextIsNull_ShouldThrowValidationException(string text)
        {
            //arrange
            var validator = new UpdateStoryCommandValidator();

            var command = new UpdateStoryCommand()
            {
                Text = text
            };


            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldHaveValidationErrorFor(story => story.Text);
        }

        [Fact]
        public void UpdateStoryCommandValidator_IfTextLengthIsMoreThan300_ShouldThrowValidationException()
        {
            //arrange
            string text = new Faker().Random.String2(301);

            var validator = new UpdateStoryCommandValidator();

            var updateStoryCommand = new UpdateStoryCommand()
            {
                Text = text
            };


            //act
            var result = validator.TestValidate(updateStoryCommand);

            //assert
            result.ShouldHaveValidationErrorFor(story => story.Text);
        }

        [Fact]
        public void UpdateStoryCommandValidator_IfTextLengthIsEqualTo300_ShouldSuccess()
        {
            //arrange
            string text = new Faker().Random.String2(300);

            var validator = new UpdateStoryCommandValidator();

            var updateStoryCommand = new UpdateStoryCommand()
            {
                Text = text
            };


            //act
            var result = validator.TestValidate(updateStoryCommand);

            //assert
            result.IsValid.Should().BeTrue();
        }
    }
}
