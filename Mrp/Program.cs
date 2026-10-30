using System.Net;
using System.Text.Json;
using Mrp.Controllers;
using Mrp.Repositories;
using Mrp.Services;
using Mrp.Models;
using Npgsql;
MediaController mediaController = new MediaController(new MediaService(new MediaRepository()));
AuthController authController = new AuthController(new AuthService(new UserRepository()));

var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

using var listener = new HttpListener();
listener.Prefixes.Add("http://localhost:8001/");

listener.Start();

Console.WriteLine("Listening on port 8001...");

while (true)
{
    HttpListenerContext context = listener.GetContext();
    using HttpListenerResponse resp = context.Response;
    bool isProtected = context.Request.Url.AbsolutePath.StartsWith("/media");
    if (isProtected && !authController.IsAuthorized(context.Request.Headers["Authorization"]))
    {
        resp.StatusCode = (int)HttpStatusCode.Unauthorized;
        continue;
    }

    if (context.Request.Url.AbsolutePath == "/media" && context.Request.HttpMethod == "GET")
    {
        string json = mediaController.GetMediaJson();
        resp.ContentType = "application/json";
        resp.StatusCode = (int)HttpStatusCode.OK;
        using (var writer = new System.IO.StreamWriter(resp.OutputStream))
        {
            writer.Write(json);
        }
    }
    else if (context.Request.Url.AbsolutePath.StartsWith("/media/") && context.Request.HttpMethod == "GET")
    {
        string idString = context.Request.Url.AbsolutePath.Substring("/media/".Length);
        if (Guid.TryParse(idString, out Guid id))
        {
            string json = mediaController.GetMediaByIdJson(id);
            resp.ContentType = "application/json";
            resp.StatusCode = (int)HttpStatusCode.OK;
            using (var writer = new System.IO.StreamWriter(resp.OutputStream))
            {
                writer.Write(json);
            }
        }
        else
        {
            resp.StatusCode = (int)HttpStatusCode.BadRequest;

        }
    }
    else if (context.Request.Url.AbsolutePath == "/media" && context.Request.HttpMethod == "POST")
    {
        using (var reader = new System.IO.StreamReader(context.Request.InputStream))
        {
            string requestBody = reader.ReadToEnd();
            MediaModel media = JsonSerializer.Deserialize<MediaModel>(requestBody);
            mediaController.AddMedia(media);
            resp.StatusCode = (int)HttpStatusCode.Created;
        }
    }
    else if (context.Request.Url.AbsolutePath.StartsWith("/media/") && context.Request.HttpMethod == "PUT")
    {
        string idString = context.Request.Url.AbsolutePath.Substring("/media/".Length);
        if (Guid.TryParse(idString, out Guid id))
        {
            using (var reader = new System.IO.StreamReader(context.Request.InputStream))
            {
                string requestBody = reader.ReadToEnd();
                MediaModel media = JsonSerializer.Deserialize<MediaModel>(requestBody);
                mediaController.UpdateMediaById(id, media);
                resp.StatusCode = (int)HttpStatusCode.OK;
            }
        }
        else
        {
            resp.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
    else if (context.Request.Url.AbsolutePath.StartsWith("/media/") && context.Request.HttpMethod == "DELETE")
    {
        string idString = context.Request.Url.AbsolutePath.Substring("/media/".Length);
        if (Guid.TryParse(idString, out Guid id))
        {
            mediaController.DeleteMediaById(id);
            resp.StatusCode = (int)HttpStatusCode.OK;
        }
        else
        {
            resp.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
    else if (context.Request.Url.AbsolutePath == "/users/register" && context.Request.HttpMethod == "POST")
    {
        using var reader = new System.IO.StreamReader(context.Request.InputStream);
        var request = JsonSerializer.Deserialize<AuthRequest>(reader.ReadToEnd(), jsonOptions);

        if (request == null)
            resp.StatusCode = (int)HttpStatusCode.BadRequest;
        else if (authController.Register(request))
            resp.StatusCode = (int)HttpStatusCode.Created;
        else
            resp.StatusCode = (int)HttpStatusCode.Conflict;
    }
    else if (context.Request.Url.AbsolutePath == "/users/login" && context.Request.HttpMethod == "POST")
    {
        using var reader = new System.IO.StreamReader(context.Request.InputStream);
        var request = JsonSerializer.Deserialize<AuthRequest>(reader.ReadToEnd(), jsonOptions);

        string? json = request == null ? null : authController.Login(request);
        if (json == null)
        {
            resp.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        else
        {
            resp.ContentType = "application/json";
            resp.StatusCode = (int)HttpStatusCode.OK;
            using var writer = new System.IO.StreamWriter(resp.OutputStream);
            writer.Write(json);
        }
    }

    else
    {
        resp.StatusCode = (int)HttpStatusCode.NotFound;
    }

}



