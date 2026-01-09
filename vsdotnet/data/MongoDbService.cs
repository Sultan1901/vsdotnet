using MongoDB.Driver;
namespace vsdotnet.data
{
	public class MongoDbService
		

	{
		private readonly IConfiguration _configuration;
        private readonly IMongoDatabase ?  _database;
		public MongoDbService(IConfiguration configuration)
		{
			_configuration = configuration;
			var connectionString = _configuration.GetConnectionString("DbConnection");
			var mongolUrl = MongoUrl.Create(connectionString);
			var mongolClient = new MongoClient(mongolUrl);
			_database = mongolClient.GetDatabase(mongolUrl.DatabaseName);
            

        }
		public IMongoDatabase? Database =>_database;
    }
}

