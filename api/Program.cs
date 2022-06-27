using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? 
                                                            throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));



var awsAccessKey = builder.Configuration.GetValue<string>("AWS:Key");
var awsAccessSecret = builder.Configuration.GetValue<string>("AWS:Secret");

var credentials = new BasicAWSCredentials(awsAccessKey, awsAccessSecret);

var config = new AmazonDynamoDBConfig()
{
    RegionEndpoint = RegionEndpoint.USEast1
};

var client = new AmazonDynamoDBClient(credentials, config);

// Get the AWS profile information from configuration providers
AWSOptions awsOptions = builder.Configuration.GetAWSOptions();

// Configure AWS service clients to use these credentials
builder.Services.AddDefaultAWSOptions(awsOptions);
// builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddSingleton<IAmazonDynamoDB>(client);
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();



// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// get CORS urls from appsettings
var corsUrl = builder.Configuration.GetSection("CorsUrl").Get<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins(corsUrl).AllowAnyHeader().AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
