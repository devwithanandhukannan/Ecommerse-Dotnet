using System.Text;
using EcommerceApp.Application.Common.Interfaces;
using EcommerceApp.Application.Products.Command;
using EcommerceApp.Infrastructure.Persistence;
using EcommerceApp.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString!));

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>()); // provider =retrieve an instance of ApplicationDbContext from the dependency injection container and register it as an implementation of the IApplicationDbContext interface. This enables other parts of the application to depend on the IApplicationDbContext interface rather than the concrete ApplicationDbContext class, promoting loose coupling and easier testing.

builder.Services.AddScoped<IJwtService, JwtService>(); // This line registers the JwtService implementation of the IJwtService interface with the dependency injection container. It specifies that a new instance of JwtService will be created for each HTTP request (scoped lifetime). This allows the application to use the JWT service for generating and validating tokens.

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateProductCommand>()); // This line adds MediatR to the dependency injection container and registers all the request handlers, notifications, and other MediatR services from the assembly that contains the CreateProductCommand class. This enables the application to use MediatR for handling commands, queries, and events in a decoupled manner.

builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommandValidator).Assembly); // This line registers all the validators from the assembly that contains the CreateProductCommandValidator class with the dependency injection container. It allows the application to automatically discover and use FluentValidation validators for validating requests and commands.

var secretKey = builder.Configuration["Jwt:SecretKey"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization(); //who are allowed to access the api, this will be used to restrict access to certain endpoints based on user roles or claims. It will be used in the controllers to restrict access to certain endpoints based on user roles or claims.
builder.Services.AddControllers(); // This line adds support for controllers in the application, enabling the use of the Model-View-Controller (MVC) pattern. It allows the application to handle HTTP requests and return responses using controller classes and action methods.
builder.Services.AddEndpointsApiExplorer(); // This line adds support for API endpoint exploration in the application. It enables the generation of OpenAPI (Swagger) documentation for the application's API endpoints, allowing developers to easily discover and test the available endpoints.

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EcommerceApp API", Version = "v1" }); // This line configures the Swagger generator to create an OpenAPI document for the API with the specified title and version. It allows developers to view and interact with the API documentation through a user-friendly interface.
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build(); // This line builds the WebApplication instance using the configured services and middleware. It prepares the application to handle incoming HTTP requests and generate responses based on the defined routes, controllers, and middleware components.

if(app.Environment.IsDevelopment()) // This line checks if the application is running in a development environment. It allows the application to conditionally execute certain code or configurations based on the current environment, such as enabling developer-friendly features or logging.
{
    app.UseSwagger(); // This line enables the Swagger middleware, which generates and serves the OpenAPI (Swagger) documentation for the API. It allows developers to view and interact with the API documentation through a user-friendly interface.
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EcommerceApp API v1")); // This line configures the Swagger UI middleware, which provides a web-based interface for exploring and testing the API endpoints. It specifies the endpoint for the generated OpenAPI document and sets the title for the Swagger UI page.
}

app.UseHttpsRedirection(); // This line enables HTTPS redirection middleware, which automatically redirects incoming HTTP requests to HTTPS. It ensures that all communication between the client and server is encrypted and secure.
app.UseAuthentication(); // This line enables the authentication middleware, which allows the application to authenticate incoming requests based on the configured authentication scheme (e.g., JWT bearer tokens). It ensures that only authenticated users can access protected resources.
app.UseAuthorization(); // This line enables the authorization middleware, which allows the application to enforce access
app.MapControllers(); // This line maps the controller routes to the application's request handling pipeline. It enables the application to route incoming HTTP requests to the appropriate controller action methods based on the defined routes.
app.Run(); // This line starts the application and begins listening for incoming HTTP requests. It blocks