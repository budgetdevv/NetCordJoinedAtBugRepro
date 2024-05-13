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
            client.GuildCreate += ClientOnGuildCreate;

            async ValueTask ClientOnGuildCreate(GuildCreateEventArgs args)
            {
                client.GuildCreate -= ClientOnGuildCreate;
                
                // Allow guild cache to be populated.
                await Task.Yield();
                
                _ = TestLoop();
            }

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
                    string currentText;
                    
                    if (guild.Users.TryGetValue(joinedUserID, out var latestUserCache))
                    {
                        var discrepancy = latestUserCache.JoinedAt - joinedUser.JoinedAt;

                        currentText = $"{joinedUser.Username} | JoinedAt discrepancy: {discrepancy.Ticks} ticks, {discrepancy.Microseconds} μs\n";
                    }

                    else
                    {
                        currentText = $"{joinedUser.Username} | User left! Removing...\n";
                    }

                    stringBuilder.Append(currentText);
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
