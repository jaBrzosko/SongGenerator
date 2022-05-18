# SongGenerator
-----
General info
-----
The above project is a simple API Client that generates random words using API generator and after that finds random recordings that contains those words.
Usage of said project is rather straight forward, you just have to type the number of words you would like to generate.
By deafault it has to be a number between 5 and 20, but it can be changed in App.config file. 

IMPORTANT NOTE!
Due to MusicBrainz policy there are some limitations to how many requests an aplication can make per second. Due to that requesting process has to be artificially slowed down to 1 request per second. API sometimes works with lower time delay, but it's not faulty proof then and can end with denial of service by the host.

------
Build Instructions
-----
First you need to make sure you have .NET Core downloaded (head to https://www.microsoft.com/net/learn/get-started-with-dotnet-tutorial for download)
Preferably you would need .NET Core 5.0 as this was a build version.

Then download ZIP of the above repository and extract it on your computer.
Head to said folder and open command prompt here.
Next type following commands:
1) dotnet restore
2) dotnet build -c release

Now either find SongRequest.exe in bin folder or try to type:
./bin/Release/net5.0/SongGenerator.exe
