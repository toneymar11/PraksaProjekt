using FluentValidation;
using LuckySix.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Validation
{
    public class TicketPostValidator : AbstractValidator<TicketPost>
    {

        public TicketPostValidator()
        {
            RuleFor(t => t.SelectedNum)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("{PropertyName} should be not empty. NEVER!");

            //.Must(IsValidUsername).WithMessage("{PropertyName} should be all letters.");


            RuleFor(u => u.Stake)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("{PropertyName} should be not empty. NEVER!");
                

        }
    }
}
