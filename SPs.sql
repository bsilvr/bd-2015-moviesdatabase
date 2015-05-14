use p5g1;

CREATE TYPE movies.genrelist
AS TABLE
(
	id int,
	genre varchar(50)
);

drop type movies.genrelist

GO
CREATE PROCEDURE movies.sp_AddMovie 
									@id int,
									@duration time, 
									@description varchar(500), 
									@age_restriction varchar(5), 
									@rating tinyint, 
									@studio_id int, 
									@director_ssn int, 
									@Genre AS movies.genrelist
AS
BEGIN
	SET NOCOUNT ON; -- reduces msgs sent to users and slightly improves performance on bigger sp's

	INSERT into movies.movie VALUES (@id, @duration, @description, @age_restriction, @rating, @studio_id, @director_ssn);

	UPDATE @Genre SET id=@id WHERE id IS NULL;

	INSERT into movies.genre SELECT * FROM @Genre;
	 
END
GO