




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Document_Restore]
	@DocumentId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Document SET
		FechaBaja = NULL,
		EndReason = NULL,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	  
				  
						   
 
					
			 
				  
	  
				  
 
						
  
			 
		 
				  
				   
				 
				 
				 
		   
	 
		  
  
		  
		  
			 
	
			  
	 
			
	
	 
   

	WHERE	
		Id = @DocumentId
	AND	CompanyId = @CompanyId

END




