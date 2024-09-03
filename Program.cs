
using MongoDB.Driver;
using MongoDB.Bson;

var MongdbUrl = "mongodb+srv://hoseinseyedinejad:FallOf68@myfirstcluster.90tholj.mongodb.net/?retryWrites=true&w=majority&appName=MyFirstCluster";
var client = new MongoClient(MongdbUrl);
var database = client.GetDatabase("blog");

#region CRUD Using Bson Document
#region using BsonDocument
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
#endregion

#region using cSharpClasses
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
#endregion
#endregion

#region Create new Collection and using of transaction in multiple updates
//database.CreateCollection("LikeShares");

var likeSharesInCSharpClass = database.GetCollection<LikeShare>("LikeShares");
// using (var session = client.StartSession())
// {
//     var fromId = "668e978a8378966655008f26";
//     var toId = "668ea2e19948e39b538f89f5";

//     var shareAmount = 375;

//     var fromPostResult = postsInCSharpClass.Find(x => x.Id == fromId).FirstOrDefault();
//     var toPostResult = postsInCSharpClass.Find(x => x.Id == toId).FirstOrDefault();

//     var share = new LikeShare
//     {
//         FromPost = "668e978a8378966655008f26",
//         ToPost = "668ea2e19948e39b538f89f5",
//         sharedAmount = shareAmount
//     };

//     var fromPostFilter = Builders<BlogPost>.Filter.Eq(x => x.Id, fromId);
//     var toPostFilter = Builders<BlogPost>.Filter.Eq(x => x.Id, toId);
//     var fromPostUpdate = Builders<BlogPost>.Update.Set(x => x.Likes, fromPostResult.Likes - shareAmount);
//     var toPostUpdate = Builders<BlogPost>.Update.Set(x => x.Likes, toPostResult.Likes + shareAmount);
//     var fromPostUpdateResult = postsInCSharpClass.UpdateOne(fromPostFilter, fromPostUpdate);
//     var toPostUpdateResult = postsInCSharpClass.UpdateOne(toPostFilter, toPostUpdate);
//     likeSharesInCSharpClass.InsertOne(share);


//     Console.WriteLine(JsonSerializer.Serialize(fromPostUpdateResult));
//     Console.WriteLine(JsonSerializer.Serialize(toPostUpdateResult));
//     Console.WriteLine(JsonSerializer.Serialize(likeSharesInCSharpClass.Find(_ => true).FirstOrDefault()));
// }
#endregion

#region using aggregate pipleline
#region using BsonDocument

// var matchStage = Builders<BsonDocument>.Filter.Lte("likes", 10);

// var aggregate = postsInBsonDoc.Aggregate().Match(matchStage);

// foreach (var post in aggregate.ToList())
// {
//     Console.WriteLine(post["likes"]);
// }
#endregion

#region using c#
//match stage
var matchStage = Builders<BlogPost>.Filter.Lte(x => x.Likes, 10);

var aggregate = postsInCSharpClass.Aggregate().Match(matchStage);

foreach (var post in aggregate.ToList())
{
    Console.WriteLine(post.Likes);
}

//match and group stage
var groupAggregate = postsInCSharpClass.Aggregate().Match(matchStage).Group(x => x.Category, r => new { category = r.Key, total = r.Sum(a => 1) });

foreach (var group in groupAggregate.ToList())
{
    Console.WriteLine(group);
}

//sort stage
var projectionStage = Builders<BlogPost>.Projection.Expression(p =>
    new
    {
        Title = p.Title,
        Likes = p.Likes
    });

//var sortAggregate = postsInCSharpClass.Aggregate().SortByDescending(x => x.Likes).Project(p => new { p.Title, p.Likes });
var sortAggregate = postsInCSharpClass.Aggregate().SortByDescending(x => x.Likes).Project(projectionStage);
var sortedProjections = sortAggregate.ToList();
foreach (var sp in sortedProjections)
{
    Console.WriteLine(sp);
}
#endregion
#endregion



//change 1
//change 2
//change 3
//change 4
//change 5
//change 6
//change 8
//change 9


//comment1
//comment2