//using Microsoft.AspNetCore.Mvc.ApiExplorer;
//using Microsoft.Extensions.Options;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace NationalParkAPI
//{
//    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
//    {
//        private readonly IApiVersionDescriptionProvider provider;

//        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;


//        public void Configure(SwaggerGenOptions options)
//        {
//            foreach (var desc in provider.ApiVersionDescriptions)
//            {
//                options.SwaggerDoc(
//                    desc.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
//                    {
//                        Title = $"National Park{desc.ApiVersion}",
//                        Version = desc.ApiVersion.ToString(),
//                    });
//            }


//            //options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
//            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
//            {
//                Description =
//                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then paste the token",
//                Name = "Authorization",
//                BearerFormat = "JWT",
//                In = ParameterLocation.Header,
//                Type = SecuritySchemeType.ApiKey,
//                Scheme = "Bearer"

//            });

//            //options.AddSecurityRequirement(new OpenApiSecurityRequirement()
//            //{
//            //    {
//            //        new OpenApiSecurityScheme
//            //        {
//            //            Reference = new OpenApiReference
//            //            {
//            //                Type = ReferenceType.SecurityScheme,
//            //                Id = "Bearer"
//            //            }
//            //            //Scheme = "oauth2",
//            //            //Name = "Bearer",
//            //            //In = ParameterLocation.Header,
//            //        },
//            //        //new List<string>()
//            //        new String[]{}
//            //    }
//            //});

//            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
//            {
//                {
//                    new OpenApiSecurityScheme
//                    {
//                        Reference = new OpenApiReference
//                        {
//                            Type = ReferenceType.SecurityScheme,
//                            Id = "Bearer"
//                        },
//                        Scheme = "oauth2",
//                        Name = "Bearer",
//                        In = ParameterLocation.Header,
//                    },
//                    new List<string>()
//                    //new String[]{}
//                }
//            });
//        }
//    }
//}
