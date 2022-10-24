# myMovies

This is an independent project work of Developer Bootcamp 2022 organized by Saranen Consulting. The application is a movie themed single page app with all CRUD methods and stack is PostgreSQL, ASP.NET Core, React. The database and webapi are deployed on Heroku cloud service and frontend on Azure. The app can be run by clicking [here](https://mymoviesapplication.azurewebsites.net/).

### `instructions`

There are not many movies in the database,
so when searching, it is recommended to just type e.g. letter A.

If you don't want to create a new account, feel free to use these credentials :
<ul>
  <li>username = username</li>
  <li>password = password</li>
</ul>

Note that when Heroku dyno doesn't receive any traffic in 30 minute,
the dyno goes to sleep, and it may take a while to wake.

### `JSON web token`

When logged in, JSON web token is set to local storage.

## ER model

<img src='database_diagram.png'></img>