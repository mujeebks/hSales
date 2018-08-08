

A,Configure the database and connection ?

1,Open the "scripts.txt" notepad 
2,copy the all script and paste the script in the sql server and excecute the script
3,This will create a database called "Sequilize" in your sql server
4,now goto "node api" folder from the instance folder you can find the sequilize.ts file there you have to configure the database 

IDE --sql server any version




B,how to run node api?

1,got to "nodeapi" folder from the parent folder
2,open the folder in cmd/powershell as administrative privilage
3,type the command npm install (this will install all the packages which are already in the package.json)-- do this only for first time 
4,type npm run start (this will start the node application and a couple of command windows may open) 

TEST API working or not 
goto http://yourlocalip:4001/api/Masters/Categories in your browser
Recommented IDE for node api is vscode




C,how to run client?

1,go to the "client" folder from the parent folder
2,open the folder in cmd/powershell as administrative privilage
3,type the command npm install (this will install all the packages which are already in the package.json)-- do this only for first time 
4, goto "hSales\client\app" folder there you can see the aa.constant.js file , edit the file in nodepad by replacing only "192.168.10.165" with your local ip address 
5,type npm run browsedev (this will start the client application on your browser)


Recommented IDE for client is vscode/any version of visual studio 