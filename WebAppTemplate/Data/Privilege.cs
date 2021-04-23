using System.Collections.Generic;

namespace WebAppTemplate.Data
{
    public class Privilege
    {
        public Privilege()
        {
            RolePrivileges = new HashSet<RolePrivilege>();
        }

        public int Id { get; set; }
        public string Name { get; set; }        
        public virtual IEnumerable<RolePrivilege> RolePrivileges { get; set; }
    }
}
