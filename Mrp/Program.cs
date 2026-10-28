using Mrp.Repositories;
using Mrp.Services;
using Mrp.Controllers;
using Npgsql;
using System.Net;

MediaController mediaController = new MediaController(new MediaService(new MediaRepository()));

using var listener = new HttpListener();
listener.Prefixes.Add("http://localhost:8002/");
listener.Start();

Console.WriteLine("Listening on port 8002");

while (true)
{
    HttpListenerContext context = listener.GetContext();
    using HttpListenerResponse res = context.Response;

    if(context.Request.Url.AbsolutePath == "/media" && context.Request.HttpMethod == "GET" )
    {
        string json = mediaController.GetMediaJson();
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);
        context.Response.ContentLength64 = buffer.Length;
        context.Response.ContentType = "application/json";
        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
    }

}