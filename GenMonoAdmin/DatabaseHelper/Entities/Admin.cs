using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseHelper.Entities
{
    public class Admin
    {
        [BsonId]
        [BsonRequired]
        public string AdminId { get; set; }

        [BsonElement("UserName")]
        [BsonRequired]
        public string UserName { get; set; }

        [BsonElement("HashPassword")]
        [BsonRequired]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string HashPassword { get; set; }

        [BsonElement("DateCreated")]
        [BsonIgnoreIfNull]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string DateCreated { get; set; }
    }
}
