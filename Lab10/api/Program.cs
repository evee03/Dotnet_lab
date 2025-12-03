using api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<Censor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseFileServer();

app.MapPost(
        "/censor",
        async (string text, Censor censor, HttpClient client) =>
        {
            try
            {
                censor.Blacklist = await client.GetFromJsonAsync<List<string>>(
                    "http://localhost:5153/api/blacklist"
                ) ?? [];
            }
            catch
            {
            }

            return censor.CensorText(text);
        }
    )
    .WithName("Censor");

app.Run();