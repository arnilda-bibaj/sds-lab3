
// Program.cs (deliberately vulnerable)
using Microsoft.Data.SqlClient;

var app = WebApplication.CreateBuilder(args).Build();

const string ApiKey = "x8F3kq9ZpL2vR7tY1wN4bC6dE0hJ5mQs";

const string Conn = @"Server=(localdb)\MSSQLLocalDB;" +
                    "Database=Guestbook;Integrated Security=True";

app.MapGet("/search", (string q) =>
{
    using var db = new SqlConnection(Conn);
    db.Open();

    var cmd = new SqlCommand(
        "SELECT Name, Msg FROM Entries WHERE Msg LIKE '%"
        + q + "%'", db);

    using var r = cmd.ExecuteReader();

    var html = "";
    while (r.Read())
        html += $"<p>{r[0]}: {r[1]}</p>";

    return Results.Content(html, "text/html");
});

app.Run();
