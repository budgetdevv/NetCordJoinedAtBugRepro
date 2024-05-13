using System;
using System.Threading.Tasks;

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
            var user = args.User;
            Console.WriteLine($"{user.Username} left the guild!");
            userCacheTest.RemoveJoinedUser(user);
            return ValueTask.CompletedTask;
        }
    }
}
