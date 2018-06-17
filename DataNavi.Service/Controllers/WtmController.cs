using DataNavi.Database;
using DataNavi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Dapper;
using System.IO;
using System.Net;
using CsvHelper;
using Newtonsoft.Json;

namespace DataNavi.Service.Controllers
{
    /// <summary>
    /// Represents test controller that should be removed.
    /// </summary>
    [RoutePrefix("api/wtm")]
    public class WtmController : ApiController
    {
        [HttpGet, Route("GetEnum")]
        public IEnumerable<myClass> GetEnum()
        {
            string filePath = @"E:\NAS_SAMPLE\";
            string fileName = "data_1.csv";
            string fullPath = filePath + fileName;

            // TextReader reader = File.OpenText(fullPath);

            //CsvReader csv = new CsvReader(reader);
            //csv.Configuration.Delimiter = "\t";
            //csv.Read();
            //csv.ReadHeader();

            //System.Diagnostics.Debug.WriteLine("csv.ReadHeader : " + w.ElapsedMilliseconds);

            //using (StreamReader reader = File.OpenText(fullPath))
            //{
            //    while (reader.Peek() > -1)
            //    {
            //        //var record = csv.GetRecord<myClass>();

            //        yield return reader.ReadLine();
            //    }
            //}

            TextReader reader = File.OpenText(fullPath);

            CsvReader csv = new CsvReader(reader);
            csv.Configuration.Delimiter = "\t";
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var record = csv.GetRecord<myClass>();

                yield return record;
            }
        }

        [HttpGet, Route("PushStreamContent")]
        public HttpResponseMessage PushStreamContent()
        {
            List<DeptModel> lstModel = new List<DeptModel>();

            var param = new DeptModel { DEPTNO = "10"  };

            //using(var con = ConnectionFactory.OrclConnection())
            //{
            //    lstModel = con.Query<DeptModel>(QueryString.Dept_Select, param).ToList();
            //}

            HttpResponseMessage response = Request.CreateResponse();
            //response.Headers.TransferEncodingChunked = true;
            StringBuilder a = new StringBuilder();
            MemoryStream ms = new MemoryStream();

            response.Content = new PushStreamContent( async (stream, content, context) =>
            {
                string filePath = @"E:\NAS_SAMPLE\";
                string fileName = "big_data_1.csv";
                string fullPath = filePath + fileName;

                // TextReader reader = File.OpenText(fullPath);

                //CsvReader csv = new CsvReader(reader);
                //csv.Configuration.Delimiter = "\t";
                //csv.Read();
                //csv.ReadHeader();

                //System.Diagnostics.Debug.WriteLine("csv.ReadHeader : " + w.ElapsedMilliseconds);
                var w = System.Diagnostics.Stopwatch.StartNew();

                char[] buffer = new char[1024*1024*100];
                int n = 0;
                using (StreamReader reader = File.OpenText(fullPath))
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    while (reader.Peek() > -1)
                    {
                        //var record = csv.GetRecord<myClass>();
                        //await sw.WriteLineAsync(await reader.ReadLineAsync());

                        n = await reader.ReadAsync(buffer, 0, buffer.Length);
                        await sw.WriteAsync(buffer, 0, n);
                        await sw.FlushAsync();
                        System.Diagnostics.Debug.WriteLine("sw end : " + w.ElapsedMilliseconds);
                    }

                    await sw.FlushAsync();
                    System.Diagnostics.Debug.WriteLine("while sw end : " + w.ElapsedMilliseconds);

                    buffer = null;
                }
            });
           
