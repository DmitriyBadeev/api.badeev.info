using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using System.Threading.Tasks;

namespace Portfolio.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class TypografQueries
    {
        [Authorize]
        public async Task<string> TypografText(string text)
        {
            return await Typograf.Typograf.Run(text);
        }
    }
}
