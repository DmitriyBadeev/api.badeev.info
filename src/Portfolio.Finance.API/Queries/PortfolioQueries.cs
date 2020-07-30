using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

namespace Portfolio.Finance.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class PortfolioQueries
    {
        [Authorize]
        public string SecretData()
        {
            return "Secret";
        }

        public string Test()
        {
            return "Test";
        }
    }
}
