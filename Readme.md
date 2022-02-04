# A simple game. 

Server: Generates a random number and saves it in db. 

Clients: Gues what number was created. 

Server: Tells user whether it is greater or leser. If any of the Client guesses correctly. It tells all client. 

Biggest hurdle: 

1. Setting up the CORS for the server. Took me a while but I finally served the client using Live Server plugin and inserted the url to the Origins to be allowed by the server.


2. Saving Data in the database whilst keeping the connection. 

## For Setup 

Run the server like you would run a web-api. 

Run the client using live server and connect using the url from the web-api


### If your client is servered on another port aside the one in Program.cs in server. You have to update CORS.