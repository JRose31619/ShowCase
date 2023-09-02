# Employee DataBase
This application is designed to allow users to have full
Create, Read, Update, Delete functionality for their personal
information. 

## User Interface Layer
The user interface was built with WPF. Having three layers of work
being done on the client side. 

1. Windows the user interacts with
2. A static class that runs validation on user actions
3. a class that passes front end data to the class library

### Windows
When the application starts, the user is brought to the home window.
They select which data base they would like to work with. Then
select their desired action

(Insert images here)

On the selected actions window, the user fills out the corresponding
fields. Users will submit the information, the application will tell
user if the submission was successful. If the submission failed, the
user will be told why it failed.

(Insert Images here)

### Validation
Once users press submit, submitted information will be put into
models and sent through validation. If user information is unacceptable,
the static methods will return false. User data will not be sent to the
class library

(Insert Images here)

### UI Actions
When validation is complete, data is then passed from the respective
windows code behind to the UI Actions class. Here, connection to 
appsettings.JSON is established. The connection string is retreived
and formatted based on the users selected data base.

(Insert Images here)

The interface for database data is created by passing in the users
selected database. Users selected action is then ran by the interface
instance passing in the users submitted data

(Insert Images here)

## Class Library
The back end class library has two layers
of work being done

1. Data Access class for using Dapper to connect to each database
2. Data classes responsible for using the data access to perform actions
   with each database

   

