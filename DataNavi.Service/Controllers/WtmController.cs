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

namespace DataNavi.Service.Controllers
{
    /// <summary>
    /// Represents test controller that should be removed.
    /// </summary>
    [RoutePrefix("api/wtm")]
    public class WtmController : ApiController
    {
        [HttpGet, Route("GetEnum")]
        public IEnumerable<string> GetEnum()
        {
            //var response = Request.CreateResponse();
            //response.Headers.TransferEncodingChunked = true;

            StringBuilder sb = new StringBuilder();
            int nMax = 99999999;
            // An example of returning large number of objects
            for (int i =0; i < nMax; i++)
            {
                //yield return new ReturnModel() { SequenceNumber = i, ID = Guid.NewGuid() };
                sb.Append(Convert.ToString(i));
                if(i %  1000000 == 0 && i > 1000000)
                { 
                    yield return sb.ToString();
                    sb.Clear();
                }

                //sb.Append(i);

                //if (i == nMax - 1)
                //{
                //    sb.AppendLine();
                //    yield return sb.ToString();
                //    sb.Clear();
                //}
            }
        }

        [HttpGet, Route("PushStreamContent")]
        public HttpResponseMessage PushStreamContent()
        {
            int nMax = 99999999;

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
                try
                {
                    for (int i = 0; i < nMax; i++)
                    {
                        //a = a + i.ToString();
                        //await sw.WriteAsync(('a'));

                        var buffer = Encoding.UTF8.GetBytes(Convert.ToString(i));

                        await ms.WriteAsync(buffer, 0, buffer.Length);

                        if (i > 100000 && i % 100000 == 0)
                        {
                            //await ms.CopyToAsync(stream);
                            //await ms.CopyToAsync(stream, ms.GetBuffer().Length);
                            byte[] aa = ms.GetBuffer();

                            //await ms.FlushAsync();
                            await stream.WriteAsync(aa, 0, aa.Length);

                            ms.Position = 0;
                            ms.SetLength(0);

                            //a.Clear();
                        }

                        //if (i % 10 == 0 && i > 10)
                        //{
                        //    await sw.WriteLineAsync();
                        //    await sw.FlushAsync();
                        //    //a = string.Empty;
                        //}

                        //var buffer = new byte[] {1,2};
                        //await stream.WriteAsync(buffer, 0, buffer.Length);
                        //await stream.FlushAsync();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    stream.Close();
                }
            });
           
            return response;
        }

        [HttpGet, Route("StreamContent")]
        public HttpResponseMessage StreamContent()
        {
            int nMax = 99999999;

            List<DeptModel> lstModel = new List<DeptModel>();

            var param = new DeptModel { DEPTNO = "10" };

            //using(var con = ConnectionFactory.OrclConnection())
            //{
            //    lstModel = con.Query<DeptModel>(QueryString.Dept_Select, param).ToList();
            //}

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Headers.TransferEncodingChunked = true;
            StringBuilder a = new StringBuilder();

            MemoryStream ms = new MemoryStream();
            response.Content = new StreamContent(ms, 1024*1024);


            try
            {
                //using (StreamWriter sw = new StreamWriter(stream))
                //{
                //sw.NewLine = Environment.NewLine;
                    StreamWriter sw = new StreamWriter(ms);

                    for (int i = 0; i < nMax; i++)
                    {
                        //a = a + i.ToString();
                        //sw. .Write((Convert.ToString(i)));
                        //sw.Flush();

                        //if (i % 10 == 0 && i > 10)
                        //{
                        //    await sw.WriteLineAsync();
                        //    await sw.FlushAsync();
                        //    //a = string.Empty;
                        //}

                        sw.Write(Convert.ToString(i));


                        //await stream.FlushAsync();

                        sw.Flush();

                        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                        //response.Content.Headers.ContentLength = ms.Length;
                    }


                //response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                //response.Content.Headers.ContentDisposition.FileName = "aaa";
              

                //}
            }
            catch
            {
                throw;
            }
            finally
            {
                //ms.Close();
            }

            return response;
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
}
