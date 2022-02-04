let connection = "";
let connectionUrl = "";
const JOINEDGAME = "JoinedGame";
const SUBMITGUESS = "SubmitGuess";
const SUBMITANSWERRES = "UserSubmitAnswerEvent";

async function start(){
    try{
        await connection.start();
        console.log("SignalR Connectioned");
    }catch(err){
        console.log(err);
        setTimeout(start, 5000);
    }
}


function ConnectApp(){
    inputUrl = document.getElementById("ConnectionUrl").value;
        if(inputUrl.length <= 0){
            document.getElementById("ErrorMsg").innerText = "Kindly Enter a valid Url";
        }else{
            connectionUrl = inputUrl;
        }

    connection = new signalR.HubConnectionBuilder()
        .withUrl(connectionUrl)
        .configureLogging(signalR.LogLevel.Information)
        .build();
    
    start();

   

    connection.on(JOINEDGAME, (message)=> {
        console.log(message);

        document.getElementById("userJoined").innerText = "🧔" + message;
    });

    
connection.on(SUBMITANSWERRES, (message) => {
    console.log(message);
    document.getElementById("resultMsg").innerText =  "😉"+ message;
});
    



    document.getElementById("ConnectionUrl").disabled = true;
    document.getElementById("ConnectButton").disabled = true;

}


async function SubmitAnswer(){
    let userInput = document.getElementById("userGuess").value;
    if(isNaN(userInput)){
        document.getElementById("ErrorMsgResult").innerText = "Kindly Enter a valid Number";
    }

    await connection.invoke(SUBMITGUESS,userInput);

  
}








// connection.onclose(async() => {
//     await start();
// });



