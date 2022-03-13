using Bogus;
using FluentAssertions;
using FluentValidation.TestHelper;
using Orion.Application.StoryAppLayer.UseCases.StoryUseCases.CreateStory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrionUnitTests.ApplicationTests.StoryTests
{
    public class CreateStoryCommandValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public void CreateStoryCommandValidator_IfTextIsNullOrEmpty_ShouldThrowValidationException(string text)
        {
            //arrange
            var validator = new CreateStoryCommandValidator();

            var createStoryCommand = new CreateStoryCommand()
            {
                Text = text
            };

            //act
            var result = validator.TestValidate(createStoryCommand);

            //assert
            result.ShouldHaveValidationErrorFor(story => story.Text);
        }

        [Fact]
        public void CreateStoryCommandValidator_IfTextLengthIsMoreThan301_ShouldThrowValidationException()
        {
            //arrange
            string text = new Faker().Random.String2(301);

            var validator = new CreateStoryCommandValidator();

            var createStoryCommand = new CreateStoryCommand()
            {
                Text = text
            };


            //act
            var result = validator.TestValidate(createStoryCommand);

            //assert
            result.ShouldHaveValidationErrorFor(story => story.Text);
        }

        [Fact]
        public void CreateStoryCommandValidator_IfTextLengthIsEqualTo300_ShouldSuccess()
        {
            //arrange
            string text = new Faker().Random.String2(300);

            var validator = new CreateStoryCommandValidator();

            var createStoryCommand = new CreateStoryCommand()
            {
                Text = text
            };


            //act
            var result = validator.TestValidate(createStoryCommand);

            //assert
            result.IsValid.Should().BeTrue();
        }
    }
    }
