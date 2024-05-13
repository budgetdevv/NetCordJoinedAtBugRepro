using NetCord.Gateway;
using NetCord.Hosting.Gateway;

using NetCordJoinedAtBugRepro.HostedServices;

namespace NetCordJoinedAtBugRepro.Handlers
{
    [GatewayEvent(nameof(GatewayClient.GuildUserRemove))]
    internal class GuildUserRemoveHandler(UserCacheTest userCacheTest): IGatewayEventHandler<GuildUserRemoveEventArgs>
    {
        public ValueTask HandleAsync(GuildUserRemoveEventArgs args)
        {
            userCacheTest.RemoveJoiningUser(args.User);
            return ValueTask.CompletedTask;
        }
    }
}
