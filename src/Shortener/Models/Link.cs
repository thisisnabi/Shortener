using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace Shortener.Models;
 
[Collection("links")]
public class Link
{
    public ObjectId Id { get; set; }

    public string ShortenCode { get; set; } = null!;
    public string DestinationUrl { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
   
}
