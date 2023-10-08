using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NZWalks.API
{
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
        }
        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        public void Configure(SwaggerGenOptions options)
        {
            var listOfApiVersionDescriptions = _apiVersionDescriptionProvider.ApiVersionDescriptions;
            foreach (var item in listOfApiVersionDescriptions)
            {
                options.SwaggerDoc(item.GroupName, CreateVersionInfo(item));
            }
            //for (int i = 1; i < listOfApiVersionDescriptions.Count(); i++)
            //{
            //    var currentItem = listOfApiVersionDescriptions[i];
            //    //Console.WriteLine("currentItem line 38", currentItem);
            //    options.SwaggerDoc(currentItem.GroupName, CreateVersionInfo(currentItem));
            //}
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
        
            var info = new OpenApiInfo
            {
                Title = "NZ Walks API",
                Version = description.ApiVersion.ToString(),
            };

            return info;
        }
    }
}
