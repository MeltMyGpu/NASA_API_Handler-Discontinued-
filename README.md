# NASA_API_Handler

 A library that handles the fetching and sorting of NASA's API's data. 
This wrapper is designed to be lightweight, but has classes to aid in the sorting of data into a more accessable format 
if you would like to use them. I've included these less complex DTO's and funtions to load the raw JSON data into, 
as accessing data inside the raw JSON can be tedious due to it's more complex layout, the downside to using my provided 
simple DTO's is that by design, they contain much less information than the raw json, and are limited to metric units.
I am willing to make more versions of these DTO's with other units in mind, or just add more data to the existing one if people would like. <br/>
 
** Currently this wrapper only supports the NEO ( near earth objects ) API ** <br/>
** Future support for other API's is planned. ** <br/>
 
 If you would like to help with the devloment, please reach out.
 This project is very simple and therefore a great thing to work on as a newbie like me <br/> 
 
 
 ## NEO Wrapper Quick Summary.
 
 The NEO Wrapper consists of:
 - ** NEOHandler: **
	- This is responsible for handling the client obj.
	- This also handles Error Logging related to the functions of the Client obj.
	- All errors this object curretly catches are throw again after logging, bear this in mind when using it.
	- Please remember to call the Dispose method when you are done using.
	
 - ** Client: **
	- This class contains the HTTPClient and handles requests to the API.
	- This class contains methods for connection checks.
	- This class Deserializes the Json's into the NEORootObject wrapper.
	
 
 
