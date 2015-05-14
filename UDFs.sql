use p5g1

go
CREATE FUNCTION movies.udf_GetMovies () RETURNS table
AS
	RETURN 
	(
		SELECT movies.movie.id, title, country, [date], duration, age_restriction, rating, name as studio, [description]
		FROM movies.movie join movies.release on movies.movie.id = movies.release.movie_id join movies.studio on movies.movie.studio_id = movies.studio.id	
	);
go

go
CREATE FUNCTION movies.udf_Actors () RETURNS table
AS
	RETURN 
	(
		SELECT *
		FROM movies.actor
	);
go

go
CREATE FUNCTION movies.udf_Writers () RETURNS table
AS
	RETURN 
	(
		SELECT *
		FROM movies.writer
	);
go

go
CREATE FUNCTION movies.udf_UniqueGenres () RETURNS table
AS
	RETURN 
	(
		SELECT distinct name
		FROM movies.genre
	);
go

go
CREATE FUNCTION movies.udf_Directors () RETURNS table
AS
	RETURN 
	(
		SELECT *
		FROM movies.director
	);
go

go
CREATE FUNCTION movies.udf_studios () RETURNS table
AS
	RETURN 
	(
		SELECT *
		FROM movies.studio
	);
go