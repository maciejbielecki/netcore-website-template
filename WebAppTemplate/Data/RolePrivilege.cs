namespace WebAppTemplate.Data
{
    public class RolePrivilege
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PrivilegeId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Privilege Privilege { get; set; }
    }
}
