

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[Alert_EmployeesWithoutUser]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		E.Id
				
				 
			
			
				
					
								   
									  
				
							  
				
				 
							   
				 
			
	FROM Employee E WITH(NOLOCK)
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
							  
		INNER JOIN ApplicationUser AU WITH(NOLOCK)
		ON	AU.Id = EUA.UserId
	ON	 EUA.EmployeeId = E.Id
	WHERE
		EUA.UserId IS NULL
END


