
using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography.X509Certificates;

var MongdbUrl = "mongodb+srv://hoseinseyedinejad:FallOf68@myfirstcluster.90tholj.mongodb.net/?retryWrites=true&w=majority&appName=MyFirstCluster";
var client = new MongoClient(MongdbUrl);
var database = client.GetDatabase("blog");

//CRUD Using Bson Document
var postsInBsonDoc = database.GetCollection<BsonDocument>("posts");
// var newBsonPost = new BsonDocument
// {
//     { "title", "Bson Document Inserted Post"},
//     { "body", "Using Bson Document Inserted Post"},
//     { "category", "Test"},
//     { "date", DateTime.UtcNow.ToString()},
//     { "likes", 336},
// };
// postsInBsonDoc.InsertOne(newBsonPost);
// var filter = Builders<BsonDocument>
//     .Filter
//     .Eq("likes", 336);
// var BsonPost = postsInBsonDoc.Find(filter).FirstOrDefault();
// Console.WriteLine(BsonPost);


// CRUD Using C# Class

//Insert
var postsInCSharpClass = database.GetCollection<BlogPost>("posts");

// var newPost = new BlogPost{
//     Title="Console App Inserted Post",
//     Body="Using .Net Console Application Inserted",
//     Category="Test",
//     Date=DateTime.UtcNow.ToString(),
//     Likes=525,
//     tags=new string[] { "tag1", "tag2", "tag3"}
// };

// postsInCSharpClass.InsertOne(newPost);
// var post = postsInCSharpClass.Find(x => x.Likes == 525).FirstOrDefault();
// Console.WriteLine(JsonSerializer.Serialize(post));


//Update
// var update = Builders<BlogPost>.Update.Set(x => x.Likes, 1375);
// var filter2 = Builders<BlogPost>
//     .Filter
//     .Eq(x => x.Likes, 1372);
// var BsonPost = postsInCSharpClass.Find(x => x.Likes == 1372).FirstOrDefault();
// var updateRes = await postsInCSharpClass.UpdateOneAsync(filter2, update);
// if(updateRes.IsAcknowledged){
//     Console.WriteLine($"Matched:{updateRes.MatchedCount}; Modified:{updateRes.ModifiedCount};");
// }

//Create new Collection
//database.CreateCollection("LikeShares");

var likeSharesInCSharpClass = database.GetCollection<LikeShare>("LikeShares");
// using transaction
using (var session = client.StartSession())
{
    var fromId = "668e978a8378966655008f26";
    var toId = "668ea2e19948e39b538f89f5";

    var shareAmount = 375;

    var fromPostResult = postsInCSharpClass.Find(x => x.Id == fromId).FirstOrDefault();
    var toPostResult = postsInCSharpClass.Find(x => x.Id == toId).FirstOrDefault();

    var share = new LikeShare
    {
        FromPost = "668e978a8378966655008f26",
        ToPost = "668ea2e19948e39b538f89f5",
        sharedAmount = shareAmount
    };

    var fromPostFilter = Builders<BlogPost>.Filter.Eq(x => x.Id, fromId);
    var toPostFilter = Builders<BlogPost>.Filter.Eq(x => x.Id, toId);
    var fromPostUpdate = Builders<BlogPost>.Update.Set(x => x.Likes, fromPostResult.Likes - shareAmount);
    var toPostUpdate = Builders<BlogPost>.Update.Set(x => x.Likes, toPostResult.Likes + shareAmount);
    var fromPostUpdateResult = postsInCSharpClass.UpdateOne(fromPostFilter, fromPostUpdate);
    var toPostUpdateResult = postsInCSharpClass.UpdateOne(toPostFilter, toPostUpdate);
    likeSharesInCSharpClass.InsertOne(share);


    Console.WriteLine(JsonSerializer.Serialize(fromPostUpdateResult));
    Console.WriteLine(JsonSerializer.Serialize(toPostUpdateResult));
    Console.WriteLine(JsonSerializer.Serialize(likeSharesInCSharpClass.Find(_ => true).FirstOrDefault()));
}


