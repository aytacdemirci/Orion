using FluentValidation;
using Orion.Application.StoryAppLayer.UseCases.StoryUseCases.UpdateStory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Application.StoryAppLayer.StoryUseCases.UpdateStory
{
    public class UpdateStoryCommandValidator : AbstractValidator<UpdateStoryCommand>
    {
        public UpdateStoryCommandValidator()
        {
            RuleFor(v => v.Text).NotNull().NotEmpty()
                .MaximumLength(300);
        }
    }
}
