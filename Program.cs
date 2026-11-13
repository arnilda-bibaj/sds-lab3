using System.Net;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Read the API key from configuration, not from source code.
var apiKey = app.Configuration["ApiKey"];

if (string.IsNullOrWhiteSpace(apiKey))
{
    app.Logger.LogWarning("ApiKey is not configured.");
}

const string Conn = @"Server=(localdb)\MSSQLLocalDB;" +
                    "Database=Guestbook;Integrated Security=True";

app.MapGet("/search", (string q) =>
{
    using var db = new SqlConnection(Conn);
    db.Open();

    var cmd = new SqlCommand(
        "SELECT Name, Msg FROM Entries WHERE Msg LIKE @q", db);

    cmd.Parameters.AddWithValue("@q", "%" + q + "%");

    using var r = cmd.ExecuteReader();

    var html = "";
    while (r.Read())
    {
        html += $"<p>{WebUtility.HtmlEncode(r[0].ToString())}: " +
         $"{WebUtility.HtmlEncode(r[1].ToString())}</p>";
    }

    return Results.Content(html, "text/html");
});

app.Run();
