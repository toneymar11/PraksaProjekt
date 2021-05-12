using AutoMapper;
using LuckySix.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace LuckySix.Api.Controllers
{

    public class Controller : ControllerBase
    {
        public readonly IRoundRepository roundRepository;
        public readonly IMapper mapper;
        public readonly ITicketRepository ticketRepository;
        public readonly ITicketValidation ticketValidation;
        public readonly ITokenRepository tokenRepository;
        public readonly IConfiguration configuration;
        public readonly IUserRepository userRepository;
        public readonly IUserValidation userValidation;

        public Controller(IRoundRepository roundRepository, IMapper mapper)
        {
            this.roundRepository = roundRepository;
            this.mapper = mapper;
        }

        public Controller(ITicketRepository ticketRepository, IMapper mapper, ITicketValidation ticketValidation, ITokenRepository tokenRepository)
        {
            this.mapper = mapper;
            this.ticketRepository = ticketRepository;
            this.ticketValidation = ticketValidation;
            this.tokenRepository = tokenRepository;
        }

        public Controller(ITokenRepository tokenRepository, IMapper mapper, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.tokenRepository = tokenRepository;
            this.configuration = configuration;
        }

        public Controller(IUserRepository userRepository, IMapper mapper, IUserValidation userValidation, ITokenRepository tokenRepository)
        {
            this.mapper = mapper;
            this.tokenRepository = tokenRepository;
            this.userRepository = userRepository;
            this.userValidation = userValidation;

        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> IsUserLogged()
        {
            int userId = GetUserFromHeader();
            string token = GetTokenFromHeader();

            if (userId == 0 || token == "no")
            {
                return false;
            }

            var userValid = await tokenRepository.IsTokenValid(userId, token);
            if (userValid == null) return false;

            return true;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public int GetUserFromHeader()
        {
            string userid = Request.Headers["userid"];


            if (userid == null || userid.Equals("null")) return 0;

            int number = Int32.Parse(userid);

            return number;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetTokenFromHeader()
        {
            string auth = Request.Headers["authorization"];


            if (auth == null || auth.Equals("null")) return "no";

            return auth;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void ResponseHeaders()
        {
            Response.Headers["authorization"] = GetTokenFromHeader();
            Response.Headers["userid"] = GetUserFromHeader().ToString();
        }


    }
}
