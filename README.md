# SongGenerator

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

Now either find SongRequest.exe in bin folder or type:
./bin/Release/net5.0/SongGenerator.exe
