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

## 0.0.14
Dependency Injection is now used to send requests to the API. The relevant scripts have been updated to support this. 

## 0.0.15
ArcgisMap.cs has been removed as the map is now generated from the code behind file. The metrolink network is displayed via a kmllayer and the stops is now clustered and displayed via graphical overlays. 

## 0.0.16
Traffic incident data is now requested from the Tom Tom API and loaded onto the map via graphical overlays. Also the stops data is now retrieved locally, additional files have been added to support this. The apiutilities class now use an interface which has been registered in the mauiprogram.cs. Additional logging has been added to the Access Portal. Unnecessary registered services have been removed from mauiprogram.cs.

## 0.0.17
Removed unnecessary graphical overlay in the backend of the hub. Redone multiple methods within the Hub to support best practice by ensuring each method only has one specific purpose. Also the map now displays proper icons for each incident. The icons have been saved within the resource directory and the old ones have been removed.

## 0.0.18
### Fixed
- Updated `env.load` to reference the correct file path in [MauiProgram](./MauiProgram.cs).
l
### Removed
- Deleted unnecessary `obj` and `bin` folders.

## 0.0.19
### Added
- Added IdentityModel.OidcClient to the project.

### Fixed
- Removed unnecessary package reference in [MauiProgram](./MauiProgram.cs) and [AccessPortal](./Pages/AccessPortal.xaml.cs)
- Relocated the ApiHandler.cs to [here](./Utilities/ApiHandler.cs)

### Removed
- Removed unnecessary items from the constructor in [AccessPortal](./Pages/AccessPortal.xaml.cs)

## 0.1.1

### Added
- **Custom URIs** added for each platform to implement callbacks.
- Introduced the [**AuthService**](./Utilities/AuthService.cs) to handle the PCKE flow using IdentityModel.
- Created a [**BrowserService**](./Utilities/BrowserService.cs) as a workaround for the WebAuthenticator issue on Windows devices due to a bug in the Windows SDK.

### Fixed/Updated
- Updated navigation method in [**AccessPortal**](./Pages/AccessPortal.xaml.cs) from `Navigation.PushAsync` to `Shell.Current.GoToAsync`.
- Improved the `GetDataFromAPI` method in [**ApiHandler**](./Utilities/ApiHandler.cs).
- Moved page route registration to [**AppShell**](./AppShell.xaml.cs) instead of [**MauiProgram**](./MauiProgram.cs).
- Enhanced dependency registration process in [**MauiProgram**](./MauiProgram.cs).

### Removed
- Removed the UI code for login fields in [**AccessPortal**](./Pages/AccessPortal.xaml).
- Eliminated unnecessary dependency on `ServiceProvider` in [**AccessPortal**](./Pages/AccessPortal.xaml.cs) as Shell navigation is now in use.

### Notes
Further improvements to the code and organization will be made in the upcoming commits.

## 0.2.0
### Added
- **Tests Directory**: Created a dedicated `Tests` directory to house all unit tests.

### Fixed/Updated
- **Project Structure**: Reorganized projects to consolidate all .NET MAUI-related files and folders within the `app` directory for better maintainability.

### Removed
- **Redundant Models**: Eliminated obsolete models to streamline the codebase.
- **Sign Up Feature**: Temporarily removed sign-up functionality pending the implementation of the new TMS_IDP authentication system.
- **Map Resources**: Removed map loading functionality and associated resources in preparation for upcoming SignalR integration refactoring.

## 0.2.1
### Added
- Integrated `ILinesHubService` with real-time data display in `Hub.xaml.cs` using SignalR.
- Added `TMS_APP.HubServices` and related dependency injection services.
- Implemented new map initialization and polyline rendering logic using ArcGIS SDK.

### Fixed/Updated
- Improved `.env` file loading logic to include a fallback check and exception handling.
- Replaced transient `AuthService` with a singleton pattern.
- Upgraded `DotNetEnv` from version `3.0.0` to `3.1.1`.
- Added `Microsoft.AspNetCore.SignalR.Client` NuGet package for real-time communication support.
- Moved `AuthService.cs` and `BrowserService.cs` from `TMS_APP.Utilities` to `TMS_APP.AccessControl`, also refactored logic.

### Removed
- Removed in-app user signup logic and UI (`signupStack`) from `AccessPortal.xaml` and `AccessPortal.xaml.cs`.
- Removed unused `Polly` reference and outdated `BaseAddress` configuration from HTTP client setup.


## 0.2.2
### Added
- Implemented protocol URI handling for Windows platform to support custom URI schemes (tmsapp://)
- Added CommunityToolkit.Mvvm package for messaging support
- Added token storage tasks running in parallel in AuthService

### Fixed/Updated
- Updated AuthService to use new redirect URIs (tmsapp://callback/, tmsapp://logout-callback/)
- Updated scope from "api1.read" to "signalR.read" in AuthService
- Refactored BrowserService to use MVVM messaging pattern instead of HttpListener
- Updated LinesHub URL to use localhost
- Updated login button implementation in AccessPortal to properly handle auth flow
- Updated Microsoft.Extensions.Logging.Console package from 8.0.0 to 9.0.4

### Removed
- Removed HttpListener-based browser implementation
- Removed unused TMS_APP.Models and System.Net.Http.Json namespaces from AuthService
- Removed localhost:5000 callback URLs


## 0.2.3
### Added
- Added cancellation token support to `LoginAsync` method in `AuthService`
- Added `DisablePushedAuthorization` flag to OIDC client configuration
- Added `Microsoft.Maui.Storage` namespace for secure storage operations
- Added `Duende.IdentityModel.OidcClient` package (v6.0.1) as replacement
- Added assembly info generation flags to project file

### Fixed/Updated
- Updated OIDC client configuration with:
  - Correct Keycloak authority URL (`https://localhost:8443/realms/maui_realm`)
  - Updated scope list (removed `openid profile`, kept `signalR.read offline_access`)
  - Added proper logout redirect URI
- Updated dependency injection registration for `IAuthService` from Singleton to Transient
- Updated all OIDC client references from `IdentityModel` to `Duende.IdentityModel` namespace
- Updated project file with proper Windows target framework spacing
- Improved error handling in login button click handler

### Removed
- Removed sign-up button and related UI elements from `AccessPortal.xaml`
- Removed sign-up button click handler from `AccessPortal.xaml.cs`
- Removed `IdentityModel.OidcClient` package (v6.0.0)
- Removed TODO comment about registration logic
- Removed unused `System.Text.RegularExpressions` and model references