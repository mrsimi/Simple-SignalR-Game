namespace server.Models
{
    public class AppManager
    {
        public AppManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly AppDbContext _dbContext;

        public int CreateGuess()
        {
            Random rand = new Random();
            int NumberToGuess = rand.Next(1, 10);
            _dbContext.AppDatas.Add( new AppData{GuessedNumber = NumberToGuess, DateCreated = DateTime.UtcNow, IsCompleted = false});
            _dbContext.SaveChanges();

            return NumberToGuess;
        }

        public long AnyActiveGuess()
        {
            var appData =  _dbContext.AppDatas.FirstOrDefault(m => m.DateCreated.Date == DateTime.UtcNow.Date && m.IsCompleted == false);
            if(appData != null)
            {
                return appData.GuessedNumber;
            }
            else
            {
                return CreateGuess();
            }
        }

        public int UpdateWinCreateNewGuess(string user, long numberGuessed)
        {
            var guessedDbData = _dbContext.AppDatas.FirstOrDefault(m => m.GuessedNumber == numberGuessed && m.DateCreated.Date == DateTime.UtcNow.Date);
            guessedDbData.IsCompleted = true; 
            guessedDbData.CorrectUser = user;

            _dbContext.SaveChanges();

            return CreateGuess();
        }
    }
}