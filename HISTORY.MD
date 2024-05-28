## 0.0.1
Basic .net maui configurations. Work has started on sign in page design and backend.

## 0.0.2
Edited the mainpage to retrieve the email address along with the correct data validation. Improved the sign up process by highlighting the confirmed password box red if the passwords don't match.

## 0.0.3
Sign up button can now post the email address and password to the database. Both hashing and salting are done on the API side. 

## 0.0.4
Setup login page, Users can choose between login and signup via buttons. Fixed the error when someone removes any text from the confirm password input field the input field would then turn white. Names changes has been made to some methods to support better readability.

## 0.0.5
Created separate folders a scripts to contain some methods that can be used more than once. Login button can now post to the API for authentication. 

## 0.0.6
Removed unused .env with an API Key that was never used for an API.

## 0.0.7
On boot up local environment variables are loaded. Some methods in the MainPage.xaml.cs have been changed to async to due PostDataToAPI method being async. A new script has been created to contain all current and future methods that will deals with APIs.

## 0.0.8
Response status code is now returned from the PostDataToAPI method. A new page has been created and can be accessed once the user has be authorized or registered. The new page currently displays a map visual. 
Couple script and method name changes. 

## 0.0.9
The bing map API key is stored in a .env file which is now retrieved from. The project has also been cleaned. 

## 0.0.10
This project now uses ArcGIS maps. The map has a starting point of Manchester and the basemap is Dark Gray. The API key is retrieved from the .env.

## 0.0.11 
This project can load the line and stops from the kml files stored on github.

## 0.0.12
The stops have been temporarily removed. The folder containing the map data have been removed due to it being stored on github. Also before sending API requests it checks if the device is connected to the internet. Readability and efficiency have been improved in initializing the map.

## 0.0.13 
The code behind the AccessPortal.xaml has been redone to improve readability and maintainability. Repetitive code has been removed and replaced with additional methods. 