# a1-s3740446-s3734938
Assignment 1 Web Dev Technologies Flexible Semester 2020 

Ryan Cassidy - s3740446 

Vineet Bugtani - S3734938 

Equal contribution by both assignment partners

Patterns used:

Factory Pattern
We decided to make two object types of account that inherit an abstract class as there was a slight difference between Checking and Savings accounts. Account objects were initialized through a factory class that can read a character and make the appropriate class type.


Repository Pattern
As our database class for handling SQL was getting quite large we decided to split it into a repository class that was segmented by the objects the SQL would handle. This includes an interface and an abstract class inherited by each repository type.
This pattern allows for organization of SQL statements by object type and future implementation of methods for each object type in order to easily modify and introduce new statements to an SQL Data Server.


 
A single sproc was used in this program which is contained in the sprocs folder.
 
References: 

Modified Json converter from the following post about deserialization in json.net
https://skrift.io/articles/archive/bulletproof-interface-deserialization-in-jsonnet/

Factory pattern modified from lecture examples. 

Json deserialization string modified from lecture examples. 

Json obscuring method from tutorial examples.
