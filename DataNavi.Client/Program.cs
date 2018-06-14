using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataNavi.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            string a = string.Empty;

            //foreach (var value in GetValues())
            //    Console.WriteLine("{0}\t{1}", value.SequenceNumber, value.ID);
            //a = value.SequenceNumber.ToString();
            //a = value;
            //Console.WriteLine(value);
            var w = System.Diagnostics.Stopwatch.StartNew();

            StringBuilder sb = new StringBuilder();

            foreach (var value in GetPushStreamContent())
            {
                //sb.Append(value);
                //Console.Write(value);
            }
            //Console.Write(sb.ToString());

            //GetStreamContent();

            Console.WriteLine(w.ElapsedMilliseconds);
            w.Stop();

            Console.ReadKey(true);
        }

        static IEnumerable<string> GetValues()
        {
            var serializer = new JsonSerializer();
            var client = new HttpClient();
            var header = new MediaTypeWithQualityHeaderValue("application/json");

            client.DefaultRequestHeaders.Accept.Add(header);

            // Note: port number might vary.
            using (var stream = client.GetStreamAsync("http://localhost:53986/api/wtm/GetEnum").Result)
            using (var sr = new StreamReader(stream))
            using (var jr = new JsonTextReader(sr))
            {
                while (jr.Read())
                {
                    // Don't worry about commas.
                    // JSON reader will handle them for us.
                    if (jr.TokenType != JsonToken.StartArray && jr.TokenType != JsonToken.EndArray)
                        yield return serializer.Deserialize<string>(jr);
                    //yield return serializer.Deserialize<String>(jr);
                }
            }
        }

        static IEnumerable<int> GetPushStreamContent()
        {
            var serializer = new JsonSerializer();
            var client = new HttpClient();
            var header = new MediaTypeWithQualityHeaderValue("application/json");

            client.DefaultRequestHeaders.Accept.Add(header);

            // Note: port number might vary.
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
                    yield return sr.Read();
                }
            }
        }


        static string GetStreamContent()
        {
            var serializer = new JsonSerializer();
            var client = new HttpClient();
            var header = new MediaTypeWithQualityHeaderValue("application/json");

            client.DefaultRequestHeaders.Accept.Add(header);

            // Note: port number might vary.
            using (var stream = client.GetStreamAsync("http://localhost:53986/api/wtm/StreamContent").Result)
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            //using (var jr = new JsonTextReader(sr))
            {
                return sr.ReadToEnd();
                //while (!sr.EndOfStream)
                //{
                //    // Don't worry about commas.
                //    // JSON reader will handle them for us.
                //    //if (jr.TokenType != JsonToken.StartArray && jr.TokenType != JsonToken.EndArray)
                //    //    yield return serializer.Deserialize<string>(jr);
                //    //yield return serializer.Deserialize<String>(jr);
                //    yield return sr.Read();
                //}
            }
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
}
