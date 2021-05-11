using FluentValidation;
using LuckySix.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Validation
{
    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("{PropertyName} should be not empty. NEVER!")
                .Length(4, 25);
                //.Must(IsValidUsername).WithMessage("{PropertyName} should be all letters.");


            RuleFor(u => u.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("{PropertyName} should be not empty. NEVER!")
                .Length(8, 25);
        }

        //private bool IsValidUsername(string name)
        //{
        //    return name.All(Char.IsLetter);
        //}
    }
}
