using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using RedditMemeManager;
using WelcomeToChilis.Functions;
using DSharpPlus.VoiceNext;
using DSharpPlus.Lavalink.EventArgs;
using DiscordMusicStreamer;

namespace WelcomeToChilis.Commands
{
    public class FunCommands: BaseCommandModule
    {

        [Command("steamsale")]
        public async Task SteamSale(CommandContext ctx)
        {
            var nextSale = FunctionManager.GetNextSteamSale();
            await ctx.Channel.SendMessageAsync(nextSale).ConfigureAwait(false);
        }

        [Command("forecast")]
        public async Task Forecast(CommandContext ctx)
        {
            var forecast = FunctionManager.GetIrishForecast();
            await ctx.Channel.SendMessageAsync(forecast).ConfigureAwait(false);
        }

        [Command("bobbyRelease")]
        public async Task BobbyRelease(CommandContext ctx)
        {
            var releaseDate = new DateTime(2021,12,11);
            await ctx.Channel.SendMessageAsync($"Bobby Shmurda has bee free {(DateTime.Today - releaseDate).Days} days").ConfigureAwait(false);
        }

        [Command("ping")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }

        [Command("leave")]
        public async Task Leave(CommandContext ctx)
        {
            var lava = ctx.Client.GetLavalink();
            var channel = ctx.Member.VoiceState.Guild;
            if (!lava.ConnectedNodes.Any())
            {
                await ctx.RespondAsync("The Lavalink connection is not established");
                return;
            }

            var node = lava.ConnectedNodes.Values.First();
            var memberChannel = ctx.Member.VoiceState.Guild;
            var conn = node.GetGuildConnection(memberChannel);

            if (conn == null)
            {
                await ctx.RespondAsync("Lavalink is not connected.");
                return;
            }

            await conn.DisconnectAsync();
            await ctx.RespondAsync($"Left {channel.Name}!");
        }

        [Command("join")]
        public async Task Join(CommandContext ctx)
        {
            await JoinChannel(ctx);
        }

        [Command("skip")]
        public async Task Skip(CommandContext ctx)
        {
            await SkipCurrentSong(ctx);
        }

        [Command("clear")]
        public async Task Clear(CommandContext ctx)
        {
            await ClearAllQueuedSongs(ctx);
        }

        [Command("play")]
        public async Task Play(CommandContext ctx, [RemainingText] string search)
        {
            var channelId = ctx.Member.VoiceState.Guild.Id.ToString();
            if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
            {
                await ctx.RespondAsync("You are not in a voice channel.");
                return;
            }

            if (!DiscordMusicStreamerManager.IsThereAnotherSongInQueue(channelId))
            {
                DiscordMusicStreamerManager.AddSongToQueue(search, channelId);
                await PlayNextInQueueTrack(ctx);
            }
            else
            {
                DiscordMusicStreamerManager.AddSongToQueue(search, channelId);
                await ctx.RespondAsync($"Added song to queue :)");
            }
        }

