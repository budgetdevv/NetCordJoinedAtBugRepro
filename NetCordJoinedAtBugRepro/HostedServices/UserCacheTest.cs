using System.Collections.Concurrent;
using System.Text;

using Microsoft.Extensions.Hosting;

using NetCord;
using NetCord.Gateway;

namespace NetCordJoinedAtBugRepro.HostedServices
{
    public class UserCacheTest(GatewayClient client): IHostedService
    {
        private static readonly TimeSpan TEST_LOOP_INTERVAL = TimeSpan.FromSeconds(10);
        
        private readonly ConcurrentDictionary<ulong, GuildUser> JoinCache = new();

        private readonly CancellationTokenSource TestLoopCancellation = new();
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = TestLoop();
            
            return Task.CompletedTask;
        }

        private async Task TestLoop()
        {
            var stringBuilder = new StringBuilder();

            var iterationNumber = 1;
            
            while (!TestLoopCancellation.IsCancellationRequested)
            {
                stringBuilder.Append($"Running iteration {iterationNumber++}\n");
                
                var (_, guild) = client.Cache.Guilds.SingleOrDefault();

                if (guild == null)
                {
                    stringBuilder.Append("No guild, skipping iteration...");
                    goto Print;
                }

                foreach (var (joinedUserID, joinedUser) in JoinCache)
                {
                    if (!guild.Users.TryGetValue(joinedUserID, out var latestUserCache))
                    {
                        continue;
                    }

                    var discrepancy = latestUserCache.JoinedAt - joinedUser.JoinedAt;
                    
                    stringBuilder.Append($"{joinedUser.Username} | JoinedAt discrepancy: {discrepancy.Ticks}\n");
                }

                Print:
                Console.WriteLine(stringBuilder.ToString());

                stringBuilder.Clear();
                
                await Task.Delay(TEST_LOOP_INTERVAL);
            }
        }

        public void AddJoiningUser(GuildUser user)
        {
            JoinCache[user.Id] = user;
        }
        
        public void RemoveJoiningUser(User user)
        {
            JoinCache.Remove(user.Id, out _);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await TestLoopCancellation.CancelAsync();
        }
    }
}
