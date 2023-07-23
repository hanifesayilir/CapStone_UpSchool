using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.Login
{
    public class AuthLoginDto
    {
        private AuthLoginDto authLoginDto;

        public string AccessToken { get; set; }

        public DateTime Expires { get; set; }

        public AuthLoginDto(string accessToken, DateTime expires)
        {
            AccessToken = accessToken;

            Expires = expires;
        }
    }
}
