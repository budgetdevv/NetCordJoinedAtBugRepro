using System.Threading.Tasks;

using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace NetCordJoinedAtBugRepro.Modules.SlashCommands
{
    public class SlashCommands(GatewayClient client) : ApplicationCommandModule<SlashCommandContext>
    {
        [SlashCommand("ping", "Get bot latency", DefaultGuildUserPermissions = Permissions.Administrator)]
        public Task<InteractionCallback> RemoveReputationAsync()
    {
        var latency = client.Latency;
        
        return Task.FromResult<InteractionCallback>(InteractionCallback.Message(new()
        {
            Content = $"{latency.TotalMilliseconds} ms!",
        }));
    }
    }
}
