





CREATE PROCEDURE [dbo].[Get_ActivityLastDay]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		AL.ActivityId,
		AL.DateTime,
		AL.UserId,
		AL.CompanyId,
		AT.Description AS Target,
		AA.Description AS Action,
		ISNULL(AL.ExtraData,'') AS ExtraData,
		EMP.LastName +', '+emp.Name as Employee,
		AL.TargetId AS TargetId,
		AL.TargetType,
		ISNULL(CASE WHEN AL.TargetType = 5	THEN D.Name
		ELSE CASE WHEN Al.TargetType = 9  THEN C.Description
			 ELSE CASE WHEN Al.TargetType = 8 THEN E.Name + ' ' + E.LastName
				  ELSE CASE WHEN Al.TargetType = 4 THEN Doc.Description
					   ELSE CASE WHEN Al.TargetType = 11 THEN L.Description
							ELSE CASE WHEN Al.TargetType = 12 THEN LAE.Name + ' ' + LAE.LastName + ' --> ' +  CASE WHEN LEN(L2.Description) < 15 THEN L2.Description ELSE LEFT(L2.Description,25) + '...' END
								 ELSE CASE WHEN Al.TargetType = 10 THEN P.Description
									  ELSE '' 
								      END 
								 END 
							END
					   END
				  END
		     END
		END,'') AS Description
	FROM ActivityLog AL WITH(NOLOCK)
	INNER JOIN ActivityTarget AT WITH(NOLOCK)
	ON	AT.ActivityTarget = AL.TargetType
	INNER JOIN ActivityAction AA WITH(NOLOCK)
	ON	AA.ActivityTarget = AL.TargetType
	AND	AA.ActivityAction = AL.ActionId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
		INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
			INNER JOIN Employee EMP WITH(NOLOCK)
			ON	EMP.Id = EUA.EmployeeId
			AND EMP.CompanyId = EUA.CompanyId
		ON EUA.UserId = AU.Id
	ON	AU.Id = AL.UserId
	LEFT JOIN Department D WITH(NOLOCK)
	ON	 D.Id = Al.TargetId
	LEFT JOIN Cargos C WITH(NOLOCK)
	ON	 C.Id = Al.TargetId
	LEFT JOIN Employee E WITH(NOLOCK)
	ON	 E.Id = Al.TargetId
	LEFT JOIN Document Doc WITH(NOLOCK)
	ON	 Doc.Id = Al.TargetId
	LEFT JOIN Learning L WITH(NOLOCK)
	ON	 L.Id = Al.TargetId
	LEFT JOIN LearningAssistant LA WITH(NOLOCK)
		INNER JOIN Employee LAE WITH(NOLOCK)
		ON	LAE.Id = LA.EmployeeId
		INNER JOIN Learning L2 WITH(NOLOCK)
		ON L2.Id = LA.LearningId
	ON	 LA.Id = Al.TargetId
	LEFT JOIN Proceso P WITH(NOLOCK)
	ON P.Id = Al.TargetId
	
	WHERE
		AL.CompanyId = @CompanyId
	AND AL.DateTime > GETDATE()-1
		
	ORDER BY Al.CompanyId ASC, AL.TargetId ASC, Al.DateTime DESC
END







