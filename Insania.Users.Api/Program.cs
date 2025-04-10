using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Serilog;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Services;

using Insania.Users.BusinessLogic;
using Insania.Users.Messages;
using Insania.Users.Middleware;
using Insania.Users.Models.Mapper;
using Insania.Users.Database.Contexts;

//�������� ���������� ��������� ���-����������
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//��������� �������� ���-����������
IServiceCollection services = builder.Services;

//��������� ������������ ���-����������
ConfigurationManager configuration = builder.Configuration;

//�������� ���������� ��� ������
var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["TokenOptions:Key"]!));
var issuer = configuration["TokenOptions:Issuer"];
var audience = configuration["TokenOptions:Audience"];

//���������� ���������� �����������
services
    .AddAuthorizationBuilder()
    .AddPolicy("Bearer", new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes("Bearer")
    .RequireAuthenticatedUser().Build());

//���������� ���������� ��������������
services
    .AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = issuer,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = audience,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = key,
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
        };
    });

//��������� ������������ ��������
services.AddSingleton(_ => configuration); //������������
services.AddScoped<ITransliterationSL, TransliterationSL>(); //������ ��������������
services.AddUsersBL(); //������� ������ � ������-������� � ���� �������������

//���������� ���������� �� � ��������� ��������
services.AddDbContext<UsersContext>(options =>
{
    string connectionString = configuration.GetConnectionString("Users") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
    options.UseNpgsql(connectionString);
}); //�� �������������
services.AddDbContext<LogsApiUsersContext>(options =>
{
    string connectionString = configuration.GetConnectionString("LogsApiUsers") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
    options.UseNpgsql(connectionString);
}); //�� ����� api � ���� �������������

//��������� ������������� ����� ���� � �������
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//���������� ���������� ������������ � �������������� json
services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

//���������� ���������� �����������
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.File(path: configuration["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
    .WriteTo.Debug()
    .CreateLogger();
services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

//���������� ���������� ������������
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Insania API", Version = "v1" });

    var filePath = Path.Combine(AppContext.BaseDirectory, "Insania.Users.Api.xml");
    options.IncludeXmlComments(filePath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "Authentication",
        Description = "����������� �� ����� ����������",
        Scheme = "Bearer"
    });
    options.OperationFilter<AuthenticationRequirementsOperationFilter>();
});

//���������� ������
services.AddCors(options =>
{
    options.AddPolicy("BadPolicy", policyBuilder => policyBuilder
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
    );

    options.DefaultPolicyName = "BadPolicy";
});

//���������� ������������
services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

//���������� ���������� �������������� �������
services.AddAutoMapper(typeof(UsersMappingProfile));

//���������� ���-����������
WebApplication app = builder.Build();

//����������� �������������
app.UseRouting();

//����������� ��������������
app.UseAuthentication();

//����������� �����������
app.UseAuthorization();

//����������� ��������
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Insania API V1");
});

//����������� ������
app.UseCors();

//����������� ������������� ������������
app.MapControllers();

//������ ���-����������
app.Run();