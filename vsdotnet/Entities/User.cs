using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


public class User
{
	[BsonId]
	[BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	[BsonElement("username"), BsonRepresentation(BsonType.String)]
	public string? Username { set; get; }
	[BsonElement("password"), BsonRepresentation(BsonType.String)]
	public string? Password { set; get; }
}


public class AuthRequestDto
{
	public string? Username { set; get; }
	public string? Password { set; get; }
}


public class AuthResponseDto
{
	public string? Token { get; set; }
}



