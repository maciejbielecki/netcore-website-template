using System.Collections.Generic;

namespace WebAppTemplate.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string GoogleId { get; set; }
        public string FacebookId { get; set; }
        public int RoleId { get; set; }

        public virtual UserData UserData { get; set; }
        public virtual Role Role { get; set; }
    }
}
