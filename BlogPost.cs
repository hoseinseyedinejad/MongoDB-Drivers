

using MongoDB.Bson.Serialization.Attributes;

public class BlogPost
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("body")]
    public string Body { get; set; }

    [BsonElement("category")]
    public string Category { get; set; }

    [BsonElement("likes")]
    public int Likes { get; set; }

    [BsonElement("tags")]
    public String[] tags { get; set; }

    [BsonElement("date")]
    public string Date { get; set; }
    //comment1
}