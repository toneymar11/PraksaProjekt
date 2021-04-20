using AutoMapper;
using LuckySix.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Controllers
{
  public class TicketsController : ControllerBase
  {
    private readonly ITicketRepository ticketRepository;
    private readonly IMapper mapper;

    public TicketsController(ITicketRepository ticketRepository, IMapper mapper)
    {
      this.ticketRepository = ticketRepository;
      this.mapper = mapper;
    }
  }
}
