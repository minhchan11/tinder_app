# Tinder
----
###

#### By

## Description

This website will match people

## Setup/Installation Requirements

##### Create Database and Tables
* In a command window:
```sql
> sqlcmd -S "(localdb)\mssqllocaldb"
> CREATE DATABASE band_tracker;
> GO
> USE band_tracker;
> GO
> CREATE TABLE bands (id INT IDENTITY(1,1), name VARCHAR(255));
> GO
> CREATE TABLE venues (id INT IDENTITY(1,1), name VARCHAR(255));
> GO
> CREATE TABLE bands_venues (id INT IDENTITY(1,1), band_id INT, venue_id INT);
> GO
```
* Requires DNU, DNX, MSSQL, and Mono
* Clone to local machine
* Use command "dnu restore" in command prompt/shell
* Use command "dnx kestrel" to start server
* Navigate to http://localhost:5004 in web browser of choice

## Specifications

#### Band Class

* The DeleteAll method for the Band class will delete all rows from the bands table.
  * Example Input: none
  * Example Output: nothing

* The GetAll method for the Band class will return an empty list if there are no entries in the Band table.
  * Example Input: N/A
  * Example Output: empty list

* The Equals method for the Band class will return true if the Band in local memory matches the Band pulled from the database.
  * Example Input:  
> Local: "Green Day" , id is 1;
> Database: "Green Day" , id is 1;

  * Example Output: `true`

* The Save method for the Band class will save new bands to the database.
  * Example Input:  
> Save ("Green Day"),

  * Example Output: no return value

* The Save method for the Band class will assign an id to each new instance of the Band class.
  * Example Input:  
> New band: "Green Day" ,`local id: 0`, Save();

  * Example Output:  
> "Green Day" , `database-assigned id`  

* The GetAll method for the Band class will return all band entries in the database in the form of a list.
  * Example Input:  
> "Green Day" , id is 10 ; Save();"Spice Girl", id is 11 ; Save(); GetAll()

  * Example Output:
> `{Green Day object}, {Spice Girl object}`

* The Find method for the Band class will return the Band as defined in the database.
  * Example Input:
> Find ("Green Day")

  * Example Output:
> "Green Day" , `database-assigned id`

* The Update method for the Band class will return the Band with the new band name and instructions.
  * Example Input:
> "Green Day" , id is 1; Update("Yellow Day");

  * Example Output:
> "Yellow Day" , id is 1

* The DeleteThis method for the Band class will delete the band from the list of Band.
  * Example Input:
> DeleteThis ("Green Day"); GetAll()

  * Example Output:
> `{No Doubt object}, {Spice Girl object}`

* The Search method for the Band class will return a list of Bands with matched name.
  * Example Input:
> Search ("Green Day")

  * Example Output:
> `{Green Day object}`


#### Venue class
* The DeleteAll method for the Venue class will delete all rows from the venues table.
  * Example Input: none
  * Example Output: nothing

* The GetAll method for the Venue class will return an empty list if there are no entries in the Venue table.
  * Example Input: N/A
  * Example Output: empty list

* The Equals method for the Venue class will return true if the Venue in local memory matches the Venue pulled from the database.
  * Example Input:  
> Local: "Manhattan Square" , id is 1;
> Database: "Manhattan Square" , id is 1;

  * Example Output: `true`

* The Save method for the Venue class will save new venues to the database.
  * Example Input:  
> Save ("Manhattan Square"),

  * Example Output: no return value

* The Save method for the Venue class will assign an id to each new instance of the Venue class.
  * Example Input:  
> New venue: "Manhattan Square" ,`local id: 0`, Save();

  * Example Output:  
> "Manhattan Square" , `database-assigned id`  

* The GetAll method for the Venue class will return all venue entries in the database in the form of a list.
  * Example Input:  
> "Manhattan Square" , id is 10 ; Save();"Seattle", id is 11 ; Save(); GetAll()

  * Example Output:
> `{Manhattan Square object}, {Seattle object}`

* The Find method for the Venue class will return the Venue as defined in the database.
  * Example Input:
> Find ("Manhattan Square")

  * Example Output:
> "Manhattan Square" , `database-assigned id`

* The Update method for the Venue class will return the Venue with the new venue name and instructions.
  * Example Input:
> "Manhattan Square" , id is 1; Update("Park");

  * Example Output:
> "Park" , id is 1

* The DeleteThis method for the Venue class will delete the venue from the list of Venue.
  * Example Input:
> DeleteThis ("Manhattan Square"); GetAll()

  * Example Output:
> `{Park object}, {Stadium object}`

