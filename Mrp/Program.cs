using Mrp.Controllers;
using Mrp.Models;
using Mrp.Repositories;
using Mrp.Services;
using Npgsql;
using System.Net;
using System.Text.Json;

MediaController mediaController = new MediaController(new MediaService(new MediaRepository()));

using var listener = new HttpListener();
listener.Prefixes.Add("http://localhost:8002/");
listener.Start();

Console.WriteLine("Listening on port 8002");

while (true)
{
    HttpListenerContext context = listener.GetContext();
    using HttpListenerResponse resp = context.Response;

    if(context.Request.Url.AbsolutePath == "/media" && context.Request.HttpMethod == "GET" )
    {
        string json = mediaController.GetMediaJson();
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);
        context.Response.ContentLength64 = buffer.Length;
        context.Response.ContentType = "application/json";
        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    }else if(context.Request.Url.AbsolutePath.StartsWith("/media/") && context.Request.HttpMethod == "GET")
    {
        string idString = context.Request.Url.AbsolutePath.Substring("/media/".Length);
        if (Guid.TryParse(idString, out Guid id))
        {
            string json = mediaController.GetMediaByIdJson(id);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.ContentType = "application/json";
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
    else if (context.Request.Url.AbsolutePath == "/media" && context.Request.HttpMethod == "POST")
    {
        using (var reader = new System.IO.StreamReader(context.Request.InputStream))
        {
            string requestBody = reader.ReadToEnd();
            MediaModel media = JsonSerializer.Deserialize<MediaModel>(requestBody);
            mediaController.AddMedia(media);
            context.Response.StatusCode = (int)HttpStatusCode.Created;
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
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
    }

}