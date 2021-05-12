using FluentValidation;
using LuckySix.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Validation
{
  public class UserRegisterValidator : AbstractValidator<UserRegister>
  {

    public UserRegisterValidator()
    {
      RuleFor(u => u.Username).NotEmpty()
                .WithMessage("{PropertyName} should be not empty. NEVER!")
                .Length(4, 25);

      RuleFor(u => u.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("{PropertyName} should be not empty. NEVER!")
                .Length(8, 25);

      RuleFor(u => u.FirstName).NotEmpty().WithMessage("{PropertyName} should be not empty. NEVER!")
        .NotEqual(u => u.LastName);
      RuleFor(u => u.LastName).NotEmpty().WithMessage("{PropertyName} should be not empty. NEVER!");


    }
  }
}
