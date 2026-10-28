using Mrp.Models;
using Mrp.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mrp.Services
{
    public class MediaService
    {
        MediaRepository MediaRepository;
        public MediaService(MediaRepository mediaRepository)
        {
            this.MediaRepository = mediaRepository;
        }

        public List<MediaModel> GetAllMedia()
        {
            return MediaRepository.GetAllMedia();
        }
      }
    }
