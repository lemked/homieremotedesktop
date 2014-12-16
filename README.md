Homie Remotedesktop
==================

Homie Remotedesktop is a tool to start and connect a remote host via internet.

###Why should I use Homie Remotedesktop?

In short, **because it's extremely easy to use and even works via internet**. The idea is to simplify the process of connecting to a remote host in non-LAN environments as much as possible. To achieve this, the tool will be installed on a server as a Windows service. It will listen for incoming requests to connect to a remote machine in the local network of the server. If required, it will start the remote host by sending a "Magic packet" to it (WOL, "Wake-on-LAN"). As soon as the remote host was started, the service will notify the client and initiate the remote desktop connection.

### Powered by 

![Image of Yaktocat](https://camo.githubusercontent.com/d94f160ac291837e52a5a9f0a56d0f087281460c/687474703a2f2f7777772e6a6574627261696e732e636f6d2f696d672f6c6f676f732f6c6f676f5f7265736861727065725f736d616c6c2e676966)

### License

[Apache License Version 2.0](https://github.com/lemked/homieremotedesktop/raw/master/LICENSE)