using MongoDB.Bson;
using MongoDB.Driver;

namespace Uniceps.app.Services
{
    public class MongoUserDto
    {
        public string? Email { get; set; }
        public string? MongoId { get; set; }
    }
    public class MongoProfileDto
    {
        public string? Name { get; set; }
        public string? MongoId { get; set; }
        public string? Phone { get; set; }
        public string? GenderMale { get; set; }
    }
    public class MongoGymDto
    {
        public string? Name { get; set; }
        public string? MongoId { get; set; }
        public string? Phone { get; set; }
        public string? OwnerName { get; set; }
        public string? Telephone { get; set; }
        public string? Address { get; set; }
    }
    public class MongoDbService
    {
        private readonly IMongoCollection<BsonDocument> _userCollection;
        private readonly IMongoCollection<BsonDocument> _profileCollection;
        private readonly IConfiguration _configuration;
        public MongoDbService(IConfiguration configuration)
        {
            _configuration = configuration;

            var settings = MongoClientSettings.FromConnectionString(_configuration["MongoDbConnection:DB_URL"]);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(_configuration["MongoDbConnection:DB_NAME"]);
            _userCollection = database.GetCollection<BsonDocument>("Users");
            _profileCollection = database.GetCollection<BsonDocument>("profiles");
        }
        public List<MongoUserDto> GetUsers()
        {
            var mongoDocuments = _userCollection.Find(FilterDefinition<BsonDocument>.Empty).ToList();
            var users = new List<MongoUserDto>();

            foreach (var doc in mongoDocuments)
            {
                users.Add(new MongoUserDto
                {
                    MongoId = doc["_id"].ToString(),
                    Email = doc["email"].AsString
                });
            }

            return users;
        }
        public List<MongoProfileDto> GetProfiles()
        {
            var mongoDocuments = _profileCollection.Find(FilterDefinition<BsonDocument>.Empty).ToList();
            var users = new List<MongoProfileDto>();

            foreach (var doc in mongoDocuments)
            {
                users.Add(new MongoProfileDto
                {
                    Name = doc["full_name"].ToString(),
                    MongoId = doc["uid"].ToString(),
                    Phone = doc["phone"].ToString(),
                    GenderMale = doc["gender_male"].ToString(),
                });
            }

            return users;
        }
       
    }
}