* The Search method for the Venue class will return a list of Venues with matched name.
  * Example Input:
> Search ("Manhattan Square")

  * Example Output:
> `{Manhattan Square object}`


#### Band && Venue classes
* The AddBand method for the Venue class will save a bands associated with that venue.
  * Example Input:
> AddBand("Green Day")

  * Example Output: nothing

* The GetBand method for the Venue class will return the list of bands associated with that venue.
  * Example Input:
> GetBand("Manhattan Square")

  * Example Output:
> `{Green Day object},{Spice Girl object}`

* The DeleteBands method for the Venue class delete the entries that connects the band ids with the venue.
  * Example Input:
  > DeleteBands("Park"), GetBand("Park")

  * Example Output: null

* The DeleteThisBand method for the Venue class delete the singular entry that connects the band id with the venue.
  * Example Input:
  > DeleteThisBand("Park","GreenDay"), GetBand("Park")

  * Example Output:
  > `{No Doubt object},{Spice Girl object}`

* The AddVenue method for the Band class will save a venue associated with that band.
  * Example Input:
> AddVenue("Park")

  * Example Output: nothing

* The GetVenue method for the Band class will return the list of venues associated with that band.
  * Example Input:
> GetVenue("Green Day")

  * Example Output:
> `{Park object}, {Stadium object}`

* The DeleteVenues method for the Venue class delete the entries that connects the band ids with all the associated venue.
  * Example Input:
  > DeleteVenues("Green Day"), GetVenue("Green Day")

  * Example Output: null

#### User Interface
* The user can add a new Band using the "Add Band" form.
  * Example Input:  
  New Band: "Green Day", id is 1; *add Band*
  * Example Output:  
  All Bands Page: "Green Day, No Doubt"

* The user can add a new Venue using the "Add venue" form.
  * Example Input:  
    New venue: "Manhattan Square", id is 10; *add Venue*
  * Example Output:  
    All venues: "Manhattan Square", "Park"

* The user can click on any venue in the venues list to view the venue's details
  * Example Input:  
    *click* "Manhattan Square"
  * Example Output: "Manhattan Square", List of venue tags

* The user can click on any band to view a list of all venues in that the band and it's tags.
  * Example Input:  
    *click* "Green Day"
  * Example Output: "Green Day", list of venues in Green Day (eg Manhattan Square)

* The user can edit a venue's venue name on the venue's page.
  * Example Input:  
    *click* "Manhattan Square"  
     New venue name: "Manhattan Park"  
  * Example Output: "Manhattan Park"

* The user can delete a venue using a link on the venue's page .
  * Example Input:  
     *click* "Manhattan Square"  
     *delete*  
  * Example Output: Return to Venues Page

* The user can edit a band's name on the band's page.
  * Example Input:    
   *click* "Green Day"  
  * Example Output: "Yellow Day"

* The user can delete a band using a link on the band's page .
  * Example Input:  
   *click* "Green Day"  
   *delete click*  
  * Example Output: Return to Bands Page

* The user can search using venue name for an venue using the search form.
  * Example Input:
    *search* "Manhattan"
  * Example Output: "Manhattan Square"

* The user can search using band name for an band using the search form.
  * Example Input:  
    *search* "Green"
  * Example Output: "Green Day"

* The user can add a venue to a band using selection form.
  * Example Input:  
    "Green Day" *add* "Manhattan Square"
  * Example Output: "Green Day", "Manhattan Square"

* The user can remove a venue from a band using selection form.
  * Example Input:  
    "Green Day" *remove* "Manhattan Square"
  * Example Output: "Green Day", "Park"

* The user can remove all venues from a band using selection form.
  * Example Input:  
    "Green Day" *remove all*
  * Example Output: "Green Day"

* The user can add a band to a venue using a selection form.
  * Example Input:  
  "Manhattan Square" *add* "Green Day"
  * Example Output: "Manhattan Square", "Green Day"

* The user can remove a band from a venue using selection form.
  * Example Input:  
    "Manhattan Square" *remove* "Green Day"
  * Example Output: "Manhattan Square", "Spice Girls"

* The user can remove all venues from a band using selection form.
  * Example Input:  
    "Manhattan Square" *remove all*
  * Example Output: "Manhattan Square"

## Support and contact details

Please contact Minh Phuong mphuong@kent.edu with any questions, concerns, or suggestions.


## Technologies Used

This web application uses:
* Nancy
* Mono
* DNVM
* C#
* Razor
* MSSQL & SSMS

****

### License

*This project is licensed under the MIT license.*

Copyright (c) 2017 _**Minh Phuong**_
