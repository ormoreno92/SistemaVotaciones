CREATE PROCEDURE sp_insertvotations
    @CandidateId int,   
	@UserId int,   
    @date datetime
AS   

    SET NOCOUNT ON;  
    INSERT INTO VOTOS VALUES (@CandidateId, @UserId, @date)
GO 