using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWTTokenProvider.Models
{
    public class TokenModelInPut
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class TokenModelOutPut
    {
        public string AccessToken { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
