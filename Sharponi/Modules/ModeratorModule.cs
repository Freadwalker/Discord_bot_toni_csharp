using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Sharponi.Attributes;

namespace Sharponi.Modules
{
    [Name("Moderator")]
    [HiddenHelp]
    [RequireContext(ContextType.Guild)]
    public class ModeratorModule : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [Summary("Kick the specified user.")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public Task Kick([Remainder] SocketGuildUser user) => ReplyAsync("Nope this command is deactivated because of security reasons");
    }
}
