use p5g1
go
CREATE FUNCTION movies.GetMovies () RETURNS table
AS
	RETURN 
	(
		SELECT movies.movie.id, title, country, [date], duration, age_restriction, rating, name as studio, [description]
		FROM movies.movie join movies.release on movies.movie.id = movies.release.movie_id join movies.studio on movies.movie.studio_id = movies.studio.id	
	)
go