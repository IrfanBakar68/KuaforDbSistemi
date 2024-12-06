namespace KuaforDbSistemi.ViewModels
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }
}