        [Command("getmeme")]
        public async Task GetMeme(CommandContext ctx)
        {
            try
            {
                var config = ConfigManager.GetConfig();
                var memePath = MemeManager.GetMeme(config.MemeFolderPath);

                if (memePath != null)
                {
                    using (var fs = new FileStream(memePath, FileMode.Open, FileAccess.Read))
                    {
                        var msg = await new DiscordMessageBuilder()
                            .WithContent("Funny meme yes?")
                            .WithFiles(new Dictionary<string, Stream>() { { memePath, fs } })
                            .SendAsync(ctx.Channel);
                    }
                }                    
                else
                    await ctx.Channel.SendMessageAsync("No memes fam check later").ConfigureAwait(false);

                var success = MemeManager.DeleteMeme(memePath);

                if(success)
                {
                    Console.WriteLine("Deleted meme successfully..");
                }
                else
                {
                    throw new Exception("Failed to delete image!");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        [Command("getname")]
        public async Task GetName(CommandContext ctx)
        {
            var name = FunctionManager.GetName();
            await ctx.Channel.SendMessageAsync(name).ConfigureAwait(false);
        }

        [Command("roco")]
        public async Task Roco(CommandContext ctx)
        {
            var rocoStatus = FunctionManager.GetRocoServerStatus();
            await ctx.Channel.SendMessageAsync("Rogue Company server status: " + rocoStatus).ConfigureAwait(false);
        }

        [Command("smite")]
        public async Task Smite(CommandContext ctx)
        {
            var smiteStatus = FunctionManager.GetSmiteServerStatus();
            await ctx.Channel.SendMessageAsync("Smite server status: " + smiteStatus).ConfigureAwait(false);
        }

        [Command("hirezStatus")]
        public async Task HirezStatus(CommandContext ctx)
        {
            var hirezStatus = FunctionManager.GetAllHirezServerStatus();
            await ctx.Channel.SendMessageAsync(hirezStatus).ConfigureAwait(false);
        }

        private async Task PlayNextInQueueTrack(CommandContext ctx)
        {
            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var memberChannel = ctx.Member.VoiceState.Guild;
            var conn = node.GetGuildConnection(memberChannel);

            if (conn == null)
            {
                await JoinChannel(ctx);
                lava = ctx.Client.GetLavalink();
                node = lava.ConnectedNodes.Values.First();
                memberChannel = ctx.Member.VoiceState.Guild;
                conn = node.GetGuildConnection(memberChannel);
                await ctx.RespondAsync("I wasn't asked to join discord, sound 4 the invite");
                if (conn == null)
                {
                    await ctx.RespondAsync("Lavalink is not enabled");
                    return;
                }
            }
            
            var search = DiscordMusicStreamerManager.GetNextSong(memberChannel.Id.ToString());
            LavalinkLoadResult loadResult;
            if (search.Contains("https"))
            {
                var url = new Uri(search);
                loadResult = await node.Rest.GetTracksAsync(url);
            }
            else
            {
                loadResult = await node.Rest.GetTracksAsync(search);
            }

            if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed
                || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                await ctx.RespondAsync($"Track search failed for {search}.");
                return;
            }

            var track = loadResult.Tracks.First();
            conn.PlaybackFinished += (sender,e) => PlayNextTrack(sender, e, ctx);
            await conn.PlayAsync(track);
            await ctx.RespondAsync($"Now playing {track.Title}!");
        }

        private async Task SkipCurrentSong(CommandContext ctx)
        {
            var channelId = ctx.Channel.Guild.Id.ToString();

            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var memberChannel = ctx.Member.VoiceState.Guild;
            var conn = node.GetGuildConnection(memberChannel);

            DiscordMusicStreamerManager.RemoveTopSong(channelId);
            await conn.SeekAsync(new TimeSpan(100,0,0));
            await PlayNextInQueueTrack(ctx);
            await ctx.RespondAsync($"Song skipped by: {ctx.User.Username}");
        }

        private async Task ClearAllQueuedSongs(CommandContext ctx)
        {
            var channelId = ctx.Channel.Guild.Id.ToString();

            while(!DiscordMusicStreamerManager.IsMyQueueEmpty(channelId))
            {
                DiscordMusicStreamerManager.RemoveTopSong(channelId);
            }

            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var memberChannel = ctx.Member.VoiceState.Guild;
            var conn = node.GetGuildConnection(memberChannel);

            await conn.StopAsync();
            await ctx.RespondAsync($"Queue has been cleared by: {ctx.User.Username}");
        }

        private async Task PlayNextTrack(LavalinkGuildConnection sender, TrackFinishEventArgs e, CommandContext ctx)
        {
            var channelId = sender.Channel.Guild.Id.ToString();
            DiscordMusicStreamerManager.RemoveTopSong(channelId);

            if (DiscordMusicStreamerManager.IsThereAnotherSongInQueue(channelId))
            {
                await PlayNextInQueueTrack(ctx);
            }
        }

        private async Task JoinChannel(CommandContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var lava = ctx.Client.GetLavalink();
            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc != null)
                throw new InvalidOperationException("Already connected in this guild.");

            var chn = ctx.Member?.VoiceState?.Channel;
            var node = lava.ConnectedNodes.Values.First();

            if (chn.Type != ChannelType.Voice)
            {
                await ctx.RespondAsync("Not a valid voice channel.");
                return;
            }

            await node.ConnectAsync(chn);
            await ctx.RespondAsync("👌");
        }
    }
}
