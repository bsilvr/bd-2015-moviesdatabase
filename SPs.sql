use p5g1;

CREATE TYPE movies.genrelist
AS TABLE
(
	id int,
	genre varchar(20)
);

drop type movies.genrelist;

drop procedure movies.sp_AddMovie;

GO
CREATE PROCEDURE movies.sp_AddMovie (
									@id int,
									@duration time, 
									@description varchar(500), 
									@age_restriction varchar(5), 
									@rating tinyint, 
									@studio_id int, 
									@director_ssn int, 
									@Genre AS movies.genrelist READONLY
									)
AS
BEGIN
	-- SET NOCOUNT ON; -- reduces msgs sent to users and slightly improves performance on bigger sp's

	INSERT into movies.movie VALUES (@id, @duration, @description, @age_restriction, @rating, @studio_id, @director_ssn);

	INSERT into movies.genre (id, name) SELECT id, genre FROM @Genre;
	 
END
GO

declare @g movies.genrelist;
insert into @g values (20, 'AAA');

exec movies.sp_AddMovie 
									@id=20,
									@duration='1:2:3', 
									@description='fdsvfdbvfdhf', 
									@age_restriction='PG', 
									@rating=100, 
									@studio_id=1, 
									@director_ssn=1, 
									@Genre=@g
									;