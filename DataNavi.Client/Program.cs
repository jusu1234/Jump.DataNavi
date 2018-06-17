using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataNavi.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);

            Run();

            Console.ReadKey(true);
        }

        static async void Run()
        {
           
            string a = string.Empty;
            var w = System.Diagnostics.Stopwatch.StartNew();

            //a =  GetStreamContent().Result;
            ////Console.Write(a);
            int i = 0;

            foreach (var value in GetStreamContent())
            {
                i++;
                //Thread.Sleep(1);

               // Console.WriteLine(value);
            }

            Console.WriteLine(w.ElapsedMilliseconds);
            w.Stop();

        }

        static IEnumerable<myClass> GetValues()
        {
            var serializer = new JsonSerializer();
            var client = new HttpClient();
            var header = new MediaTypeWithQualityHeaderValue("application/json");

            client.DefaultRequestHeaders.Accept.Add(header);
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

            // Note: port number might vary.
            using (var stream = client.GetStreamAsync("http://localhost:53986/api/wtm/GetEnum").Result)
            using (var sr = new StreamReader(stream))
            using (var jr = new JsonTextReader(sr))
            {
                while (jr.Read())
                {
                    jr.SupportMultipleContent = true;
                    // Don't worry about commas.
                    // JSON reader will handle them for us.
                   // if (jr.TokenType != JsonToken.StartArray && jr.TokenType != JsonToken.EndArray)
                        yield return serializer.Deserialize<myClass>(jr);
                    //yield return serializer.Deserialize<String>(jr);
                }
            }
        }

        static IEnumerable<string> GetPushStreamContent()
        {
            var serializer = new JsonSerializer();
            var client = new HttpClient();
            var header = new MediaTypeWithQualityHeaderValue("application/json");

            client.DefaultRequestHeaders.Accept.Add(header);
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

            using (var stream = client.GetStreamAsync("http://localhost:53986/api/wtm/PushStreamContent").Result)
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            //using (var jr = new JsonTextReader(sr))
            {
                while (!sr.EndOfStream)
                {
                    // Don't worry about commas.
                    // JSON reader will handle them for us.
                    //if (jr.TokenType != JsonToken.StartArray && jr.TokenType != JsonToken.EndArray)
                    //    yield return serializer.Deserialize<string>(jr);
                    //yield return serializer.Deserialize<String>(jr);
                    yield return sr.ReadLine();
                }
            }
        }


        static IEnumerable<myClass> GetStreamContent()
        {
            var serializer = new JsonSerializer();
            var client = new HttpClient();
            var header = new MediaTypeWithQualityHeaderValue("application/json");
            StringBuilder sb = new StringBuilder();
            client.DefaultRequestHeaders.Accept.Add(header);
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

            //Task<Stream> getStreamTask = client.GetStreamAsync("http://localhost:53986/api/wtm/StreamContent");

            //Stream stream = await getStreamTask;
            // Note: port number might vary.
            using (var stream = client.GetStreamAsync("http://localhost:53986/api/wtm/StreamContent").Result)
            using (var sr = new StreamReader(stream, Encoding.UTF8))
           using (var jr = new JsonTextReader(sr))
            {
                //var j = new JsonSerializer();
                //j.Deserialize(sr);

                //return sr.ReadToEnd();

                //return sr.ReadToEnd();
                while (jr.Read())
                {
                    // Don't worry about commas.
                    // JSON reader will handle them for us.
                    if (jr.TokenType != JsonToken.StartArray && jr.TokenType != JsonToken.EndArray)
                        yield return serializer.Deserialize<myClass>(jr);
                    //yield return sr.ReadLine();
                    //sb.Append(await sr.ReadLineAsync());
                    //await sr.ReadLineAsync();
                }
            }

            
            //return sb.ToString();
        }
    }

    [DataContract]
    public class ReturnModel
    {
        [DataMember]
        public int SequenceNumber { get; set; }

        [DataMember]
        public Guid ID { get; set; }
    }

    [DataContract]
    public class myClass
    {
        [DataMember(Name = "column1")]
        public string column1 { get; set; }

        [DataMember(Name = "column2")]
        public string column2 { get; set; }

        [DataMember(Name = "column3")]
        public string column3 { get; set; }

        [DataMember(Name = "column4")]
        public string column4 { get; set; }
    }
}
