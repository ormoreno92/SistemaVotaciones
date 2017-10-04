CREATE PROCEDURE sp_editUsers
	@userId int,
    @Name nvarchar(50),
	@LastName nvarchar(50),
	@Doc nvarchar(50),
	@Birthday date,
	@Email nvarchar(50),	
	@Active bit
AS   

    SET NOCOUNT ON;  
    UPDATE USUARIOS 
	SET
	usuario_nombres = @Name,
	usuario_apellidos = @LastName,
	usuario_activo=@Active,
	usuario_documento=@Doc,
	usuario_email = @Email,
	usuario_fec_nac=@Birthday
	WHERE usuario_id = @userId