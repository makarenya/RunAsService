# What is RunAsService

During a search for a version control system to replace Visual Source Safe, I
found out about SubVersion.  
SubVersion seems to be a good enough VCS replacement, so I downloaded a code
snapshot, build it and started the svnserve.exe program (the subversion server
instance). I got the whole thing up and running in no-time.  
But what struck me about it, was that I wasn't able to start the server as a
windows service.

There's a whole group of software out there for which it would be very handy if
they could be started as a windows service. Think for instance about JINI
services as written in java. Or other standalone java applications that have no
user interface, but supply some kind of service for other applications.

This list is not only restricted to java applications, every application that
just needs to be there without a user being logged in, and start it by hand (or
by adding it to the startup group), should be started as a service to allow it
to start automatically when the machine is turned on.

Thinking this over, the idea for RunAsService service was born.

### What should it do

The basic requirements for this application are as follows:

- RunAsService should be able to start one or more applications as a
  Windows Service.  
  Since it is relatively easy to create a windows device using the
  Microsoft.Net framework, I choose to write it in C#.
- Configuration should be made easy and centralized.    
  Ms.Net applications can be configured using the app.config xml file.
- Log4Net should be used for debug and error logging.    
  I learned to appreciate Log4J as logging tool and debugging aid, so to me its
  just commen sence to use its Ms.Net counterpart.

This list is all but exhaustive, its just the goal for the first release.

Since I think that fellow developers can also, and should, benefit from this I
immediately decided it was going to be an open source project.  
If anyone out there likes to join this project, feel free to
[let me know](mailto:tumblindice@users.sourceforge.net)!
					
The installation and configuration guide is described [here](Manual.html)