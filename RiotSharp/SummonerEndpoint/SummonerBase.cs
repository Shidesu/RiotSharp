using Newtonsoft.Json;
using RiotSharp.GameEndpoint;
using RiotSharp.Http;
using RiotSharp.Http.Interfaces;
using RiotSharp.LeagueEndpoint;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RiotSharp.Misc;

namespace RiotSharp.SummonerEndpoint
{
    /// <summary>
    /// Class representing the name and id of a Summoner in the API.
    /// </summary>
    public class SummonerBase
    {
        private const string RootUrl = "/api/lol/{0}/v1.4/summoner";
        private const string MasteriesUrl = "/{0}/masteries";
        private const string RunesUrl = "/{0}/runes";

        private const string GameRootUrl = "/api/lol/{0}/v1.3/game";
        private const string RecentGamesUrl = "/by-summoner/{0}/recent";

        private const string IdUrl = "/{0}";

        private IRateLimitedRequester requester;
        public Region Region { get; set; }

        internal SummonerBase()
        {
            requester = Requesters.RiotApiRequester;
        }

        //summoner base not default constructor
        internal SummonerBase(string id, string name, IRateLimitedRequester requester, Region region)
        {
            this.requester = requester;
            Region = region;
            Name = name;
            Id = long.Parse(id);
        }

        /// <summary>
        /// Summoner ID.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Account ID
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// Summoner name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Get rune pages for this summoner synchronously.
        /// </summary>
        /// <returns>A list of rune pages.</returns>
        public List<RunePage> GetRunePages()
        {
            var json =
                requester.CreateGetRequest(string.Format(RootUrl, Region) + string.Format(RunesUrl, Id), Region);
            return JsonConvert.DeserializeObject<Dictionary<string, RunePages>>(json).Values.FirstOrDefault().Pages;
        }

        /// <summary>
        /// Get rune pages for this summoner asynchronously.
        /// </summary>
        /// <returns>A list of rune pages.</returns>
        public async Task<List<RunePage>> GetRunePagesAsync()
        {
            var json = await requester.CreateGetRequestAsync(
                string.Format(RootUrl, Region) + string.Format(RunesUrl, Id),
                Region);
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<Dictionary<string, RunePages>>(json))).Values.FirstOrDefault().Pages;
        }

        /// <summary>
        /// Get mastery pages for this summoner synchronously.
        /// </summary>
        /// <returns>A list of mastery pages.</returns>
        public List<MasteryPage> GetMasteryPages()
        {
            var json = requester.CreateGetRequest(
                string.Format(RootUrl, Region) + string.Format(MasteriesUrl, Id),
                Region);
            return JsonConvert.DeserializeObject<Dictionary<long, MasteryPages>>(json)
                .Values.FirstOrDefault().Pages;
        }

        /// <summary>
        /// Get mastery pages for this summoner asynchronously.
        /// </summary>
        /// <returns>A list of mastery pages.</returns>
        public async Task<List<MasteryPage>> GetMasteryPagesAsync()
        {
            var json = await requester.CreateGetRequestAsync(
                string.Format(RootUrl, Region) + string.Format(MasteriesUrl, Id),
                Region);
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<Dictionary<long, MasteryPages>>(json))).Values.FirstOrDefault().Pages;
        }

        /// <summary>
        /// Get the 10 most recent games for this summoner synchronously.
        /// </summary>
        /// <returns>A list of the 10 most recent games.</returns>
        public List<Game> GetRecentGames()
        {
            var json = requester.CreateGetRequest(
                string.Format(GameRootUrl, Region) + string.Format(RecentGamesUrl, Id),
                Region);
            return JsonConvert.DeserializeObject<RecentGames>(json).Games;
        }

        /// <summary>
        /// Get the 10 most recent games for this summoner asynchronously.
        /// </summary>
        /// <returns>A list of the 10 most recent games.</returns>
        public async Task<List<Game>> GetRecentGamesAsync()
        {
            var json = await requester.CreateGetRequestAsync(
                string.Format(GameRootUrl, Region) + string.Format(RecentGamesUrl, Id),
                Region);
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<RecentGames>(json))).Games;
        }
    }
}
