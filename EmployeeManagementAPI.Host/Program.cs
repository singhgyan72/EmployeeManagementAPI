using EmployeeManagementAPI.Contracts;
using EmployeeManagementAPI.Controllers.ActionFilters;
using EmployeeManagementAPI.Host;
using EmployeeManagementAPI.Host.Extensions;
using EmployeeManagementAPI.Services.DataShaping;
using EmployeeManagementAPI.SharedResources.DTO;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;

var builder = WebApplication.CreateBuilder(args);

//Configuring logger service for logging messages
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureValidationFilter();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
builder.Services.ConfigureVersioning();
//builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureOutputCaching();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureSwagger();

//we are suppressing a default model state validation that is implemented due to
//the existence of the [ApiController] attribute in all API controllers.
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

//builder.Services.AddScoped<ValidateMediaTypeAttribute>();

//This method registers only the controllers in IServiceCollection and not Views or Pages
//because they are not required in the Web API, app will find all of the controllers
//inside of the EmployeeManagementAPI.Controllers project and configure them with the framework.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(EmployeeManagementAPI.Controllers.AssemblyReference).Assembly);

builder.Services.AddCustomMediaTypes();

var app = builder.Build();

//extract the ILoggerManager service after the var app = builder.Build() code line
//because the Build method builds the WebApplication and
//registers all the services added with IOC.

//var logger = app.Services.GetRequiredService<ILoggerManager>();
//app.ConfigureExceptionHandler(logger);

app.UseExceptionHandler(opt => { });

// Configure the HTTP request pipeline.

//will add middleware for using HSTS, which adds the Strict-Transport-Security header.
if (app.Environment.IsProduction())
    app.UseHsts();

//used to add the middleware for the redirection from HTTP to HTTPS.
app.UseHttpsRedirection();

//enables using static files for the request.
//If we don’t set a path to the static files directory,
//it will use a wwwroot folder in our project by default.
app.UseStaticFiles();

//will forward proxy headers to the current request.
//This will help us during application deployment.
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

app.UseRateLimiter();
app.UseCors("CorsPolicy");
//Microsoft recommends having UseCors before UseResponseCaching
//app.UseResponseCaching();
app.UseOutputCache();

app.UseAuthentication();
//adds the authorization middleware to the specified IApplicationBuilder to enable authorization capabilities.
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API v1");
    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Employee Management API v2");
});

//adds the endpoints from controller actions to the IEndpointRouteBuilder
app.MapControllers();

//4. Run Middleware: This terminates the pipeline
app.Run();