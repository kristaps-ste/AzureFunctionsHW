# Azure Functions Demo


## How to use :
To get logs in specific time period (from/to)
 
call /getLogs with query parameters with following dateTime format :

yyyy/MM/dd/HH:mm    It`s possible to change desired dateTime format in config settings
 
```pytho
example for /getLogs call : 
host/api/getLogs?from=2020/10/12/09:00&to=2020/12/13/09:00
```
All logs in given time range returned. (1000 record limit per call)



To get payload data for particular id call /getPayload with id parameter which coud be retrieved from /getLogs:

```pytho
demo:
host/api/GetPayload?id=637385575205572816
```
Payload content returned if exists, 404 Status otherwise.
