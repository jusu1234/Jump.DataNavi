﻿using System.Web.Http;
using Swashbuckle.Application;

namespace WebApiStarter.Template1
{
    /// <summary>
    /// Represent Swagger configuration.
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Configures Swagger API 
        /// </summary>
        /// <param name="configuration">Instance of <see cref="HttpConfiguration"/>.</param>
        public static void Configure(HttpConfiguration configuration)
        {
            configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "My.Api");
                    c.PrettyPrint();
                    // Include if you want to setup XML comments.
                    //c.IncludeXmlComments(() => new XPathDocument(GetXmlDocumentationPath()));
                })
                .EnableSwaggerUi(c => { });
        }
    }
}