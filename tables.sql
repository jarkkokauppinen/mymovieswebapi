CREATE TABLE movie (
	idmovie varchar(100) PRIMARY KEY,
	title varchar(100) UNIQUE NOT NULL,
	year char(4) DEFAULT NULL,
	description varchar(255) DEFAULT NULL,
	image varchar(255) DEFAULT NULL,
	iddirector varchar DEFAULT NULL,
	idgenre int DEFAULT NULL,
	iduser int NOT NULL
);

CREATE TABLE director (
	iddirector varchar(100) PRIMARY KEY,
	firstname varchar(45) NOT NULL,
	lastname varchar(45) NOT NULL
);

CREATE TABLE genre (
	idgenre SERIAL PRIMARY KEY,
	genre varchar(45) NOT NULL
);

CREATE TABLE actor (
	idactor varchar(100) PRIMARY KEY,
	firstname varchar(45) NOT NULL,
	lastname varchar(45) NOT NULL
);

CREATE TABLE movieactor (
	idmovieactor SERIAL PRIMARY KEY,
	idmovie varchar NOT NULL,
	idactor varchar NOT NULL
);

CREATE TABLE rating (
	idrating varchar(100) PRIMARY KEY,
	rating int NOT NULL,
	iduser int NOT NULL
);

CREATE TABLE movierating (
	idmovierating SERIAL PRIMARY KEY,
	idmovie varchar NOT NULL,
	idrating varchar NOT NULL
);

CREATE TABLE app_user (
	iduser SERIAL PRIMARY KEY,
	username varchar(45) UNIQUE NOT NULL,
	password varchar(255) NOT NULL,
	firstname varchar(45) NOT NULL,
	lastname varchar(45) NOT NULL
);

CREATE TABLE quote (
	idquote SERIAL PRIMARY KEY,
	quote varchar(255) NOT NULL,
	movie varchar(45) NOT NULL,
	year char(4) NOT NULL
);