using HotChocolate;

namespace Portfolio.Finance.API
{
    public class CurrentUserIdGlobalState : GlobalStateAttribute
    {
        public CurrentUserIdGlobalState() : base("currentUserId")
        {
        }
    }
}