            return response;
        }

        [HttpGet, Route("StreamContent")]
        public HttpResponseMessage StreamContent()
        {
            int nMax = 10;

            List<DeptModel> lstModel = new List<DeptModel>();

            var param = new DeptModel { DEPTNO = "10" };

            //using(var con = ConnectionFactory.OrclConnection())
            //{
            //    lstModel = con.Query<DeptModel>(QueryString.Dept_Select, param).ToList();
            //}

            HttpResponseMessage response = Request.CreateResponse();
            //response.Headers.TransferEncodingChunked = true;
            StringBuilder a = new StringBuilder();
            MemoryStream ms = new MemoryStream();

            //StreamWriter sw = new StreamWriter(ms);
            //sw.Write("a");
            //sw.Flush();
            //ms.Seek(0, SeekOrigin.Begin);

            //response.Content = new StreamContent(ms);

            string filePath = @"E:\NAS_SAMPLE\";
            string fileName = "big_data_1.csv";
            string fullPath = filePath + fileName;

            TextReader reader = File.OpenText(fullPath);

            var w = System.Diagnostics.Stopwatch.StartNew();

            try
            {


                CsvReader csv = new CsvReader(reader);
                csv.Configuration.Delimiter = "\t";
                csv.Read();
                csv.ReadHeader();

                System.Diagnostics.Debug.WriteLine("csv.ReadHeader : " + w.ElapsedMilliseconds);

                StreamWriter sw = new StreamWriter(ms);
                List<myClass> lstMyclass = new List<myClass>();

                while (csv.Read())
                {
                    var record = csv.GetRecord<myClass>();

                    if (record.column1.Equals("a"))
                    {
                        lstMyclass.Add(record);
                    }

                }
                //fs2.Close();
                csv.Dispose();

                try
                { 
               // var aa = csv.GetRecords<myClass>().ToList();
                System.Diagnostics.Debug.WriteLine("lstMyclass add end : " + w.ElapsedMilliseconds);

                    //string bb = 
                    //System.Diagnostics.Debug.WriteLine("ser end : " + w.ElapsedMilliseconds);

                    //JsonConvert.DeserializeObject<List<myClass>>(bb);
                    //System.Diagnostics.Debug.WriteLine("des end : " + w.ElapsedMilliseconds);

                    //System.Diagnostics.Debug.WriteLine("while : " + w.ElapsedMilliseconds);
                    string aaaa = JsonConvert.SerializeObject(lstMyclass, Formatting.None);
                    System.Diagnostics.Debug.WriteLine("json end : " + w.ElapsedMilliseconds);
                    sw.Write(aaaa);
                sw.Flush();
                System.Diagnostics.Debug.WriteLine("flush end : " + w.ElapsedMilliseconds);
                    lstMyclass.Clear();

                System.Diagnostics.Debug.WriteLine("lst clear end : " + w.ElapsedMilliseconds);
                    //response.Content = new StreamContent(File.Open(filePath, FileMode.Open));

                    //sw.Flush();
                    System.Diagnostics.Debug.WriteLine("write end: " + w.ElapsedMilliseconds);
                ms.Seek(0, SeekOrigin.Begin);
                response.Content = new StreamContent(ms, 1024);
                System.Diagnostics.Debug.WriteLine("StreamContent end : " + w.ElapsedMilliseconds);
                }catch(Exception ex)
                {
                    throw;
                }

            }
            catch
            {
                ms.Close();
            }

            return response;

            //try
            //{
            //    //using (StreamWriter sw = new StreamWriter(stream))
            //    //{
            //    //sw.NewLine = Environment.NewLine;
            //        StreamWriter sw = new StreamWriter(ms);

            //        for (int i = 0; i < nMax; i++)
            //        {
            //            //a = a + i.ToString();
            //            //sw. .Write((Convert.ToString(i)));
            //            //sw.Flush();StreamContent

            //            //if (i % 10 == 0 && i > 10)
            //            //{
            //            //    await sw.WriteLineAsync();
            //            //    await sw.FlushAsync();
            //            //    //a = string.Empty;
            //            //}

            //            sw.WriteLine(Convert.ToString(i));


            //            //await stream.FlushAsync();

            //            sw.Flush();

            //            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            //            //response.Content.Headers.ContentLength = ms.Length;
            //        }


            //response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            //response.Content.Headers.ContentDisposition.FileName = "aaa";


            //}
            //}
            //catch
            //{
            //    throw;
            //}
            //finally
            //{
            //    //ms.Close();
            //}

        }
    }


    public class Handler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = base.SendAsync(request, cancellationToken);

            //response.Result.Headers.TransferEncodingChunked = true;

            return response;
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
