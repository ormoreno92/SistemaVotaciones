CREATE PROCEDURE sp_insertproposals
    @CandidateId int,   
	@ProposalName varchar(50),   
    @ProposalDescription varchar(500)
AS   

    SET NOCOUNT ON;  
    INSERT INTO PROPUESTAS VALUES (@ProposalName, @ProposalDescription, @CandidateId)
GO 