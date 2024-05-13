using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;

using NetCordJoinedAtBugRepro.HostedServices;

namespace NetCordJoinedAtBugRepro.Handlers
{
    [GatewayEvent(nameof(GatewayClient.GuildUserRemove))]
    internal class GuildUserRemoveHandler(UserCacheTest userCacheTest): IGatewayEventHandler<GuildUser>
    {
        public ValueTask HandleAsync(GuildUser user)
        {
            userCacheTest.RemoveJoiningUser(user);
            return ValueTask.CompletedTask;
        }
    }
}
