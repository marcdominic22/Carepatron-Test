using System.Linq;
using System.Reflection;

using Microsoft.OpenApi.Models;

using Newtonsoft.Json;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Swagger
{
    public class ParameterFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var ignoredProperties = context.MethodInfo.GetParameters()
                .SelectMany(p => p.ParameterType.GetProperties()
                .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() is not null))
                .Select(p => p.Name.ToLower());

            operation.Parameters = operation.Parameters
                .Where(p => p.In is not ParameterLocation.Query || (p.In is ParameterLocation.Query && !ignoredProperties.Contains(p.Name.ToLower())))
                .Where(x => x.Schema.Type is not null)
                .ToList();
        }
    }
}