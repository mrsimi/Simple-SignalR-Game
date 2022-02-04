using Microsoft.AspNetCore.SignalR;
using server.Models;

namespace server.Hubs
{
    public class GameHub : Hub
    {
        private readonly AppDbContext  _dbContext; 
        public long NumberToGuess; 
        public const string SUBMITANSWERRES = "UserSubmitAnswerEvent";
        public const string JOINED_GAMED = "JoinedGame";

        private AppManager _appManager;
        public GameHub(AppDbContext dbContext)
        {
           
            _dbContext = dbContext;

            var appData =  _dbContext.AppDatas.FirstOrDefault(m => m.DateCreated.Day == DateTime.UtcNow.Day && m.IsCompleted == false);

            if(appData == null)
            {
                Random rand = new Random();
                NumberToGuess = rand.Next(1, 10);
                _dbContext.AppDatas.Add( new AppData{GuessedNumber = NumberToGuess, DateCreated = DateTime.UtcNow, IsCompleted = false});
                _dbContext.SaveChanges();
            }
            else
            {
                NumberToGuess = appData.GuessedNumber;
            }
            //NumberToGuess = _appManager.AnyActiveGuess();
           
            Console.WriteLine($"------{this.NumberToGuess} was the guessed number");
        }

        public Task SubmitGuess(string input)
        {
            string generalMsg = $"A user just made a wrong guess";
            //Console.WriteLine(outMsg);
            long comparerInput; 
            long.TryParse(input, out comparerInput);

            if(comparerInput == NumberToGuess)
            {
                string generalCorrectMsg = $"A User with connectionId {Context.ConnectionId} has guessed it correctly. Answer is {NumberToGuess}";
                string privateMsg = "You have successfully guessed the number";


                var guessedDbData = _dbContext.AppDatas.FirstOrDefault(m => m.GuessedNumber == NumberToGuess && m.DateCreated.Day == DateTime.UtcNow.Day);
                guessedDbData.IsCompleted = true; 
                guessedDbData.CorrectUser = Context.ConnectionId;
                //_dbContext.SaveChanges();

                Random rand = new Random();
                NumberToGuess = rand.Next(1, 10);
                _dbContext.AppDatas.Add( new AppData{GuessedNumber = NumberToGuess, DateCreated = DateTime.UtcNow, IsCompleted = false});
                _dbContext.SaveChanges();
                   

                Console.WriteLine("**new guess"+ NumberToGuess);

                Clients.Caller.SendAsync(SUBMITANSWERRES,  privateMsg);
                return Clients.AllExcept(new List<string> {Context.ConnectionId}).SendAsync(SUBMITANSWERRES, generalCorrectMsg);
            }
            else
            {
              if(comparerInput > NumberToGuess)
                    {
                        string userMsg = "You guess is greater than the number";
                        Clients.Caller.SendAsync(SUBMITANSWERRES, userMsg);
                        
                    }
                    if(comparerInput < NumberToGuess)
                    {
                        string userMsg = "You guess is lesser than the number";
                        Clients.Caller.SendAsync(SUBMITANSWERRES, userMsg);
                       // return Clients.AllExcept(new List<string> {Context.ConnectionId}).SendAsync(SUBMITANSWERRES, generalMsg);
                    }   
            }


            return Clients.AllExcept(new List<string> {Context.ConnectionId}).SendAsync(SUBMITANSWERRES, generalMsg);
        }

        public override async Task OnConnectedAsync()
        {
            string welcomeMsg = $"User with connectionId {Context.ConnectionId} just joined the game"; 
            string welcomeMsgPrivate = "You just joined the game";

            await Clients.Caller.SendAsync(JOINED_GAMED, welcomeMsgPrivate);
            await Clients.AllExcept(new List<string> {Context.ConnectionId}).SendAsync(JOINED_GAMED, welcomeMsg);
        }
    }
}