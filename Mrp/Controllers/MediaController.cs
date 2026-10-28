using Mrp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Mrp.Controllers
{
    public class MediaController
    {
        public MediaService mediaService;

        public MediaController(MediaService mediaService)
        {
            this.mediaService = mediaService;
        }

        public string GetMediaJson()
        {
            var mediaList = mediaService.GetAllMedia();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(mediaList, options);
            return json;
        }


        public string GetMediaByIdJson(Guid id)
        {
            var mediaList = mediaService.GetMediaById(id);
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(mediaList, options);
            return json;
        }
    }
    }
