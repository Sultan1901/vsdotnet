using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace vsdotnet.Entities
{
	public class Customer
	{
		[BsonId]
		[BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
        [BsonElement("customer_name"), BsonRepresentation(BsonType.String)]
        public string? CustomerName { set; get; }
        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public string? Email { set; get; }
	}
}

