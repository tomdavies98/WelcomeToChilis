using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace DiscordMusicStreamer
{
    public static class DiscordMusicStreamerManager
    {
        private static ConcurrentDictionary<string, ConcurrentQueue<string>> _allServerMusicQueues = new ConcurrentDictionary<string, ConcurrentQueue<string>>() { };

        private static bool _isRepeatingSong = false;

        private static ConcurrentQueue<string> GetMyServersMusicQueue(string serverGuild)
        {
            if(_allServerMusicQueues.ContainsKey(serverGuild))
            {
                return _allServerMusicQueues[serverGuild];
            }
            else
            {
                return new ConcurrentQueue<string>();
            }
        }

        private static void AddOrUpdateMyServersQueue(string serversGuild, ConcurrentQueue<string> newQueue)
        {
            try
            {
                if(newQueue == null)
                {
                    _allServerMusicQueues.AddOrUpdate(serversGuild, new ConcurrentQueue<string>(), (k, v) => new ConcurrentQueue<string>());
                }
                else
                {
                    _allServerMusicQueues.AddOrUpdate(serversGuild, newQueue, (k, v) => newQueue);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void AddSongToQueue(string song, string serversGuild)
        {
            var myServersQueue = GetMyServersMusicQueue(serversGuild);
            myServersQueue.Enqueue(song);
            AddOrUpdateMyServersQueue(serversGuild, myServersQueue);
        }

        private static bool RemoveSongFromQueue(string serversGuild, out string removedSong)
        {
            var myServersQueue = GetMyServersMusicQueue(serversGuild);
            if(myServersQueue.TryDequeue(out removedSong))
            {
                AddOrUpdateMyServersQueue(serversGuild, myServersQueue);
                return true;
            }
            return false;
        }

        public static bool IsMyQueueEmpty(string serversGuild)
        {
            var myServersQueue = GetMyServersMusicQueue(serversGuild);
            return myServersQueue.IsEmpty;
        }

        public static string SkipSong(string serversGuild)
        {
            if(RemoveSongFromQueue(serversGuild, out string removedSong))
            {
                return DiscordMessageConstants.SongSkipped + removedSong;
            }
            return DiscordMessageConstants.FailedSkip;
        }

        public static string RemoveTopSong(string serversGuild)
        {
            if (RemoveSongFromQueue(serversGuild, out string removedSong))
            {
                return "Finished playing: " + removedSong;
            }
            return DiscordMessageConstants.FailedSkip;
        }

  
        public static bool IsThereAnotherSongInQueue(string serversGuild)
        {
            var myServersQueue = GetMyServersMusicQueue(serversGuild);
            if (myServersQueue == null) return false;
            return myServersQueue?.Count > 0;
        }

        public static string GetNextSong(string serversGuild)
        {
            var myServersQueue = GetMyServersMusicQueue(serversGuild);
            if (myServersQueue.TryPeek(out var nextSong))
            {
                return nextSong;
            }
            return null;
        }
    }
}
