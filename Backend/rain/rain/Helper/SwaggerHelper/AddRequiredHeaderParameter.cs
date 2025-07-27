using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        if (!operation.Parameters.Any(p => p.Name == "x-userid"))
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-userid",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Description = "User ID passed in request header"
            });
        }
    }
}