using AutoMapper;
using LuckySix.Api.Models;
using LuckySix.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Profiles
{
  public class TicketsProfile : Profile
  {
    public TicketsProfile()
    {
      CreateMap<Ticket, TicketsRound>();
      CreateMap<TicketPost, Ticket>();
      CreateMap<Ticket, TicketDto>();

    }
  }
}
