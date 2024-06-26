﻿using System;
using System.Threading.Tasks;

using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;

using NetCordJoinedAtBugRepro.HostedServices;

namespace NetCordJoinedAtBugRepro.Handlers
{
    [GatewayEvent(nameof(GatewayClient.GuildUserAdd))]
    internal class GuildUserAddHandler(UserCacheTest userCacheTest): IGatewayEventHandler<GuildUser>
    {
        public ValueTask HandleAsync(GuildUser user)
        {
            Console.WriteLine($"{user.Username} joined the guild!");
            userCacheTest.AddJoiningUser(user);
            return ValueTask.CompletedTask;
        }
    }
}
