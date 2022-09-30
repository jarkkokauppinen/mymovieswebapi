CREATE TABLE movie (
	idmovie SERIAL PRIMARY KEY,
	title varchar(45) DEFAULT NULL,
  year char(4) DEFAULT NULL,
  description varchar(255) DEFAULT NULL,
  image_url varchar(255) DEFAULT NULL,
  iddirector int DEFAULT NULL,
  idgenre int DEFAULT NULL,
  iduser int NOT NULL
);

CREATE TABLE director (
	iddirector SERIAL PRIMARY KEY,
	firstname varchar(45) NOT NULL,
	lastname varchar(45) NOT NULL
);

CREATE TABLE genre (
	idgenre SERIAL PRIMARY KEY,
	genre varchar(45) NOT NULL
);

CREATE TABLE actor (
	idactor SERIAL PRIMARY KEY,
	firstname varchar(45) NOT NULL,
  lastname varchar(45) NOT NULL
);

CREATE TABLE movieactor (
	idmovieactor SERIAL PRIMARY KEY,
	idmovie int NOT NULL,
  idactor int NOT NULL
);

CREATE TABLE rating (
	idrating SERIAL PRIMARY KEY,
	rating int NOT NULL,
  iduser int NOT NULL
);

CREATE TABLE movierating (
	idmovierating SERIAL PRIMARY KEY,
	idmovie int NOT NULL,
  idrating int NOT NULL
);

CREATE TABLE app_user (
	iduser SERIAL PRIMARY KEY,
	username varchar(45) UNIQUE NOT NULL,
  password varchar(255) NOT NULL,
  firstname varchar(45) NOT NULL,
  lastname varchar(45) NOT NULL
);