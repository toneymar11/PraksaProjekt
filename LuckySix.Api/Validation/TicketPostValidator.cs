using FluentValidation;
using LuckySix.Api.Models;


namespace LuckySix.Api.Validation
{
  public class TicketPostValidator : AbstractValidator<TicketPost>
  {

    public TicketPostValidator()
    {
      RuleFor(t => t.SelectedNum)
          .Cascade(CascadeMode.Stop)
          .NotEmpty()
          .WithMessage("{PropertyName} should be not empty. NEVER!").Length(11,17);




      RuleFor(u => u.Stake)
          .Cascade(CascadeMode.Stop)
          .NotEmpty()
          .WithMessage("{PropertyName} should be not empty. NEVER!")
          .InclusiveBetween(1, 100).WithMessage("{PropertyName} must be in range 1 to 100");


    }
  }
}
