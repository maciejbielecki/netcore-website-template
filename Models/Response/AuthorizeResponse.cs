using WebAppTemplate.Data;
using WebAppTemplate.Models.Request;

namespace WebAppTemplate.Models.Response
{
    public class AuthorizeResponse
    {
        public Register Register { get; set; }
        public User User { get; set; }
        public string[] Errors { get; set; }
    }
}
