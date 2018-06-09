# GPStudio

A tool that presents an accessible interface for Genetic Programming.

This is a project I started out of personal interest back in early 2014.  For several years I worked on it, eventually releasing it as a commercial product.  It was a terrific learning experience developing the application, both in terms of learning about Genetic Programming and building my skills in C# and .NET.  During 2008 I decided to stop working on the project, as my attention turned towards earning a PhD in Computer Science, in distributed systems.  The code has sat silent in a private repo since then.  Recently I've decided to make the code publicly available to allow others to possibly learn from what I had done all those years ago.

I don't have any plans to return to working with the project in any serious way.  If anyone is interested in working on it, I'm very happy to aid in the creation of issues and take pull requests based on those issue.

## Areas for Improvement

**DISCLAIMER:** I wrote this a long time ago!!  C# and the .NET framework have made significant improvements in that time.  Additionally, my personal software development skills are dramatically better than at the time I wrote this code.  Keep this in mind while looking at the code.  I **know** it can be better.

* The database using an MS Access file.  This is probably the highest priority, should be changed to something else, perhaps sqlite.
* Update the use of C# to take advantage of things like auto properties, LINQ (where appropriate), etc.
* User Interface
  * Replace the diagramming control.
  * Replace the charting control.
  * Consider a full re-write using WPF or maybe something more dramatic like a web front-end.
  * Related to above: Consider re-thinking the way data and projects are stored.  Maybe us a "project file" approach and put each project into its own file, rather than a single DB that contains all data and projects.
* Improve the multi-core utilization.  Right now it does use multiple cores, but it should be able to be quite dramatically improved.
* Reconsider the use of .NET Remoting for distributed computing.  This is written a long time ago, possibly much better approaches to take now.

# Building The Project

The project is written in C# and set to use Microsoft's Visual Studio 2017.

Building the project is as easy as loading the GPDevelopment.sln file in Visual Studio and then doing a full build of the solution.

# Running The Application

There are three key pieces to the application:

1. The GPStudio.exe executable (and related files) that provides the user interface.
2. The GPServer.exe executable (and related files) that provides the genetic programming modeling engine.
3. A database that stores the training and validation data, along with the project configurations and modeling results.

The GPServer.exe is designed to be run from the command line or as a Windows service.  The code, as provided, builds the server to be run from the command line, but all the code to build and run it as a Windows service is still there.

## Steps to Running

1.  Build the solution

When the solution is built, the GPStudio and GPServer bin/Release folders contain all the executable and configuration files necessary to run.

2.  Prepare a database

Make a copy of the 'GPStudio - Empty.mdb' DB file (located in the /database folder) and place it in a folder of your choosing.  Alternatively, you can make a copy of the 'GPStudio - User Guide.mdb' file, which contains all of the projects and results used as part of the User Guide document.

3.  Run the GPServer

Open a command shell and move into the GPServer/bin/Release folder (assuming you want to run the Release code).  Inside the command shell run the GPServer.exe file, without any command line parameters.

4.  Modify the GPStudio configuration file

The file 'GPStudio.exe.config' file needs to be modified to identify the location of the database.  Open this file in a text editor and modify the 'GPDatabase' entry with the correct path and filename to where you have located the database to use.  The path can either be absolute or relative to the location of the 'GPStudio.exe' file.  For example, if you copy the database into the same folder as the 'GPStudio.exe' file, no path is necessary, only the name of the database file is needed.

4.  Run the GPStudio application

If server is running and the database configuration correctly, the application will start.  From this point, refer to the User Guide to learn how to use the application.
