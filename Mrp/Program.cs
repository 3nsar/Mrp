using Mrp.Repositories;
using Npgsql;

MediaRepository mediaRepository = new MediaRepository();
var mediaList = mediaRepository.GetAllMedia();
foreach (var media in mediaList)
{
    Console.WriteLine($"ID: {media.ID}");
    Console.WriteLine($"Title: {media.Title}");
    Console.WriteLine($"Description: {media.Description}");
    Console.WriteLine($"Media Type: {media.MediaType}");
    Console.WriteLine($"Release Year: {media.ReleaseYear}");
    Console.WriteLine($"Genre: {media.Genre}");
    Console.WriteLine($"Age Restriction: {media.AgeRestriction}");
    Console.WriteLine($"Average Score: {media.AverageScore}");
    Console.WriteLine($"Favorite Count: {media.FavoriteCount}");
    Console.WriteLine();
}