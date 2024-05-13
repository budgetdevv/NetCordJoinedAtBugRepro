using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace NetCordJoinedAtBugRepro.Modules.SlashCommands
{
    public class SlashCommands(GatewayClient client) : ApplicationCommandModule<SlashCommandContext>
    {
        [SlashCommand("ping", "Get bot latency", DefaultGuildUserPermissions = Permissions.Administrator)]
        public InteractionCallback Ping() 
        {
            var latency = client.Latency;

            return InteractionCallback.Message(new() { Content = $"{latency.TotalMilliseconds} ms!" }); 
        }
    }
}
