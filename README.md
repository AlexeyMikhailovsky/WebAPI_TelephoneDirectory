## TelephoneDirectory

### Requirements
1. The application must allow editing and viewing hierarchical structure of enterprise divisions (Department->Sub-department->Sub-Sub-department, etc.).
2. For each of the departments, the application should allow editing and viewing a list of department employees with their attributes.
3. The application should allow search for employees by the entered user line. The search is performed by determining the occurrences of the searched string in employee attribute text.
4. The application is a WebAPI service. Developed in languages: C# (.NETCore) or Python (Flask, Django frameworks)
5. Data is stored in the database. It is possible to use SQLite, PostgreSQL, MySql.
6. Access of the "Server" to the database data occurs by executing direct SQL commands without using any ORM.

Test task. Used .Net Core, MySQL. Information about departments and employees can be viewed, added, deleted, updated and changed.
### Implemented functionality
- Departments:
    - can be updated
    - can be deleted
    - can be created
    - can be viewed hierarchically
- Users:
    - can be updated
    - can be deleted
    - can be created
    - can be viewed as part of department hierarchy
    - can be searched by attributes
    
    
![2](https://user-images.githubusercontent.com/63253786/186040022-453d9447-0871-4ab2-8ffd-ea0f5e7e84f8.png)
