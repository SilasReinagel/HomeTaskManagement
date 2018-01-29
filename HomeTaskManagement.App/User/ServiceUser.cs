
namespace HomeTaskManagement.App.User
{
    public sealed class ServiceUser
    {
        private string Id = "bbf38ff2-60ec-4ac3-8c2f-364f0d3277a3";

        public static implicit operator string(ServiceUser user)
        {
            return user.Id;
        }
    }
}
