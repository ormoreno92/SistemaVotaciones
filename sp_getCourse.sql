CREATE PROCEDURE sp_getCourse
	@name varchar(50)
AS
	SELECT curso_id as Id, curso_nombre as Nombre, curso_valido_votacion as Valido
	FROM Cursos
	WHERE curso_nombre = @name
