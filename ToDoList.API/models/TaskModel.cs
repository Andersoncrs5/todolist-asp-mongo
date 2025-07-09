using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoList.API.models
{
    public class TaskModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("Name")]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; } = string.Empty;

        [BsonElement("Description")]
        [BsonRepresentation(BsonType.String)]
        public string Description { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.Boolean)]
        public bool IsComplete { get; set; } = false;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? UpdatedAt { get; set; } 

    }
}