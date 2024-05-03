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