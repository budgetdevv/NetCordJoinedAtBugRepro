using System;
using System.Threading.Tasks;

using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;

using NetCordJoinedAtBugRepro.HostedServices;

namespace NetCordJoinedAtBugRepro.Handlers
{
    [GatewayEvent(nameof(GatewayClient.GuildUserUpdate))]
    internal class GuildUserUpdateHandler(UserCacheTest userCacheTest): IGatewayEventHandler<GuildUser>
    {
        public ValueTask HandleAsync(GuildUser user)
        {
            if (userCacheTest.TryGetJoinedUser(user.Id, out var joinedUser) && user.IsPending != joinedUser.IsPending)
            {
                Console.WriteLine($"{UserCacheTest.CalculateDiscrepancy(newUser: user, oldUser: joinedUser)} [ Post is_pending ]");
            }
            
            return ValueTask.CompletedTask;
        }
    }
}
