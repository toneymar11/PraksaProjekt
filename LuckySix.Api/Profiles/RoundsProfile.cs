using AutoMapper;
using LuckySix.Api.Models;
using LuckySix.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Profiles
{
  public class RoundsProfile : Profile
  {
    public RoundsProfile()
    {
      CreateMap<Round, ReadyRound>();
      CreateMap<Round, RunningRound>();
    }
  }
}
