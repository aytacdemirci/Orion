using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Application.StoryAppLayer.StoryUseCases.CreateStory
{
    public class CreateStoryCommandValidator:AbstractValidator<CreateStoryCommand>
    {
        public CreateStoryCommandValidator()
        {
            RuleFor(x => x.Text).NotNull().NotEmpty();
        }
    }
}
