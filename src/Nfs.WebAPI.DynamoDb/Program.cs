/*--****************************************************************************
 --* Project Name    : Nfs.WebAPI.DynamoDb
 --* Reference       : Amazon.DynamoDBv2
 --*                   Amazon.DynamoDBv2.DataModel
 --*                   Amazon.Runtime
 --* Description     : Program
 --* Configuration Record
 --* Review            Ver  Author           Date      Cr       Comments
 --* 001               001  A HATKAR         20/06/24  CR-XXXXX Original
 --****************************************************************************/
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add aws dynamoDb services
var accessKey = builder.Configuration.GetSection("AWS:AccessKey").Value;
var secretKey = builder.Configuration.GetSection("AWS:SecretKey").Value;
var credentials = new BasicAWSCredentials(accessKey, secretKey);
var config = new AmazonDynamoDBConfig()
{
    RegionEndpoint = Amazon.RegionEndpoint.APSoutheast2
};
var client = new AmazonDynamoDBClient(credentials, config);
builder.Services.AddSingleton<IAmazonDynamoDB>(client);
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseAuthorization();
app.MapControllers();
await app.RunAsync();
