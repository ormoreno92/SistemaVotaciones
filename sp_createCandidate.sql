CREATE PROCEDURE sp_createCandidate
    @date datetime,   
    @UserId int
AS   

    SET NOCOUNT ON;  
    INSERT INTO CANDIDATOS VALUES (@date, @UserId)
GO 