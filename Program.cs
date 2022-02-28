using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GhibliAPI
{
    class Ghibli
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("original_title_romanised")]
        public string OriginalTitleRomanised { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("producer")]
        public string Producer { get; set; }

        [JsonProperty("release_date")]
        public int ReleaseDate { get; set; }

        [JsonProperty("running_time")]
        public int RunningTime { get; set; }

        [JsonProperty("rt_score")]
        public int Rating { get; set; }

    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            await ProcessRepositories();
        }

        private static async Task ProcessRepositories()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter a Ghibli movie title (Case-Sensitive). Press Enter without writing a title to quit the program.");

                    var ghibliTitle = Console.ReadLine();

                    if (string.IsNullOrEmpty(ghibliTitle))
                    {
                        break;
                    }

                    var result = await client.GetAsync("https://ghibliapi.herokuapp.com/films/?title=" + ghibliTitle);
                    var resultRead = await result.Content.ReadAsStringAsync();
                    var ghibliFilm = JsonConvert.DeserializeObject<List<Ghibli>>(resultRead);

                    Console.InputEncoding = System.Text.Encoding.Unicode;
                    Console.OutputEncoding = System.Text.Encoding.Unicode;
                    Console.WriteLine("---");
                    ghibliFilm.ForEach(ghibli => Console.Write("Ghibli Movie: " + ghibli.Title +
                                                      "\nJapanese Title: " + ghibli.OriginalTitle +
                                                      "\nRomanised Title: " + ghibli.OriginalTitleRomanised +
                                                      "\nYear Released: " + ghibli.ReleaseDate +
                                                      "\nMovie Length: " + ghibli.RunningTime + " minutes" +
                                                      "\nDescription: " + ghibli.Description +
                                                      "\nDirector: " + ghibli.Director +
                                                      "\nProducer: " + ghibli.Producer +
                                                      "\nRating Score: " + ghibli.Rating));
                    Console.WriteLine("\n---");
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR. Please enter a valid Ghibli movie title!", e);
                }
                
            }
        }
    }
}

