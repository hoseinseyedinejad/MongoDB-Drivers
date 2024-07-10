using MongoDB.Bson.Serialization.Attributes;

public class LikeShare{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("fromPost")]
    public string FromPost { get; set; }

    [BsonElement("toPost")]
    public string ToPost { get; set; }

    [BsonElement("sharedAmount")]
    public int sharedAmount { get; set; }
}