use p5g1;

CREATE TYPE movies.genrelist
AS TABLE
(
	genre varchar(20)
);

CREATE TYPE movies.actorlist
AS TABLE
(
	ssn int
);

CREATE TYPE movies.writerlist
AS TABLE
(
	ssn int
);

drop type movies.genrelist;
drop type movies.actorlist;
drop type movies.writerlist;


----------------------------------------------------------------------------------------------------------------------------------------------
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
									@Genre AS movies.genrelist READONLY,
									@Actors AS movies.actorlist READONLY,
									@Writers AS movies.writerlist READONLY
									)
AS
BEGIN
	-- SET NOCOUNT ON; -- reduces msgs sent to users and slightly improves performance on bigger sp's

	INSERT into movies.movie VALUES (@id, @duration, @description, @age_restriction, @rating, @studio_id, @director_ssn);
	
	declare @tmp varchar(20);
	declare c cursor for select * from @Genre;
	--
	open c;
	fetch c into @tmp;

	while @@FETCH_STATUS = 0
	begin
		insert into movies.genre values(@id, @tmp);
		fetch c into @tmp;
	end
	close c;
	--
	declare @ssn int;
	declare c cursor for select * from @Actors;

	open c;
	fetch c into @ssn;

	while @@FETCH_STATUS = 0
	begin
		insert into movies.performed_by values(@id, @ssn);
		fetch c into @ssn;
	end
	close c;
	---
	declare c cursor for select * from @Writers;

	open c;
	fetch c into @ssn;

	while @@FETCH_STATUS = 0
	begin
		insert into movies.written_by values(@id, @ssn);
		fetch c into @ssn;
	end
	close c;
	---
	return; 
END
GO

declare @g movies.genrelist;
insert into @g values ('AAA');
insert into @g values ('BBB');
insert into @g values ('CCC');

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



---------------------------------------------------------------------------------------------------------
-- Create SP to search movies
go
CREATE procedure movies.sp_searchMovies (
										@Title varchar(50) = null, 
										@Age_restriction varchar(5) = null,
										@Country varchar(50) = null, 
										@Studio_id int = null, 
										@Year int = null, 
										@Actors AS movies.actorlist READONLY
											)
AS
BEGIN
	declare @tmp table(id int, title varchar(50), country varchar(50), [date] date, duration time, age_restriction varchar(5), rating int, studio varchar(50), [description] varchar(500), studio_id int);
	declare @out table(id int, title varchar(50), country varchar(50), [date] date, duration time, age_restriction varchar(5), rating int, studio varchar(50), [description] varchar(500), studio_id int);
	
	if exists(select * from @Actors)
		insert into @tmp select id, title, country, [date], duration, age_restriction, rating, studio, [description], studio_id from @Actors join movies.performed_by on ssn=actor_ssn join movies.udf_GetMovies () on movie_id=id;
	else
		insert into @tmp select * from movies.udf_GetMovies();

	if not @Title is null
		insert into @out select * from @tmp where title=@Title;
	else
		insert into @out select * from @tmp;

	delete from @tmp;

	if not @Age_restriction is null
		insert into @tmp select * from @out where age_restriction=@Age_restriction;
	else
		insert into @tmp select * from @out;

	delete from @out

	if not @Country is null
		insert into @out select * from @tmp where country=@Country;
	else
		insert into @out select * from @tmp;

	delete from @tmp;

	if not @Studio_id is null
		insert into @tmp select * from @out where studio_id=@Studio_id;
	else
		insert into @tmp select * from @out;

	delete from @out

	if not @Year is null
		insert into @out select * from @tmp where YEAR([date])=@Year;
	else
		insert into @out select * from @tmp;

	select * from @out;


END
go

drop procedure movies.sp_searchMovies;

declare @g movies.actorlist;
insert into @g values (1);

exec movies.sp_searchMovies  @Studio_id=1;

-------------------------------------------------------------------------------------------------------------------

-- Create SP to insert Actors
go
create procedure movies.sp_AddActor (
									@ssn int,
									@name varchar(50),
									@bdate date,
									@rank int,
									@bio varchar(600)
									)
as
begin

insert into movies.actor values (@ssn, @name, @bdate, @rank, @bio);

end
go

exec movies.sp_AddActor @ssn = 100, @name = 'aaaaaa', @bdate = '1/2/3', @rank = 100, @bio = 'dasuhdufashduhsfu';

--------------------------------------------------------------------------------------------------------------------