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

        public List<MediaModel> GetMediaById(Guid id)
        {
            return MediaRepository.GetMediaById(id);
        }

        public void AddMedia(MediaModel media)
        {
            MediaRepository.AddMedia(media);
        }

        public void UpdateMediaById(Guid id, MediaModel media)
        {
            MediaRepository.UpdateMediaById(id, media);
        }

        public void DeleteMediaById(Guid id)
        {
            MediaRepository.DeleteMediaById(id);
        }
    }
    }
