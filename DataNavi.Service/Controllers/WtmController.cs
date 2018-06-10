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

namespace DataNavi.Service.Controllers
{
    /// <summary>
    /// Represents test controller that should be removed.
    /// </summary>
    [RoutePrefix("api/wtm")]
    public class WtmController : ApiController
    {
        [HttpGet, Route("GetEnum")]
        public IEnumerable<ReturnModel> GetEnum()
        {
            StringBuilder sb = new StringBuilder();
            int nMax = 1000;

            // An example of returning large number of objects
            foreach (var i in Enumerable.Range(0, nMax))
            {
                yield return new ReturnModel() { SequenceNumber = i, ID = Guid.NewGuid() };
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
            HttpResponseMessage response = Request.CreateResponse();
            //response.Headers.TransferEncodingChunked = true;

            response.Content = new PushStreamContent( async (stream, content, context) =>
            {
                try
                {
                    foreach (var i in Enumerable.Range(0, 10000000))
                    {
                        var buffer = Encoding.UTF8.GetBytes(i.ToString() + Environment.NewLine);
                            await stream.WriteAsync(buffer, 0, buffer.Length);
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

    }


    public class Handler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = base.SendAsync(request, cancellationToken);

            response.Result.Headers.TransferEncodingChunked = true;

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
