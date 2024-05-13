using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.Interactions;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Interactions;

using NetCordJoinedAtBugRepro;
using NetCordJoinedAtBugRepro.Helpers;
using NetCordJoinedAtBugRepro.HostedServices;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddDiscordGateway(options =>
    {
        options.Configuration = new()
        {
            Intents = GatewayIntents.All
        };
    })
    .AddApplicationCommandService<SlashCommandInteraction, SlashCommandContext>(OptionsHelper.ConfigureApplicationCommandService)
    .AddApplicationCommandService<UserCommandInteraction, UserCommandContext>(OptionsHelper.ConfigureApplicationCommandService)
    .AddApplicationCommandService<MessageCommandInteraction, MessageCommandContext>(OptionsHelper.ConfigureApplicationCommandService)
    .AddInteractionService<ButtonInteraction, ButtonInteractionContext>(OptionsHelper.ConfigureInteractionService)
    .AddInteractionService<StringMenuInteraction, StringMenuInteractionContext>(OptionsHelper.ConfigureInteractionService)
    .AddInteractionService<UserMenuInteraction, UserMenuInteractionContext>(OptionsHelper.ConfigureInteractionService)
    .AddInteractionService<ModalSubmitInteraction, ModalSubmitInteractionContext>(OptionsHelper.ConfigureInteractionService)
    .AddHttpClient()
    .AddGatewayEventHandlers(typeof(Program).Assembly)
    .AddHostedSingletonService<UserCacheTest>()
    .AddOptions<Configuration>()
    .BindConfiguration(string.Empty);;

var host = builder.Build()
    .AddModules(typeof(Program).Assembly)
    .UseGatewayEventHandlers();

await host.RunAsync();
