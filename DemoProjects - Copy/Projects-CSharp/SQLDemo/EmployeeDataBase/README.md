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

![HomeWindow](https://github.com/JRose31619/ShowCase/assets/135455213/0df92b07-17ef-4320-a59b-58a3662e5a18)

On the selected actions window, the user fills out the corresponding
fields. Users will submit the information, the application will tell
user if the submission was successful. If the submission failed, the
user will be told why it failed.

![CreateEntrySuccess](https://github.com/JRose31619/ShowCase/assets/135455213/0ead4654-985c-4088-9c17-fc1c17243a6b)
![CreateErrorMessage](https://github.com/JRose31619/ShowCase/assets/135455213/b5f5a94e-49de-4767-8233-d8aef300d9ec)
![CreateEntryFail](https://github.com/JRose31619/ShowCase/assets/135455213/92508cc0-16ce-4e79-9fc1-79ccc9fb70ef)

### Create
The create window allows users to make new entries for new employees.
Users can enter new employees both with and without employer information.

![CreateHasEmployer](https://github.com/JRose31619/ShowCase/assets/135455213/ccf952d5-736d-45dc-964e-8f39644bd940)
![CreateNoEmployer](https://github.com/JRose31619/ShowCase/assets/135455213/28491934-4df4-4e98-9e4a-e9e6c2c4c5ba)

### Add
Here, users will be able to make additions to preexisting entries.
New addresses, and employer information can be added to entries.
First and last names can not be added to preexisting entries.
Employer information can only be added if the entry does not
already have employer information. Users will enter the Id of
the desired entry and fill out the fields of what they would 
like to add. Pressing submit, the application will tell the user
if the information was successfully added or not.

![AddSuccess](https://github.com/JRose31619/ShowCase/assets/135455213/41b31e4a-79bf-4663-a1c0-aca31666d836)
![AddFailure](https://github.com/JRose31619/ShowCase/assets/135455213/81e49e8e-7888-4aba-b49d-2a4714d939c9)
![AddFailure2](https://github.com/JRose31619/ShowCase/assets/135455213/68f72a89-4e12-449c-a4a0-ce6563ad05ea)


### Read
The read window allows user to read data from a selected datebase.
Users will enter the Id of the entry they would like to see. Once
submitted, the application will either show the user the entered 
entry, or tell them it could not be found. The reason for why will
be stated to the user.

![ReadSuccess](https://github.com/JRose31619/ShowCase/assets/135455213/db060ae8-b053-4dd8-a364-20b1a5634cbb)
![ReadFailure](https://github.com/JRose31619/ShowCase/assets/135455213/4cb9df7c-4806-4441-a0fb-68716a63f4f1)

Address search allows users to search for specfic addresses that
entries have. Users will enter the Id of the entry and the desired
address they would like to pull up. If the information entered 
is incorrect, the search will fail and the application will state
why it fail.

(Insert Images Here)
### Update


(Insert Images Here)
### Delete

(Insert Images Here)
### Validation
Once users press submit, submitted information will be put into
models and sent through validation. If user information is unacceptable,
the static methods will return false. User data will not be sent to the
class library

### UI Actions
When validation is complete, data is then passed from the respective
windows code behind to the UI Actions class. Here, connection to 
appsettings.JSON is established. The connection string is retreived
and formatted based on the users selected data base.

The interface for database data is created by passing in the users
selected database. Users selected action is then ran by the interface
instance passing in the users submitted data

(Insert Images here)

## Class Library
The back end class library has two layers
of work being done

1. Data Access classes for using Dapper to connect to each database
2. Data classes responsible for using the data access to perform actions
   with each database

## DataBases
Data is stored between three different relational databases.
This application uses a SQL server, Sqlite, and a MySql database.
Each data base stores the same data and in the same way. Users can
only work with one database at a time. However, users can change
which database they work with at any time.

