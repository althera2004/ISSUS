USE [issusdb]
GO

/****** Object:  StoredProcedure [dbo].[Actions_GetByCompany]    Script Date: 24/10/2018 20:20:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Actions_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		IA.Id,
		IA.CompanyId,
		IA.ActionType,
		IA.Origin,
		D.Id,
		D.Name,
		P.Id,
		P.Description,
		C.Id,
		C.Description,
		IA.WhatHappend,
		WH.Id,
		WH.Name,
		WH.LastName,
		IA.WhatHappendOn,
		IA.Causes,
		CAUSES.Id,
		CAUSES.Name,
		CAUSES.LastName,
		IA.CausesOn,
		IA.Actions,
		ACTIONS.Id,
		ACTIONS.Name,
		ACTIONS.LastName,
		IA.ActionsOn,
		IA.Monitoring,
		CLOSED.Id,
		CLOSED.Name,
		CLOSED.LastName,
		IA.ClosedOn,
		IA.Notes,
		IA.Active,
		EUA.UserId,
		EMP.Id,
		EMP.Name,
		EMP.LastName,
		IA.ModifiedOn
    FROM IncidentAction IA WITH(NOLOCK)
    INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee EMP
		ON	EMP.Id = EUA.EmployeeId
		AND EMP.CompanyId = @CompanyId
	ON EUA.UserId = IA.ModifiedBy
    LEFT JOIN Department D WITH(NOLOCK)
    ON	D.Id = IA.DepartmentId
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = IA.ProviderId
    LEFT JOIN Customer C WITH(NOLOCK)
    ON	C.Id = IA.CustomerId
    LEFT JOIN Incident I WITH(NOLOCK)
    ON	I.Id = IA.IncidentId
    LEFT JOIN Employee WH WITH(NOLOCK)
    ON	WH.Id = IA.WhatHappendBy
    LEFT JOIN Employee CAUSES WITH(NOLOCK)
    ON	CAUSES.Id = IA.CausesBy
    LEFT JOIN Employee ACTIONS WITH(NOLOCK)
    ON	ACTIONS.Id = IA.ActionsBy
    LEFT JOIN Employee CLOSED WITH(NOLOCK)
    ON	CLOSED.Id = IA.ClosedBy
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityCompany]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityCompany]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		1,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityCustomer]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityCustomer]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		15,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityDepartment]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityDepartment]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(200)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		5,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityDocument]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityDocument]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		4,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityEmployee]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityEmployee]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		8,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityJobPosition]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityJobPosition]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		9,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityLearning]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityLearning]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		11,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityLearningAssistant]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityLearningAssistant]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		12,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityLogin]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityLogin]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		7,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityProcess]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityProcess]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		10,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityProvider]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityProvider]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		14,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivitySecurityGroup]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivitySecurityGroup]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		@UserId,
		@CompanyId,
		3,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[ActivityUser]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ActivityUser]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		2,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[Alert_Department]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Alert_Department]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
		D.Id,
		D.Name
	FROM Department D WITH(NOLOCK)
	LEFT JOIN EmployeeDepartmentMembership EDM WITH(NOLOCK)
	ON EDM.DepartmentId = D.Id
	WHERE
		D.CompanyId = @CompanyId
	AND EDM.DepartmentId IS NULL
	AND D.Deleted = 0
END





GO

/****** Object:  StoredProcedure [dbo].[Alert_EmployeesWithoutUser]    Script Date: 24/10/2018 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


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

GO

/****** Object:  StoredProcedure [dbo].[Alert_JobPositionWithoutEmployees]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Alert_JobPositionWithoutEmployees]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
		C.Id,
		C.Description,
		1
	FROM Cargos C WITH(NOLOCK)
	LEFT JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
	ON	ECA.CargoId = C.Id
	WHERE
		C.Active = 1
	AND C.CompanyId = @CompanyId
	AND ECA.EmployeeId IS NULL
END





GO

/****** Object:  StoredProcedure [dbo].[Alert_JobPositionWithoutResposable]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Alert_JobPositionWithoutResposable]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
		C.Id,
		C.Description,
		2
	FROM Cargos C WITH(NOLOCK)
	INNER JOIN Cargos C2 WITH(NOLOCK)
	ON	C2.Id = C.ResponsableId
	AND C2.Active = 1
	WHERE
		C.Active = 1
	AND C.CompanyId = @CompanyId
	AND C2.Id IS NULL
END





GO

/****** Object:  StoredProcedure [dbo].[Alert_Timed]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Alert_Timed]
	@TableName nvarchar(50),
	@FieldName nvarchar(50),
	@Timed int,
	@FieldCondition nvarchar(50),
	@FieldConditionValue nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @SQL nvarchar(4000)
    SET @SQL ='SELECT * FROM '+@TableName+' WHERE '+@FieldName+' > getdate()+'+CAST(@Timed as nvarchar(10))
    
    IF @FieldCondition IS NOT NULL
    BEGIN
		SET @SQL = @SQL + ' AND ' + @FieldCondition + '=' + @FieldConditionValue
    END
    

	PRINT @SQL

	BEGIN
		EXEC sp_executesql @SQL
	END
END





GO

/****** Object:  StoredProcedure [dbo].[Application_GetItems]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Application_GetItems]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
    *
    FROM ApplicationItem WITH(NOLOCK)
    ORDER BY Id
END





GO

/****** Object:  StoredProcedure [dbo].[Application_GetMenu]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Application_GetMenu]
	@ApplicationUserId int
AS
BEGIN
	SET NOCOUNT ON;

	/*SELECT
		AI.Id AS ItemId,
		AI.Description,
		AI.Icon,
		AI.UrlList,
		AI.Parent,
		AI.Container,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead, 0) END AS BIT) END AS GrantToRead,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead, 0) END AS BIT) END AS GrantToWrite,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead, 0) END AS BIT) END AS GrantToDelete,
		Au.Admin
	FROM ApplicationItem AI WITH(NOLOCK)
	LEFT JOIN ApplicationGrant AG WITH(NOLOCK)
	ON	AG.ItemId = AI.Id
	AND	AG.UserId = @ApplicationUserId
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	 AU.Id = @ApplicationUserId

	WHERE
	(
		AI.Id IS NOT NULL
		OR
		AI.Container = 1
	)
	AND AI.Parent <> -1
	AND
	(
		AI.Container = 1
		OR
		AG.GrantToRead IS NOT NULL
	)
	
	ORDER BY Parent ASC, [Order] ASC*/

	SELECT
		AI.Id AS ItemId,
		AI.Description,
		AI.Icon,
		AI.UrlList,
		AI.Parent,
		AI.Container,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead,0) END AS BIT) END AS GrantToRead,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToWrite,0) END AS BIT) END AS GrantToWrite,
		CASE WHEN AI.CONTAINER = 1 THEN 1 ELSE CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToDelete,0) END AS BIT) END AS GrantToDelete
		,AU.Admin
	FROM ApplicationItem AI WITH(NOLOCK)
	LEFT JOIN ApplicationGrant AG WITH(NOLOCK)
	ON	AG.ItemId = AI.Id
	AND	AG.UserId = @ApplicationUserId
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = @ApplicationUserId

	WHERE
	/*(
		--AG.UserId = @ApplicationUserId
		--OR
		AI.Container = 1
	)
	AND*/ AI.Parent <> -1
	AND
	(
		AI.Container = 1
		OR
		ISNULL(AG.GrantToRead,0) = 1
	)
		OR (AU.Admin = 1 AND	AI.Id <21)
	
	ORDER BY Parent ASC, [Order] ASC
END




GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_ByEmployee]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_ByEmployee]
	@EmployeeId bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		AU.Id,
		AU.Login,
		AU.MustResetPassword,
		AU.Status,
		ISNULL(G.SecurityGroupId, 0) AS SecurityGroupId,
		ISNULL(SG.Name,'') AS SecurityGroupName,
		ISNULL(AU.PrimaryUser,0) AS PrimaryUser
	FROM EmployeeUserAsignation EUA WITH(NOLOCK)
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	EUA.EmployeeId = @EmployeeId
	AND EUA.UserId = AU.Id
	AND EUA.CompanyId = AU.CompanyId
	AND EUA.CompanyId = @CompanyId
	LEFT JOIN ApplicationUserSecurityGroupMembership G WITH(NOLOCK)
		INNER JOIN SecurityGroup SG WITH(NOLOCK)
		ON SG.Id = G.SecurityGroupId
	ON	G.CompanyId = AU.CompanyId
	AND G.ApplicationUserId = AU.Id
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_ChangeAvatar]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_ChangeAvatar]
	@UserId int,
	@Avatar nvarchar(50),
	@CompanyId int
AS
BEGIN
	UPDATE ApplicationUser SET
		Avatar = @Avatar
	WHERE
		Id = @UserId
	AND CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		2,
		@UserId,
		4,
		GETDATE(),
		''
	)
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_ChangePassword]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_ChangePassword]
	@UserId int,
	@OldPassword nvarchar(50),
	@NewPassword nvarchar(50),
	@CompanyId int,
	@Result int out
AS
BEGIN
	SELECT
	*
	FROM ApplicationUser AU
	WHERE
		AU.Id = @UserId
	AND AU.Password = @OldPassword
	AND AU.CompanyId = @CompanyId
	
	IF @@ROWCOUNT = 0 
	BEGIN
		SET @Result = 0
	END
	ELSE
	BEGIN
		SET @Result = 1
		UPDATE ApplicationUser SET
			Password = @NewPassword,
			MustResetPassword = 0
		WHERE
			Id = @UserId
		AND CompanyId = @CompanyId
		
		INSERT INTO ActivityLog
		(
			ActivityId,
			UserId,
			CompanyId,
			TargetType,
			TargetId,
			ActionId,
			DateTime,
			ExtraData
		)
		VALUES
		(
			NEWID(),
			@UserId,
			@CompanyId,
			2,
			@UserId,
			4,
			GETDATE(),
			''
		)
    END
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_ChangeUserName]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_ChangeUserName]
	@ApplicationUserId int,
	@CompanyId int,
	@UserName nvarchar(50),
	@extraData nvarchar(200),
	@UserId int,
	@EmployeeId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE ApplicationUser SET
		Login = UPPER(REPLACE(RTRIM(LTRIM(@UserName)),' ',''))
	WHERE
		Id = @ApplicationUserId
	AND CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		4,
		@ApplicationUserId,
		9,
		GETDATE(),
		@extraData
    )

	DECLARE @exists int;
	SELECT @exists = COUNT(*) FROM EmployeeUserAsignation WHERE UserId = @ApplicationUserId;
	IF @exists = 0
	BEGIN
		INSERT INTO EmployeeUserAsignation
		(
			[UserId],
			[EmployeeId],
			[CompanyId]
		)
		VALUES
		(
			@ApplicationUserId,
			@EmployeeId,
			@CompanyId
		)
	END
	ELSE
	BEGIN
		UPDATE EmployeeUserAsignation SET
			EmployeeId = @EmployeeId
		WHERE
			UserId = @ApplicationUserId
		AND	CompanyId = @CompanyId
	END
	
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_Delete]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_Delete]
	@UserItemId bigint,
	@CompanyId int,
	@UserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ApplicationUser SET
		Status = 0
	WHERE
		Id = @UserItemId
	AND CompanyId = @CompanyId

								   
	  
					  
						   
		
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_GetById]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_GetById]
	@UserId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		AU.Id,
		AU.Login,
		AU.MustResetPassword,
		AU.Status,
		ISNULL(G.SecurityGroupId, 0) AS SecurityGroupId,
		ISNULL(SG.Name,'') AS SecurityGroupName,
		ISNULL(E.Id,0) AS EmployeeId,
		ISNULL(E.Name,'') AS EmployeeName,
		ISNULL(E.LastName,'') AS EmployeeLastName,
		ISNULL(AU.Email,'') AS Email,
		ISNULL(AU.PrimaryUser, 0) AS PrimaryUser,
		ISNULL(AU.[Admin], 0),
		ISNULL(AU.[Language], C.[Language]) AS Language
	FROM ApplicationUser AU WITH(NOLOCK)
	INNER JOIN Company C WITH(NOLOCK)
	ON	 C.Id = AU.CompanyId
	LEFT JOIN ApplicationUserSecurityGroupMembership G WITH(NOLOCK)
		INNER JOIN SecurityGroup SG WITH(NOLOCK)
		ON SG.Id = G.SecurityGroupId
	ON	G.CompanyId = AU.CompanyId
	AND G.ApplicationUserId = AU.Id
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	E.Id = EUA.EmployeeId
	ON	 EUA.UserId = AU.Id

	WHERE
		AU.Id = @UserId
	AND AU.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_GetCommpanies]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[ApplicationUser_GetCommpanies]
	@ApplicationUserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		C.Id,
		C.Logo,
		C.Name
	FROM ApplicationUserCompany AUC WITH(NOLOCK)
	INNER JOIN Company C WITH(NOLOCK)
	ON	C.Id = AUC.CompanyId

	WHERE
		AUC.UserId = @ApplicationUserId
END



GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_GetEffectiveGrants]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_GetEffectiveGrants]
	@ApplicationUserId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		AI.Id,
		AI.Description,
		--ISNULL(AG.GrantToRead,0) AS WrantToRead,
		--ISNULL(AG.GrantToWrite,0) AS GrantToWrite,
		--ISNULL(AG.GrantToDelete,0) AS GrantToDelete,
		CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead, 0) END AS BIT) AS GrantToRead,
		CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToWrite, 0) END AS BIT) AS GrantToWrite,
		CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToDelete, 0) END AS BIT) AS GrantToDelete,
		AI.UrlList
    FROM ApplicationItem AI WITH(NOLOCK)
    LEFT JOIN ApplicationGrant AG
    ON AG.ItemId = Ai.Id
    AND AG.UserId = @ApplicationUserId
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON AU.Id = @ApplicationUserId
    
    WHERE
		AI.Container = 0
	AND (AI.Id < 21 OR @ApplicationUserId = 2)
    
    
    
    ORDER BY Ai.Id
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_GetGrants]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_GetGrants]
	@ApplicationUserId int
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	/*SELECT
	@ApplicationUserId,
	AI.Id,
	CAST(CASE WHEN AI.Id = 1 THEN 1 ELSE ISNULL(AG.GrantToRead,0) END AS bit) AS GrantToRead,
	CAST(ISNULL(AG.GrantToWrite,0) AS bit) AS GrantToWrite,
	CAST(ISNULL(AG.GrantToWrite,0) AS bit) AS GrantToDelete,
	AI.UrlList,
	AI.Description
	FROM ApplicationItem AI WITH(NOLOCK)
	LEFT JOIN ApplicationGrant AG WITH(NOLOCK)
	ON	AG.ItemId = AI.Id
	AND AG.UserId = @ApplicationUserId
	WHERE
		(AI.Id < 21 OR @ApplicationUserId = 2)*/

		SELECT
		@ApplicationUserId,
		AI.Id,
		CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToRead, 0) END AS BIT) AS GrantToRead,
		CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToWrite, 0) END AS BIT) AS GrantToWrite,
		CAST(CASE WHEN AU.Admin = 1 THEN 1 ELSE ISNULL(AG.GrantToDelete, 0) END AS BIT) AS GrantToDelete,
		ISNULL(AI.UrlList,'') AS UrlList,
		AI.[Description]
	FROM ApplicationItem AI WITH(NOLOCK)
	LEFT JOIN ApplicationGrant AG WITH(NOLOCK)
	ON	AG.ItemId = AI.Id
	AND AG.UserId = @ApplicationUserId
		LEFT JOIN ApplicationUser AU WITH(NOLOCK)
		ON AU.Id = @ApplicationUserId
	WHERE
		(AI.Id < 21 OR @ApplicationUserId = 2)
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_GetShortcutAvailables]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_GetShortcutAvailables]
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		MS.Id,
		MS.Label,
		MS.Link,
		MS.Icon
	FROM MenuShortcuts MS WITH(NOLOCK)
	INNER JOIN ApplicationGrant AG WITH(NOLOCK)
	ON	AG.UserId = @UserId
	AND	CAST(AG.ItemId AS nvarchar(50)) = MS.SecurityGroupId
	INNER JOIN ApplicationItem AI WITH(NOLOCK)
	ON	AI.Id = AG.ItemId
		
	
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_Insert]    Script Date: 24/10/2018 20:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ApplicationUser_Insert]
	@Id int output,
	@CompanyId int,
	@Login nvarchar(50),
	@Email nvarchar(50),
	@Language nvarchar(50),
	@Password nvarchar(50),
	@Admin bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[ApplicationUser]
           ([CompanyId]
           ,[Login]
           ,[Password]
		   ,[Email]
		   ,[Admin]
           ,[Status]
           ,[LoginFailed]
           ,[MustResetPassword]
           ,[Language]
           ,[ShowHelp]
           ,[Avatar])
     VALUES
           (@CompanyId,
           @Login
           ,@Password
		   ,@Email
		   ,@Admin
           ,1
           ,0
           ,1
		   ,@Language
           ,0
           ,'avatar2.png')

		   SET @Id = @@IDENTITY

	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,1,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,2,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,3,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,4,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,5,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,6,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,7,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,8,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,9,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,10,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,11,0,0,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,12,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,13,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,14,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,15,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,16,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,17,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,18,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,19,0,0,0,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@Id,20,0,0,0,1,GETDATE(),1,GETDATE())

END




GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_ResetPassword]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_ResetPassword]
	@UserId int,
	@CompanyId int,
	@Result int out,
	@UserName nvarchar(50) out,
	@Password nvarchar(50) out,
	@Email nvarchar(50) out
AS
BEGIN

	SELECT *
	FROM ApplicationUser A WITH(NOLOCK)
	WHERE
		A.Id = @UserId
	AND A.CompanyId = @CompanyId
	
	IF @@ROWCOUNT = 0 
	BEGIN
		SET @Result = 0
	END
	ELSE
	BEGIN
		SELECT @Password = [dbo].GeneratePassword(6)
		SET @Result = 1
		UPDATE ApplicationUser SET
			[Password] = @Password,
			MustResetPassword = 1
		WHERE
			Id = @UserId
		AND CompanyId = @CompanyId

		SELECT
			@UserName = [Login],
			@Password = [Password],
			@Email = Email
		FROM ApplicationUser WITH(NOLOCK)
		WHERE
			Id = @UserId
		AND CompanyId = @CompanyId
			
			
		
		INSERT INTO ActivityLog
		(
			ActivityId,
			UserId,
			CompanyId,
			TargetType,
			TargetId,
			ActionId,
			DateTime,
			ExtraData
		)
		VALUES
		(
			NEWID(),
			@UserId,
			@CompanyId,
			2,
			@UserId,
			4,
			GETDATE(),
			''
		)
    END
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_RevokeGrant]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_RevokeGrant]
	@ApplicationUserId int,
	@CompanyId int,
	@SecurityGroupId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE ApplicationUserSecurityGroupMembership
	WHERE
		ApplicationUserId = @ApplicationUserId
	AND CompanyId = @CompanyId
	AND SecurityGroupId = @SecurityGroupId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		2,
		@ApplicationUserId,
		9,
		GETDATE(),
		''
    )
	
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_SetGrant]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_SetGrant]
	@ApplicationUserId int,
	@CompanyId int,
	@SecurityGroupId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE ApplicationUserSecurityGroupMembership
	WHERE
		ApplicationUserId = @ApplicationUserId
	AND CompanyId = @CompanyId
	AND SecurityGroupId = @SecurityGroupId
	
	INSERT INTO ApplicationUserSecurityGroupMembership
	(
		ApplicationUserId,
	    SecurityGroupId,
	    CompanyId
	)
	VALUES
	(
		@ApplicationUserId,
		@SecurityGroupId,
		@CompanyId
	)	
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		2,
		@ApplicationUserId,
		8,
		GETDATE(),
		''
    )
	
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_SetLanguage]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ApplicationUser_SetLanguage]
	@ApplicationUserId int,
	@Language nvarchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ApplicationUser SET
		[Language] = @Language
	WHERE
		Id = @ApplicationUserId
END




GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_SetPassword]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_SetPassword]
	@UserId int,
	@Password nvarchar(50)
AS
BEGIN

	UPDATE ApplicationUser SET
		[Password] = @Password,
		[MustResetPassword] = 0
	WHERE
		Id = @UserId
END
GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_Update]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ApplicationUser_Update]
	@ApplicationUserId int,
	@UserName nvarchar(50),
	@Email nvarchar(50),
	@Language nvarchar(50),
	@Admin bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ApplicationUser SET
		[Login] = @UserName,
		[Email] = @Email,
		[Language] = @Language,
		[Admin] = @Admin
	WHERE
		Id = @ApplicationUserId
END




GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_UpdateShortCut]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUser_UpdateShortCut]
	@ApplicationUserId int,
	@CompanyId int,
	@Green int,
	@Blue int,
	@Yellow int,
	@Red int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE FROM UserShortCuts
    WHERE
		ApplicationUserId = @ApplicationUserId
	AND CompanyId = @CompanyId
	
	INSERT INTO UserShortCuts
	(
		ApplicationUserId,
		CompanyId,
		ShorcutGreen,
		ShorcutBlue,
		ShortcutYellow,
		ShortcutRed
	)
	VALUES
	(
		@ApplicationUserId,
		@CompanyId,
		@Green,
		@Blue,
		@Yellow,
		@Red
	)
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_UpdateShortCut2]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ApplicationUser_UpdateShortCut2]
	@ApplicationUserId int,
	@CompanyId int,
	@Green int,
	@Blue int,
	@Yellow int,
	@Red int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @NewId int

    DELETE FROM UserShortCuts
    WHERE
		ApplicationUserId = @ApplicationUserId
	AND CompanyId = @CompanyId
	
	SELECT @NewId = MAX(Id) FROM UserShortCuts
	SET @NewId = @NewId+1
	
	INSERT INTO UserShortCuts
	(
		Id,
		ApplicationUserId,
		CompanyId,
		ShorcutGreen,
		ShorcutBlue,
		ShortcutYellow,
		ShortcutRed
	)
	VALUES
	(
		@NewId,
		@ApplicationUserId,
		@CompanyId,
		@Green,
		@Blue,
		@Yellow,
		@Red
	)

END



GO

/****** Object:  StoredProcedure [dbo].[ApplicationUser_UpdateUserInterface]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[ApplicationUser_UpdateUserInterface]
	@ApplicationUserId int,
	@CompanyId int,
	@Language nvarchar(2),
	@ShowHelp bit
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE ApplicationUser SET
		ShowHelp = @ShowHelp,
		Language = @Language
	WHERE
		Id = @ApplicationUserId
	AND CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUserGrant_Clear]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUserGrant_Clear]
	@ApplicationUserId int
AS
BEGIN
	SET NOCOUNT ON;

    
    DELETE FROM ApplicationGrant WHERE UserId = @ApplicationUserId
END





GO

/****** Object:  StoredProcedure [dbo].[ApplicationUserGrant_Save]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ApplicationUserGrant_Save]
	@ApplicationUserId int,
	@ItemId int,
	@GrantToRead bit,
	@GrantToWrite bit,
	@GrantToDelete bit,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @TOTAL int
    
    SELECT @TOTAL = COUNT(*)
    FROM ApplicationGrant AG WITH(NOLOCK)
    WHERE
		AG.ItemId = @ItemId
	AND AG.UserId = @ApplicationUserId
	
	IF @TOTAL = 0
		BEGIN
			INSERT INTO ApplicationGrant
           ([UserId]
           ,[ItemId]
           ,[GrantToRead]
           ,[GrantToWrite]
           ,[GrantToDelete]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn])
     VALUES
           (@ApplicationUserId
           ,@ItemId
           ,@GrantToRead
           ,@GrantToWrite
           ,@GrantToDelete
           ,@UserId
           ,GETDATE()
           ,@UserId
           ,GETDATE())
		END
	ELSE
		BEGIN
		
		UPDATE ApplicationGrant SET 
		  [GrantToRead] = @GrantToRead,
		  [GrantToWrite] = @GrantToWrite,
		  [GrantToDelete] = @GrantToDelete,
		  [ModifiedBy] = @UserId,
		  [ModifiedOn] = GETDATE()
		WHERE
			UserId = @ApplicationUserId
		AND ItemId = @ItemId		
		END

	SELECT @TOTAL AS TOTAL
END





GO

/****** Object:  StoredProcedure [dbo].[BusinessRisk_Active]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[BusinessRisk_Active]
	@BusinessRiskId int,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE BusinessRisk3 SET
		Active = 1,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @BusinessRiskId
	AND	CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		5,
		@BusinessRiskId,
		3,
		GETDATE(),
		@Reason
    )
	
	
END



GO

/****** Object:  StoredProcedure [dbo].[BusinessRisk_Delete]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[BusinessRisk_Delete]
	@BusinessRiskId bigint,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE BusinessRisk3 SET
		Active = 0,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @BusinessRiskId
	AND	CompanyId = @CompanyId

	--UPDATE IncidentAction SET
	--	Active = 0,
	--	ModifiedOn = GETDATE(),
	--	ModifiedBy = @UserId
	--WHERE
	--	BusinessRiskId = @BusinessRiskId
	--AND CompanyId = @CompanyId
	--END
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		5,
		@BusinessRiskId,
		3,
		GETDATE(),
		@Reason
    )
	
	
END



GO

/****** Object:  StoredProcedure [dbo].[BusinessRisk_Filter]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[BusinessRisk_Filter]
	@CompanyId int,
	@DateFrom datetime,
	@DateTo datetime,
	@RulesId bigint,
	@ProcessId bigint,
	@Type int
AS
BEGIN
	SET NOCOUNT ON;	
	SELECT
	*
	FROM
	(
		SELECT
			B.Id AS BusinessRiskId,
			B.DateStart AS OpenDate,
			B.[Description],
			B.Code,
			B.ModifiedOn AS CloseDate,		
			B.ProcessId,	
			PRO.Description As ProcessDescription,
			B.RuleId,
			R.Description As RuleDescription,
			CASE WHEN B.FinalDate IS NULL THEN ISNULL(B.StartResult,0) ELSE ISNULL(B.FinalResult,0) END AS StartResult,
			--B.FinalResult AS FinalResult,
			CASE WHEN B.FinalDate IS NULL THEN ISNULL(B.StartResult,0) ELSE ISNULL(B.FinalResult,0) END AS FinalResult,
			R.Limit,
			ISNULL(B.StartAction,0) AS StartAction,
			ISNULL(B.FinalAction,0) AS FinalAction,
			B.Assumed,			
			CASE WHEN B.Assumed = 1 OR FinalAction = 1 AND B.FinalDate IS NOT NULL THEN 1	
			ELSE
				CASE WHEN B.StartResult = 0 THEN 4
				ELSE
					CASE WHEN B.FinalResult > 0 AND B.FinalResult >= R.Limit THEN 2
					ELSE 
						CASE WHEN B.StartResult >= R.Limit AND B.FinalDate IS NULL THEN 2
						ELSE
						3
						END
					END
				END
			END AS Status

		FROM BusinessRisk3 B WITH(NOLOCK)
		Inner Join Proceso PRO With (Nolock)
		On PRO.Id = B.ProcessId
		Inner Join Rules R With (Nolock)
		On R.Id = B.RuleId
		WHERE
			B.CompanyId = @CompanyId
		AND B.Active = 1
		AND	(@DateFrom IS NULL OR B.DateStart >= @DateFrom)
		AND (@DateTo IS NULL OR B.DateStart <= @DateTo)
		AND (@RulesId IS NULL OR B.RuleId = @RulesId)
		AND (@ProcessId IS NULL OR B.ProcessId = @ProcessId)
		--AND (@Type <> 1 OR @Type IS NULL OR (@Type = 1 AND (B.Assumed = 1 OR B.FinalAction = 1)))
		--AND ((@Type <> 2 OR @Type IS NULL OR (@Type = 2 AND ISNULL(B.FinalResult, B.StartResult) >= R.Limit) AND B.Assumed = 0 AND (B.FinalAction <> 1 OR B.FinalAction IS NULL)))
		--AND ((@Type <> 3 OR @Type IS NULL OR (@Type = 3 AND ISNULL(B.FinalResult, B.StartResult) < R.Limit) AND B.Assumed = 0 AND B.FinalAction <> 1 AND B.StartResult > 0))
		--AND ((@Type <> 4 OR @Type IS NULL OR (@Type = 4 AND B.StartResult = 0) AND B.Assumed = 0 AND B.FinalAction <> 1))
		
	) AS Data

	ORDER BY
		Data.Code
END





GO

/****** Object:  StoredProcedure [dbo].[BusinessRisk_GetActive]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[BusinessRisk_GetActive]
	@CompanyId int
AS
	SELECT DISTINCT
	B.Id,
	B.Description,
	B.CreatedBy,
	B.CreatedOn,
	CB.Login As CreatedByName,
	B.ModifiedBy,
	B.ModifiedOn,
	MB.Login As ModifiedByName,
	B.Code,
	B.ItemDescription,
	B.StartControl,
	B.Notes,
	B.Result,
	B.ApplyAction,
	B.RuleId,
	R.Description As RuleDescription,
	R.Limit As RuleRangeId,
	B.ProbabilityId,
	B.SecurityId,
	B.Active,
	B.InitialValue,
	B.DateStart,
	B.ProcessId,
	PRO.Description As ProcessDescription,
	CAST (CASE WHEN B.FinalAction = 1 THEN 1 ELSE B.Assumed END AS BIT) AS Assumed,
	B.PreviousBusinessRiskId,
	B2.Id,
	B2.PreviousBusinessRiskId
	From BusinessRisk3 B with (Nolock)
		LEFT JOIN BusinessRisk B2 WITH(NOLOCK)
		ON	B2.PreviousBusinessRiskId = B.Id
	Inner Join Rules R With (Nolock)
	On R.Id = B.RuleId
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = B.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = B.ModifiedBy
	Inner Join Proceso PRO With (Nolock)
	On PRO.Id = B.ProcessId

	Where 
		B.CompanyId = @CompanyId
	And B.Active = 1
	AND B2.Id IS NULL
	Order By B.Code, B.Id
RETURN 0



GO

/****** Object:  StoredProcedure [dbo].[BusinessRisk_GetAll]    Script Date: 24/10/2018 20:20:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[BusinessRisk_GetAll]
	@CompanyId int
AS
	SELECT DISTINCT
	B.Id,
	B.Description,
	B.CreatedBy,
	B.CreatedOn,
	CB.Login As CreatedByName,
	B.ModifiedBy,
	B.ModifiedOn,
	MB.Login As ModifiedByName,
	B.Code,
	B.ItemDescription,
	B.StartControl,
	B.Notes,
	ISNULL(B.StartResult,0) AS StartResult,
	ISNULL(B.StartAction,0) AS StartAction,
	B.RuleId,
	R.Description As RuleDescription,
	R.Limit As RuleRangeId,
	ISNULL(B.StartProbability,0) AS StartProbability,
	ISNULL(B.StartSeverity,0) AS StartSeverity,
	B.Active,
	B.InitialValue,
	B.DateStart,
	B.ProcessId,
	PRO.Description As ProcessDescription,
	CAST (CASE WHEN B.FinalAction = 1 THEN 1 ELSE B.Assumed END AS BIT) AS Assumed,
	B.PreviousBusinessRiskId,
	B.Causes,
	ISNULL(B.FinalProbability,0) AS FinalProbability,
	ISNULL(B.FinalSeverity,0) AS FinalSeverity,
	ISNULL(B.FinalResult,0) AS FinalResult,
	B.FinalDate,
	ISNULL(b.FinalAction,0) AS FinalAction,
	ISNULL(b.StartAction,0) AS StartAction,
	ISNULL(b.StartResult,0) AS StartResult
	From BusinessRisk3 B with (Nolock)
	Inner Join Rules R With (Nolock)
	On R.Id = B.RuleId
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = B.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = B.ModifiedBy
	Inner Join Proceso PRO With (Nolock)
	On PRO.Id = B.ProcessId

	WHERE
		B.CompanyId = @CompanyId
	ORDER BY
		B.Code, B.Id
RETURN 0



GO

/****** Object:  StoredProcedure [dbo].[BusinessRisk_GetById]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[BusinessRisk_GetById]
	@CompanyId int,
	@Id bigint
AS
	SELECT DISTINCT
		B.Id,
		B.Description,
		B.CreatedBy,
		B.CreatedOn,
		CB.Login As CreatedByName,
		B.ModifiedBy,
		B.ModifiedOn,
		MB.Login As ModifiedByName,
		B.Code,
		B.ItemDescription,
		B.StartControl,
		B.Notes,
		ISNULL(B.StartResult,0) AS StartResult,
		ISNULL(B.StartAction,0) AS StartAction,
		B.RuleId,
		R.Description As RuleDescription,
		R.Limit As RuleRangeId,
		ISNULL(B.StartProbability,0) AS StartProbability,
		ISNULL(B.StartSeverity,0) AS StartSeverity,
		B.Active,
		B.InitialValue,
		B.DateStart,
		B.ProcessId,
		PRO.Description As ProcessDescription,
		B.Assumed,
		B.PreviousBusinessRiskId,
		B.Causes,
		ISNULL(B.FinalProbability,0) AS FinalProbability,
		ISNULL(B.FinalSeverity,0) AS FinalSeverity,
		ISNULL(B.FinalResult,0) AS FinalResult,
		B.FinalDate,
		ISNULL(b.FinalAction,0) AS FinalAction,
		ISNULL(b.StartAction,0) AS StartAction,
		ISNULL(b.StartResult,0) AS StartResult

	From BusinessRisk3 B with (Nolock)
	Inner Join Rules R With (Nolock)
	On R.Id = B.RuleId
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = B.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = B.ModifiedBy
	Inner Join Proceso PRO With (Nolock)
	On PRO.Id = B.ProcessId

	Where B.CompanyId = @CompanyId And B.Id = @Id
	Order By B.Code, B.Id
RETURN 0



GO

/****** Object:  StoredProcedure [dbo].[BusinessRisk_GetByRules]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[BusinessRisk_GetByRules]
	@RulesId bigint,
	@CompanyId int
AS
	SELECT DISTINCT
	B.Id,
	B.Description,
	B.CreatedBy,
	B.CreatedOn,
	CB.Login As CreatedByName,
	B.ModifiedBy,
	B.ModifiedOn,
	MB.Login As ModifiedByName,
	B.Code,
	B.ItemDescription,
	B.StartControl,
	B.Notes,
	ISNULL(B.StartResult,0) AS StartResult,
	ISNULL(B.StartAction,0) AS StartAction,
	B.RuleId,
	R.Description As RuleDescription,
	R.Limit As RuleRangeId,
	ISNULL(B.StartProbability,0) AS StartProbability,
	ISNULL(B.StartSeverity,0) AS StartSeverity,
	B.Active,
	B.InitialValue,
	B.DateStart,
	B.ProcessId,
	PRO.Description As ProcessDescription,
	B.Assumed,
	B.PreviousBusinessRiskId,
	B.Causes,
	ISNULL(B.FinalProbability,0) AS FinalProbability,
	ISNULL(B.FinalSeverity,0) AS FinalSeverity,
	ISNULL(B.FinalResult,0) AS FinalResult,
	B.FinalDate,
	ISNULL(b.FinalAction,0) AS FinalAction,
	ISNULL(b.StartAction,0) AS StartAction,
	ISNULL(b.StartResult,0) AS StartResult
	From BusinessRisk3 B with (Nolock)
	Inner Join Rules R With (Nolock)
	On R.Id = B.RuleId
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = B.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = B.ModifiedBy
	Inner Join Proceso PRO With (Nolock)
	On PRO.Id = B.ProcessId

	WHERE 
		B.CompanyId = @CompanyId 
	AND	B.Active = 1
	AND B.RuleId = @RulesId
	ORDER BY 
		B.Code, B.Id




GO

/****** Object:  StoredProcedure [dbo].[BusinessRisk_GetHistory]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[BusinessRisk_GetHistory]
	@Code bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		B.Code,
		B.Id,
		B.PreviousBusinessRiskId,
		B.DateStart,
		B.InitialValue,
		ISNULL(B.StartResult,0) AS StartValue,
		ISNULL(B.StartAction, 0 ) AS StartValue
	FROM BusinessRisk3 B WITH(NOLOCK)
	WHERE
		B.Code = @Code and
		B.CompanyId = @CompanyId

ORDER BY
	B.Code,
	B.PreviousBusinessRiskId
END




GO

/****** Object:  StoredProcedure [dbo].[BusinessRisk_Insert]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[BusinessRisk_Insert]
	@Id bigint out,
	@CompanyId int,
	@Description nvarchar(100),
	@Code int,
	@RuleId bigint,
	@ProcessId bigint,
	@PreviousBusinessRiskId bigint,
	@ItemDescription nvarchar(2000),
	@Notes nvarchar(2000),
	@Causes nvarchar(2000),
	@StartControl nvarchar(2000),
	@StartProbability int,
	@StartSeverity int,
	@StartResult int,
	@StartAction int,
	@Assumed bit,
	@UserId int,
	@DateStart datetime
AS
BEGIN
	If @Code = 0 
	Begin
		SELECT @Code = ISNULL(MAX(I.Code),1) + 1
		FROM BusinessRisk3 I WITH(NOLOCK)
		WHERE
			I.CompanyId = @CompanyId
	End

	INSERT INTO BusinessRisk3
	(
		CompanyId,
		Description,
		Code,
		RuleId,
		PreviousBusinessRiskId,
		ItemDescription,
		Notes,
		Causes,
		StartProbability,
		StartSeverity,
		StartResult,
		StartAction,
        CreatedBy,
        CreatedOn,
        ModifiedBy,
        ModifiedOn,
		Active,
		DateStart,
		StartControl,
		ProcessId,
		ProbabilityId,
		SecurityId,
		Result,
		Assumed,
		ApplyAction
				   
				
			  
			 
	)
    VALUES
	(
		@CompanyId,
        @Description,
		@Code,
		@RuleId,
		@PreviousBusinessRiskId,
		@ItemDescription,
		@Notes,
		@Causes,
		@StartProbability,
		@StartSeverity,
		@StartResult,
        @StartAction,
        @UserId,
        GETDATE(),
        @UserId,
        GETDATE(),
        1,
		@DateStart,
		@StartControl,
		@ProcessId,
		0,
		0,
		0,
		@Assumed,
	
	
	
	
		0
	)

	Set @Id = @@Identity

	UPDATE IncidentAction SET
		BusinessRiskId = @Id
	WHERE
		BusinessRiskId = @PreviousBusinessRiskId
	AND @PreviousBusinessRiskId IS NOT NULL
	AND ClosedOn IS NULL
END






GO

/****** Object:  StoredProcedure [dbo].[BusinessRisk_Update]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[BusinessRisk_Update]
	@Id bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@Code int,
	@RuleId bigint,
	@ProcessId bigint,
	@ItemDescription nvarchar(2000),
	@Notes nvarchar(2000),
	@Causes nvarchar(2000),
	@StartProbability int,
	@StartSeverity int,
	@StartResult int,
	@StartAction int,
	@InitialValue int,
	@DateStart datetime,
	@StartControl nvarchar(2000),
	@FinalProbability int,
	@FinalSeverity int,
	@FinalResult int,
	@FinalAction int,
	@FinalDate datetime,
	@Assumed bit,
	@UserId int
AS
BEGIN
	UPDATE BusinessRisk3 SET
	
		CompanyId = @CompanyId,
		Description = @Description,
		Code = @Code,
		RuleId = @RuleId,
		ItemDescription = @ItemDescription,
		Notes = @Notes,
		Causes = @Causes,
		InitialValue = @InitialValue,
		DateStart = @DateStart,
		ProcessId = @ProcessId,
		StartControl = @StartControl,

		StartProbability = @StartProbability,
		StartSeverity = @StartSeverity,
		StartResult = @StartResult,
		StartAction = @StartAction,

		FinalProbability = @FinalProbability,
		FinalSeverity = @FinalSeverity,
		FinalResult = @FinalResult,
		FinalAction = @FinalAction,
		FinalDate = @FinalDate,
		Assumed = @Assumed,

        ModifiedBy = @UserId,
        ModifiedOn = GETDATE()

	WHERE
		Id = @Id AND CompanyId = @CompanyId
END






GO

/****** Object:  StoredProcedure [dbo].[BusinessRiskAction_Insert]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[BusinessRiskAction_Insert]
	@IncidentActionId bigint output,
	@CompanyId int,
	@ActionType int,
	@Description nvarchar(50),
	@Origin int,
	@ReporterType int,
	@DeparmentId int,
	@ProviderId int,
	@CustomerId int,
	@Number bigint,
	@BusinessRiskId bigint,
	@WhatHappend nvarchar(255),
	@WhatHappendBy bigint,
	@WhatHappendDate datetime,
	@Causes nvarchar(255),
	@CausesBy bigint,
	@CausesDate datetime,
	@Actions nvarchar(255),
	@ActionsBy bigint,
	@ActionsExecuter int,
	@ActionsSchedule datetime,
	@ActionsDate datetime,
	@Monitoring nvarchar(255),
	@ClosedBy bigint,
	@ClosedDate datetime,
	@ClosedExecutor bigint,
	@ClosedExecutorOn datetime,
	@Notes text,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT @Number = ISNULL(MAX(Number) ,0) + 1
	FROM IncidentAction I WITH(NOLOCK)
	WHERE
		I.CompanyId = @CompanyId
	AND I.Active = 1

    INSERT INTO IncidentAction
    (
		CompanyId,
		ActionType,
		Description,
		Origin,
		ReporterType,
		DepartmentId,
		ProviderId,
		CustomerId,
		Number,
		BusinessRiskId,
		WhatHappend,
		WhatHappendBy,
		WhatHappendOn,
		Causes,
		CausesBy,
		CausesOn,
		Actions,
		ActionsBy,
		ActionsOn,
		ActionsExecuter,
		ActionsSchedule,
		Monitoring,
		ClosedBy,
		ClosedOn,
		ClosedExecutor,
		ClosedExecutorOn,
		Notes,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
    VALUES
    (
		@CompanyId,
		@ActionType,
		@Description,
		@Origin,
		@ReporterType,
		@DeparmentId,
		@ProviderId,
		@CustomerId,
		@Number,
		@BusinessRiskId,
		@WhatHappend,
		@WhatHappendBy,
		@WhatHappendDate,
		@Causes,
		@CausesBy,
		@CausesDate,
		@Actions,
		@ActionsBy,
		@ActionsDate,
		@ActionsExecuter,
		@ActionsSchedule,
		@Monitoring,
		@ClosedBy,
		@ClosedDate,
		@ClosedExecutor,
		@ClosedExecutorOn,
		@Notes,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()		
	)

	SET @IncidentActionId = @@IDENTITY

END






GO

/****** Object:  StoredProcedure [dbo].[BusinessRiskAction_Update]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[BusinessRiskAction_Update]
	@IncidentActionId bigint,
	@CompanyId int,
	@Description nvarchar(50),
	@ActionType int,
	@Origin int,
	@Number bigint,
	@ReporterType int,
	@DepartmentId int,
	@ProviderId bigint,
	@CustomerId bigint,
	@BusinessRiskId bigint,
	@WhatHappend nvarchar(155),
	@WhatHappendBy bigint,
	@WhatHappendOn datetime,
	@Causes nvarchar(255),
	@CausesBy bigint,
	@CausesOn datetime,
	@Actions nvarchar(255),
	@ActionsBy bigint,
	@ActionsOn datetime,
	@ActionsExecuter int,
	@ActionsSchedule datetime,
	@Monitoring nvarchar(255),
	@ClosedBy bigint,
	@ClosedOn datetime,
    @ClosedExecutor bigint,
    @ClosedExecutorOn datetime,
	@Notes text,
	@UserId int 
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE IncidentAction SET
		ActionType = @ActionType,
		Description = @Description,
		Origin = @Origin,
		ReporterType = @ReporterType,
		DepartmentId = @DepartmentId,
		ProviderId = @ProviderId,
		CustomerId = @CustomerId,
		Number = @Number,
		BusinessRiskId = @BusinessRiskId,
		WhatHappend = @WhatHappend,
		WhatHappendBy = @WhatHappendBy,
		WhatHappendOn = @WhatHappendOn,
		Causes = @Causes,
		CausesBy = @CausesBy,
		CausesOn = @CausesOn,
		Actions = @Actions,
		ActionsBy = @ActionsBy,
		ActionsOn = @ActionsOn,
		ActionsExecuter = @ActionsExecuter,
		ActionsSchedule = @ActionsSchedule,
		Monitoring = @Monitoring,
		ClosedBy = @ClosedBy,
		ClosedOn = @ClosedOn,
		ClosedExecutor = @ClosedExecutor,
		ClosedExecutorOn = @ClosedExecutorOn,
		Notes = @Notes,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @IncidentActionId
	AND CompanyId = @CompanyId


END






GO

/****** Object:  StoredProcedure [dbo].[Company_Create]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Company_Create]
	@CompanyId int out,
	@Login nvarchar(50) out,
	@Password nvarchar(50) out,
	@Name nvarchar(50),
	@Code nvarchar(10),
	@NIF nvarchar(15),
	@Address nvarchar(50),
	@PostalCode nvarchar(10),
	@City nvarchar(50),
	@Province nvarchar(50),
	@Country nvarchar(15),
	@Phone nvarchar(15),
	@Mobile nvarchar(15),
	@Fax nvarchar(15),
	@UserName nvarchar(50),
	@Email nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO Company
    (
		Name,
		SubscriptionStart,
		SubscriptionEnd,
		Language,
		DefaultAddress,
		[NIF-CIF],
		Code,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@Name,
		GETDATE(),
		GETDATE()+365,
		'es',
		null,
		@NIF,
		@Code,
		1,
		GETDATE()
	)
	
	SET @CompanyId = @@IDENTITY
	DECLARE @CompanyAddressId int
	
	INSERT INTO CompanyAddress
	(
		CompanyId,
		Address,
		PostalCode,
		City,
		Province,
		Country,
		Phone,
		Mobile,
		Email,
		Notes,
		Fax,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@Address,
		@PostalCode,
		@City,
		@Province,
		@Country,
		@Phone,
		@Mobile,
		@Email,
		'',
		@Fax,
		1,
		1,
		GETDATE(),
		1,
		GETDATE()
	)
	
	SET @CompanyAddressId = @@IDENTITY
	
	UPDATE Company SET DefaultAddress = @CompanyAddressId WHERE Id = @CompanyId

	INSERT INTO Document_Category (CompanyId,Description,Editable) VALUES (@CompanyId,'Instrucciones',0)
	INSERT INTO Document_Category (CompanyId,Description,Editable) VALUES (@CompanyId,'Procedimientos',0)
	INSERT INTO Document_Category (CompanyId,Description,Editable) VALUES (@CompanyId,'Registros',0)

	INSERT INTO Procedencia (CompanyId,Description,Editable) VALUES (@CompanyId,'Normas',0)
	INSERT INTO Procedencia (CompanyId,Description,Editable) VALUES (@CompanyId,'Legislación internacional',0)
	INSERT INTO Procedencia (CompanyId,Description,Editable) VALUES (@CompanyId,'Legislación nacional',0)
	INSERT INTO Procedencia (CompanyId,Description,Editable) VALUES (@CompanyId,'Legislación autonómica',0)
	INSERT INTO Procedencia (CompanyId,Description,Editable) VALUES (@CompanyId,'Legislación local',0)
		
	SET @Password = [dbo].GeneratePassword(6)

	INSERT INTO ApplicationUser
	(
		CompanyId,
		Login,
		Password,
		Status,
		LoginFailed,
		MustResetPassword,
		Language,
		ShowHelp,
		Email,
		PrimaryUser,
		[Admin]
	)
	VALUES
	(
		@CompanyId,
		@UserName,
		@Password,
		1,
		0,
		0,
		'ca',
		1,
		@Email,
		1,
		1
	)
	
	DECLARE @UserId int
	SET @UserId = @@IDENTITY

	
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,1,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,2,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,3,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,4,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,5,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,6,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,7,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,8,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,9,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,10,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,11,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,12,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,13,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,14,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,15,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,16,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,17,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,18,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,19,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,20,1,1,1,1,GETDATE(),1,GETDATE())

	SET @Login = @UserName
END




GO

/****** Object:  StoredProcedure [dbo].[Company_DeleteAddress]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_DeleteAddress]
	@CompanyId int,
	@AddressId int,
	@UserId int	
AS
BEGIN
	UPDATE CompanyAddress SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		CompanyId = @CompanyId
	AND Id = @AddressId
END





GO

/****** Object:  StoredProcedure [dbo].[Company_DepartmentMemberShip]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_DepartmentMemberShip]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		EDM.DepartmentId,
		EDm.EmployeeId
    FROM EmployeeDepartmentMembership EDM WITH(NOLOCK)
    INNER JOIN Employee E WITH(NOLOCK)
    ON	E.Id = EDM.EmployeeId
    AND E.CompanyId = EDM.CompanyId
    AND E.Active = 1
    INNER JOIN Department D WITH(NOLOCK)
    ON	D.Id = EDM.DepartmentId
    AND D.CompanyId = EDM.CompanyId
    AND D.Deleted = 0
    WHERE
		EDM.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetActualDocuments]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Company_GetActualDocuments]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		D.Id AS DocumentId,
		DV.Id AS VersionId,
		D.Description,
		DV.Version,
		DV.UserCreate,
		DV.Status,
		DV.Date
	FROM Document D WITH(NOLOCK)
	INNER JOIN DocumentsVersion DV WITH(NOLOCK)
	ON
		D.CompanyId = @CompanyId
	AND D.CompanyId = DV.Company
	AND D.Id = DV.DocumentId
	AND D.ActualVersion = DV.Version
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetAdress]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Company_GetAdress]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		CA.Id,
		CA.CompanyId,
		ISNULL(CA.Address,'') AS Address,
		ISNULL(CA.PostalCode,'') AS PostalCode,
		ISNULL(CA.City,'') AS City,
		ISNULL(CA.Province,'') AS Province,
		ISNULL(CA.Country,'') AS Country,
		ISNULL(CA.Phone,'') AS Phone,
		ISNULL(CA.Mobile,'') AS Mobile,
		ISNULL(CA.Fax,'') AS Fax,
		ISNULL(CA.Email,'') AS Email,
		ISNULL(CA.Notes,'') AS Notes,
		CASE WHEN CA.Id = C.DefaultAddress THEN 1 ELSE 0 END AS DefaultAddress
	FROM CompanyAddress CA WITH (NOLOCK)
	INNER JOIN Company C WITH(NOLOCK)
	ON	CA.CompanyId = C.Id
	AND C.Id = @CompanyId
	AND CA.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetByCode]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_GetByCode]
	@CompanyCode nvarchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		C.Id
	FROM Company C WITH(NOLOCK)
	WHERE
		C.Code = @CompanyCode
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetById]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_GetById]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		C.Id,
		C.Name,
		C.SubscriptionStart,
		C.SubscriptionEnd,
		C.Language,
		ISNULL(C.[NIF-CIF],'') AS [NIF-CIF],
		ISNULL(C.Code,''),
		ISNULL(C.Logo,''),
		ISNULL(C.DiskQuote,10000000),
		ISNULL(C.Agreement,0) 
	FROM Company C WITH(NOLOCK)
	WHERE
		C.Id = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetCountries]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_GetCountries]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		CC.CountryId,
		CA.Description
	FROM CompanyCountries CC WITH(NOLOCK)
	INNER JOIN CountriesAvialables CA WITH (NOLOCK)
	ON	CC.CountryId = CA.Id	
	WHERE
		CC.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetDepartments]    Script Date: 24/10/2018 20:20:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_GetDepartments]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		D.Id,
		D.Name
	FROM Department D WITH(NOLOCK)
	WHERE
		D.CompanyId = @CompanyId
	AND D.Deleted = 0
	ORDER BY D.Name ASC
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetDocumentCategories]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Company_GetDocumentCategories]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT
		DC.Id,
		DC.Description,
		DC.Editable,
		CAST (CASE WHEN AA.CategoryId IS NULL THEN 1 ELSE 0 END AS BIT) AS Deletable
	FROM Document_Category DC WITH(NOLOCK)
	LEFT JOIN
	(
		SELECT D.CategoryId, D.CompanyId FROM Document D WITH(NOLOCK) WHERE D.Activo = 1 
	) AA
	ON	AA.CompanyId = DC.CompanyId
	AND DC.Id = AA.CategoryId
	WHERE
		DC.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetDocumentProcedencias]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Company_GetDocumentProcedencias]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT
		P.Id,
		P.Description,
		P.Editable,
		CAST (CASE WHEN AA.ProcedenciaId IS NULL THEN 1 ELSE 0 END AS BIT) AS Deletable
	FROM Procedencia P WITH(NOLOCK)	
	LEFT JOIN
	(
		SELECT D.ProcedenciaId, D.CompanyId FROM Document D WITH(NOLOCK)
	) AA
	ON	AA.CompanyId = P.CompanyId
	AND P.Id = AA.ProcedenciaId
	WHERE
		P.CompanyId = @CompanyId
	ORDER BY P.Description ASC
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetDocuments]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Company_GetDocuments]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		D.Id AS DocumentId,
		DV.Id AS VersionId,
		D.Description,
		DV.Version,
		DV.UserCreate,
		DV.Status,
		DV.Date AS DocumentDateVersion,
		ISNULL(D.Codigo,'') AS Codigo,
		ISNULL(DV.Reason,'') AS Reason,
		ISNULL(DC.Id,0) AS CategoryId,
		ISNULL(DC.Description,'') AS CategoryName,
		ISNULL(P.Id,0) AS ProcedenciaId,
		ISNULL(P.Description,'') AS ProcedenciaName,
		ISNULL(AU.Id,0) AS EmployeeCreateId,
		ISNULL(AU.Login, '') AS EmployeeCreateName,
		'' AS EmployeeCreateLastName,
		D.FechaAlta AS StartDate,
		D.FechaBaja AS EndDate,
		D.Conservacion AS ConservationId,
		D.ConservacionType AS ConservationType,
		D.Ubicacion
		
	FROM Document D WITH(NOLOCK)
	INNER JOIN DocumentsVersion DV WITH(NOLOCK)
	ON
		D.CompanyId = @CompanyId
	AND D.CompanyId = DV.Company
	AND D.Id = DV.DocumentId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	DV.UserCreate = AU.Id
	INNER JOIN Document_Category DC WITH(NOLOCK)
	ON	D.CategoryId = DC.Id
	AND D.CompanyId = DC.CompanyId
	LEFT JOIN Procedencia P WITH(NOLOCK)
	ON	D.ProcedenciaId = P.Id
	AND D.CompanyId = P.CompanyId
	WHERE
		D.CompanyId = @CompanyId
	AND D.Activo = 1
						
	
	ORDER BY DV.DocumentId ASC, DV.Version ASC
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetDocumentsInactive]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Company_GetDocumentsInactive]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		D.Id AS DocumentId,
		DV.Id AS VersionId,
		D.Description,
		DV.Version,
		DV.UserCreate,
		DV.Status,
		DV.Date AS DocumentDateVersion,
		ISNULL(D.Codigo,'') AS Codigo,
		ISNULL(DV.Reason,'') AS Reason,
		ISNULL(DC.Id,0) AS CategoryId,
		ISNULL(DC.Description,'') AS CategoryName,
		ISNULL(P.Id,0) AS ProcedenciaId,
		ISNULL(P.Description,'') AS ProcedenciaName,
		ISNULL(D.CreatedBy,0) AS EmployeeCreateId,
		ISNULL(AU.Login, '') AS EmployeeCreateName,
		'' AS EmployeeCreateLastName,
		D.FechaAlta AS StartDate,
		D.FechaBaja AS EndDate,
		D.Conservacion AS ConservationId,
		D.ConservacionType AS ConservationType,
		D.ModifiedOn,
		D.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName
	FROM Document D WITH(NOLOCK)
	INNER JOIN DocumentsVersion DV WITH(NOLOCK)
	ON
		D.CompanyId = @CompanyId
	AND D.CompanyId = DV.Company
	AND D.Id = DV.DocumentId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	DV.UserCreate = AU.Id
	LEFT JOIN Document_Category DC WITH(NOLOCK)
	ON	D.CategoryId = DC.Id
	AND D.CompanyId = DC.CompanyId
	LEFT JOIN Procedencia P WITH(NOLOCK)	
	ON	D.CategoryId = P.Id
	AND D.CompanyId = P.CompanyId
	WHERE
		D.CompanyId = @CompanyId
	AND D.Activo = 1
	AND D.FechaBaja IS NOT NULL
	
	ORDER BY D.FechaBaja DESC, DV.Version ASC
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetEmployees]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_GetEmployees]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		E.Id,
		E.Name,
		E.LastName,
		E.Active AS Active,
		E.FechaBaja AS FechaBaja,
		ISNULL(E.NIF,'') AS NIF,
		ISNULL(E.Email,'') AS Email,
		ISNULL(E.Phone,'') AS Phone,
		ISNULL(E.Address,'') AS Address,
		ISNULL(E.PostalCode,'') AS PostalCode,
		ISNULL(E.City,'') AS City,
		ISNULL(E.Province,'') AS Province,
		ISNULL(E.Country,0) AS Country,
		CASE WHEN AU.Id IS NULL THEN 0 ELSE 1 END AS HasUserAssigned,
		CASE WHEN ACTIONS.EmployeeId IS NULL THEN 0 ELSE 1 END AS HasActionsAssigned
	FROM Employee E WITH(NOLOCK)
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser AU WITH(NOLOCK)
		ON	AU.Id = EUA.UserId
		AND AU.[Status] = 1
	ON	EUA.EmployeeId = E.Id

	LEFT JOIN
	(
		SELECT
			EQ.Resposable AS EmployeeId
		FROM Equipment EQ WITH(NOLOCK)
		WHERE
			EQ.Active = 1

		UNION

		SELECT
			ECD.Responsable AS EmployeeId
		FROM EquipmentCalibrationDefinition ECD WITH(NOLOCK)
		WHERE
			ECD.Active = 1

		UNION

		SELECT
			EVD.Id AS EmployeeId
		FROM EquipmentVerificationDefinition EVD WITH(NOLOCK)
		WHERE
			EVD.Active = 1

		UNION

		SELECT
			EMD.ResponsableId AS EmployeeId
		FROM EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
		WHERE
			EMD.Active = 1

		UNION

		SELECT
			I.Id AS EmployeeId
		FROM Incident I WITH(NOLOCK)
		WHERE
			I.Active = 1
		AND I.ActionsSchedule > GETDATE()
	) ACTIONS
	ON ACTIONS.EmployeeId = E.Id

	WHERE
		E.CompanyId = @CompanyId
	ANd E.Active = 1
	ORDER BY E.Name ASC, E.LastName
	
END





GO

/****** Object:  StoredProcedure [dbo].[Company_GetUserNames]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_GetUserNames]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		UPPER(AU.Login) AS UserName,
		AU.Id AS UserId,
		AU.Status
	FROM ApplicationUser AU WITH(NOLOCK)
	WHERE
		AU.CompanyId = @CompanyId
	AND AU.Status <> 0
	
	ORDER BY UPPER(AU.Login) ASC
END





GO

/****** Object:  StoredProcedure [dbo].[Company_SetCountry]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_SetCountry]
	@CountryId int,
	@CompanyId int
AS
BEGIN
	INSERT INTO CompanyCountries
	(
		CountryId,
		CompanyId
	)
	VALUES
	(
		@CountryId,
		@CompanyId
	)

END





GO

/****** Object:  StoredProcedure [dbo].[Company_SetDefaultAddress]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_SetDefaultAddress]
	@CompanyId int,
	@AddressId int,
	@UserId int	
AS
BEGIN
	UPDATE Company SET
		DefaultAddress = @AddressId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Company_UnSetCountry]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_UnSetCountry]
	@CountryId int,
	@CompanyId int
AS
BEGIN
	DELETE FROM CompanyCountries
	WHERE
		CountryId = @CountryId
	AND	CompanyId = @CompanyId

END





GO

/****** Object:  StoredProcedure [dbo].[Company_Update]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Company_Update]
	@CompanyId int,
	@Name nvarchar(50),
	@Nif nvarchar(15),
	@DefaultAddress int,
	@Language nvarchar(2),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @Data nvarchar(200)
	
	/*SELECT 
		@Data = Name + '->' + @Name +',' +
		ISNULL([NIF-CIF],'')+ '->' + @Nif + ',' +
		CAST(ISNULL(DefaultAddress,0) AS Nvarchar(2)) + '->' + CAST(@DefaultAddress as nvarchar(2)) +
		CAST(ISNULL([Language],'es') AS Nvarchar(2)) + '->' + CAST(@Language as nvarchar(2))
	FROM Company WITH(NOLOCK)
	WHERE
		Id = @CompanyId*/

    UPDATE Company SET
		[NIF-CIF] = @Nif,
		Name = @Name,
		DefaultAddress = @DefaultAddress,
		--[Language] = @Language,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @CompanyId
		
		
	UPDATE CompanyAddress SET
		@DefaultAddress = 0
	WHERE CompanyId = @CompanyId
		
	UPDATE CompanyAddress SET
		@DefaultAddress = 1
	WHERE 
		Id = @DefaultAddress
	AND	CompanyId = @CompanyId
		
	/*INSERT INTO ActivityLog
    (
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		1,
		@CompanyId,
		1,
		GETDATE(),
		@Data
	)*/

END





GO

/****** Object:  StoredProcedure [dbo].[CompanyAddress_Insert]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[CompanyAddress_Insert] 
	-- Add the parameters for the stored procedure here
	@CompanyAddressId int out,
	@CompanyId int,
	@Address nvarchar(100),
	@PostalCode nvarchar(10),
	@City nvarchar(50),
	@Province nvarchar(50),
	@Country nvarchar(15),
	@Phone nvarchar(15),
	@Mobile nvarchar(15),
	@Email nvarchar(50),
	@Notes text,
	@Fax nvarchar(15),
	@UserId int
AS
BEGIN
	INSERT INTO CompanyAddress
	(
		CompanyId,
		Address,
		PostalCode,
		City,
		Province,
		Country,
		Phone,
		Mobile,
		Email,
		Notes,
		Fax,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@Address,
		@PostalCode,
		@City,
		@Province,
		@Country,
		@Phone,
		@Mobile,
		@Email,
		@Notes,
		@Fax,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)

	SET @CompanyAddressId = @@IDENTITY
END





GO

/****** Object:  StoredProcedure [dbo].[CompanyAddress_Update]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[CompanyAddress_Update]
	@CompanyAddressId int,
	@CompanyId int,
	@Address nvarchar(100),
	@PostalCode nvarchar(10),
	@City nvarchar(50),
	@Province nvarchar(50),
	@Country nvarchar(15),
	@Phone nvarchar(15),
	@Mobile nvarchar(15),
	@Email nvarchar(50),
	@Fax nvarchar(15),
	@UserId int
AS
BEGIN
	UPDATE CompanyAddress SET
		Address = @Address,
		PostalCode = @PostalCode,
		City = @City,
		Province = @Province,
		Country = @Country,
		Phone = @Phone,
		Mobile = @Mobile,
		Email = @Email,
		Fax = @Fax,
		Notes = '',
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @CompanyAddressId
	AND CompanyId = @CompanyId


END





GO

/****** Object:  StoredProcedure [dbo].[CompanyAddressInsert]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CompanyAddressInsert]
	@CompanyId int,
	@Address nvarchar(100),
	@PostalCode nvarchar(10),
	@City nvarchar(50),
	@Province nvarchar(50),
	@Country nvarchar(15),
	@Phone nvarchar(15),
	@Mobile nvarchar(15),
	@Fax nvarchar(15),
	@Email nvarchar(50),
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	INSERT INTO CompanyAddress
    (
		CompanyId,
		Address,
		PostalCode,
		City,
		Province,
		Country,
		Phone,
		Mobile,
		Email,
		Notes,
		Fax
    )
    VALUES
    (
		@CompanyId,
		@Address,
		@PostalCode,
		@City,
		@Province,
		@Country,
		@Phone,
		@Mobile,
		@Email,
		'',
		@Fax
	)
     
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		6,
		@@IDENTITY,
		1,
		GETDATE(),
		''
	)
		
END





GO

/****** Object:  StoredProcedure [dbo].[CostDefinition_Activate]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[CostDefinition_Activate]
	@CostDefinitionId bigint,
	@CompanyId int,
	@ApplicationUserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CostDefinition SET
		Active = 1,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @CostDefinitionId
	AND CompanyId = @CompanyId

END




GO

/****** Object:  StoredProcedure [dbo].[CostDefinition_Deactivate]    Script Date: 24/10/2018 20:20:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[CostDefinition_Deactivate]
	@CostDefinitionId bigint,
	@CompanyId int,
	@ApplicationUserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CostDefinition SET
		Active = 0,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @CostDefinitionId
	AND CompanyId = @CompanyId

END




GO

/****** Object:  StoredProcedure [dbo].[CostDefinition_GetAll]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[CostDefinition_GetAll]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		CD.Id,
		CD.CompanyId,
		CD.[Description],
		CD.Amount,
		CD.CreatedBy,
		CB.[Login] AS CreatedByName,
		CD.CreatedOn,
		CD.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		CD.ModifiedOn,
		CD.Active
	FROM CostDefinition CD WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = CD.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = CD.ModifiedBy

	WHERE
		CD.CompanyId = @CompanyId
END




GO

/****** Object:  StoredProcedure [dbo].[CostDefinition_Insert]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[CostDefinition_Insert]
	@CostDefinitionId bigint output,
	@CompanyId int,
	@Description nvarchar(100),
	@Amount decimal(18,3),
	@ApplicationUserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO CostDefinition
	(
		CompanyId,
		[Description],
		Amount,
		Active,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@CompanyId,
		@Description,
		@Amount,
		1,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE()
	)

	SET @CostDefinitionId = @@IDENTITY
END




GO

/****** Object:  StoredProcedure [dbo].[CostDefinition_Update]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[CostDefinition_Update]
	@CostDefinitionId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@Amount decimal(18,3),
	@ApplicationUserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CostDefinition SET
		[Description] = @Description,
		Amount = @Amount,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @CostDefinitionId
	AND CompanyId = @CompanyId

END




GO

/****** Object:  StoredProcedure [dbo].[Countries_GetAll]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Countries_GetAll]
	@CompanyId int
AS
BEGIN
	SELECT DISTINCT
		CA.Id,
		CA.Description,
		CASE WHEN CC.CountryId IS NULL THEN 0 ELSE 1 END AS Selected,
		CASE WHEN E.Id IS NULL THEN
			CASE WHEN AD.Id IS NULL THEN 1
			ELSE 0
			END
		ELSE 0 END AS Deletable,
		Ad.Address,
		E.lastname
	FROM CountriesAvialables CA WITH(NOLOCK)
	LEFT JOIN CompanyCountries CC WITH(NOLOCK)
	ON	CC.CountryId = CA.Id
	AND CC.CompanyId = @CompanyId
	LEFT JOIN Employee E WITH(NOLOCK)
	ON	E.Country = CA.Description
	AND	E.FechaBaja IS NULL
	AND E.CompanyId = @CompanyId
	LEFT JOIN CompanyAddress Ad
	ON	AD.CompanyId = @CompanyId
	AND	AD.Country = CA.Id
	AND AD.Active = 1
	
	ORDER BY CA.Description ASC
END





GO

/****** Object:  StoredProcedure [dbo].[Customer_Delete]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Customer_Delete]
	@CustomerId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE Customer SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @CustomerId
	AND	CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		20,
		@CustomerId,
		2,
		GETDATE(),
		''
    )

END





GO

/****** Object:  StoredProcedure [dbo].[Customer_GetByCompany]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Customer_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT
		C.Id,
		C.CompanyId,
		C.Description,
		C.Active,
		C.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		C.ModifiedOn,
		CASE WHEN I.Id IS NULL THEN 0 ELSE 1 END AS InIncident,
		CASE WHEN IA.Id IS NULL THEN 0 ELSE 1 END AS InActionIncident
    FROM Customer C WITH(NOLOCK)
    INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = C.ModifiedBy
	LEFT JOIN Incident I WITH(NOLOCK)
	ON	I.CustomerId = C.Id
	AND	I.Active = 1
	LEFT JOIN IncidentAction IA WITH(NOLOCK)
	ON	IA.CustomerId = C.Id
	AND	IA.Active = 1
	WHERE
		C.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Customer_GetById]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Customer_GetById]
	@CustomerId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		C.Id,
		C.CompanyId,
		C.Description,
		C.Active,
		C.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		C.ModifiedOn
    FROM Customer C WITH(NOLOCK)
    INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = C.ModifiedBy
	WHERE
		C.Id= @CustomerId
	AND C.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Customer_GetIncidentActions]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Customer_GetIncidentActions]
	@CustomerId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		Item.*
	FROM
	(
		SELECT
			'Incident' AS ItemType,
			I.Id,
			I.Description,
			I.WhatHappendOn,
			I.CausesOn,
			I.ActionsOn,
			I.ClosedOn,
			-1 AS Origin,
			ISNULL(IA.Id,'') AS AssociantedId,
			ISNULL(IA.Description,'') AS AssociatedDescription,
			I.Code AS IncidentCode,
			ISNULL(IA.Number,0) AS ActionCode
		FROM Incident I WITH(NOLOCK)
		LEFT JOIN IncidentAction IA WITH(NOLOCK)
		ON	IA.IncidentId = I.Id
		AND	IA.CompanyId = I.CompanyId
		AND IA.Active = 1
		WHERE
			I.CustomerId = @CustomerId
		AND	I.CompanyId = @CompanyId
		AND I.Active = 1
		
		UNION
		
		
		SELECT
			'Action' AS ItemType,
			IA.Id,
			IA.Description,
			IA.WhatHappendOn,
			IA.CausesOn,
			IA.ActionsOn,
			IA.ClosedOn,
			IA.Origin,
			ISNULL(I.Id,0) AS AssociantedId,
			ISNULL(I.Description,'') AS AssociatedDescription,
			ISNULL(I.Code,0) AS IncidentCode,
			IA.Number AS ActionCode
		FROM IncidentAction IA WITH(NOLOCK)
		LEFT JOIN Incident I WITH(NOLOCK)
		ON	I.Id = IA.IncidentId
		AND I.CompanyId = Ia.CompanyId
		WHERE
			IA.CustomerId = @CustomerId
		AND	IA.CompanyId = @CompanyId
		AND IA.Active = 1
	) AS Item
	
END





GO

/****** Object:  StoredProcedure [dbo].[Customer_Insert]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Customer_Insert]
	@CustomerId bigint output,
	@CompanyId int,
	@Description nvarchar(100),
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Customer
	(
		[CompanyId],
		[Description],
		[Active],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn]
	)
	VALUES
	(
		@CompanyId,
		@Description,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)
	
	SET @CustomerId = @@IDENTITY
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		20,
		@CustomerId,
		1,
		GETDATE(),
		@Description
    )

END





GO

/****** Object:  StoredProcedure [dbo].[Customer_Update]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Customer_Update]
	@CustomerId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE Customer SET
		Description = @Description,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @CustomerId
	AND	CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		20,
		@CustomerId,
		2,
		GETDATE(),
		@Description
    )

END





GO

/****** Object:  StoredProcedure [dbo].[Department_Delete]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Department_Delete]
	@DepartmentId int,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Department SET
		Deleted = 1,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @DepartmentId
	AND	CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		5,
		@DepartmentId,
		3,
		GETDATE(),
		@Reason
    )
	
	
END





GO

/****** Object:  StoredProcedure [dbo].[Department_GetById]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Department_GetById]
	@Id int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		D.Id AS DepartmentId,
		D.Name AS DepartmentDescription,
		D.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		D.ModifiedOn
	FROM Department D WITH(NOLOCK)
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = D.ModifiedBy
	AND AU.CompanyId = D.CompanyId	
	
	WHERE
		D.Id = @Id
	AND D.CompanyId = @CompanyId
	
		
END





GO

/****** Object:  StoredProcedure [dbo].[Department_GetEmployess]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Department_GetEmployess]
	@DepartmentId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT
		C.Id,
		C.Description,
		C2.Id,
		C2.Description,
		E.Id,
		E.Name,
		E.LastName,
		ISNULL(E.NIF,''),
		ISNULL(E.Email,''),
		ISNULL(E.Phone,'')
	FROM Cargos C WITH(NOLOCK)
	LEFT JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
		LEFT JOIN Employee E WITH(NOLOCK)
		ON  ECA.EmployeeId = E.Id
		AND E.Active = 1
		AND ECA.FechaBaja IS NULL
		AND E.FechaBaja IS NULL
	ON	ECA.CargoId = C.Id
	AND C.Active = 1
	LEFT JOIN Cargos C2 WITH(NOLOCK)
	ON	C2.Id = C.ResponsableId
	WHERE
		C.CompanyId = @CompanyId
	AND C.DepartmentId = @DepartmentId
	AND C.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[Department_Insert]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Department_Insert]
	@DepartmentId int out,
	@CompanyId int,
	@Description nvarchar(50),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @Id int
	
	SELECT @Id = Id FROM Department D WITH(NOLOCK)
	WHERE
		D.CompanyId = @CompanyId
	AND D.Name = @Description
		
	IF @Id IS NOT NULL 
	BEGIN
		UPDATE Department SET
			Deleted = 0
		WHERE
			CompanyId = @CompanyId
		AND Id = @Id
		
		SET @DepartmentId = @Id
	END
	ELSE
	BEGIN
		INSERT INTO Department
		(
			CompanyId,
			Name,
			Deleted,
			CreatedBy,
			CreatedOn,
			ModifiedBy,
			ModifiedOn
		)
		VALUES
		(
			@CompanyId,
			@Description,
			0,
			@UserId,
			GETDATE(),
			@UserId,
			GETDATE()
		)
		
		SET @DepartmentId = @@IDENTITY
	END
END





GO

/****** Object:  StoredProcedure [dbo].[Department_Update]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Department_Update]
	@DepartmentId int,
	@CompanyId int,
	@Description nvarchar(50),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Department SET
		Name = @Description,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @DepartmentId
	AND	CompanyId = @CompanyId
	
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		5,
		@DepartmentId,
		2,
		GETDATE(),
		'Name:' + @Description
    )
	
	
END





GO

/****** Object:  StoredProcedure [dbo].[Departments_GetByCompany]    Script Date: 24/10/2018 20:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Departments_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT
		D.Id,
		D.Name,
		D.CompanyId,
		D.Deleted,
		CASE WHEN C.Id IS NULL THEN 0 ELSE 1 END
	FROM Department D WITH(NOLOCK)
	LEFT JOIN Cargos C WITH(NOLOCK)
	ON C.DepartmentId = D.Id
	WHERE
		D.CompanyId = @CompanyId
	ORDER BY D.Name ASC
	
END





GO

/****** Object:  StoredProcedure [dbo].[Document_Anulate]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Document_Anulate]
	@DocumentId int,
	@CompanyId int,
	@EndDate datetime,
	@EndReason nvarchar(500),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Document SET
		FechaBaja = @EndDate,
		EndReason = @EndReason,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @DocumentId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Document_Delete]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Document_Delete]
	@DocumentId bigint,
	@CompanyId int,
	@UserId int,
	@ExtraData nvarchar(200)
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE Document SET
		Activo = 0
	WHERE
		Id = @DocumentId
	AND CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		4,
		@DocumentId,
		3,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[Document_GetById]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Document_GetById]
	@DocumentId bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		D.Id AS DocumentId,
		DV.Id AS VersionId,
		D.Description,
		DV.Version,
		DV.UserCreate,
		DV.Status,
		DV.Date,
		ISNULL(DV.Reason,'') AS Reason,
		DV.UserCreate,
		ISNULL(D.CategoryId,0) AS CategoryId,
		ISNULL(DC.Description,'') AS CategoryName,
		ISNULL(P.Id,0) AS ProcedenciaId,
		ISNULL(P.Description,'') AS ProcedenciaName,
		ISNULL(D.Codigo,'') AS Codigo,
		D.FechaAlta,
		D.FechaBaja,
		D.Conservacion,
		D.ConservacionType,
		D.Origen,
		ISNULL(D.Ubicacion,'') AS Ubicacion,
		D.ModifiedOn,
		D.ModifiedBy AS ModifiedByUserId,
		AU.Login,
		ISNULL(D.EndReason,'') AS EndReason
	FROM Document D WITH(NOLOCK)
	INNER JOIN DocumentsVersion DV WITH(NOLOCK)
	ON
		D.CompanyId = @CompanyId
	AND D.Id = @DocumentId
	AND D.CompanyId = DV.Company
	AND D.Id = DV.DocumentId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON
		DV.UserCreate = AU.Id
	LEFT JOIN Document_Category DC WITH(NOLOCK)
	ON
		D.CategoryId = DC.Id
	AND D.CompanyId = DC.CompanyId
	LEFT JOIN Procedencia P WITH(NOLOCK)
	ON
		P.Id = D.ProcedenciaId
	AND P.CompanyId = D.CompanyId
	
	ORDER BY DV.DocumentId ASC, DV.Version ASC
END





GO

/****** Object:  StoredProcedure [dbo].[Document_Insert]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Document_Insert]
	@DocumentId bigint out,
	@CompanyId int,
	@Description nvarchar(100),
	@CategoryId int,
	@Origen int,
	@FechaAlta datetime,
	@ProcedenciaId int,
	@Conservacion int,
	@ConservacionType int,
	@Activo bit,
	@Codigo nvarchar(10),
	@Ubicacion nvarchar(100),
	@Version int,
	@UserId int
	
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO Document
    (
		CompanyId,
		ActualVersion,
		Description,
		CategoryId,
		FechaAlta,
		FechaBaja,
		Origen,
		ProcedenciaId,
		Conservacion,
		ConservacionType,
		Activo,
		Codigo,
		Ubicacion,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		1,
		LTRIM(RTRIM(@Description)),
		@CategoryId,
		@FechaAlta,
		null,
		@Origen,
		@ProcedenciaId,
		@Conservacion,
		@ConservacionType,
		@Activo,
		LTRIM(RTRIM(@Codigo)),
		@Ubicacion,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()		
	)
	
	SET @DocumentId = @@IDENTITY
	
	INSERT INTO DocumentsVersion
    (
		DocumentId,
		Company,
		Version,
		UserCreate,
		Status,
		Date,
		Reason
	)
    VALUES
    (
		@DocumentId,
        @CompanyId,
        @Version,
        @UserId,
        1,
        GETDATE(),
        'Creation'
    )


END





GO

/****** Object:  StoredProcedure [dbo].[Document_LastVersion]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Document_LastVersion]
	@DocumentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		CASE WHEN MAX(Dv.Version) = 0 THEN 1 ELSE MAX(Dv.Version) END
	FROM DocumentsVersion DV WITH(NOLOCK)
	WHERE
		DV.DocumentId = @DocumentId
	AND DV.Company = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Document_Restore]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





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



GO

/****** Object:  StoredProcedure [dbo].[Document_Update]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Document_Update]
	@DocumentId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@CategoryId int,
	@FechaAlta date,
	--@FechaBaja date,
	@Origen int,
	@ProcedenciaId int,
	@Conservacion int,
	@ConservacionType int,
	@Activo bit,
	@Codigo nvarchar(10),
	@Ubicacion nvarchar(100),
	@UserId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Document SET
		Description = @Description,
		CategoryId = @CategoryId,
		FechaAlta = @Fechaalta,
		--FechaBaja = @FechaBaja,
		Origen = @Origen,
		ProcedenciaId = @ProcedenciaId,
		Conservacion = @Conservacion,
		ConservacionType = @ConservacionType,
		Activo = 1,
		Codigo = @Codigo,
		Ubicacion = @Ubicacion,
		ModifiedBy = @UserId
	WHERE	
		Id = @DocumentId
	AND CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Document_Versioned]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Document_Versioned]
	@DocumentId int,
	@CompanyId int,
	@Version int,
	@UserId int,
	@Reason nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO DocumentsVersion
    (
		DocumentId,
		Company,
		Version,
		UserCreate,
		Status,
		Date,
		Reason
	)
    VALUES
    (
		@DocumentId,
        @CompanyId,
        @Version,
        @UserId,
        1,
        GETDATE(),
        @Reason
    )
    
    UPDATE Document SET
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @DocumentId
	AND CompanyId = @CompanyId

END





GO

/****** Object:  StoredProcedure [dbo].[DocumentAttach_Active]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[DocumentAttach_Active]
	@Id bigint,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE DocumentAttach SET
		[Active] = 1,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn] = GETDATE()
	WHERE
		Id = @Id
		
END


GO

/****** Object:  StoredProcedure [dbo].[DocumentAttach_Delete]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[DocumentAttach_Delete]
	@Id bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM DocumentAttach
	WHERE
		[CompanyId] = @CompanyId
	AND [Id] = @Id
END


GO

/****** Object:  StoredProcedure [dbo].[DocumentAttach_GetByDoumentId]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[DocumentAttach_GetByDoumentId]
	@DocumentId bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		DA.Id,
		DA.CompanyId,
		DA.DocumentId,
		DA.[Description],
		DA.Extension,
		DA.[Version],
		DA.CreatedBy,
		CB.[Login] AS CreatedByName,
		DA.CreatedOn,
		DA.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		DA.ModifiedOn,
		DA.Active
	FROM DocumentAttach DA WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = DA.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = DA.ModifiedBy
	WHERE
		DA.CompanyId = @CompanyId
	AND DA.DocumentId = @DocumentId
END


GO

/****** Object:  StoredProcedure [dbo].[DocumentAttach_GetById]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[DocumentAttach_GetById]
	@Id bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		DA.Id,
		DA.CompanyId,
		DA.DocumentId,
		DA.[Description],
		DA.Extension,
		DA.[Version],
		DA.CreatedBy,
		CB.[Login] AS CreatedByName,
		DA.CreatedOn,
		DA.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		DA.ModifiedOn,
		DA.Active
	FROM DocumentAttach DA WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = DA.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = DA.ModifiedBy
	WHERE
		DA.CompanyId = @CompanyId
	AND DA.Id = @Id
END


GO

/****** Object:  StoredProcedure [dbo].[DocumentAttach_Inactive]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[DocumentAttach_Inactive]
	@Id bigint,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE DocumentAttach SET
		[Active] = 0,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn] = GETDATE()
	WHERE
		Id = @Id
		
END


GO

/****** Object:  StoredProcedure [dbo].[DocumentAttach_Insert]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[DocumentAttach_Insert]
	@Id bigint output,
	@DocumentId bigint,
	@CompanyId int,
	@Version int,
	@Description nvarchar(50),
	@Extension nvarchar(10),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM DocumentAttach
	WHERE
		[CompanyId] = @CompanyId
	AND [Version] = @Version
	AND [DocumentId] = @DocumentId 


    -- Insert statements for procedure here
	INSERT INTO [DocumentAttach]
	(
		[CompanyId],
		[DocumentId],
		[Version],
		[Description],
		[Extension],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	VALUES
	(
		@CompanyId,
		@DocumentId,
		@Version,
		@Description,
		@Extension,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		1
	)

	SET @Id = @@IDENTITY
END


GO

/****** Object:  StoredProcedure [dbo].[DocumentAttach_RestoreName]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[DocumentAttach_RestoreName] 
	@Id bigint,
	@Name nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE DocumentAttach SET
		[Description] = @Name
	WHERE
		[Id] = @Id

END


GO

/****** Object:  StoredProcedure [dbo].[DocumentAttach_Update]    Script Date: 24/10/2018 20:20:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[DocumentAttach_Update]
	@Id bigint,
	@Description nvarchar(50),
	@Extension nvarchar(10),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE DocumentAttach SET
		[Description] = @Description,
		[Extension] = @Extension,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn] = GETDATE()
	WHERE
		Id = @Id
		
END


GO

/****** Object:  StoredProcedure [dbo].[DocumentCategory_Delete]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[DocumentCategory_Delete]
	@DocumentCategoryId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM Document_Category
	WHERE
		Id = @DocumentCategoryId
	AND CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[DocumentCategory_Insert]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[DocumentCategory_Insert]
	@DocumentCategoryId int out,
	@CompanyId int,
	@Description nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO Document_Category
	(
		CompanyId,
        Description,
        Editable
    )
    VALUES    
    (
		@CompanyId,
		@Description,
		1
    ) 
    
    SET @DocumentCategoryId = @@IDENTITY
END





GO

/****** Object:  StoredProcedure [dbo].[DocumentCategory_Update]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[DocumentCategory_Update]
	@DocumentCategoryId int,
	@CompanyId int,
	@Description nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Document_Category SET
		Description = @Description
	WHERE
		Id = @DocumentCategoryId
	AND CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[DocumentProcedencia_Delete]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[DocumentProcedencia_Delete]
	@DocumentProcedenciaId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM Procedencia 
	WHERE
		Id = @DocumentProcedenciaId
	AND CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[DocumentProcedencia_Insert]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[DocumentProcedencia_Insert]
	@DocumentProcedenciaId int out,
	@CompanyId int,
	@Description nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO Procedencia
	(
		CompanyId,
        Description,
        Editable
    )
    VALUES    
    (
		@CompanyId,
		@Description,
		1
    ) 
    
    SET @DocumentProcedenciaId = @@IDENTITY
END





GO

/****** Object:  StoredProcedure [dbo].[DocumentProcedencia_Update]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[DocumentProcedencia_Update]
	@DocumentProcedenciaId int,
	@CompanyId int,
	@Description nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Procedencia SET
		Description = @Description
	WHERE
		Id = @DocumentProcedenciaId
	AND CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Documents_GetInactive]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Documents_GetInactive]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		D.Id,
		D.Codigo,
		D.Description,
		D.ActualVersion,
		E.Id AS ModifiedById,
		ISNULL(E.Name,'') AS ModifiedByName,
		ISNULL(E.LastName,'') AS ModifiedByLastName,
		D.ModifiedOn
	FROM Document D WITH(NOLOCK)
	INNER JOIN Employee E WITH(NOLOCK)
	ON	D.ModifiedBy = E.Id
	WHERE
		D.CompanyId = @CompanyId
	AND D.FechaBaja IS NOT NULL
	ORDER BY 
		D.FechaBaja DESC
END





GO

/****** Object:  StoredProcedure [dbo].[Documents_LastModified]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Documents_LastModified]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 10
		D.Id,
		D.Codigo,
		D.Description,
		V.Version,
		D.ModifiedBy AS ModifiedById,
		AU.[Login] AS ModifiedByUserName,
		D.ModifiedOn
	FROM Document D WITH(NOLOCK)
	INNER JOIN ApplicationUser AU
	ON AU.Id = D.ModifiedBy
	INNER JOIN 
	(
		SELECT 
			DocumentId,
			MAX(Version) Version
		FROM DocumentsVersion WITH(NOLOCK)
		GROUP BY DocumentId
	) AS V
	ON	V.DocumentId = D.Id
	WHERE
		D.CompanyId = @CompanyId
	ORDER BY 
		D.ModifiedOn DESC
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_AsignateJobPosition]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_AsignateJobPosition]
	@EmployeeId bigint,
	@JobPositionId bigint,
	@Date datetime,
	@CompanyId int,
	@UserId int
AS
BEGIN

	
		INSERT INTO EmployeeCargoAsignation
		(
			EmployeeId,
			CargoId,
			CompanyId,
			FechaAlta,
			FechaBaja
		)
		VALUES
		(
			@EmployeeId,
			@JobPositionId,
			@CompanyId,
			@Date,
			NULL
		)

		INSERT INTO ActivityLog
		(
			ActivityId,
			UserId,
			CompanyId,
			TargetType,
			TargetId,
			ActionId,
			DateTime,
			ExtraData
		)
		VALUES
		(
			NEWID(),
			@UserId,
			@CompanyId,
			8,
			@EmployeeId,
			6,
			GETDATE(),
			'JobPoisitionId:' + CAST(@JobPositionId AS NVARCHAR)
		)
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_AssociateToDepartment]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_AssociateToDepartment]
	@EmployeeId bigint,
	@DepartmentId int,
	@CompanyId int
AS
BEGIN
INSERT INTO EmployeeDepartmentMembership
	(
		EmployeeId,
		DepartmentId,
		CompanyId
	)
    VALUES
    (
		@EmployeeId,
		@DepartmentId,
		@CompanyId
	)

END





GO

/****** Object:  StoredProcedure [dbo].[Employee_Delete]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_Delete]
	@EmployeeId bigint,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;	
	
	UPDATE Employee SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id =@EmployeeId
	AND CompanyId = @CompanyId
	
	UPDATE EmployeeCargoAsignation SET
		FechaBaja = GETDATE()
	WHERE
		EmployeeId = @EmployeeId
	AND CompanyId = @CompanyId

	DELETE FROM EmployeeUserAsignation
	WHERE
		EmployeeId = @EmployeeId
	AND CompanyId = @CompanyId
	
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_DeleteJobPosition]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_DeleteJobPosition]
	@EmployeeId bigint,
	@JobPositionId bigint
AS
BEGIN
	
		DELETE FROM EmployeeCargoAsignation
		WHERE
			EmployeeId = @EmployeeId
		AND	CargoId = @JobPositionId
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_DesassociateToDepartment]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_DesassociateToDepartment]
	@EmployeeId bigint,
	@DepartmentId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE FROM EmployeeDepartmentMembership
    WHERE
		CompanyId = @CompanyId
	AND DepartmentId = @DepartmentId
	AND EmployeeId = @EmployeeId
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_Disable]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_Disable]
	@EmployeeId bigint,
	@CompanyId int,
	@UserId int,
	@FechaBaja date
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Employee SET
		FechaBaja = @FechaBaja,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id =@EmployeeId
	AND CompanyId = @CompanyId
	
	UPDATE EmployeeCargoAsignation SET
		FechaBaja = @FechaBaja
	WHERE
		EmployeeId = @EmployeeId
	AND CompanyId = @CompanyId
	
	DECLARE @EmployeeUserId int
	SELECT @EmployeeUserId = EUA.UserId
	FROM EmployeeUserAsignation EUA WITH(NOLOCK)
	WHERE	EUA.EmployeeId = @EmployeeId
	AND		EUA.CompanyId = @CompanyId
	
	UPDATE ApplicationUser SET
		Status = 0
	WHERE
		Id = @EmployeeUserId

	DELETE FROM EmployeeUserAsignation
	WHERE	EmployeeId = @EmployeeId
	
	/*INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		8,
		@EmployeeId,
		9,
		GETDATE(),
		''
    )*/
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_GetActions]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Employee_GetActions]
	@EmployeeId bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT
		'E' AS AssignationType,
		E.Id AS ItemPageType,
		E.Id AS ItemType,
		E.Description AS ItemDescription
	FROM Equipment E WITH(NOLOCK)
	WHERE
		E.CompanyId = @CompanyId
	AND	E.Active = 1
	AND E.Resposable = @EmployeeId

	UNION

	SELECT
		CASE WHEN ECD.Type = 0 THEN 'ECDI' ELSE 'ECDE' END AS AssignationType,
		ECD.EquipmentId AS ItemPageType,
		ECD.Id AS ItemType,
		E.Description + ' - ' + ECD.Operation AS ItemDescription
	FROM EquipmentCalibrationDefinition ECD WITH(NOLOCK)
	INNER JOIN Equipment E WITH(NOLOCK)
	ON	E.Id = ECD.EquipmentId
	AND	E.Active = 1
	WHERE
		ECD.CompanyId = @CompanyId
	AND	ECD.Active = 1
	AND ECD.Responsable = @EmployeeId

	UNION

	SELECT
		CASE WHEN EVD.VerificationType = 0 THEN 'EVDI' ELSE 'EVDE' END AS AssignationType,
		EVD.EquipmentId AS ItemPageType,
		EVD.Id AS ItemType,
		E.Description + ' - ' + EVD.Operation AS ItemDescription
	FROM EquipmentVerificationDefinition EVD WITH(NOLOCK)
	INNER JOIN Equipment E WITH(NOLOCK)
	ON	E.Id = EVD.EquipmentId
	AND	E.Active = 1
	WHERE
		EVD.CompanyId = @CompanyId
	AND	EVD.Active = 1
	AND EVD.Responsable = @EmployeeId

	UNION

	SELECT
		CASE WHEN EMD.Type = 0 THEN 'EMDI' ELSE 'EMDE' END AS AssignationType,
		EMD.EquipmentId AS ItemPageType,
		EMD.Id AS ItemType,
		E.Description + ' - ' + EMD.Operation AS ItemDescription
	FROM EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
	INNER JOIN Equipment E WITH(NOLOCK)
	ON	E.Id = EMD.EquipmentId
	AND	E.Active = 1
	WHERE
		EMD.CompanyId = @CompanyId
	AND	EMD.Active = 1
	AND EMD.ResponsableId = @EmployeeId

	UNION

	SELECT
		'IAE',
		I.Id AS ItemTypePage,
		I.Id AS ItemId,
		RIGHT('0000' + CAST(I.Code AS NVARCHAR(5)),5) + ' - ' + I.Description AS ItemDescription
	FROM Incident I WITH(NOLOCK)
	WHERE
		I.CompanyId = @CompanyId
	AND I.Active = 1
	AND I.ActionsExecuter = @EmployeeId
	AND I.ActionsSchedule > GETDATE()
END




GO

/****** Object:  StoredProcedure [dbo].[Employee_GetByCompany]    Script Date: 24/10/2018 20:20:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		E.Id,
		E.Name,
		E.LastName,
		ISNULL(E.Email,'') AS Email,
		ISNULL(E.Phone,'') AS Phone,
		D.Id AS DepartmentId,
		AU.Id AS UserId,
		ISNULL(D.Name,'') AS DepartmentName,
		ISNULL(E.NIF ,'') AS Nif,
		ISNULL(E.Address,'') AS Address,
		ISNULL(E.PostalCode,'') AS PostalCode,
		ISNULL(E.City,'') AS City,
		ISNULL(E.Province,'') AS Province,
		ISNULL(E.Country,'') AS Country,
		ISNULL(E.Notes,'') AS Notes,
		E.CompanyId AS CompanyId,
		AU.Id AS ModifiedByUserId,
		0 AS ModifiedByEmployeeId,
		AU.[Login] AS ModifiedName,
		'' AS ModifiedByLastName,
		E.ModifiedOn,
		C.Id AS CargoId,
		C.Description AS CargoDescription,
		E.FechaBaja,
		E.Active
	FROM Employee E WITH(NOLOCK)
	LEFT JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
		LEFT JOIN Cargos C WITH(NOLOCK)
			LEFT JOIN Department D WITH (NOLOCK)
			ON	D.CompanyId = C.CompanyId
			AND	D.Id = C.DepartmentId
			AND D.Deleted = 0
		ON	C.Id = ECA.CargoId
		AND C.CompanyId = ECA.CompanyId
		AND C.Active = 1
	ON  E.Id = ECA.EmployeeId
	AND	E.CompanyId = ECA.CompanyId
	AND ECA.FechaBaja IS NULL
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON AU.Id = E.ModifiedBy
	WHERE
		E.CompanyId = @CompanyId
	-- AND E.FechaBaja IS NULL
	ORDER BY D.Name ASC
	
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_GetByCompanyWithUser]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_GetByCompanyWithUser]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		E.Id,
		E.Name,
		E.LastName,		
		E.Active,
		AU.Id AS UserId,
		AU.Login
	FROM Employee E WITH(NOLOCK)
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
	ON  E.Id = EUA.EmployeeId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = EUA.UserId
	WHERE
		E.CompanyId = @CompanyId
	
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_GetById]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_GetById]
	@EmployeeId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		E.Id,
		E.Name,
		E.LastName,
		ISNULL(E.Email,'') AS Email,
		ISNULL(E.Phone,'') AS Phone,
		D.Id AS DepartmentId,
		0 AS UserId,
		ISNULL(D.Name,'') AS DepartmentName,
		ISNULL(E.NIF ,'') AS Nif,
		ISNULL(E.Address,'') AS Address,
		ISNULL(E.PostalCode,'') AS PostalCode,
		ISNULL(E.City,'') AS City,
		ISNULL(E.Province,'') AS Province,
		ISNULL(E.Country,'') AS Country,
		ISNULL(E.Notes,'') AS Notes,
		E.CompanyId AS CompanyId,
		AU.Id AS ModifiedByUserId,
		AU.Login AS ModifiedByUserName,
		E.ModifiedOn,
		E.FechaBaja,
		E.Active
	FROM Employee E WITH(NOLOCK)
	LEFT JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
		LEFT JOIN Cargos C WITH(NOLOCK)
			LEFT JOIN Department D WITH (NOLOCK)
			ON	D.CompanyId = C.CompanyId
			AND	D.Id = C.DepartmentId
			AND D.Deleted = 0
		ON	C.Id = ECA.CargoId
		AND C.CompanyId = ECA.CompanyId
		AND C.Active = 1
	ON  E.Id = ECA.EmployeeId
	AND	E.CompanyId = ECA.CompanyId
	AND ECA.FechaBaja IS NULL
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON AU.Id = E.ModifiedBy
	WHERE
		E.Id = @EmployeeId
	ORDER BY D.Name ASC
	
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_GetByUserId]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_GetByUserId]
	@USerId bigint
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		E.Id,
		E.Name,
		E.LastName,
		ISNULL(E.Email,'') AS Email,
		ISNULL(E.Phone,'') AS Phone,
		ISNULL(E.NIF,'') AS Nif,
		ISNULL(E.Address,'') AS Address,
		ISNULL(E.PostalCode,'') AS PostalCode,
		ISNULL(E.City,'') AS City,
		ISNULL(E.Country,'') AS Country,
		ISNULL(E.Province,'') AS Province
	FROM Employee E WITH(NOLOCK)
	INNER JOIN EmployeeUserAsignation EUA3 WITH(NOLOCK)
	ON	EUA3.EmployeeId = E.Id
	WHERE
		EUA3.UserId = @UserId
	
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_GetImplications]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Employee_GetImplications]
	@EmployeeId bigint,
	@CompanyId int
AS
BEGIN
	SELECT
		'E' AS ItemType,
		E.Id AS ItemId,
		E.Description AS ItemDescription,
		E.Resposable
	FROM Equipment E WITH(NOLOCK)
	WHERE
		E.active = 1
	AND E.CompanyId = @CompanyId
	AND E.Resposable = @EmployeeId

	UNION
	
	SELECT
		'ECD' AS ItemType,
		ECD.Id AS ItemId,
		ECD.Operation AS ItemDescription,
		ECD.Responsable
	FROM EquipmentCalibrationDefinition ECD WITH(NOLOCK)
	WHERE
		ECD.active = 1
	AND ECD.CompanyId = @CompanyId
	AND ECD.Responsable = @EmployeeId

	UNION
	
	SELECT
		'EVD' AS ItemType,
		EVD.Id AS ItemId,
		EVD.Operation AS ItemDescription,
		EVD.Responsable
	FROM EquipmentVerificationDefinition EVD WITH(NOLOCK)
	WHERE
		EVD.active = 1
	AND EVD.CompanyId = @CompanyId
	AND EVD.Responsable = @EmployeeId

	UNION
	
	SELECT
		'EMD' AS ItemType,
		EMD.Id AS ItemId,
		EMD.Operation AS ItemDescription,
		EMD.ResponsableId
	FROM EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
	WHERE
		EMD.active = 1
	AND EMD.CompanyId = @CompanyId
	AND EMD.ResponsableId = @EmployeeId	

	UNION
	
	SELECT
		'IAW' AS ItemType,
		IA.Id AS ItemId,
		IA.Description AS ItemDescription,
		IA.WhatHappendBy
	FROM IncidentAction IA WITH(NOLOCK)
	WHERE
		IA.active = 1
	AND IA.CompanyId = @CompanyId
	AND IA.WhatHappendBy = @EmployeeId

	UNION
	
	SELECT
		'IAC' AS ItemType,
		IA.Id AS ItemId,
		IA.Description AS ItemDescription,
		IA.WhatHappendBy
	FROM IncidentAction IA WITH(NOLOCK)
	WHERE
		IA.active = 1
	AND IA.CompanyId = @CompanyId
	AND IA.ActionsBy = @EmployeeId




END




GO

/****** Object:  StoredProcedure [dbo].[Employee_GetJobPositionHistoric]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_GetJobPositionHistoric]
	@EmployeeId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		ECA.CompanyId,
		ECA.CargoId,
		C.Description AS CargoDescription,
		D.Id AS DepartmentId,
		D.Name AS DepartmentName,
		E.Id,
		ISNULL(E.NIF,'') AS Nif,
		ISNULL(E.Name,'') AS EmployeeName,
		ISNULL(E.LastName,'') AS EmployeeLastName,
		ECA.FechaAlta,
		ECA.FechaBaja,
		C.Active,
		C2.Id,
		C2.Description
    FROM EmployeeCargoAsignation ECA WITH(NOLOCK)
    INNER JOIN Employee E WITH(NOLOCK)
    ON	E.Id = ECA.EmployeeId
    AND E.CompanyId = ECA.CompanyId
    INNER JOIN Cargos C WITH(NOLOCK)
		LEFT JOIN Cargos C2 WITH(NOLOCK)
		ON	C2.Id = C.ResponsableId
		AND C2.Active = 1
    ON	C.CompanyId = ECA.CompanyId
    AND C.Id = ECA.CargoId
    AND C.Active = 1
    INNER JOIN Department D WITH(NOLOCK)
    ON	D.CompanyId = C.CompanyId
    AND D.Id = C.DepartmentId
    WHERE
		ECA.EmployeeId = @EmployeeId
	AND	ECA.CompanyId = @CompanyId
	ORDER BY
		ECA.FechaAlta DESC
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_GetJobPositions]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Employee_GetJobPositions]
	@EmployeeId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		ECA.FechaAlta,
		ECA.FechaBaja,
		C.Id,
		C.DepartmentId,
		C.Description,
		E.Id,
		E.Name,
		E.LastName,
		'',
		D.Name
	FROM EmployeeCargoAsignation ECA WITH(NOLOCK)
	INNER JOIN Cargos C WITH(NOLOCK)
	ON	ECA.CompanyId = C.CompanyId
	AND ECA.CargoId = C.Id
	AND C.Active = 1
	INNER JOIN Employee E WITH(NOLOCK)
	ON	ECA.EmployeeId = E.Id
	AND C.CompanyId = E.CompanyId
	INNER JOIN Department D WITH(NOLOCK)
	ON	C.DepartmentId = D.Id
	AND C.CompanyId = D.CompanyId
	WHERE
		ECA.EmployeeId = @EmployeeId
	AND ECA.CompanyId = @CompanyId
	AND	C.CompanyId = E.CompanyId
	ORDER BY ECA.FechaAlta ASC
	
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_GetLearningAssitance]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_GetLearningAssitance]
	@EmployeeId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		LA.Success,
		LA.Completed,
		L.DateStimatedDate,
		L.Id AS LearningId,
		L.Description AS LearningDescription,
		C.Id AS CargoId,
		C.Description AS CargoDescription,
		D.Id AS DepartmentId,
		D.Name AS DepartmentDescription,
		La.Id,
		L.Status AS LearningStatus,
		L.RealFinish AS LearningFinishDate
	FROM LearningAssistant LA WITH(NOLOCK)
	INNER JOIN Learning L WITH(NOLOCK)
	ON	LA.LearningId = L.Id
				 
	AND	LA.CompanyId = L.CompanyId
	LEFT JOIN Cargos C WITH(NOLOCK)
	ON	C.CompanyId = LA.CompanyId
	AND C.Id = LA.CargoId
	LEFT JOIN Department D WITH(NOLOCK)
	ON	D.CompanyId = C.CompanyId
	AND	D.Id = C.DepartmentId
	WHERE
		L.Active = 1
	AND	LA.EmployeeId = @EmployeeId
	ORDER BY L.DateStimatedDate ASC
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_Insert]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_Insert]
	@EmployeeId bigint out,
	@CompanyId int,
	@Name nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Phone nvarchar(50),
	@NIF nvarchar(15),
	@Address nvarchar(50),
	@PostalCode nvarchar(10),
  	@City nvarchar(50),
	@Province nvarchar(50),
	@Country nvarchar(50),
	@Notes text,
	@UserId int,
	@UserName nvarchar(50),
	@Password nvarchar(6) out
AS
	SET NOCOUNT ON;
	INSERT INTO Employee
	(
		CompanyId,
		Name,
		LastName,
		Email,
		Phone,
		NIF,
		Address,
		PostalCode,
		City,
		Province,
		Country,
		Notes,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn,
		Active
	)
    VALUES
    (
		@CompanyId,
		@Name,
		@LastName,
		@Email,
		@Phone,
		@NIF,
		@Address,
		@PostalCode,
		@City,
		@Province,
		@Country,
		@Notes,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE(),
		1
    )

	SET @EmployeeId = @@IDENTITY
	/*SELECT @Password = [dbo].GeneratePassword(6)
	
	INSERT INTO ApplicationUser
	(
		CompanyId,
		Login,
		Password,
		Status,
		LoginFailed,
		MustResetPassword,
		Language,
		ShowHelp
	)
	VALUES
	(
		@CompanyId,
		@UserName,
		'root',--@Password,
		1,
		0,
		1,
		'es',
		1
	)
	
	DECLARE @newUser int
	SET @newUser = @@IDENTITY
	
	INSERT INTO EmployeeUserAsignation
	(
		UserId,
		EmployeeId,
		CompanyId
	)
    VALUES
    (
		@newUser,
		@EmployeeId,
		@CompanyId
    )*/
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		8,
		@EmployeeId,
		1,
		GETDATE(),
		''
    )






GO

/****** Object:  StoredProcedure [dbo].[Employee_Restore]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_Restore]
	@EmployeeId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Employee SET
		FechaBaja = NULL,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id =@EmployeeId
	AND CompanyId = @CompanyId
	
	/*DECLARE @EmployeeUserId int
	SELECT @EmployeeUserId = EUA.UserId
	FROM EmployeeUserAsignation EUA WITH(NOLOCK)
	WHERE	EUA.EmployeeId = @EmployeeId
	AND		EUA.CompanyId = @CompanyId
	
	UPDATE ApplicationUser SET
		Status = 1
	WHERE
		Id = @EmployeeUserId*/
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		8,
		@EmployeeId,
		10,
		GETDATE(),
		''
    )
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_SetUser]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Employee_SetUser]
	@EmployeeId bigint,
	@UserId bigint,
	@CompanyId int
AS
BEGIN

DELETE FROM [EmployeeUserAsignation] 
WHERE
	UserId = @UserId
AND	CompanyId = @CompanyId


INSERT [EmployeeUserAsignation]
           ([UserId]
           ,[EmployeeId]
           ,[CompanyId])
     VALUES
           (@UserId,
           @EmployeeId,
           @CompanyId)


END





GO

/****** Object:  StoredProcedure [dbo].[Employee_UnasignateJobPosition]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_UnasignateJobPosition]
	@EmployeeId bigint,
	@JobPositionId bigint,
	@Date datetime,
	@CompanyId int,
	@UserId int
AS
BEGIN

	UPDATE EmployeeCargoAsignation SET
		FechaBaja = @Date
	WHERE
		EmployeeId = @EmployeeId
	AND CargoId = @JobPositionId
	AND CompanyId = @CompanyId
	AND FechaBaja IS NULL
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		[DateTime],
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		8,
		@JobPositionId,
		8,
		GETDATE(),
		'JobPoisitionId:' + CAST(@JobPositionId AS NVARCHAR)
	)
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_UnsetUser]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_UnsetUser]
	@UserId bigint,
	@CompanyId int
AS
BEGIN

	DELETE FROM EmployeeUserAsignation
	WHERE
		UserId = @UserId
	AND CompanyId = @CompanyId

END





GO

/****** Object:  StoredProcedure [dbo].[Employee_Update]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Employee_Update]
	@EmployeeId bigint,
	@CompanyId int,
	@Name nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Phone nvarchar(50),
	@NIF nvarchar(15),
	@Address nvarchar(50),
	@PostalCode nvarchar(10),
	@City nvarchar(50),
	@Province nvarchar(50),
	@Country nvarchar(50),
	@Notes text,
	@ModifiedBy int
AS
BEGIN
	UPDATE Employee SET
		Name = @Name,
		LastName = @LastName,
		Email = LOWER(@Email),
		Phone = @Phone,
		NIF = @NIF,
		Address = @Address,
		PostalCode = @PostalCode,
		City = @City,
		Province = @Province,
		Country = @Country,
		Notes = @Notes,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = GETDATE()
	WHERE
		Id = @EmployeeId
	AND	CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Employee_WithoutUser]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Employee_WithoutUser]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT
		E.Id,
		E.Name,
		E.LastName,
		EUA.*,
		AU.[Status]
	FROM Employee E WITH(NOLOCK)
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser AU WITH(NOLOCK)
		ON AU.Id = EUA.UserId
	ON	EUA.EmployeeId = E.Id

	WHERE
		E.CompanyId = @CompanyId
	AND AU.[Status] = 1
	AND E.Active = 1
	AND E.FechaBaja IS NULL
END




GO

/****** Object:  StoredProcedure [dbo].[EmployeeSkills_GetByEmployee]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EmployeeSkills_GetByEmployee]
	@EmployeeId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		ES.Id,
		ES.EmployeeId,
		ES.CompanyId,
		ES.Academic,
		ES.AcademicValid,
		ES.Specific,
		ES.SpecificValid,
		ES.WorkExperience,
		ES.WorkExperienceValid,
		ES.Hability,
		ES.HabilityValid,
		ES.ModifiedOn,
		ES.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName
	FROM EmployeeSkills ES WITH(NOLOCK)
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = Es.ModifiedBy
	WHERE
		ES.EmployeeId = @EmployeeId
	AND ES.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[EmployeeSkills_Insert]    Script Date: 24/10/2018 20:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EmployeeSkills_Insert]
	@Id int out,
	@EmployeeId bigint,
	@CompanyId int,
	@Academic text,
	@Specific text,
	@WorkExperience text,
	@Hability text,
	@AcademicValid bit,
	@SpecificValid bit,
	@WorkExperienceValid bit,
	@HabilityValid bit,
	@UserId int
AS
BEGIN
	INSERT INTO EmployeeSkills
	(
		EmployeeId,
		CompanyId,
		Academic,
		Specific,
		WorkExperience,
		Hability,
		AcademicValid,
		SpecificValid,
		WorkExperienceValid,
		HabilityValid,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@EmployeeId,
		@CompanyId,
		@Academic,
		@Specific,
		@WorkExperience,
		@Hability,
		@AcademicValid,
		@SpecificValid,
		@WorkExperienceValid,
		@HabilityValid,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)

	SET @Id = @@IDENTITY
END





GO

/****** Object:  StoredProcedure [dbo].[EmployeeSkills_Update]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EmployeeSkills_Update] 
	@Id int,
	@EmployeeId bigint,
	@CompanyId int,
	@Academic text,
	@Specific text,
	@WorkExperience text,
	@Hability text,
	@AcademicValid bit,
	@SpecificValid bit,
	@WorkExperienceValid bit,
	@HabilityValid bit,
	@UserId int
AS
BEGIN
	UPDATE EmployeeSkills SET
		Academic = @Academic,
		AcademicValid = @AcademicValid,
		Specific = @Specific,
		SpecificValid = @SpecificValid,
		WorkExperience = @WorkExperience,
		WorkExperienceValid = @WorkExperienceValid,
		Hability = @Hability,
		HabilityValid = @HabilityValid,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		EmployeeId = @EmployeeId
	AND CompanyId = @CompanyId

END





GO

/****** Object:  StoredProcedure [dbo].[EmployeeWithUser_GetByCompany]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EmployeeWithUser_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		E.Id,
		E.Name,
		E.LastName,
		ISNULL(E.Email,'') AS Email,
		ISNULL(E.Phone,'') AS Phone,
		D.Id AS DepartmentId,
		AU.Id AS UserId,
		ISNULL(D.Name,'') AS DepartmentName,
		ISNULL(E.NIF ,'') AS Nif,
		ISNULL(E.Address,'') AS Address,
		ISNULL(E.PostalCode,'') AS PostalCode,
		ISNULL(E.City,'') AS City,
		ISNULL(E.Province,'') AS Province,
		ISNULL(E.Country,'') AS Country,
		ISNULL(E.Notes,'') AS Notes,
		E.CompanyId AS CompanyId,
		AU.Id AS ModifiedByUserId,
		0 AS ModifiedByEmployeeId,
		AU.[Login] AS ModifiedName,
		'' AS ModifiedByLastName,
		E.ModifiedOn,
		C.Id AS CargoId,
		C.Description AS CargoDescription,
		E.FechaBaja,
		E.Active
	FROM Employee E WITH(NOLOCK)
	LEFT JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
		LEFT JOIN Cargos C WITH(NOLOCK)
			LEFT JOIN Department D WITH (NOLOCK)
			ON	D.CompanyId = C.CompanyId
			AND	D.Id = C.DepartmentId
			AND D.Deleted = 0
		ON	C.Id = ECA.CargoId
		AND C.CompanyId = ECA.CompanyId
		AND C.Active = 1
	ON  E.Id = ECA.EmployeeId
	AND	E.CompanyId = ECA.CompanyId
	AND ECA.FechaBaja IS NULL
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON AU.Id = E.ModifiedBy
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
	ON	E.Id = EUA.EmployeeId
	WHERE
		E.CompanyId = @CompanyId
	-- AND E.FechaBaja IS NULL
	ORDER BY D.Name ASC
	
END





GO

/****** Object:  StoredProcedure [dbo].[Equipment_Anulate]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Equipment_Anulate]
	@EquipmentId int,
	@CompanyId int,
	@EndDate datetime,
	@EndReason nvarchar(500),
	@EndResponsible int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Equipment SET
		EndDate = @EndDate,
		EndReason = @EndReason,
		EndResponsible = @EndResponsible,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @EquipmentId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Equipment_Delete]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Equipment_Delete]
	@EquipmentId bigint,
	@Reason nvarchar(50),
	@UserId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Equipment SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @EquipmentId
	AND CompanyId = @CompanyId

    INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		13,
		@EquipmentId,
		2,
		GETDATE(),
		@Reason
    )
END





GO

/****** Object:  StoredProcedure [dbo].[Equipment_GetById]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Equipment_GetById]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
	E.Id,
	E.CompanyId,
	E.Code,
	E.Description,
	E.TradeMark,
	E.Model,
	E.SerialNumber,
	E.Location,
	E.MeasureRange,
	E.ScaleDivision,
	E.MeasureUnit,
	ISNULL(ESD.Description,''),
	E.Resposable,
	RESP.Name,
	RESP.LastName,
	E.IsCalibration,
	E.IsVerification,
	E.IsMaintenance,
	ISNULL(E.Notes,'') AS Notes,
	ISNULL(E.Image,'images/noimage.png') AS Image,
	E.Observations,
	E.ModifiedBy AS ModifiedByUserId,
	AU.[Login] AS ModifiedByUserName,
	E.ModifiedOn,
	E.StartDate,
	E.EndDate,
	E.EndResponsible,
	E.EndReason,
	ISNULL(ENDEMP.Name,'') AS EndResponsibleName,
	ISNULL(ENDEMP.LastName,'') AS EndResponsibleLastName
	FROM Equipment E WITH(NOLOCK)
	LEFt JOIN ApplicationUser AU WITH(NOLOCK)
	ON AU.Id = E.ModifiedBy
	INNER JOIN Employee RESP WITH(NOLOCK)
	ON RESP.Id = E.Resposable
	LEFT JOIN EquipmentScaleDivision ESD WITH(NOLOCK)
	ON	ESD.Id = E.MeasureUnit
	LEFT JOIN Employee ENDEMP WITH(NOLOCK)
	ON	 ENDEMP.Id = E.EndResponsible
	
	WHERE
		E.Id = @EquipmentId
	AND E.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Equipment_GetCalibration]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Equipment_GetCalibration]
	@EquipmentId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		EC.Id,
		EC.CompanyId,
		EC.EquipmentCalibrationType,
		EC.Date,
		EC.Cost,
		EC.ProviderId,
		P.Description,
		EC.Responsable,
		E.Name,
		E.LastName,
		EC.ModifiedBy,
		EMP2.Name,
		EMP2.LastName,
		EC.ModifiedOn
	FROM EquipmentCalibrationAct EC WITH(NOLOCK)
	INNER JOIN Provider P WITH(NOLOCK)
	ON	P.Id = EC.ProviderId
	INNER JOIN Employee E WITH(NOLOCK)
	ON	E.Id = EC.Responsable
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee EMP2 WITH(NOLOCK)
		ON EMP2.Id = EUA.EmployeeId
	ON	EUA.UserId = EC.ModifiedBy
	WHERE
		EquipmentId = @EquipmentId
END





GO

/****** Object:  StoredProcedure [dbo].[Equipment_GetCalibrationDefinition]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Equipment_GetCalibrationDefinition]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 
		EC.Id,
		EC.CompanyId,
		EC.EquipmentId,
		EC.Type,
		EC.Operation,
		EC.Periodicity,
		EC.Uncertainty,
		EC.Pattern,
		EC.Cost,
		EC.Notes,
		EC.Range,
		ISNULL(EC.ProviderId,0) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderDescription,
		EC.Responsable,
		RESP.Name,
		RESP.LastName,
		EC.ModifiedBy,
		AU.[Login] AS ModifiedByUserName,
		EC.ModifiedOn
    FROM EquipmentCalibrationDefinition EC WITH(NOLOCK)
	LEFT JOIN Provider P WITH(NOLOCK)
	ON	P.Id = EC.ProviderId
    INNER JOIN Employee RESP WITH(NOLOCK)
    ON	RESP.Id = EC.Responsable
    INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = EC.ModifiedBy
	
	WHERE
		EC.EquipmentId = @EquipmentId
	AND EC.CompanyId = @CompanyId
	AND EC.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[Equipment_GetList]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Equipment_GetList]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT
		E.Id,
		E.Code,
		E.Description,
		ISNULL(E.Location,'') AS Location,
		E.Resposable AS ResponsableId,
		EMP.Name AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		E.IsCalibration,
		E.IsVerification,
		E.IsMaintenance,
		E.EndDate,
		CASE WHEN UF.Id IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS Adjuntos,
		ISNULL(Costes.Coste,0) AS Coste
    FROM Equipment E WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = E.Resposable
	LEFT JOIN UploadFiles UF WITH(NOLOCK)
	ON	UF.ItemId = E.Id
	AND UF.ItemLinked = 11
	AND UF.Active = 1
	LEFT JOIN 
	(
			SELECT
				SUM(Data.Cost) AS Coste,
				EquipmentId
			FROM
			(
				select
				 SUM(ISNULL(Cost,0)) AS Cost,
					EquipmentId
				FROM EquipmentCalibrationAct
				WHERE Active = 1
				GROUP BY EquipmentId

				UNION

				select
				 SUM(ISNULL(Cost,0)) AS Cost,
					EquipmentId
				FROM EquipmentVerificationAct
				WHERE Active = 1
				GROUP BY EquipmentId

				UNION

				select
				 SUM(ISNULL(Cost,0)) AS Cost,
					EquipmentId
				FROM EquipmentMaintenanceAct
				WHERE Active = 1
				GROUP BY EquipmentId

				UNION

				select
				 SUM(ISNULL(Cost,0)) AS Cost,
					EquipmentId
				FROM EquipmentRepair
				WHERE Active = 1
				GROUP BY EquipmentId
			) AS Data
			GROUP BY Data.EquipmentId
		) Costes
	ON Costes.EquipmentId = E.Id
    WHERE
		E.CompanyId = @CompanyId
	AND E.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[Equipment_GetRecords]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Equipment_GetRecords]
	@EquipmentId bigint,
	@CompanyId int,
	@CalibrationInt bit,
	@CalibrationExt bit,
	@VerificationInt bit,
	@VerificationExt bit,
	@MaintenanceInt bit,
	@MaintenanceExt bit,
	@RepairInt bit,
	@RepairExt bit,
	@DateFrom datetime,
	@DateTo datetime
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		TOTAL.Date,
		TOTAL.Item,
		TOTAL.Type,
		TOTAL.Operation,
		TOTAL.Cost,
		TOTAL.Responsable,
		EMP.Name,
		EMP.LastName,
		TOTAL.Active
	FROM
	(
		SELECT
			ECA.CompanyId,
			ECA.Date,
			'Calibration' AS Item,
			ECA.EquipmentCalibrationType AS Type,
			ECA.Operation,
			ECA.Responsable,
			ECA.Cost,
			ECA.Active,
			ECA.EquipmentId
		FROM EquipmentCalibrationAct ECA WITH(NOLOCK)
		
		UNION	
		
		SELECT
			EVA.CompanyId,
			EVA.Date,
			'Verification' AS Item,
			EVA.EquipmentVerificationType AS Type,
			EVA.Operation,
			EVA.Responsable,
			EVA.Cost,
			EVA.Active,
			EVA.EquipmentId
		FROM EquipmentVerificationAct EVA WITH(NOLOCK)
		
		UNION	
		
		SELECT
			EMA.CompanyId,
			EMA.Date,
			'Maintenance' AS Item,
			EMD.Type,
			EMA.Operation AS Operation,
			EMA.ResponsableId,
			EMA.Cost,
			EMA.Active,
			EMA.EquipmentId
		FROM EquipmentMaintenanceAct EMA WITH(NOLOCK)
		INNER JOIN EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
		ON	EMD.Id = EMA.EquipmentMaintenanceDefinitionId
		
		UNION	
		
		SELECT
			R.CompanyId,
			R.Date,
			'Repair' AS Item,
			R.RepairType AS Type,
			CAST(R.Description AS nvarchar(50)) AS Operation,
			R.ResponsableId,
			R.Cost,
			R.Active,
			R.EquipmentId
		FROM EquipmentRepair R WITH(NOLOCK)
	) TOTAL
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	TOTAL.Responsable = EMP.Id
	
	WHERE
		TOTAL.CompanyId = @CompanyId
	AND	TOTAL.Active = 1
	AND TOTAL.EquipmentId = @EquipmentId
	AND
	(
		(TOTAL.Item='Calibration' AND TOTAL.Type=0 AND @CalibrationInt=1)
		OR
		(TOTAL.Item='Calibration' AND TOTAL.Type=1 AND @CalibrationExt=1)
		OR
		(TOTAL.Item='Verification' AND TOTAL.Type=0 AND @VerificationInt=1)
		OR
		(TOTAL.Item='Verification' AND TOTAL.Type=1 AND @VerificationExt=1)
		OR
		(TOTAL.Item='Maintenance' AND TOTAL.Type=0 AND @MaintenanceInt=1)
		OR
		(TOTAL.Item='Maintenance' AND TOTAL.Type=1 AND @MaintenanceExt=1)
		OR
		(TOTAL.Item='Repair' AND TOTAL.Type=0 AND @RepairInt=1)
		OR
		(TOTAL.Item='Repair' AND TOTAL.Type=1 AND @RepairExt=1)		
	)
	AND
	(@DateFrom IS NULL OR TOTAL.Date >= @DateFrom)
	AND
	(@DateTo IS NULL OR TOTAL.Date <= @DateTo)
	
	ORDER BY TOTAL.Date
END





GO

/****** Object:  StoredProcedure [dbo].[Equipment_GetVerification]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Equipment_GetVerification]
	@EquipmentId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		EV.Id,
		EV.CompanyId,
		EV.VerificationType,
		EV.CreatedOn,
		EV.Cost,
		EV.Responsable,
		E.Name,
		E.LastName,
		EV.ModifiedBy,
		EMP2.Name,
		EMP2.LastName,
		EV.ModifiedOn
	FROM EquipmentVerificationDefinition EV WITH(NOLOCK)
	INNER JOIN Employee E WITH(NOLOCK)
	ON	E.Id = EV.Responsable
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee EMP2 WITH(NOLOCK)
		ON EMP2.Id = EUA.EmployeeId
	ON	EUA.UserId = EV.ModifiedBy
	WHERE
		EV.Active = 1
	AND EquipmentId = @EquipmentId
END





GO

/****** Object:  StoredProcedure [dbo].[Equipment_GetVerificationDefinition]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Equipment_GetVerificationDefinition]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 
		EV.Id,
		EV.CompanyId,
		EV.EquipmentId,
		EV.VerificationType,
		EV.Operation,
		EV.Periodicity,
		EV.Uncertainty,
		EV.Pattern,
		EV.Cost,
		EV.Notes,
		EV.Range,
		EV.Responsable,
		RESP.Name,
		RESP.LastName,
		EV.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		EV.ModifiedOn,
		ISNULL(EV.ProviderId,0) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderDescription
    FROM EquipmentVerificationDefinition EV WITH(NOLOCK)
    INNER JOIN Employee RESP WITH(NOLOCK)
    ON	RESP.Id = EV.Responsable
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = EV.ModifiedBy
	LEFT JOIN Provider P WITH(NOLOCK)
	ON	P.Id = EV.ProviderId
	
	WHERE
		EV.EquipmentId = @EquipmentId
	AND EV.CompanyId = @CompanyId
	AND EV.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[Equipment_Insert]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Equipment_Insert]
	@EquipmentId bigint output,
	@CompanyId int,
	@Code nvarchar(50),
	@Description nvarchar(150),
	@TradeMark nvarchar(50),
	@Model nvarchar(50),
	@SerialNumber nvarchar(50),
	@Location nvarchar(50),
	@MeasureRange nvarchar(50),
	@ScaleDivision numeric(18,4),
	@MeasureUnit bigint,
	@Responsable int,
	@IsCalibration bit,
	@IsVerification bit,
	@IsMaintenance bit,
	@Observations nvarchar(500),
	@UserId int,
	@Notes nvarchar(500),
	@StartDate datetime
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Equipment
	(
		CompanyId,
		Code,
		Description,
		TradeMark,
		Model,
		SerialNumber,
		Location,
		MeasureRange,
		ScaleDivision,
		MeasureUnit,
		Resposable,
		IsCalibration,
		IsVerification,
		IsMaintenance,
		Observations,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn,
		Notes,
		StartDate
	)
	VALUES
	(
		@CompanyId,
		@Code,
		@Description,
		@TradeMark,
		@Model,
		@SerialNumber,
		@Location,
		@MeasureRange,
		@ScaleDivision,
		@MeasureUnit,
		@Responsable,
		@IsCalibration,
		@IsVerification,
		@IsMaintenance,
		@Observations,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE(),
		@Notes,
		@StartDate
	)
	
	SET @EquipmentId = @@IDENTITY;

	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		13,
		@EquipmentId,
		1,
		GETDATE(),
		@Description
    )
END





GO

/****** Object:  StoredProcedure [dbo].[Equipment_Restore]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Equipment_Restore]
	@EquipmentId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Equipment SET
		EndDate = NULL,
		EndReason = NULL,
		EndResponsible = NULL,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @EquipmentId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Equipment_SubtituteEmployee]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Equipment_SubtituteEmployee]
	@ItemId bigint,
	@CompanyId int,
	@NewEmployee bigint,
	@UserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE Equipment SET
		Resposable = @NewEmployee,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @ItemId
	AND CompanyId = @CompanyId
END




GO

/****** Object:  StoredProcedure [dbo].[Equipment_Update]    Script Date: 24/10/2018 20:20:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Equipment_Update]
	@EquipmentId bigint,
	@CompanyId int,
	@Code nvarchar(50),
	@Description nvarchar(150),
	@TradeMark nvarchar(50),
	@Model nvarchar(50),
	@SerialNumber nvarchar(50),
	@Location varchar(50),
	@MeasureRange nvarchar(50),
	@ScaleDivision numeric(18,4),
	@MeasureUnit bigint,
	@Responsable int,
	@IsCalibration bit,
	@IsVerification bit,
	@IsMaintenance bit,
	@Observations nvarchar(500),
	@Notes nvarchar(500),
	@Active bit,
	@UserId int,
	@Trace nvarchar(50),
	@StartDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE Equipment SET
		[Code] = @Code,
		[Description] = @Description,
		[TradeMark] = @TradeMark,
		[Model] = @Model,
		[SerialNumber] = @Serialnumber,
		[Location] = @Location,
		[MeasureRange] = @MeasureRange,
		[ScaleDivision] = @ScaleDivision,
		[MeasureUnit] = @MeasureUnit,
		[Resposable] = @Responsable,
		[IsCalibration] = @IsCalibration,
		[IsVerification] = @IsVerification,
		[IsMaintenance] = @IsMaintenance,
		[Observations] = @Observations,
		[Notes] = @Notes,
		[Active] = @Active,
		[ModifiedBy] = @UserId,
		[ModifiedOn] = GETDATE(),
		[StartDate] = @StartDate
	WHERE
		Id = @EquipmentId
	AND CompanyId = @CompanyId	
	
		DELETE FROM EquipmentCalibrationDefinition
		WHERE
			EquipmentId = @EquipmentId
		AND @IsCalibration = 0 
		
		DELETE FROM EquipmentVerificationDefinition
		WHERE
			EquipmentId = @EquipmentId
		AND @IsVerification = 0 

	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		13,
		@EquipmentId,
		2,
		GETDATE(),
		@Trace
    )
END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentCalibrationAct_Delete]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentCalibrationAct_Delete]
	@EquipmentCalibrationActId bigint output,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentCalibrationAct SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE	
		Id = @EquipmentCalibrationActId

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentCalibrationAct_GetByEquipmentId]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentCalibrationAct_GetByEquipmentId]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		ECA.Id,
		ECA.CompanyId,
		ECA.EquipmentId,
		ECA.EquipmentCalibrationType,
		ECA.Date,
		ECA.Vto,
		ECA.Result,
		ECA.MaxResult,
		ECA.Cost,
		ISNULL(ECA.ProviderId,0) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderName,
		ECA.Responsable AS ResponsableId,
		RESP.Name AS ResponsableName,
		RESP.LastName AS ResponsableLastName,
		ECA.Active,
		ECA.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		ECA.ModifiedOn
    FROM EquipmentCalibrationAct ECA WITH(NOLOCK)
    INNER JOIN Employee RESP WITH(NOLOCK)
    ON	RESP.Id = ECA.Responsable
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = ECA.ProviderId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = ECA.ModifiedBy
	
	WHERE
		ECA.EquipmentId = @EquipmentId
	AND ECA.CompanyId = @CompanyId
	AND ECA.Active = 1
	
	ORDER BY ECA.Date DESC
END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentCalibrationAct_Insert]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentCalibrationAct_Insert]
	@EquipmentCalibrationActId bigint output,
	@EquipmentId bigint,
	@CompanyId int,
	@EquipmentCalibrationType int,
	@Operation nvarchar(50),
	@Date datetime,
	@Vto datetime,
	@Result numeric(18,6),
	@MaxResult numeric(18,6),
	@Cost numeric(18,3),
	@ProviderId bigint,
	@ResponsableId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO EquipmentCalibrationAct
    (
		EquipmentId,
		CompanyId,
		EquipmentCalibrationType,
		Operation,
		Date,
		Vto,
		Result,
		MaxResult,
		Cost,
		ProviderId,
		Responsable,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@EquipmentId,
		@CompanyId,
		@EquipmentCalibrationType,
		@Operation,
		@Date,
		@Vto,
		@Result,
		@MaxResult,
		@Cost,
		@ProviderId,
		@ResponsableId,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)

	SET @EquipmentCalibrationActId = @@IDENTITY

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentCalibrationAct_Update]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentCalibrationAct_Update]
	@EquipmentCalibrationActId bigint output,
	@EquipmentId bigint,
	@CompanyId int,
	@EquipmentCalibrationType int,
	@Date datetime,
	@Vto datetime,
	@Result numeric(18,6),
	@MaxResult numeric(18,6),
	@Cost numeric(18,3),
	@ProviderId bigint,
	@ResponsableId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentCalibrationAct SET
		EquipmentCalibrationType = @EquipmentCalibrationType,
		Date = @Date,
		Vto = @Vto,
		Result = @Result,
		MaxResult = @MaxResult,
		Cost = @Cost,
		ProviderId = @ProviderId,
		Responsable = @ResponsableId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE	
		Id = @EquipmentCalibrationActId
	AND	EquipmentId = @EquipmentId
	AND CompanyId = @CompanyId

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentCalibrationDefinition_Delete]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentCalibrationDefinition_Delete]
	@EquipmentCalibrationDefinitionId bigint,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentCalibrationDefinition SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
      WHERE
		Id = @EquipmentCalibrationDefinitionId

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentCalibrationDefinition_Insert]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentCalibrationDefinition_Insert]
	@EquipmentCalibrationDefinitionId bigint output,
	@EquipmentId bigint,
    @CompanyId int,
    @Operation nvarchar(50),
    @CalibrationType int,
    @Periodicity int,
    @Uncertainty numeric(18,6),
    @Range nvarchar(50),
    @Pattern nvarchar(50),
    @Cost numeric(18,3),
    @Notes text,
    @Responsable int,
    @Provider int,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO EquipmentCalibrationDefinition
    (
		EquipmentId,
		CompanyId,
		Operation,
		Type,
		Periodicity,
		Uncertainty,
		Range,
		Pattern,
		Cost,
		Notes,
		Responsable,
		ProviderId,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@EquipmentId,
		@CompanyId,
		@Operation,
		@CalibrationType,
		@Periodicity,
		@Uncertainty,
		@Range,
		@Pattern,
		@Cost,
		@Notes,
		@Responsable,
		@Provider,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)

	SET @EquipmentCalibrationDefinitionId = @@IDENTITY
	 
	UPDATE Equipment SET
		IsCalibration = 1
		WHERE
		Id = @EquipmentId


END




GO

/****** Object:  StoredProcedure [dbo].[EquipmentCalibrationDefinition_SubtituteEmployee]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[EquipmentCalibrationDefinition_SubtituteEmployee]
	@ItemId bigint,
	@CompanyId int,
	@NewEmployee bigint,
	@UserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE EquipmentCalibrationDefinition SET
		Responsable = @NewEmployee,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @ItemId
	AND CompanyId = @CompanyId
END




GO

/****** Object:  StoredProcedure [dbo].[EquipmentCalibrationDefinition_Update]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentCalibrationDefinition_Update]
	@EquipmentCalibrationDefinitionId bigint,
	@EquipmentId bigint,
    @CompanyId int,
    @Operation nvarchar(50),
    @CalibrationType int,
    @Periodicity int,
    @Uncertainty numeric(18,6),
    @Range nvarchar(50),
    @Pattern nvarchar(50),
    @Cost numeric(18,3),
    @Notes text,
    @Responsable int,
    @Provider int,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentCalibrationDefinition SET
		EquipmentId = @EquipmentId,
		Type = @CalibrationType,
		Operation = @Operation,
		Periodicity = @Periodicity,
		Uncertainty = @Uncertainty,
		Range = @Range,
		Pattern = @Pattern,
		Cost = @Cost,
		Notes = @Notes,
		Responsable = @Responsable,
		ProviderId = @Provider,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
      WHERE
		Id = @EquipmentCalibrationDefinitionId
	AND CompanyId = @CompanyId

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentMaintenance_GetByEquipmentId]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentMaintenance_GetByEquipmentId]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		M.Id,
		M.EquipmentId,
		M.CompanyId,
		M.Operation,
		M.Type,
		M.Periodicity,
		M.Accessories,
		M.Cost,
		M.Active,
		ISNULL(P.Id,-1) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderDescription,
		M.ResponsableId,
		R.Name,
		R.LastName,
		M.ModifiedBy AS ModifiedByUserId,
		0 AS ModifiedByEmployeeId,
		AU.[Login] AS ModifiedByName,
		'' AS ModifiedByLastName,
		M.ModifiedOn
    FROM EquipmentMaintenanceDefinition M WITH(NOLOCK)
    INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = M.ModifiedBy
	LEFT JOIN Provider P
	ON P.Id = M.ProviderId
	INNER JOIN Employee R WITH(NOLOCK)
	ON	R.Id = M.ResponsableId
	WHERE
		M.EquipmentId = @EquipmentId
	AND M.CompanyId = @CompanyId
	AND M.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentMaintenance_Insert]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentMaintenance_Insert]
	@EquipmentMaintenanceId bigint output,
	@EquipmentId bigint,
	@CompanyId int,
	@Operation nvarchar(50),
	@EquipmentMaintenanceType int,
	@Periodicity int,
	@Accessories nvarchar(50),
	@Cost numeric(18,3),
	@ProviderId bigint,
	@ResponsableId int,
	@UserId int	
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO EquipmentMaintenanceDefinition
    (
		EquipmentId,
		CompanyId,
		Operation,
		Type,
		Periodicity,
		Accessories,
		Cost,
		ProviderId,
		ResponsableId,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@EquipmentId,
		@CompanyId,
		@Operation,
		@EquipmentMaintenanceType,
		@Periodicity,
		@Accessories,
		@Cost,
		@ProviderId,
		@ResponsableId,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)
	
	SET @EquipmentMaintenanceId = @@IDENTITY
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		16,
		@EquipmentMaintenanceId,
		1,
		GETDATE(),
		''
    )

	UPDATE Equipment SET
		IsMaintenance = 1
		WHERE
		Id = @EquipmentId

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentMaintenanceAct_Delete]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentMaintenanceAct_Delete]
	@EquipmentMaintenanceActId bigint,
	@CompanyId int,
	@UserId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE EquipmentMaintenanceAct SET
		Active = 0,
		ModifiedBy = @USerId,
		ModifiedOn = GETDATE()
    WHERE
		Id = @EquipmentMaintenanceActId
	AND CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		17,
		@EquipmentMaintenanceActId,
		3,
		GETDATE(),
		''
    )

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentMaintenanceAct_GetByEquipmentId]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentMaintenanceAct_GetByEquipmentId]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		EMA.Id,
		EMA.EquipmentId,
		EMA.EquipmentMaintenanceDefinitionId,
		EMA.CompanyId,
		EMA.[Date],
		EMA.Operation,
		EMA.Observations,
		ISNULL(P.Id,-1) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderDescription,
		EMA.ResponsableId,
		R.Name AS ResponsableName,
		R.LastName AS ResponsableLastName,
		EMA.Cost,
		EMA.Vto,
		EMA.Active,
		EMA.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		EMA.ModifiedOn 
    FROM EquipmentMaintenanceAct EMA WITH(NOLOCK)
    LEFT JOIN Employee R WITH(NOLOCK)
    ON	R.Id = EMA.ResponsableId
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = EMA.ProviderId
    INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = EMA.ModifiedBy
	
	WHERE
		EMA.EquipmentId = @EquipmentId
	AND EMA.CompanyId = @CompanyId
	AND EMA.Active = 1
	
	ORDER BY EMA.Date
END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentMaintenanceAct_Insert]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentMaintenanceAct_Insert]
	@EquipmentMaintenanceActId bigint output,
	@EquipmentMaintenanceDefinitionId bigint,
	@EquipmentId bigint,
	@CompanyId int,
	@Date datetime,
	@Operation nvarchar(50),
	@Observations text,
	@ProviderId bigint,
	@ResponsableId int,
	@Cost numeric(18,3),
	@Vto datetime,
	@UserId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO EquipmentMaintenanceAct
    (
		EquipmentId,
		EquipmentMaintenanceDefinitionId,
		CompanyId,
		Date,
		Operation,
		Observations,
		ProviderId,
		ResponsableId,
		Cost,
		Vto,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@EquipmentId,
		@EquipmentMaintenanceDefinitionId,
		@CompanyId,
		@Date,
		@Operation,
		@Observations,
		@ProviderId,
		@ResponsableId,
		@Cost,
		@Vto,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)
	
	SET @EquipmentMaintenanceActId = @@IDENTITY
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		17,
		@EquipmentMaintenanceActId,
		1,
		GETDATE(),
		@Operation
    )


END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentMaintenanceAct_Update]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentMaintenanceAct_Update]
	@EquipmentMaintenanceActId bigint,
	@CompanyId int,
	@Date datetime,
	@Operation nvarchar(50),
	@Observations text,
	@ProviderId bigint,
	@ResponsableId int,
	@Cost numeric(18,3),
	@Vto datetime,
	@UserId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE EquipmentMaintenanceAct SET
		Date = @Date,
		Operation = @Operation,
		Observations = @Observations,
		ProviderId = @ProviderId,
		ResponsableId = @ResponsableId,
		Cost = @Cost,
		Vto = @Vto,
		ModifiedBy = @USerId,
		ModifiedOn = GETDATE()
    WHERE
		Id = @EquipmentMaintenanceActId
	AND CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		17,
		@EquipmentMaintenanceActId,
		2,
		GETDATE(),
		@Operation
    )


END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentMaintenanceDefinition_Delete]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentMaintenanceDefinition_Delete]
	@EquipmentMaintenanceDefinitionId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentMaintenanceDefinition SET
		Active = 0
	WHERE
		Id = @EquipmentMaintenanceDefinitionId
		
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		16,
		@EquipmentMaintenanceDefinitionId,
		3,
		GETDATE(),
		''
    )	
END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentMaintenanceDefinition_Insert]    Script Date: 24/10/2018 20:20:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentMaintenanceDefinition_Insert]
	@EquipmentMaintenanceDefinitionId bigint output,
	@EquipmentId bigint,
	@CompanyId int,
	@Operation nvarchar(50),
	@Type int,
	@Periodicity int,
	@Accesories nvarchar(50),
	@Cost numeric(18,3),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO EquipmentMaintenanceDefinition
           ([EquipmentId]
           ,[CompanyId]
           ,[Operation]
           ,[Type]
           ,[Periodicity]
           ,[Accessories]
           ,[Cost]
           ,[Active]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn])
     VALUES
           (@EquipmentId
           ,@CompanyId
           ,@Operation
           ,@Type
           ,@Periodicity
           ,@Accesories
           ,@Cost
           ,1
           ,@UserId
           ,GETDATE()
           ,@UserId
           ,GETDATE())
	
	SET @EquipmentMaintenanceDefinitionId = @@IDENTITY
	 
	 UPDATE Equipment SET
		IsMaintenance = 1
		WHERE
		Id = @EquipmentId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		16,
		@EquipmentMaintenanceDefinitionId,
		1,
		GETDATE(),
		@Operation
    )
END




GO

/****** Object:  StoredProcedure [dbo].[EquipmentMaintenanceDefinition_SubtituteEmployee]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[EquipmentMaintenanceDefinition_SubtituteEmployee]
	@ItemId bigint,
	@CompanyId int,
	@NewEmployee bigint,
	@UserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE EquipmentMaintenanceDefinition SET
		ResponsableId = @NewEmployee,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @ItemId
	AND CompanyId = @CompanyId
END




GO

/****** Object:  StoredProcedure [dbo].[EquipmentMaintnanceDefinition_Update]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentMaintnanceDefinition_Update]
	@EquipmentMaintenanceDefinitionId bigint,
	@EquipmentId bigint,
	@CompanyId int,
	@Operation nvarchar(50),
	@MaintenanceType int,
	@Periodicity int,
	@Accessories nvarchar(50),
	@Cost numeric(18,3),
	@ProviderId bigint,
	@ResponsableId int,
	@Differences text,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE EquipmentMaintenanceDefinition SET
		Operation = @Operation,
		Type = @MaintenanceType,
		Periodicity = @Periodicity,
		Accessories = @Accessories,
		Cost = @Cost,
		ProviderId = @ProviderId,
		ResponsableId = @ResponsableId,
		Active = 1,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @EquipmentMaintenanceDefinitionId
	AND	EquipmentId = @EquipmentId
	AND CompanyId = @CompanyId

	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		16,
		@EquipmentMaintenanceDefinitionId,
		3,
		GETDATE(),
		@Differences
    )

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentRepair_Delete]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentRepair_Delete]
	@EquipmentRepairId bigint,
	@CompanyId int,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentRepair SET
    	Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @EquipmentRepairId
	AND CompanyId = @CompanyId


END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentRepair_GetByEquipmentId]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentRepair_GetByEquipmentId]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		R.Id,
		R.EquipmentId,
		R.CompanyId,
		R.RepairType,
		R.Date,
		R.Description,
		R.Tools,
		R.Observations,
		R.Cost,
		ISNULL(R.ProviderId,0) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderDescription,
		R.ResponsableId AS ResponsableUserId,
		RESP.Id AS ResponsableEmployeeId,
		RESP.Name AS ResponsableName,
		RESP.LastName AS ResponsableName,
		R.Active,
		R.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		R.ModifiedOn
	FROM EquipmentRepair R WITH(NOLOCK)
	INNER JOIN Employee RESP WITH(NOLOCK)
	ON	RESP.Id = R.ResponsableId
	AND RESP.CompanyId = R.CompanyId
	LEFT JOIN Provider P WITH(NOLOCK)
	ON	P.Id = R.ProviderId
	AND P.CompanyId = R.CompanyId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = R.ModifiedBy
	
	WHERE
		R.EquipmentId = @EquipmentId
	AND R.CompanyId = @CompanyId
	
	ORDER BY R.Date
END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentRepair_Insert]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentRepair_Insert] 
	@EquipmentRepairId bigint output,
	@EquipmentId bigint,
	@CompanyId int,
	@RepairType int,
    @Date datetime,
    @Description text,
    @Tools text,
    @Observations text,
    @Cost numeric(18,3),
    @ProviderId bigint,
    @ResponsableId int,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO EquipmentRepair
    (
		EquipmentId,
		CompanyId,
		RepairType,
		Date,
		Description,
		Tools,
		Observations,
		Cost,
		ProviderId,
		ResponsableId,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@EquipmentId,
		@CompanyId,
		@RepairType,
		@Date,
		@Description,
		@Tools,
		@Observations,
		@Cost,
		@ProviderId,
		@ResponsableId,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)

	SET @EquipmentRepairId = @@IDENTITY

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentRepair_Update]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentRepair_Update]
	@EquipmentRepairId bigint,
	@EquipmentId bigint,
	@CompanyId int,
	@RepairType int,
    @Date datetime,
    @Description text,
    @Tools text,
    @Observations text,
    @Cost numeric(18,3),
    @ProviderId bigint,
    @ResponsableId int,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentRepair SET
    	RepairType = @RepairType,
		Date = @Date,
		Description = @Description,
		Tools = @Tools,
		Observations = @Observations,
		Cost = @Cost,
		ProviderId = @ProviderId,
		ResponsableId = @ResponsableId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @EquipmentRepairId
	AND CompanyId = @CompanyId


END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentScaleDivision_Delete]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentScaleDivision_Delete]
	@EquipmentScaleDivisionId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentScaleDivision SET
		Active = 0
	WHERE
		Id = @EquipmentScaleDivisionId
		
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		19,
		@EquipmentScaleDivisionId,
		3,
		GETDATE(),
		''
    )	
END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentScaleDivision_GetByCompany]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentScaleDivision_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
		ESD.Id,
		ESD.CompanyId,
		ESD.Description,
		ESD.Active,
		ESD.ModifiedBy AS ModifiedByUserId,
		ISNULL(AU.[Login],'') AS ModifiedByUserName,
		ESD.ModifiedOn,		
		CASE WHEN EQ.Id IS NULL THEN 0 ELSE 1 END AS InEquipment
	FROM EquipmentScaleDivision ESD WITH(NOLOCK)
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		LEFT JOIN ApplicationUser AU WITH(NOLOCK)
		ON	AU.Id = EUA.UserId
	ON EUA.UserId = ESD.ModifiedBy
	LEFT JOIN Equipment EQ WITH(NOLOCK)
	ON	EQ.MeasureUnit = ESD.Id
	AND	EQ.Active = 1
	
	WHERE
		ESD.CompanyId = @CompanyId
	AND ESD.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentScaleDivision_Insert]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentScaleDivision_Insert]
	@EquipmentScaleDivisionId bigint output,
	@Description varchar(20),
	@UserId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO EquipmentScaleDivision
    (
		CompanyId,
		Description,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@Description,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)
	
	SET @EquipmentScaleDivisionId = @@IDENTITY
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		19,
		@EquipmentScaleDivisionId,
		1,
		GETDATE(),
		@Description
    )
END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentScaleDivision_Update]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentScaleDivision_Update]
	@EquipmentScaleDivisionId bigint,
	@Description nvarchar(20),
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
    UPDATE EquipmentScaleDivision SET
		Description = @Description
	WHERE
		Id = @EquipmentScaleDivisionId
		
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		19,
		@EquipmentScaleDivisionId,
		2,
		GETDATE(),
		'Description:' + @Description
    )	
END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentVerificationAct_Delete]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentVerificationAct_Delete]
	@EquipmentVerificationActId bigint output,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentVerificationAct SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE	
		Id = @EquipmentVerificationActId

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentVerificationAct_GetByEquipmentId]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentVerificationAct_GetByEquipmentId]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		ECA.Id,
		ECA.CompanyId,
		ECA.EquipmentId,
		ECA.EquipmentVerificationType,
		ECA.Date,
		ECA.Vto,
		ECA.Result,
		ECA.MaxResult,
		ECA.Cost,
		ISNULL(ECA.ProviderId,0) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderName,
		ECA.Responsable AS ResponsableId,
		RESP.Name AS ResponsableName,
		RESP.LastName AS ResponsableLastName,
		ECA.Active,
		ECA.ModifiedBy AS ModifiedByUserId,
		0 AS ModifiedByEmployeeId,
		AU.[Login] AS ModifiedByUserName,
		ECA.ModifiedOn
    FROM EquipmentVerificationAct ECA WITH(NOLOCK)
    INNER JOIN Employee RESP WITH(NOLOCK)
    ON	RESP.Id = ECA.Responsable
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = ECA.ProviderId
    INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = ECA.ModifiedBy

	WHERE
		ECA.EquipmentId = @EquipmentId
	AND ECA.CompanyId = @CompanyId
	AND ECA.Active = 1
	
	ORDER BY ECA.Date DESC
END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentVerificationAct_Insert]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentVerificationAct_Insert]
	@EquipmentVerificationActId bigint output,
	@EquipmentId bigint,
	@CompanyId int,
	@EquipmentVerificationType int,
	@Operation nvarchar(50),
	@Date datetime,
	@Vto datetime,
	@Result numeric(18,6),
	@MaxResult numeric(18,6),
	@Cost numeric(18,3),
	@ProviderId bigint,
	@ResponsableId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO EquipmentVerificationAct
    (
		EquipmentId,
		CompanyId,
		EquipmentVerificationType,
		Operation,
		Date,
		Vto,
		Result,
		MaxResult,
		Cost,
		ProviderId,
		Responsable,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@EquipmentId,
		@CompanyId,
		@EquipmentVerificationType,
		@Operation,
		@Date,
		@Vto,
		@Result,
		@MaxResult,
		@Cost,
		@ProviderId,
		@ResponsableId,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)

	SET @EquipmentVerificationActId = @@IDENTITY

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentVerificationAct_Update]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentVerificationAct_Update]
	@EquipmentVerificationActId bigint,
	@EquipmentId bigint,
	@CompanyId int,
	@EquipmentVerificationType int,
	@Date datetime,
	@Vto datetime,
	@Result numeric(18,6),
	@MaxResult numeric(18,6),
	@Cost numeric(18,3),
	@ProviderId bigint,
	@ResponsableId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentVerificationAct SET
		EquipmentVerificationType = @EquipmentVerificationType,
		Date = @Date,
		Vto = @Vto,
		Result = @Result,
		MaxResult = @MaxResult,
		Cost = @Cost,
		ProviderId = @ProviderId,
		Responsable = @ResponsableId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE	
		Id = @EquipmentVerificationActId
	AND	EquipmentId = @EquipmentId
	AND CompanyId = @CompanyId

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentVerificationDefinition_Delete]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentVerificationDefinition_Delete]
	@EquipmentVerificationDefinitionId bigint,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentVerificationDefinition SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
      WHERE
		Id = @EquipmentVerificationDefinitionId

END





GO

/****** Object:  StoredProcedure [dbo].[EquipmentVerificationDefinition_Insert]    Script Date: 24/10/2018 20:20:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentVerificationDefinition_Insert]
	@EquipmentVerificationDefinitionId bigint output,
	@EquipmentId bigint,
    @CompanyId int,
    @Operation nvarchar(50),
    @VerificationType int,
    @Periodicity int,
    @Uncertainty numeric(18,6),
    @Range nvarchar(50),
    @Pattern nvarchar(50),
    @Cost numeric(18,3),
    @Notes text,
    @Responsable int,
	@ProviderId bigint,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO EquipmentVerificationDefinition
    (
		EquipmentId,
		CompanyId,
		Operation,
		VerificationType,
		Periodicity,
		Uncertainty,
		Range,
		Pattern,
		Cost,
		Notes,
		Responsable,
		ProviderId,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
     )
     VALUES
     (
		@EquipmentId,
		@CompanyId,
		@Operation,
		@VerificationType,
		@Periodicity,
		@Uncertainty,
		@Range,
		@Pattern,
		@Cost,
		@Notes,
		@Responsable,
		@ProviderId,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
     )
     
     SET @EquipmentVerificationDefinitionId = @@IDENTITY
	 
	 UPDATE Equipment SET
		IsVerification = 1
		WHERE
		Id = @EquipmentId
END




GO

/****** Object:  StoredProcedure [dbo].[EquipmentVerificationDefinition_SubtituteEmployee]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[EquipmentVerificationDefinition_SubtituteEmployee]
	@ItemId bigint,
	@CompanyId int,
	@NewEmployee bigint,
	@UserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE EquipmentVerificationDefinition SET
		Responsable = @NewEmployee,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @ItemId
	AND CompanyId = @CompanyId
END




GO

/****** Object:  StoredProcedure [dbo].[EquipmentVerificationDefinition_Update]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[EquipmentVerificationDefinition_Update]
	@EquipmentVerificationDefinitionId bigint output,
	@EquipmentId bigint,
    @CompanyId int,
    @Operation nvarchar(50),
    @VerificationType int,
    @Periodicity int,
    @Uncertainty numeric(18,6),
    @Range nvarchar(50),
    @Pattern nvarchar(50),
    @Cost numeric(18,3),
    @Notes text,
    @Responsable int,
	@ProviderId bigint,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentVerificationDefinition SET
		EquipmentId = @EquipmentId,
		VerificationType = @VerificationType,
		Operation = @Operation,
		Periodicity = @Periodicity,
		Uncertainty = @Uncertainty,
		Range = @Range,
		Pattern = @Pattern,
		Cost = @Cost,
		Notes = @Notes,
		Responsable = @Responsable,
		ProviderId = @ProviderId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
      WHERE
		Id = @EquipmentVerificationDefinitionId
	AND CompanyId = @CompanyId

END





GO

/****** Object:  StoredProcedure [dbo].[Get_Activity]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Get_Activity]
	@CompanyId int,
	@TargetType int,
	@ItemId int,
	@From date,
	@To date
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
		AU.[Login] AS Employee,
		AL.TargetId AS TargetId
	FROM ActivityLog AL WITH(NOLOCK)
	INNER JOIN ActivityTarget AT WITH(NOLOCK)
	ON	AT.ActivityTarget = AL.TargetType
	INNER JOIN ActivityAction AA WITH(NOLOCK)
	ON	AA.ActivityTarget = AL.TargetType
	AND	AA.ActivityAction = AL.ActionId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = AL.UserId
	
	WHERE
		(AL.TargetId = @ItemId OR @ItemId IS NULL)
	AND	(AL.CompanyId = @CompanyId OR @CompanyId IS NULL)
	AND	(AL.TargetType = @TargetType OR @TargetType IS NULL)
	AND (
			(@From IS NULL AND @To IS NULL)
			OR
			(@From IS NULL AND @To > AL.DateTime)
			OR
			(@From < AL.DateTime AND @To IS NULL)
			OR
			(AL.DateTime BETWEEN @From AND @To)
		)
		
	ORDER BY Al.CompanyId ASC, AL.TargetId ASC, Al.DateTime DESC
END





GO

/****** Object:  StoredProcedure [dbo].[Get_ActivityLastDay]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






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






GO

/****** Object:  StoredProcedure [dbo].[Get_EmployeeDepartmentAsignation]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Get_EmployeeDepartmentAsignation] 
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		EMD.EmployeeId,
		EMD.DepartmentId
	FROM EmployeeDepartmentMembership EMD
	WHERE
		EMD.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Get_LogLogins]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Get_LogLogins]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
    L.Result,
    L.Date,
    L.Ip,
    ISNULL(L.CompanyCode,'') AS Companycode,
    AU.Login
    FROM Logins L WITH(NOLOCK)
    LEFT JOIN ApplicationUser AU WITH(NOLOCK)
    ON
		L.UserId = AU.Id
    WHERE
		(@CompanyId IS NULL OR L.CompanyId= @CompanyId)
	ORDER BY Date DESC
END





GO

/****** Object:  StoredProcedure [dbo].[Get_Procesos]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Get_Procesos]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    
	SELECT 
		P.Id,
		P.CompanyId,
		P.Type,
		ISNULL(P.Inicio,'') AS Inicio,
		ISNULL(P.Desarrollo,'') AS Desarrollo,
		ISNULL(P.Fin,'') AS Fin,
		ISNULL(P.Description,'') AS Description,
		C.Id AS CargoId,
		C.Description AS CargoDescription,
		CASE WHEN I.Id IS NULL THEN CAST(1 AS bit) ELSE CAST(0 AS Bit) END AS CanBeDeleted
	FROM Proceso P WITH(NOLOCK)
	LEFT JOIN Cargos C WITH(NOLOCK)
	ON	C.Id = P.CargoId
	AND C.Active = 1
	LEFT JOIN Indicador I WITH(NOLOCK)
	ON	I.ProcessId = P.Id
	AND	I.Active = 1
	WHERE
		P.CompanyId = @CompanyId
	AND P.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[Get_ProcesosById]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Get_ProcesosById]
	@Id int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
    
	SELECT 
		P.Id,
		P.CompanyId,
		P.Type,
		ISNULL(P.Inicio,'') AS Inicio,
		ISNULL(P.Desarrollo,'') AS Desarrollo,
		ISNULL(P.Fin,'') AS Fin,
		ISNULL(P.Description,'') AS Description,
		P.CargoId,
		P.ModifiedBy,
		P.ModifiedOn
	FROM Proceso P WITH(NOLOCK)
	WHERE
		P.Id = @Id
	AND	P.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[GetLogin]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[GetLogin]
	@Login nvarchar(50),
	@Password nvarchar(15)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		AU.Id,
		AU.Login,
		CASE WHEN C.ID = 1 
			THEN 1000
			ELSE CASE WHEN C.Id = 2 
				THEN 1002
				ELSE AU.Status
			END
		END AS Status,
		AU.Language,
		C.Id AS CompanyId,
		C.Language AS CompanyLanguage,
		AGM.SecurityGroupId,
		0,
		SG.Name,
		AU.Status,
		AU.MustResetPassword,
		0 AS PrimaryUser,
		ISNULL(C.Agreement,0) AS Agreement
	FROM ApplicationUser AU WITH(NOLOCK)
	LEFT JOIN Company C WITH(NOLOCK)
	ON	AU.CompanyId = C.Id
	LEFT JOIN ApplicationUserSecurityGroupMembership AGM WITH(NOLOCK)
	ON	AGM.ApplicationUserId = AU.Id
	AND	AGM.CompanyId = C.Id
	LEFT JOIN SecurityGroup SG WITH(NOLOCK)
	ON SG.Id = AGM.SecurityGroupId
	
	WHERE
		AU.Email = @Login
	AND AU.Password = @Password
	AND AU.[Status] = 1
END





GO

/****** Object:  StoredProcedure [dbo].[Grants_GetAll]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Grants_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
	-1,
	Id,
	0 AS GrantToRead,
	0 AS GrantToWrite,
	0 AS GrantToWrite,
	AI.UrlList
	FROM  ApplicationItem AI

	ORDER BY AI.Description
END





GO

/****** Object:  StoredProcedure [dbo].[IncidenAction_SusbtituteExecutor]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[IncidenAction_SusbtituteExecutor]
	@ItemId bigint,
	@CompanyId int,
	@NewEmployee bigint,
	@UserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE Incident SET
		ActionsExecuter = @NewEmployee,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @ItemId
	AND CompanyId = @CompanyId
END




GO

/****** Object:  StoredProcedure [dbo].[Incident_Anulate]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Incident_Anulate]
	@IncidentId int,
	@CompanyId int,
	@EndDate datetime,
	@EndResponsible int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Incident SET
		ClosedOn = @EndDate,
		ClosedBy = @EndResponsible,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IncidentId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Incident_ChartReport]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Incident_ChartReport]
	@CompanyId int,
	@DateFrom datetime
AS
BEGIN
	SET NOCOUNT ON;
	
	
		SELECT
			I.Id AS IncidentId,
			I.WhatHappendOn AS OpenDate,
			I.CausesOn AS CausesDate,
			I.ActionsOn AS ActionsDate,
			ISNULL(I.DepartmentId,0) AS DepartmentId,
			ISNULL(D.Name,'') AS DepartmentName,
			ISNULL(P.Id,0) AS ProviderId,
			ISNULL(P.Description,'') AS ProviderDescription,
			ISNULL(C.Id,0) AS CustomerId,
			ISNULL(C.Description,'') AS CustomerDescription,
			I.Description,
			I.Code,
			I.ClosedOn AS CloseDate,
			I.Origin
				
		FROM Incident I WITH(NOLOCK)
		LEFT JOIN Department D WITH(NOLOCK)
		ON	D.Id = I.DepartmentId
		AND D.CompanyId = I.CompanyId
		LEFT JOIN Provider P WITH(NOLOCK)
		ON	P.Id = I.ProviderId
		AND P.CompanyId = I.CompanyId
		LEFT JOIN Customer C WITH(NOLOCK)
		ON	C.Id = CustomerId
		AND	C.CompanyId = I.CompanyId
		LEFT JOIN IncidentAction IA WITH(NOLOCK)
		ON	IA.IncidentId = I.Id
		AND	IA.CompanyId = I.CompanyId
		AND IA.Active = 1
		LEFT JOIN IncidentCost IC WITH(NOLOCK)
		ON	IC.IncidentId = I.Id
		AND	IC.CompanyId = I.CompanyId
		AND IC.Active = 1
		WHERE
			I.CompanyId = @CompanyId
		AND I.Active = 1
		AND	(@DateFrom IS NULL OR I.WhatHappendOn >= @DateFrom)
		
		GROUP BY
			I.Id ,
			I.WhatHappendOn,
			I.DepartmentId,
			D.Name,
			P.Id,
			P.Description,
			C.Id,
			C.Description,
			I.Description,
			I.Code,
			I.ClosedOn,
			IA.Id,
			IA.Number,
			I.Origin,
			I.ActionsOn,
			I.CausesOn
END





GO

/****** Object:  StoredProcedure [dbo].[Incident_Delete]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Incident_Delete]
	@IncidentId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE Incident SET 
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentId
	AND CompanyId = @CompanyId

	UPDATE IncidentAction SET
		Active = 0,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		IncidentId = @IncidentId
	AND CompanyId = @CompanyId

	SELECT * FROM IncidentAction WHERE IncidentId = @IncidentId

END





GO

/****** Object:  StoredProcedure [dbo].[Incident_DeleteActions]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Incident_DeleteActions]
	@IncidentId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    Update IncidentAction SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		IncidentId = @IncidentId
	AND	CompanyId = @CompanyId
	AND Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[Incident_Filter]    Script Date: 24/10/2018 20:20:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Incident_Filter]
	@CompanyId int,
	@DateFrom datetime,
	@DateTo datetime,
	@StatusIdentified bit,
	@StatusAnalyzed bit,
	@StatusInProcess bit,
	@StatusClosed bit,
	@Origin int,
	@DepartmentId int,
	@ProviderId bigint,
	@CustomerId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
	*
	FROM
	(
		SELECT
			I.Id AS IncidentId,
			I.WhatHappendOn AS OpenDate,
			ISNULL(I.DepartmentId,0) AS DepartmentId,
			ISNULL(D.Name,'') AS DepartmentName,
			ISNULL(P.Id,0) AS ProviderId,
			ISNULL(P.Description,'') AS ProviderDescription,
			ISNULL(C.Id,0) AS CustomerId,
			ISNULL(C.Description,'') AS CustomerDescription,
			I.Description,
			I.Code,
			I.ClosedOn AS CloseDate,
			ISNULL(IA.Id,0) AS IncidentActionId,
			ISNULL(IA.[Description],'') AS IncidentActionDescription,
			I.Origin,
			SUM(ISNULL(IC.Amount * IC.Quantity,0)) AS Amount,
			CASE	WHEN I.ClosedOn IS NOT NULL  THEN 4 
					WHEN I.ActionsOn IS NOT NULL  THEN 3 
					WHEN I.CausesOn IS NOT NULL  THEN 2 
					WHEN I.WhatHappendOn IS NOT NULL  THEN 1 
			ELSE 0 END AS Status
				
		FROM Incident I WITH(NOLOCK)
		LEFT JOIN Department D WITH(NOLOCK)
		ON	D.Id = I.DepartmentId
		AND D.CompanyId = I.CompanyId
		LEFT JOIN Provider P WITH(NOLOCK)
		ON	P.Id = I.ProviderId
		AND P.CompanyId = I.CompanyId
		LEFT JOIN Customer C WITH(NOLOCK)
		ON	C.Id = CustomerId
		AND	C.CompanyId = I.CompanyId
		LEFT JOIN IncidentAction IA WITH(NOLOCK)
		ON	IA.IncidentId = I.Id
		AND	IA.CompanyId = I.CompanyId
		AND IA.Active = 1
		LEFT JOIN IncidentCost IC WITH(NOLOCK)
		ON	IC.IncidentId = I.Id
		AND	IC.CompanyId = I.CompanyId
		AND IC.Active = 1
		WHERE
			I.CompanyId = @CompanyId
		AND I.Active = 1
		AND	(@DateFrom IS NULL OR I.WhatHappendOn >= @DateFrom)
		AND (@DateTo IS NULL OR I.WhatHappendOn <= @DateTo)
		AND 
		(
			@Origin=0
			OR
			(
				@Origin = 1 AND I.DepartmentId<> 0  AND (I.DepartmentId = @DepartmentId OR @DepartmentId = -2)
			)
			OR
			(
				@Origin = 2 AND I.ProviderId <> 0 AND (I.ProviderId = @ProviderId OR @ProviderId = -2)
			)
			OR
			(
				@Origin = 3 AND I.CustomerId <> 0 AND (I.CustomerId = @CustomerId OR @CustomerId = -2)
			)
		)
		
		GROUP BY
			I.Id ,
			I.WhatHappendOn,
			I.DepartmentId,
			D.Name,
			P.Id,
			P.[Description],
			C.Id,
			C.[Description],
			I.[Description],
			I.Code,
			I.ClosedOn,
			IA.Id,
			IA.[Description],
			I.Origin,
			I.ActionsOn,
			I.CausesOn
	) AS Data
	WHERE
		(@StatusIdentified = 1 AND Data.Status =1)
		OR
		(@StatusAnalyzed = 1 AND Data.Status = 2)
		OR
		(@StatusInProcess = 1 AND Data.Status = 3)
		OR
		(@StatusClosed = 1 AND Data.Status =4)
END





GO

/****** Object:  StoredProcedure [dbo].[Incident_GetById]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Incident_GetById]
	@IncidentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		I.Id,
		I.CompanyId,
		I.Code,
		I.Origin,
		ISNULL(D.Id,0) AS DepartmentId,
		ISNULL(D.Name,'') AS DepartmentName,
		ISNULL(P.Id,0) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderName,
		ISNULL(C.Id,0) AS CustomerId,
		ISNULL(C.Description,'') AS CustomerName,
		I.WhatHappend,
		ISNULL(WH.Id,0) AS WhatHappendById,
		ISNULL(WH.Name,'') AS WhatHappendByName,
		ISNULL(WH.LastName,'') AS WhatHappendByLastname,
		I.WhatHappendOn,
		I.Causes,
		ISNULL(CAUSES.Id,0) AS WhatCausesById,
		ISNULL(CAUSES.Name,'') AS WhatCausesByName,
		ISNULL(CAUSES.LastName,'') AS WhatCausesByLastname,
		I.CausesOn,
		I.Actions,
		ISNULL(ACTIONS.Id,0) AS ActionsById,
		ISNULL(ACTIONS.Name,'') AS ActionsName,
		ISNULL(ACTIONS.LastName,'') AS ActionsLastname,
		I.ActionsOn,
		EXECUTER.Id,
		EXECUTER.Name,
		EXECUTER.LastName,
		I.ActionsSchedule,
		I.ApplyAction,
		ISNULL(CLOSED.Id,0) AS ClosedById,
		ISNULL(CLOSED.Name,'') AS ClosedByName,
		ISNULL(CLOSED.LastName,'') AS ClosedLastName,
		I.Active,
		I.ModifiedBy AS ModifiedByUserId,		
		AU.[Login] AS ModifiedByUserName,
		I.ModifiedOn,
		I.Description,
		I.Notes,
		I.ClosedOn,
		I.Anotations
    FROM Incident I WITH(NOLOCK)
    LEFT JOIN Department D WITH(NOLOCK)
    ON	D.Id = I.DepartmentId
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = I.ProviderId
    LEFT JOIN Customer C WITH(NOLOCK)
    ON	C.Id = I.CustomerId
    LEFT JOIN Employee WH WITH(NOLOCK)
    ON	WH.Id = WhatHappendBy
    LEFT JOIN Employee CAUSES WITH(NOLOCK)
    ON	CAUSES.Id = CausesBy
    LEFT JOIN Employee ACTIONS WITH(NOLOCK)
    ON	ACTIONS.Id = I.ActionsBy
    LEFT JOIN Employee EXECUTER WITH(NOLOCK)
    ON	EXECUTER.Id = I.ActionsExecuter
    AND EXECUTER.CompanyId = I.CompanyId
    LEFT JOIN Employee CLOSED WITH(NOLOCK)
    ON	CLOSED.Id = I.ClosedBy
    LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = I.ModifiedBy
    WHERE
		I.Id = @IncidentId
	AND I.CompanyId = @CompanyId
    
END





GO

/****** Object:  StoredProcedure [dbo].[Incident_GetTimed]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Incident_GetTimed]
	@CompanyId int,
	@Days int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
	*
	FROM Incident I WITH(NOLOCK)
	WHERE
		I.Active = 1
	AND WhatHappendOn >= GETDATE() - @Days
	
END





GO

/****** Object:  StoredProcedure [dbo].[Incident_Insert]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Incident_Insert]
	@IncidentId bigint output,
	@CompanyId int,
	@Description nvarchar(100),
	@Origin int,
	@DepartmentId int,
	@ProviderId bigint,
	@CustomerId bigint,
	@WhatHappend nvarchar(2000),
	@WhatHappendBy bigint,
	@WhatHappendOn datetime,
	@Causes nvarchar(2000),
	@CausesBy bigint,
	@CausesOn datetime,
	@Actions nvarchar(2000),
	@ActionsBy bigint,
	@ActionsOn datetime,
	@ActionsExecuter bigint,
	@ActionsSchedule datetime,
	@ApplyAction bit,
	@Notes nvarchar(2000),
	@Anotations nvarchar(2000),
	@ClosedBy bigint,
	@ClosedOn datetime,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @Code bigint
	
	SELECT @Code = ISNULL(MAX(I.Code),1) + 1
	FROM Incident I WITH(NOLOCK)
	WHERE
		I.CompanyId = @CompanyId
	AND I.Active = 1

					 
	  
				 
	

				   
	  
				 
	

				   
	  
				 
	
	

    INSERT INTO Incident
    (
		CompanyId,
		Description,
		Code,
		Origin,
		DepartmentId,
		ProviderId,
		CustomerId,
		WhatHappend,
		WhatHappendBy,
		WhatHappendOn,
		Causes,
		CausesBy,
		CausesOn,
		Actions,
		ActionsBy,
		ActionsOn,
		ActionsExecuter,
		ActionsSchedule,
		ApplyAction,
		Notes,
		Anotations,
		ClosedBy,
		ClosedOn,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@Description,
		@Code,
		@Origin,
		@DepartmentId,
		@ProviderId,
		@CustomerId,
		@WhatHappend,
		@WhatHappendBy,
		@WhatHappendOn,
		@Causes,
		@CausesBy,
		@CausesOn,
		@Actions,
		@ActionsBy,
		@ActionsOn,
		@ActionsExecuter,
		@ActionsSchedule,
		@ApplyAction,
		@Notes,
		@Anotations,
		@ClosedBy,
		@ClosedOn,
        1,
        @UserId,
        GETDATE(),
        @UserId,
        GETDATE()
	)

	SET @IncidentId = @@IDENTITY
END





GO

/****** Object:  StoredProcedure [dbo].[Incident_Restore]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Incident_Restore]
	@IncidentId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Incident SET
		ClosedOn = NULL,
		ClosedBy = NULL,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IncidentId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Incident_StatusReport]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Incident_StatusReport]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	
		SELECT
			I.Id AS IncidentId,
			I.WhatHappendOn AS OpenDate,
			I.CausesOn AS CausesDate,
			I.ActionsOn AS ActionsDate,
			ISNULL(I.DepartmentId,0) AS DepartmentId,
			ISNULL(D.Name,'') AS DepartmentName,
			ISNULL(P.Id,0) AS ProviderId,
			ISNULL(P.Description,'') AS ProviderDescription,
			ISNULL(C.Id,0) AS CustomerId,
			ISNULL(C.Description,'') AS CustomerDescription,
			I.Description,
			I.Code,
			I.ClosedOn AS CloseDate,
			I.Origin
				
		FROM Incident I WITH(NOLOCK)
		LEFT JOIN Department D WITH(NOLOCK)
		ON	D.Id = I.DepartmentId
		AND D.CompanyId = I.CompanyId
		LEFT JOIN Provider P WITH(NOLOCK)
		ON	P.Id = I.ProviderId
		AND P.CompanyId = I.CompanyId
		LEFT JOIN Customer C WITH(NOLOCK)
		ON	C.Id = CustomerId
		AND	C.CompanyId = I.CompanyId
		LEFT JOIN IncidentAction IA WITH(NOLOCK)
		ON	IA.IncidentId = I.Id
		AND	IA.CompanyId = I.CompanyId
		AND IA.Active = 1
		LEFT JOIN IncidentCost IC WITH(NOLOCK)
		ON	IC.IncidentId = I.Id
		AND	IC.CompanyId = I.CompanyId
		AND IC.Active = 1
		WHERE
			I.CompanyId = @CompanyId
		AND I.Active = 1
		
		GROUP BY
			I.Id ,
			I.WhatHappendOn,
			I.DepartmentId,
			D.Name,
			P.Id,
			P.Description,
			C.Id,
			C.Description,
			I.Description,
			I.Code,
			I.ClosedOn,
			IA.Id,
			IA.Number,
			I.Origin,
			I.ActionsOn,
			I.CausesOn
END





GO

/****** Object:  StoredProcedure [dbo].[Incident_Update]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Incident_Update]
	@IncidentId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@Code int,
	@Origin int,
	@DepartmentId int,
	@ProviderId bigint,
	@CustomerId bigint,
	@WhatHappend nvarchar(2000),
	@WhatHappendBy bigint,
	@WhatHappendOn datetime,
	@Causes nvarchar(2000),
	@CausesBy bigint,
	@CausesOn datetime,
	@Actions nvarchar(2000),
	@ActionsBy bigint,
	@ActionsOn datetime,
	@ActionsExecuter bigint,
	@ActionsSchedule datetime,
	@ApplyAction bit,
	@Notes nvarchar(2000),
	@Anotations nvarchar(2000),
	@ClosedBy bigint,
	@ClosedOn datetime,
	@UserId int,
	@Differences text
AS
BEGIN
	SET NOCOUNT ON;

					 
	  
				 
	

				   
	  
				 
	

				   
	  
				 
	

    UPDATE Incident SET 
      Description = @Description,
      Origin = @Origin,
      DepartmentId = @DepartmentId,
      ProviderId = @ProviderId,
      CustomerId = @CustomerId,
      WhatHappend = @WhatHappend,
      WhatHappendBy = @WhatHappendBy,
      WhatHappendOn = @WhatHappendOn,
      Causes = @Causes,
      CausesBy = @CausesBy,
      CausesOn = @CausesOn,
      Actions = @Actions,
      ActionsBy = @ActionsBy,
      ActionsOn = @ActionsOn,
      ActionsExecuter = @ActionsExecuter,
      ActionsSchedule = @ActionsSchedule,
      ApplyAction = @ApplyAction,
      Notes = @Notes, 
      Anotations = @Anotations,
      --ClosedBy = @ClosedBy,
      --ClosedOn = @ClosedOn, 
      ModifiedBy = @UserId,
      ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentId
	AND CompanyId = @CompanyId

	IF @ApplyAction = 0
	BEGIN
		UPDATE IncidentAction SET
			Active = 0,
			ModifiedBy = @UserId,
			ModifiedOn = GETDATE()
		WHERE
			Active = 1
		AND	IncidentId = @IncidentId
		AND CompanyId = @CompanyId
	END
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		22,
		@IncidentId,
		1,
		GETDATE(),
		@Differences
	)

END





GO

/****** Object:  StoredProcedure [dbo].[IncidentAction_Anulate]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IncidentAction_Anulate]
	@IncidentActionId int,
	@CompanyId int,
	@EndDate datetime,
	@EndResponsible int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE IncidentAction SET
		ClosedOn = @EndDate,
		ClosedBy = @EndResponsible,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IncidentActionId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[IncidentAction_Delete]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentAction_Delete]
	@IncidentActionId bigint,
	@CompanyId int,
	@UserId int 
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE IncidentAction SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @IncidentActionId
	AND CompanyId = @CompanyId


END





GO

/****** Object:  StoredProcedure [dbo].[IncidentAction_GetByBusinessRiskCode]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[IncidentAction_GetByBusinessRiskCode]
	@BusinessRiskCode bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		IA.Id,
		IA.Description,
		IA.BusinessRiskId,
		IA.WhatHappendOn,
		IA.CausesOn,
		IA.ActionsOn,
		IA.ClosedOn
    FROM IncidentAction IA WITH(NOLOCK)
	inner join BusinessRisk3 BR WITH(NOLOCK)
	on BR.Id = IA.BusinessRiskId
	WHERE
		BR.Code = @BusinessRiskCode
	AND IA.CompanyId = @CompanyId
	--AND IA.Active = 1
    
END






GO

/****** Object:  StoredProcedure [dbo].[IncidentAction_GetByBusinessRiskId]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentAction_GetByBusinessRiskId]
	@BusinessRiskId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @code int;

	SELECT @Code = Code From BusinessRisk3 WHERE Id = @BusinessRiskId

    SELECT
		IA.Id,
		IA.CompanyId,
		IA.ActionType,
		IA.Origin,
		IA.Description,
		IA.DepartmentId,
		IA.ProviderId,
		IA.CustomerId,
		IA.ReporterType,
		IA.BusinessRiskId,
		BR.Code,
		IA.IncidentId,
		'',--I.Code,
		IA.Number,
		IA.WhatHappend,
		WH.Id,
		WH.Name,
		WH.LastName,
		IA.WhatHappendOn,
		IA.Causes,
		CAUSES.Id,
		CAUSES.Name,
		CAUSES.LastName,
		IA.CausesOn,
		IA.Actions,
		ACTIONS.Id,
		ACTIONS.Name,
		ACTIONS.LastName,
		IA.ActionsOn,
		EXECUTER.Id ExecuterId,
		EXECUTER.Name ExecuterName,
		EXECUTER.LastName ExecuterLastName,
		IA.ActionsSchedule ExecuterSchedule,
		IA.Monitoring,
		CLOSED.Id,
		CLOSED.Name,
		CLOSED.LastName,
		IA.ClosedOn,
		CLOSEDEXECUTOR.Id,
		CLOSEDEXECUTOR.Name,
		CLOSEDEXECUTOR.LastName,
		IA.ClosedExecutorOn,
		IA.Notes,
		IA.Active,
		IA.ModifiedBy,
		AU.[Login],
		IA.ModifiedOn
    FROM IncidentAction IA WITH(NOLOCK)
    LEFT JOIN IncidentActionType IAT WITH(NOLOCK)
    ON	IAT.Id = IA.ActionType
    LEFT JOIN Department D WITH(NOLOCK)
    ON	D.Id = IA.DepartmentId
    AND	D.Id = IA.CompanyId
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = IA.ProviderId
    AND P.CompanyId= IA.CompanyId
    LEFT JOIN Customer C WITH(NOLOCK)
    ON	C.Id = IA.CustomerId
    AND	C.CompanyId = IA.CompanyId
    LEFT JOIN BusinessRisk3 BR WITH(NOLOCK)
    ON	BR.Id = IA.BusinessRiskId
    AND BR.CompanyId = IA.CompanyId
    LEFT JOIN Employee WH WITH(NOLOCK)
    ON	WH.Id = IA.WhatHappendBy
    AND	WH.CompanyId = IA.CompanyId
    LEFT JOIN Employee CAUSES WITH(NOLOCK)
    ON	CAUSES.Id = IA.CausesBy
    AND CAUSES.CompanyId = IA.CompanyId
    LEFT JOIN Employee ACTIONS WITH(NOLOCK)
    ON	ACTIONS.Id = IA.ActionsBy
    AND ACTIONS.CompanyId = IA.CompanyId
    LEFT JOIN Employee EXECUTER WITH(NOLOCK)
    ON	EXECUTER.Id = IA.ActionsExecuter
    AND EXECUTER.CompanyId = IA.CompanyId
    LEFT JOIN Employee CLOSED WITH(NOLOCK)
    ON	CLOSED.Id = IA.ClosedBy
    AND CLOSED.CompanyId = IA.CompanyId
    LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = IA.ModifiedBy
	AND AU.CompanyId = IA.CompanyId
	LEFT JOIN Employee CLOSEDEXECUTOR WITH(NOLOCK)
	ON	CLOSEDEXECUTOR.Id = IA.ClosedExecutor
	WHERE
	--	IA.BusinessRiskId = @BusinessRiskId
		BR.Code = @Code
	AND IA.CompanyId = @CompanyId
	AND IA.Active = 1
    
END





GO

/****** Object:  StoredProcedure [dbo].[IncidentAction_GetByCode]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[IncidentAction_GetByCode]
	@BusinessRiskCode bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		IA.Id,
		IA.CompanyId,
		IA.ActionType,
		IA.Origin,
		IA.Description,
		0 AS DepartmentId, --no aplica
		0 AS ProviderId, -- no aplica
		0 AS CustomerId, -- no aplica
		IA.ReporterType,
		IA.BusinessRiskId,
		BR.Code,
		0 AS IncidentId, -- no aplica
		0 AS Code, -- no aplica
		IA.Number,
		IA.WhatHappend,
		WH.Id,
		WH.Name,
		WH.LastName,
		IA.WhatHappendOn,
		IA.Causes,
		CAUSES.Id,
		CAUSES.Name,
		CAUSES.LastName,
		IA.CausesOn,
		IA.Actions,
		ACTIONS.Id,
		ACTIONS.Name,
		ACTIONS.LastName,
		IA.ActionsOn,
		EXECUTER.Id ExecuterId,
		EXECUTER.Name ExecuterName,
		EXECUTER.LastName ExecuterLastName,
		IA.ActionsSchedule ExecuterSchedule,
		IA.Monitoring,
		CLOSED.Id,
		CLOSED.Name,
		CLOSED.LastName,
		IA.ClosedOn,
		CLOSEDEXECUTOR.Id,
		CLOSEDEXECUTOR.Name,
		CLOSEDEXECUTOR.LastName,
		IA.ClosedExecutorOn,
		IA.Notes,
		IA.Active,
		IA.ModifiedBy,
		AU.[Login],
		IA.ModifiedOn
    FROM IncidentAction IA WITH(NOLOCK)
    LEFT JOIN IncidentActionType IAT WITH(NOLOCK)
    ON	IAT.Id = IA.ActionType
    /*LEFT JOIN Department D WITH(NOLOCK)
    ON	D.Id = IA.DepartmentId
    AND	D.Id = IA.CompanyId
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = IA.ProviderId
    AND P.CompanyId= IA.CompanyId
    LEFT JOIN Customer C WITH(NOLOCK)
    ON	C.Id = IA.CustomerId
    AND	C.CompanyId = IA.CompanyId
    LEFT JOIN Incident I WITH(NOLOCK)
    ON	I.Id = IA.IncidentId
    AND I.CompanyId = IA.CompanyId*/
	LEFT JOIN BusinessRisk BR WITH(NOLOCK)
	ON	BR.Id = IA.BusinessRiskId
	AND BR.CompanyId = Ia.CompanyId
    LEFT JOIN Employee WH WITH(NOLOCK)
    ON	WH.Id = IA.WhatHappendBy
    AND	WH.CompanyId = IA.CompanyId
    LEFT JOIN Employee CAUSES WITH(NOLOCK)
    ON	CAUSES.Id = IA.CausesBy
    AND CAUSES.CompanyId = IA.CompanyId
    LEFT JOIN Employee ACTIONS WITH(NOLOCK)
    ON	ACTIONS.Id = IA.ActionsBy
    AND ACTIONS.CompanyId = IA.CompanyId
    LEFT JOIN Employee EXECUTER WITH(NOLOCK)
    ON	EXECUTER.Id = IA.ActionsExecuter
    AND EXECUTER.CompanyId = IA.CompanyId
    LEFT JOIN Employee CLOSED WITH(NOLOCK)
    ON	CLOSED.Id = IA.ClosedBy
    AND CLOSED.CompanyId = IA.CompanyId    
    LEFT JOIN Employee CLOSEDEXECUTOR WITH(NOLOCK)
    ON	CLOSEDEXECUTOR.Id = IA.ClosedBy
    AND CLOSEDEXECUTOR.CompanyId = IA.CompanyId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = IA.ModifiedBy
	WHERE
		BR.Code = @BusinessRiskCode
	AND IA.CompanyId = @CompanyId
	AND IA.Active = 1
    
END






GO

/****** Object:  StoredProcedure [dbo].[IncidentAction_GetById]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentAction_GetById]
	@IncidentActionId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		IA.Id,
		IA.CompanyId,
		IA.ActionType,
		IA.Origin,
		IA.Description,
		IA.DepartmentId,
		IA.ProviderId,
		IA.CustomerId,
		IA.ReporterType,
		IA.BusinessRiskId,
		ISNULL(BR.Code,''),
		IA.IncidentId,
		ISNULL(I.Code,''),
		IA.Number,
		IA.WhatHappend,
		IA.WhatHappendBy AS WhatHappendResponsibleId,
		ISNULL(WH.Name,'') AS WhatHappendResponsibleName,
		ISNULL(WH.LastName,'') AS WhatHappendResponsibleLastName,
		IA.WhatHappendOn,
		IA.Causes,
		CAUSES.Id,
		ISNULL(CAUSES.Name,''),
		ISNULL(CAUSES.LastName,''),
		ISNULL(IA.CausesOn, GETDATE()-36000),
		IA.Actions,
		ACTIONS.Id AS ActionsResponsibleId,
		ISNULL(ACTIONS.Name,'') AS ActionsResponsibleName,
		ISNULL(ACTIONS.LastName,'') AS ActionsResponsibleLastName,
		ISNULL(IA.ActionsOn, GETDATE()-36000),
		EXECUTER.Id ExecuterId,
		ISNULL(EXECUTER.Name,'') ExecuterName,
		ISNULL(EXECUTER.LastName,'') ExecuterLastName,
		IA.ActionsSchedule ExecuterSchedule,
		IA.Monitoring,
		CLOSED.Id AS ClosedById,
		ISNULL(CLOSED.Name,'') AS ClosedByName,
		ISNULL(CLOSED.LastName,'') AS ClosedByLastName,
		IA.ClosedOn,
		CLOSEDEXECUTOR.Id,
		CLOSEDEXECUTOR.Name,
		CLOSEDEXECUTOR.LastName,
		IA.ClosedExecutorOn,
		IA.Notes,
		IA.Active,
		IA.ModifiedBy,
		AU.[Login],
		IA.ModifiedOn
    FROM IncidentAction IA WITH(NOLOCK)
    LEFT JOIN IncidentActionType IAT WITH(NOLOCK)
    ON	IAT.Id = IA.ActionType
    LEFT JOIN Department D WITH(NOLOCK)
    ON	D.Id = IA.DepartmentId
    AND	D.Id = IA.CompanyId
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = IA.ProviderId
    AND P.CompanyId= IA.CompanyId
    LEFT JOIN Customer C WITH(NOLOCK)
    ON	C.Id = IA.CustomerId
    AND	C.CompanyId = IA.CompanyId
    LEFT JOIN Incident I WITH(NOLOCK)
    ON	I.Id = IA.IncidentId
    AND I.CompanyId = IA.CompanyId
	LEFT JOIN BusinessRisk3 BR WITH(NOLOCK)
	ON	BR.Id = IA.BusinessRiskId
	AND I.CompanyId = Ia.CompanyId
    LEFT JOIN Employee WH WITH(NOLOCK)
    ON	WH.Id = IA.WhatHappendBy
    AND	WH.CompanyId = IA.CompanyId
    LEFT JOIN Employee CAUSES WITH(NOLOCK)
    ON	CAUSES.Id = IA.CausesBy
    AND CAUSES.CompanyId = IA.CompanyId
    LEFT JOIN Employee ACTIONS WITH(NOLOCK)
    ON	ACTIONS.Id = IA.ActionsBy
    AND ACTIONS.CompanyId = IA.CompanyId
    LEFT JOIN Employee EXECUTER WITH(NOLOCK)
    ON	EXECUTER.Id = IA.ActionsExecuter
    AND EXECUTER.CompanyId = IA.CompanyId
    LEFT JOIN Employee CLOSED WITH(NOLOCK)
    ON	CLOSED.Id = IA.ClosedBy
    AND CLOSED.CompanyId = IA.CompanyId    
    LEFT JOIN Employee CLOSEDEXECUTOR WITH(NOLOCK)
    ON	CLOSEDEXECUTOR.Id = IA.ClosedBy
    AND CLOSEDEXECUTOR.CompanyId = IA.CompanyId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = IA.ModifiedBy
	WHERE
		IA.Id = @IncidentActionId
	AND IA.CompanyId = @CompanyId
	AND IA.Active = 1
    
END





GO

/****** Object:  StoredProcedure [dbo].[IncidentAction_GetByIncidentId]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentAction_GetByIncidentId]
	@IncidentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		IA.Id,
		IA.CompanyId,
		IA.ActionType,
		IA.Origin,
		IA.Description,
		IA.DepartmentId,
		IA.ProviderId,
		IA.CustomerId,
		IA.ReporterType,
		IA.BusinessRiskId,
		0,--BR.Code,
		IA.IncidentId,
		I.Code,
		IA.Number,
		IA.WhatHappend,
		WH.Id AS WhatHappendById,
		WH.Name AS WhatHappendByName,
		WH.LastName AS WhatHappendByLastName,
		IA.WhatHappendOn,
		IA.Causes,
		CAUSES.Id AS CausesById,
		CAUSES.Name AS CausesByName,
		CAUSES.LastName AS CausesByLastName,
		IA.CausesOn,
		IA.Actions,
		ACTIONS.Id AS ActionsById,
		ACTIONS.Name AS ActionsByName,
		ACTIONS.LastName AS ActionsByLastName,
		IA.ActionsOn,
		EXECUTER.Id ExecuterId,
		EXECUTER.Name ExecuterName,
		EXECUTER.LastName ExecuterLastName,
		IA.ActionsSchedule ExecuterSchedule,
		IA.Monitoring,
		CLOSED.Id AS ClosedById,
		CLOSED.Name AS ClosedByIdName,
		CLOSED.LastName AS ClosedByIdLastName,
		IA.ClosedOn,
		CLOSEDEXECUTOR.Id AS ClosedExecutorId,
		CLOSEDEXECUTOR.Name AS ClosedExecutorName,
		CLOSEDEXECUTOR.LastName AS ClosedExecutorLastName,
		IA.ClosedExecutorOn,
		IA.Notes,
		IA.Active,
		IA.ModifiedBy AS ModifiedByUserId,
		AU.Login AS ModifiedByUserName,
		IA.ModifiedOn,
		IA.ReporterType
    FROM IncidentAction IA WITH(NOLOCK)
    LEFT JOIN IncidentActionType IAT WITH(NOLOCK)
    ON	IAT.Id = IA.ActionType
    LEFT JOIN Department D WITH(NOLOCK)
    ON	D.Id = IA.DepartmentId
    AND	D.Id = IA.CompanyId
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = IA.ProviderId
    AND P.CompanyId= IA.CompanyId
    LEFT JOIN Customer C WITH(NOLOCK)
    ON	C.Id = IA.CustomerId
    AND	C.CompanyId = IA.CompanyId
    LEFT JOIN Incident I WITH(NOLOCK)
    ON	I.Id = IA.IncidentId
    AND I.CompanyId = IA.CompanyId
    LEFT JOIN Employee WH WITH(NOLOCK)
    ON	WH.Id = IA.WhatHappendBy
    AND	WH.CompanyId = IA.CompanyId
    LEFT JOIN Employee CAUSES WITH(NOLOCK)
    ON	CAUSES.Id = IA.CausesBy
    AND CAUSES.CompanyId = IA.CompanyId
    LEFT JOIN Employee ACTIONS WITH(NOLOCK)
    ON	ACTIONS.Id = IA.ActionsBy
    AND ACTIONS.CompanyId = IA.CompanyId
    LEFT JOIN Employee EXECUTER WITH(NOLOCK)
    ON	EXECUTER.Id = IA.ActionsExecuter
    AND EXECUTER.CompanyId = IA.CompanyId
    LEFT JOIN Employee CLOSED WITH(NOLOCK)
    ON	CLOSED.Id = IA.ClosedBy
    AND CLOSED.CompanyId = IA.CompanyId
    LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = IA.ModifiedBy
	AND AU.CompanyId = IA.CompanyId
	LEFT JOIN Employee CLOSEDEXECUTOR WITH(NOLOCK)
	ON	CLOSEDEXECUTOR.Id = IA.ClosedExecutor
	WHERE
		IA.IncidentId = @IncidentId
	AND IA.CompanyId = @CompanyId
	AND IA.Active = 1
    
END





GO

/****** Object:  StoredProcedure [dbo].[IncidentAction_Insert]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentAction_Insert]
	@IncidentActionId bigint output,
	@CompanyId int,
	@ActionType int,
	@Description nvarchar(100),
	@Origin int,
	@ReporterType int,
	@DeparmentId int,
	@ProviderId int,
	@CustomerId int,
	@Number bigint,
	@IncidentId bigint,
	@WhatHappend nvarchar(2000),
	@WhatHappendBy bigint,
	@WhatHappendDate datetime,
	@Causes nvarchar(2000),
	@CausesBy bigint,
	@CausesDate datetime,
	@Actions nvarchar(2000),
	@ActionsBy bigint,
	@ActionsExecuter int,
	@ActionsSchedule datetime,
	@ActionsDate datetime,
	@Monitoring nvarchar(2000),
	@ClosedBy bigint,
	@ClosedDate datetime,
	@ClosedExecutor bigint,
	@ClosedExecutorOn datetime,
	@Notes nvarchar(2000),
	@UserId int,
	@BusinessRiskId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT @Number = ISNULL(MAX(Number) ,0) + 1
	FROM IncidentAction I WITH(NOLOCK)
	WHERE
		I.CompanyId = @CompanyId
	AND I.Active = 1

    INSERT INTO IncidentAction
    (
		CompanyId,
		ActionType,
		Description,
		Origin,
		ReporterType,
		DepartmentId,
		ProviderId,
		CustomerId,
		Number,
		IncidentId,
		WhatHappend,
		WhatHappendBy,
		WhatHappendOn,
		Causes,
		CausesBy,
		CausesOn,
		Actions,
		ActionsBy,
		ActionsOn,
		ActionsExecuter,
		ActionsSchedule,
		Monitoring,
		ClosedBy,
		ClosedOn,
		ClosedExecutor,
		ClosedExecutorOn,
		Notes,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn,
		BusinessRiskId
	)
    VALUES
    (
		@CompanyId,
		@ActionType,
		@Description,
		@Origin,
		@ReporterType,
		@DeparmentId,
		@ProviderId,
		@CustomerId,
		@Number,
		@IncidentId,
		@WhatHappend,
		@WhatHappendBy,
		@WhatHappendDate,
		@Causes,
		@CausesBy,
		@CausesDate,
		@Actions,
		@ActionsBy,
		@ActionsDate,
		@ActionsExecuter,
		@ActionsSchedule,
		@Monitoring,
		@ClosedBy,
		@ClosedDate,
		@ClosedExecutor,
		@ClosedExecutorOn,
		@Notes,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE(),
		@BusinessRiskId		
	)

	SET @IncidentActionId = @@IDENTITY

END





GO

/****** Object:  StoredProcedure [dbo].[IncidentAction_Restore]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IncidentAction_Restore]
	@IncidentActionId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE IncidentAction SET
		ClosedOn = NULL,
		ClosedBy = NULL,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IncidentActionId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[IncidentAction_Update]    Script Date: 24/10/2018 20:20:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentAction_Update]
	@IncidentActionId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@ActionType int,
	@Origin int,
	@Number bigint,
	@ReporterType int,
	@DepartmentId int,
	@ProviderId bigint,
	@CustomerId bigint,
	@IncidentId bigint,
	@WhatHappend nvarchar(2000),
	@WhatHappendBy bigint,
	@WhatHappendOn datetime,
	@Causes nvarchar(2000),
	@CausesBy bigint,
	@CausesOn datetime,
	@Actions nvarchar(2000),
	@ActionsBy bigint,
	@ActionsOn datetime,
	@ActionsExecuter int,
	@ActionsSchedule datetime,
	@Monitoring nvarchar(2000),
	@ClosedBy bigint,
	@ClosedOn datetime,
    @ClosedExecutor bigint,
    @ClosedExecutorOn datetime,
	@Notes nvarchar(2000),
	@UserId int,
	@BusinessRiskId bigint
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE IncidentAction SET
		ActionType = @ActionType,
		Description = @Description,
		Origin = @Origin,
		ReporterType = @ReporterType,
		DepartmentId = @DepartmentId,
		ProviderId = @ProviderId,
		CustomerId = @CustomerId,
		Number = @Number,
		IncidentId = @IncidentId,
		WhatHappend = @WhatHappend,
		WhatHappendBy = @WhatHappendBy,
		WhatHappendOn = @WhatHappendOn,
		Causes = @Causes,
		CausesBy = @CausesBy,
		CausesOn = @CausesOn,
		Actions = @Actions,
		ActionsBy = @ActionsBy,
		ActionsOn = @ActionsOn,
		ActionsExecuter = @ActionsExecuter,
		ActionsSchedule = @ActionsSchedule,
		Monitoring = @Monitoring,
		--ClosedBy = @ClosedBy,
		--ClosedOn = @ClosedOn,
		--ClosedExecutor = @ClosedExecutor,
		--ClosedExecutorOn = @ClosedExecutorOn,
		Notes = @Notes,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE(),
		BusinessRiskId = @BusinessRiskId
	WHERE 
		Id = @IncidentActionId
	AND CompanyId = @CompanyId


END





GO

/****** Object:  StoredProcedure [dbo].[IncidentActionCost_Delete]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentActionCost_Delete]
	@IncidentActionCostId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE IncidentActionCost SET	
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentActionCostId
	AND CompanyId = @CompanyId


END





GO

/****** Object:  StoredProcedure [dbo].[IncidentActionCost_Insert]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentActionCost_Insert]
	@IncidentActionCostId bigint output,
	@IncidentActionId bigint,
	@CompanyId int,
	@Description nvarchar(50),
	@Amount numeric(18,3),
	@Quantity numeric(18,3),
	@ResponsableId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO IncidentActionCost
	(
		IncidentActionId,
		CompanyId,
		Description,
		Amount,
		Quantity,
		Responsable,
		Active,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@IncidentActionId,
		@CompanyId,
		@Description,
		@Amount,
		@Quantity,
		@ResponsableId,
		1,
		@UserId,
		GETDATE()
	)
	
	SET @IncidentActionCostId = @@IDENTITY


END





GO

/****** Object:  StoredProcedure [dbo].[IncidentActionCost_Update]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentActionCost_Update]
	@IncidentActionCostId bigint,
	@IncidentActionId bigint,
	@CompanyId int,
	@Description nvarchar(50),
	@Amount numeric(18,3),
	@Quantity numeric(18,3),
	@ResponsableId int,
	@UserId int,
	@Differences text
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE IncidentActionCost SET	
		Description = @Description,
		Amount = @Amount,
		Quantity = @Quantity,
		Responsable = @ResponsableId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentActionCostId
	AND IncidentActionId = @IncidentActionId
	AND CompanyId = @CompanyId


END





GO

/****** Object:  StoredProcedure [dbo].[IncidentActions_Filter]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentActions_Filter]
	@CompanyId int,
	@DateFrom datetime,
	@DateTo datetime,
	@StatusIdentified bit,
	@StatusAnalyzed bit,
	@StatusInProcess bit,
	@StatusClosed bit,
	@TypeImpovement bit,
	@TypeFix bit,
	@TypePrevent bit,
	@Origin int,
	@Reporter int
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		I.Id AS IncidentActionId,
		I.ActionType,
		I.Origin,
		I.[Description],
		I.WhatHappendOn AS OpenDate,
		I.CausesOn AS CausesDate,
		I.ActionsOn AS ImplementationDate,
		I.ClosedOn AS CloseDate,
		I.Number,
		I.ReporterType,
		ISNULL(ISNULL(INC.Id, BR.Id), 0) AS IncidentId,
		ISNULL(ISNULL(INC.[Description], BR.[Description]),'') AS IncidentCode,
		SUM(ISNULL(IAC.Amount * IAC.Quantity,0)) AS Amount
	FROM IncidentAction I WITH(NOLOCK)
	LEFT JOIN Incident INC WITH(NOLOCK)
	ON	INC.Id = I.IncidentId
	AND INC.CompanyId = I.CompanyId
	AND INC.Id > 0
	LEFT JOIN BusinessRisk3 BR WITH(NOLOCK)
	ON	BR.Id = I.BusinessRiskId
	AND BR.CompanyId= I.CompanyId
	LEFT JOIN IncidentActionCost IAC WITH(NOLOCK)
	ON	IAC.IncidentActionId = I.Id
	AND	IAC.CompanyId = I.CompanyId
	AND IAC.Active = 1
	WHERE
		I.CompanyId = @CompanyId
	AND I.Active = 1
	AND	(@DateFrom IS NULL OR I.WhatHappendOn >= @DateFrom)
	AND (@DateTo IS NULL OR I.WhatHappendOn <= @DateTo)
	AND
	(
		(@TypeImpovement = 1 AND I.ActionType=1)
		OR
		(@TypeFix = 1 AND I.ActionType=2)
		OR
		(@TypePrevent = 1 AND I.ActionType=3)
	)
	AND
	(
		(@StatusIdentified = 1 AND I.WhatHappendOn IS NOT NULL AND I.CausesOn IS NULL)
		OR
		(@StatusAnalyzed = 1 AND I.CausesOn IS NOT NULL AND I.ActionsOn IS NULL)
		OR
		(@StatusInProcess = 1 AND I.ActionsOn IS NOT NULL AND I.ClosedOn IS NULL)
		OR
		(@StatusClosed = 1 AND I.ClosedOn IS NOT NULL)
	)
	AND (@Origin = I.Origin OR @Origin=-1)
	AND (@Reporter IS NULL OR @Reporter = 0 OR @Reporter = I.ReporterType)

	GROUP BY
		I.Id,
		I.ActionType,
		I.Origin,
		I.[Description],
		I.WhatHappendOn,
		I.CreatedOn,
		I.CausesOn,
		I.ActionsOn,
		I.ClosedOn,
		I.Number,
		I.ReporterType,
		INC.Id,
		INC.[Description],
		BR.Id,
		BR.[Description]
	
END





GO

/****** Object:  StoredProcedure [dbo].[IncidentCost_Delete]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentCost_Delete]
	@IncidentCostId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE IncidentCost SET	
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentCostId
	AND CompanyId = @CompanyId


END





GO

/****** Object:  StoredProcedure [dbo].[IncidentCost_Insert]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentCost_Insert]
	@IncidentCostId bigint output,
	@IncidentId bigint,
	@BusinessRiskId bigint,
	@CompanyId int,
	@Description nvarchar(50),
	@Amount numeric(18,3),
	@Quantity numeric(18,3),
	@ResponsableId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO IncidentCost
	(
		IncidentId,
		BusinessRiskId,
		CompanyId,
		Description,
		Amount,
		Quantity,
		Responsable,
		Active,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@IncidentId,
		@BusinessRiskId,
		@CompanyId,
		@Description,
		@Amount,
		@Quantity,
		@ResponsableId,
		1,
		@UserId,
		GETDATE()
	)
	
	SET @IncidentCostId = @@IDENTITY


END





GO

/****** Object:  StoredProcedure [dbo].[IncidentCost_Update]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IncidentCost_Update]
	@IncidentCostId bigint,
	@IncidentId bigint,
	@BusinessRiskId bigint,
	@CompanyId int,
	@Description nvarchar(50),
	@Amount numeric(18,3),
	@Quantity numeric(18,3),
	@ResponsableId int,
	@UserId int,
	@Differences text
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE IncidentCost SET	
		Description = @Description,
		Amount = @Amount,
		Quantity = @Quantity,
		Responsable = @ResponsableId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentCostId
	AND IncidentId = @IncidentId
	AND BusinessRiskId = @BusinessRiskId
	AND CompanyId = @CompanyId


END





GO

/****** Object:  StoredProcedure [dbo].[IndecidentActionCost_GetByCompanyId]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IndecidentActionCost_GetByCompanyId]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		IC.Id,
		IC.CompanyId,
		IC.IncidentActionId,
		IC.Description,
		IC.Amount,
		IC.Quantity,
		EMP.Id AS ResponsableId,
		EMP.Name AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		IC.Active
	FROM IncidentActionCost IC WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = IC.Responsable
	
	WHERE
		IC.CompanyId = @CompanyId
	AND IC.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[IndecidentActionCost_GetByIndicentActionId]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IndecidentActionCost_GetByIndicentActionId]
	@IncidentActionId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		IC.Id,
		IC.CompanyId,
		IC.IncidentActionId,
		IC.Description,
		IC.Amount,
		IC.Quantity,
		IC.Id AS ResponsableId,
		EMP.Name AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		IC.Active
	FROM IncidentActionCost IC WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = IC.Responsable
	
	WHERE
		IC.IncidentActionId = @IncidentActionId
	AND IC.CompanyId = @CompanyId
	AND IC.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[IndecidentCost_GetByBusinessRiskId]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IndecidentCost_GetByBusinessRiskId]
	@BusinessRiskId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @code int;
	SELECT @Code = Code From BusinessRisk3 WHERE Id = @BusinessRiskId
	
	SELECT
		IC.Id,
		IC.CompanyId,
		IC.IncidentActionId,
		IC.Description,
		IC.Amount,
		IC.Quantity,
		EMP.Id AS ResponsableId,
		EMP.Name AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		IC.Active
	FROM IncidentActionCost IC WITH(NOLOCK)
	INNER JOIN IncidentAction IA WITH(NOLOCK)
	ON	IA.BusinessRiskId = @BusinessRiskId
	ANd IC.IncidentActionId = IA.Id
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = IC.Responsable
	
	WHERE
		IC.CompanyId = @CompanyId
	AND IC.Active = 1
	/*SELECT
		IC.Id,
		IC.CompanyId,
		IC.IncidentId,
		--IC.BusinessRiskId,
		IC.Description,
		IC.Amount,
		IC.Quantity,
		EMP.Id AS ResponsableId,
		EMP.Name AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		IC.Active
	FROM IncidentCost IC WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = IC.Responsable
	INNER JOIN BusinessRisk3 BR
	ON	IC.BusinessRiskId = BR.Id
	
	WHERE
	--	IC.BusinessRiskId = @BusinessRiskId
		BR.Code = @code
	AND IC.CompanyId = @CompanyId
	AND IC.Active = 1*/
END





GO

/****** Object:  StoredProcedure [dbo].[IndecidentCost_GetByCompanyId]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IndecidentCost_GetByCompanyId]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		IC.Id,
		IC.CompanyId,
		IC.IncidentId,
		--IC.BusinessRiskId,
		IC.Description,
		IC.Amount,
		IC.Quantity,
		EMP.Id AS ResponsableId,
		EMP.Name AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		IC.Active
	FROM IncidentCost IC WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = IC.Responsable
	
	WHERE
		IC.CompanyId = @CompanyId
	AND IC.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[IndecidentCost_GetByIndicentId]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IndecidentCost_GetByIndicentId]
	@IncidentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		IC.Id,
		IC.CompanyId,
		IC.IncidentId,
		--IC.BusinessRiskId,
		IC.Description,
		IC.Amount,
		IC.Quantity,
		EMP.Id AS ResponsableId,
		EMP.Name AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		IC.Active
	FROM IncidentCost IC WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = IC.Responsable
	
	WHERE
		IC.IncidentId = @IncidentId
	AND IC.CompanyId = @CompanyId
	AND IC.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[Indicador_Activate]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_Activate]
	@IndicadorId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Indicador SET
		Active = 1,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IndicadorId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Indicador_Anulate]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_Anulate]
	@IndicadorId int,
	@CompanyId int,
	@EndDate datetime,
	@EndReason nvarchar(500),
	@EndResponsible int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Indicador SET
		EndDate = @EndDate,
		EndReason = @EndReason,
		EndResponsible = @EndResponsible,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IndicadorId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Indicador_GetActive]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_GetActive]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		I.Id,
		I.CompanyId,
		I.Descripcion,
		I.ProcessId,
		P.[Description],
		0,--I.ObjetivoId,
		'',--O.[Description],
		I.MetaComparer,
		I.Meta,
		I.AlarmaComparer,
		I.Alarma,
		ISNULL(I.Calculo,'') AS Calculo,
		I.EndDate,
		ISNULL(I.EndReason,'') AS EndReason,
		I.EndResponsible,
		I.UnidadId,
		ISNULL(U.[Description],'') AS UnidadDescripcion,
		I.Periodicity,
		I.CreatedBy,
		CB.[Login] AS CreatedByName,
		I.CreatedOn,
		I.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		I.ModifiedOn,
		I.Active,
		I.ResponsableId,
		EMPRES.Id,
		EMPRES.Name,
		EMPRES.LastName,
		USERRES.[Login],
		I.EndResponsible,
		DELRES.Id,
		DELRES.Name,
		DELRES.LastName,
		USERDEL.[Login],
		I.Startdate
	FROM Indicador I WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = I.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = I.ModifiedBy

	INNER JOIN Employee EMPRES WITH(NOLOCK)
	ON	EMPRES.Id = I.ResponsableId
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser USERRES WITH(NOLOCK)
		ON	USERRES.Id = EUA.UserId
		AND USERRES.[Status] = 1
	ON	EUA.EmployeeId = I.ResponsableId
	
	LEFT JOIN ApplicationUser USERDEL WITH(NOLOCK)
	ON	USERDEL.Id = I.EndResponsible
	LEFT JOIN EmployeeUserAsignation EUA2 WITH(NOLOCK)
		LEFT JOIN Employee DELRES WITH(NOLOCK)
		ON	DELRES.Id = EUA2.EmployeeId
	ON	EUA.UserId = I.EndResponsible
	AND	EMPRES.CompanyId = I.CompanyId
	
	LEFT JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = I.ProcessId
	LEFT JOIN Unidad U WITH(NOLOCK)
	ON	U.Id = I.UnidadId

	WHERE
		I.CompanyId = @CompanyId
	AND I.Active = 1

END



GO

/****** Object:  StoredProcedure [dbo].[Indicador_GetAll]    Script Date: 24/10/2018 20:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_GetAll]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		I.Id,
		I.CompanyId,
		I.Descripcion,
		CASE WHEN I.ProcessId < 1 THEN NULL ELSE I.ProcessId END AS ProcessId,
		P.[Description],
		0,--I.ObjetivoId,
		'',--O.[Description],
		I.MetaComparer,
		I.Meta,
		I.AlarmaComparer,
		I.Alarma,
		ISNULL(I.Calculo,'') AS Calculo,
		I.EndDate,
		ISNULL(I.EndReason,'') AS EndReason,
		CASE WHEN I.EndResponsible  <0 THEN NULL ELSE I.EndResponsible END AS EndResponsible,
		I.UnidadId,
		ISNULL(U.[Description],'') AS UnidadDescripcion,
		I.Periodicity,
		I.CreatedBy,
		CB.[Login] AS CreatedByName,
		I.CreatedOn,
		I.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		I.ModifiedOn,
		I.Active,
		I.ResponsableId,
		EMPRES.Id,
		EMPRES.Name,
		EMPRES.LastName,
		USERRES.[Login],
		CASE WHEN I.EndResponsible  <0 THEN NULL ELSE I.EndResponsible END AS EndResponsible,
		EMPDEL.Id,
		EMPDEL.Name,
		EMPDEL.LastName,
		DELRES.[Login],
		I.Startdate
	FROM Indicador I WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = I.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = I.ModifiedBy

	INNER JOIN Employee EMPRES WITH(NOLOCK)
	ON	EMPRES.Id = I.ResponsableId
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser USERRES WITH(NOLOCK)
		ON	USERRES.Id = EUA.UserId
		AND USERRES.[Status] = 1
	ON	EUA.EmployeeId = I.ResponsableId
	
	LEFT JOIN Employee EMPDEL WITH(NOLOCK)
	ON	EMPDEL.Id = I.EndResponsible
	LEFT JOIN EmployeeUserAsignation EUA2 WITH(NOLOCK)
		LEFT JOIN ApplicationUser DELRES WITH(NOLOCK)
		ON	DELRES.Id = EUA2.UserId
	ON	EUA2.EmployeeId = I.EndResponsible

	LEFT JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = I.ProcessId
	--LEFT JOIN Objetivo O WITH(NOLOCK)
	--ON	O.Id = I.ObjetivoId
	LEFT JOIN Unidad U WITH(NOLOCK)
	ON	U.Id = I.UnidadId

	WHERE
		I.CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Indicador_GetAllPeriodicity]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_GetAllPeriodicity]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		I.Id,
		I.Periodicity
	FROM Indicador I WITH(NOLOCK)
	WHERE
		I.CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Indicador_GetAvailablesForObjetivo]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_GetAvailablesForObjetivo]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		I.Id,
		I.CompanyId,
		I.Descripcion,
		I.ProcessId,
		P.[Description],
		0,--I.ObjetivoId,
		'',--ISNULL(O.[Description],'') AS ObjetivoDescription,
		I.MetaComparer,
		I.Meta,
		I.AlarmaComparer,
		I.Alarma,
		ISNULL(I.Calculo,'') AS Calculo,
		I.EndDate,
		ISNULL(I.EndReason,'') AS EndReason,
		I.EndResponsible,
		I.UnidadId,
		ISNULL(U.[Description],'') AS UnidadDescripcion,
		I.Periodicity,
		I.CreatedBy,
		CB.[Login] AS CreatedByName,
		I.CreatedOn,
		I.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		I.ModifiedOn,
		I.Active,
		I.ResponsableId,
		EMPRES.Id,
		EMPRES.Name,
		EMPRES.LastName,
		USERRES.[Login],
		I.EndResponsible,
		DELRES.Id,
		DELRES.Name,
		DELRES.LastName,
		USERDEL.[Login],
		I.StartDate
	FROM Indicador I WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = I.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = I.ModifiedBy

	INNER JOIN Employee EMPRES WITH(NOLOCK)
	ON	EMPRES.Id = I.ResponsableId
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser USERRES WITH(NOLOCK)
		ON	USERRES.Id = EUA.UserId
		AND USERRES.[Status] = 1
	ON	EUA.EmployeeId = I.ResponsableId
	
	LEFT JOIN ApplicationUser USERDEL WITH(NOLOCK)
	ON	USERDEL.Id = I.EndResponsible
	LEFT JOIN EmployeeUserAsignation EUA2 WITH(NOLOCK)
		LEFT JOIN Employee DELRES WITH(NOLOCK)
		ON	DELRES.Id = EUA2.EmployeeId
	ON	EUA.UserId = I.EndResponsible
	AND	EMPRES.CompanyId = I.CompanyId
	
	LEFT JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = I.ProcessId
	--LEFT JOIN Objetivo O WITH(NOLOCK)
	--ON	O.Id = I.ObjetivoId
	LEFT JOIN Unidad U WITH(NOLOCK)
	ON	U.Id = I.UnidadId

	WHERE
		I.CompanyId = @CompanyId
	-- AND (I.ProcessId IS NULL OR I.ProcessId = 0)
	AND I.Active = 1

END



GO

/****** Object:  StoredProcedure [dbo].[Indicador_GetById]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_GetById]
	@IndicadorId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		I.Id,
		I.CompanyId,
		I.Descripcion,
		I.ProcessId,
		P.[Description],
		0,--I.ObjetivoId,
		'',--ISNULL(O.[Description],'') AS ObjetivoDescription,
		I.MetaComparer,
		I.Meta,
		I.AlarmaComparer,
		I.Alarma,
		ISNULL(I.Calculo,'') AS Calculo,
		I.EndDate,
		ISNULL(I.EndReason,'') AS EndReason,
		I.EndResponsible,
		I.UnidadId,
		ISNULL(U.[Description],'') AS UnidadDescripcion,
		I.Periodicity,
		I.CreatedBy,
		CB.[Login] AS CreatedByName,
		I.CreatedOn,
		I.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		I.ModifiedOn,
		I.Active,
		I.ResponsableId,
		EMPRES.Id,
		ISNULL(EMPRES.Name,''),
		ISNULL(EMPRES.LastName,''),
		ISNULL(USERRES.[Login],'no user'),
		I.EndResponsible,
		DELRES.Id,
		DELRES.Name,
		DELRES.LastName,
		ISNULL(USERDEL.[Login],'') AS Login,
		I.StartDate
	FROM Indicador I WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = I.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = I.ModifiedBy

	INNER JOIN Employee EMPRES WITH(NOLOCK)
	ON	EMPRES.Id = I.ResponsableId
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser USERRES WITH(NOLOCK)
		ON	USERRES.Id = EUA.UserId
		AND USERRES.[Status] = 1
	ON	EUA.EmployeeId = I.ResponsableId

	LEFT JOIN Employee DELRES WITH(NOLOCK)
	ON	DELRES.Id = I.EndResponsible
	LEFT JOIN EmployeeUserAsignation EUADEL WITH(NOLOCK)
		INNER JOIN ApplicationUser USERDEL WITH(NOLOCK)
		ON	USERDEL.Id = EUADEL.UserId
		AND USERDEL.[Status] = 1
	ON	EUA.EmployeeId = I.EndResponsible
	
	/*LEFT JOIN ApplicationUser USERDEL WITH(NOLOCK)
	ON	USERDEL.Id = I.EndResponsible
	LEFT JOIN EmployeeUserAsignation EUA2 WITH(NOLOCK)
		LEFT JOIN Employee DELRES WITH(NOLOCK)
		ON	DELRES.Id = EUA2.EmployeeId
	ON	EUA.UserId = I.EndResponsible
	AND	EMPRES.CompanyId = I.CompanyId*/
	
	LEFT JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = I.ProcessId
	--LEFT JOIN Objetivo O WITH(NOLOCK)
	--ON	O.Id = I.ObjetivoId
	LEFT JOIN Unidad U WITH(NOLOCK)
	ON	U.Id = I.UnidadId

	WHERE
		I.CompanyId = @CompanyId
	AND I.Id = @IndicadorId

END



GO

/****** Object:  StoredProcedure [dbo].[Indicador_GetByObjetivoId]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_GetByObjetivoId]
	@ObjetivoId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		I.Id,
		I.CompanyId,
		I.Descripcion,
		I.ProcessId,
		P.[Description],
		-1,
		'',
		I.MetaComparer,
		I.Meta,
		I.AlarmaComparer,
		I.Alarma,
		ISNULL(I.Calculo,'') AS Calculo,
		NULL,
		'' AS EndReason,
		-1,
		I.UnidadId,
		ISNULL(U.[Description],'') AS UnidadDescripcion,
		I.Periodicity,
		I.CreatedBy,
		CB.[Login] AS CreatedByName,
		I.CreatedOn,
		I.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		I.ModifiedOn,
		I.Active,
		I.ResponsableId,
		EMPRES.Id AS ResponsableEmpoyeeId,
		EMPRES.Name AS ResponsableEmpoyeeId,
		EMPRES.LastName AS ResponsableEmpoyeeId,
		USERRES.[Login],
		-1,
		-1,
		'',
		'',
		'',
		I.StartDate
	FROM Indicador I WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = I.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = I.ModifiedBy

	INNER JOIN Employee EMPRES WITH(NOLOCK)
	ON	EMPRES.Id = I.ResponsableId
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser USERRES WITH(NOLOCK)
		ON	USERRES.Id = EUA.UserId
		ANd	USERRES.[Status] = 1
	ON	EUA.EmployeeId = I.ResponsableId
	
	INNER JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = I.ProcessId
	INNER JOIN Unidad U WITH(NOLOCK)
	ON	U.Id = I.UnidadId

	WHERE
		I.CompanyId = @CompanyId
	--D I.ObjetivoId = @ObjetivoId
	AND I.EndDate IS NULL
	AND I.Active = 1

END


GO

/****** Object:  StoredProcedure [dbo].[Indicador_GetByProcessId]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_GetByProcessId]
	@ProcessId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		I.Id,
		I.CompanyId,
		I.Descripcion,
		I.ProcessId,
		P.[Description],
		-1,
		'',
		I.MetaComparer,
		I.Meta,
		I.AlarmaComparer,
		I.Alarma,
		ISNULL(I.Calculo,'') AS Calculo,
		NULL,
		'' AS EndReason,
		-1,
		I.UnidadId,
		ISNULL(U.[Description],'') AS UnidadDescripcion,
		I.Periodicity,
		I.CreatedBy,
		CB.[Login] AS CreatedByName,
		I.CreatedOn,
		I.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		I.ModifiedOn,
		I.Active,
		I.ResponsableId,
		EMPRES.Id AS ResponsableEmpoyeeId,
		EMPRES.Name AS ResponsableEmpoyeeId,
		EMPRES.LastName AS ResponsableEmpoyeeId,
		USERRES.[Login],
		-1,
		-1,
		'',
		'',
		'',
		I.StartDate
	FROM Indicador I WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = I.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = I.ModifiedBy

	INNER JOIN Employee EMPRES WITH(NOLOCK)
	ON	EMPRES.Id = I.ResponsableId
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser USERRES WITH(NOLOCK)
		ON	USERRES.Id = EUA.UserId
		ANd	USERRES.[Status] = 1
	ON	EUA.EmployeeId = I.ResponsableId
	
	INNER JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = I.ProcessId
	INNER JOIN Unidad U WITH(NOLOCK)
	ON	U.Id = I.UnidadId

	WHERE
		I.CompanyId = @CompanyId
	AND I.ProcessId = @ProcessId
	AND I.EndDate IS NULL
	AND I.Active = 1

END


GO

/****** Object:  StoredProcedure [dbo].[Indicador_Inactivate]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_Inactivate]
	@IndicadorId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Indicador SET
		Active = 0,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IndicadorId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Indicador_Insert]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_Insert] 
	@IndicadorId int output,
	@CompanyId int,
	@Description nvarchar(150),
	@ResponsableId int,
	@ProcessId int,
	--@ObjetivoId int,
	@Calculo nvarchar(2000),
	@MetaComparer nvarchar(10),
	@Meta decimal(18,6),
	@AlarmaComparer nvarchar(10),
	@Alarma decimal(18,6),
	@Periodicity int,
	@StartDate datetime,
	@EndDate datetime,
	@EndReason nvarchar(500),
	@EndResponsible int,
	@UnidadId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[Indicador]
           ([Descripcion]
		   ,[CompanyId]
		   ,[ResponsableId]
           ,[ProcessId]
           --,[ObjetivoId]
           ,[Calculo]
           ,[MetaComparer]
           ,[Meta]
           ,[AlarmaComparer]
           ,[Alarma]
           ,[Periodicity]
		   ,[StartDate]
           ,[EndDate]
           ,[EndReason]
           ,[EndResponsible]
           ,[UnidadId]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@Description
		   ,@CompanyId
		   ,@ResponsableId
           ,@ProcessId
           --,@ObjetivoId
           ,@Calculo
           ,@MetaComparer
           ,@Meta
           ,@AlarmaComparer
           ,@Alarma
           ,@Periodicity
		   ,@StartDate
           ,@EndDate
           ,@EndReason
           ,@EndResponsible
           ,@UnidadId
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

	SET @IndicadorId = @@IDENTITY

END



GO

/****** Object:  StoredProcedure [dbo].[Indicador_Restore]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_Restore]
	@IndicadorId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE INdicador SET
		EndDate = NULL,
		EndReason = NULL,
		EndResponsible = NULL,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IndicadorId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Indicador_Update]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_Update]
	@IndicadorId int,
	@CompanyId int,
	@Descripcion nvarchar(150),
	@ResponsableId int,
	@ProcessId int,
	--@ObjetivoId int,
	@Calculo nvarchar(2000),
	@MetaComparer nvarchar(10),
	@Meta decimal(18,6),
	@AlarmaComparer nvarchar(10),
	@Alarma decimal(18,6),
	@Periodicity int,
	@StartDate datetime,
	@EndDate datetime,
	@EndReason nvarchar(500),
	@EndResponsable int,
	@UnidadId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Indicador SET
		Descripcion = @Descripcion,
		Meta = @Meta,
		MetaComparer = @MetaComparer,
		Alarma = @Alarma,
		AlarmaComparer = @AlarmaComparer,
		Calculo = @Calculo,
		StartDate = @StartDate,
		EndDate = @EndDate,
		EndReason = @EndReason,
		EndResponsible = @EndResponsable,
		ResponsableId = @ResponsableId,
		ProcessId = @ProcessId,
		--ObjetivoId = @ObjetivoId,
		UnidadId = @UnidadId,
		Periodicity = @Periodicity,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @IndicadorId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[IndicadorObjetivo_Delete]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[IndicadorObjetivo_Delete]
	@ObjetivoId int,
	@IndicadorId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE IndicadorObjetivo SET
		Active = 0,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		IndicadorId = @IndicadorId
	AND	ObjetivoId = @ObjetivoId
	AND CompanyId = @CompanyId

END

GO

/****** Object:  StoredProcedure [dbo].[IndicadorObjetivo_GetByIndicadorId]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[IndicadorObjetivo_GetByIndicadorId]
	@IndicadorId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		IO.IndicadorId,
		IO.ObjetivoId,
		IO.CompanyId,
		IO.Active
	FROM IndicadorObjetivo IO WITH(NOLOCK)
	WHERE
		IndicadorId = @IndicadorId
	AND CompanyId = @CompanyId
	AND Active = 1

END

GO

/****** Object:  StoredProcedure [dbo].[IndicadorObjetivo_GetByObjetivoId]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[IndicadorObjetivo_GetByObjetivoId]
	@ObjetivoId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		IO.IndicadorId,
		IO.ObjetivoId,
		IO.CompanyId,
		IO.Active
	FROM IndicadorObjetivo IO WITH(NOLOCK)
	WHERE
		ObjetivoId = @ObjetivoId
	AND CompanyId = @CompanyId
	AND Active = 1

END

GO

/****** Object:  StoredProcedure [dbo].[IndicadorObjetivo_Save]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[IndicadorObjetivo_Save]
	@ObjetivoId int,
	@IndicadorId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE IndicadorObjetivo SET
		Active = 1,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		IndicadorId = @IndicadorId
	AND	ObjetivoId = @ObjetivoId
	AND CompanyId = @CompanyId

	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO IndicadorObjetivo
		(
			[IndicadorId]
           ,[ObjetivoId]
           ,[CompanyId]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active]
		)
		VALUES
		(
			@IndicadorId
           ,@ObjetivoId
           ,@CompanyId
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1
		)

	END
END

GO

/****** Object:  StoredProcedure [dbo].[IndicadorRegistro_Activate]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IndicadorRegistro_Activate]
	@IndicadorRegistroId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE IndicadorRegistro SET
		Active = 1,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IndicadorRegistroId
	AND CompanyId = @CompanyId

END


GO

/****** Object:  StoredProcedure [dbo].[IndicadorRegistro_GetById]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IndicadorRegistro_GetById]
	@IndicadorRegistroId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		IR.Id,
		IR.IndicadorId,
		I.Descripcion AS IndicadorDescripcion,
		IR.Meta,
		IR.Alarm,
		IR.Value,
		IR.[Date] AS Date,
		IR.ResponsibleId,
		EMP.Name AS ResponsibleName,
		EMP.LastName AS ResponsibleLastName,
		ISNULL(AU.Id,-1) AS EmployeeUserId,
		ISNULL(AU.[Login],'') AS EmployeeUserName,
		IR.CreatedBy,
		CB.[Login] AS CreatedByName,
		IR.CreatedOn,
		IR.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		IR.ModifiedOn,
		IR.Active,
		IR.Comments,
		IR.MetaComparer,
		IR.AlarmComparer
	FROM IndicadorRegistro IR WITH(NOLOCK)
	INNER JOIN Indicador I WITH(NOLOCK)
	ON	I.Id = IR.IndicadorId
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = IR.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = IR.ModifiedBy
	INNER JOIN Employee EMP WITH(NOLOCK)
		LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
			INNER JOIN ApplicationUser AU WITH(NOLOCK)
			ON	AU.Id = EUA.UserId
		ON	EUA.EmployeeId = EMP.Id
	ON	EMP.Id = IR.ResponsibleId

	WHERE
		IR.Id = @IndicadorRegistroId
	AND	IR.CompanyId = @CompanyId

END

GO

/****** Object:  StoredProcedure [dbo].[IndicadorRegistro_GetByIndicadorId]    Script Date: 24/10/2018 20:20:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IndicadorRegistro_GetByIndicadorId]
	@IndicadorId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		IR.Id,
		IR.IndicadorId,
		I.Descripcion AS IndicadorDescripcion,
		IR.Meta,
		IR.Alarm,
		IR.Value,
		IR.[Date] AS Date,
		IR.ResponsibleId,
		EMP.Name AS ResponsibleName,
		EMP.LastName AS ResponsibleLastName,
		ISNULL(AU.Id,-1) AS EmployeeUserId,
		ISNULL(AU.[Login],'') AS EmployeeUserName,
		IR.CreatedBy,
		CB.[Login] AS CreatedByName,
		IR.CreatedOn,
		IR.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		IR.ModifiedOn,
		IR.Active,
		IR.Comments,
		IR.MetaComparer,
		IR.AlarmComparer
	FROM IndicadorRegistro IR WITH(NOLOCK)
	INNER JOIN Indicador I WITH(NOLOCK)
	ON	I.Id = IR.IndicadorId
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = IR.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = IR.ModifiedBy
	INNER JOIN Employee EMP WITH(NOLOCK)
		LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
			INNER JOIN ApplicationUser AU WITH(NOLOCK)
			ON	AU.Id = EUA.UserId
			AND AU.[Status] = 1
		ON	EUA.EmployeeId = EMP.Id
	ON	EMP.Id = IR.ResponsibleId

	WHERE
		IR.IndicadorId = @IndicadorId
	AND	IR.CompanyId = @CompanyId
	AND IR.Active = 1

END


GO

/****** Object:  StoredProcedure [dbo].[IndicadorRegistro_Inactivate]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IndicadorRegistro_Inactivate]
	@IndicadorRegistroId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE IndicadorRegistro SET
		Active = 0,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IndicadorRegistroId
	AND CompanyId = @CompanyId

END


GO

/****** Object:  StoredProcedure [dbo].[IndicadorRegistro_Insert]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IndicadorRegistro_Insert]
	@Id int output,
	@IndicadorId int,
	@CompanyId int,
	@Date datetime,
	@MetaComparer nvarchar(10),
	@Meta decimal(18,6),
	@AlarmComparer nvarchar(10),
	@Alarm decimal (18,6),
	@Value decimal (18,6),
	@ResponsibleId int,
	@Comments nvarchar(500),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[IndicadorRegistro]
           ([CompanyId]
           ,[IndicadorId]
		   ,[MetaComparer]
           ,[Meta]
		   ,[AlarmComparer]
           ,[Alarm]
           ,[Date]
           ,[Value]
           ,[ResponsibleId]
           ,[Comments]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@CompanyId
           ,@IndicadorId
		   ,@MetaComparer
           ,@Meta
		   ,@AlarmComparer
           ,@Alarm
           ,@Date
           ,@Value
           ,@ResponsibleId
           ,@Comments
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

	SET @Id = @@IDENTITY

END


GO

/****** Object:  StoredProcedure [dbo].[IndicadorRegistro_Update]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IndicadorRegistro_Update]
	@Id int,
	@Date datetime,
	@Value decimal (18,6),
	@ResponsibleId int,
	@Comments nvarchar(500),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE IndicadorRegistro SET
		[Date] = @Date,
		Value = @Value,
		ResponsibleId = @ResponsibleId,
		Comments = @Comments,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id

END


GO

/****** Object:  StoredProcedure [dbo].[Indicator_Filter]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[Indicator_Filter]
	@CompanyId int,
	@IndicadorType int,
	@DateFrom datetime,
	@DateTo datetime,
	@ProcessId int,
	@ProcessTypeId int,
	@ObjetivoId int,
	@Status int
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		Data.IndicadorId,
		Data.Description,
		Data.ProcessId,
		Data.ProcessDescription,
		Data.ProcessType,
		Data.ObjetivoId,
		Data.ObjetivoName,
		Data.ObjetivoResponsibleName,
		Data.ObjetivoResponsableLastName,
		Data.ProcesoResponsible,
		Data.StartDate,
		Data.EndDate,
		Data.MetaComparer,
		Data.Meta,
		Data.AlarmaComparer,
		Data.Alarma,
		IR.Value,
		IR.Date
	FROM
	(
		SELECT DISTINCT
			I.Id AS IndicadorId,
			I.Descripcion AS Description,
			ISNULL(I.ProcessId,0) AS ProcessId,
			ISNULL(PR.[Description],'') AS ProcessDescription,
			ISNULL(PR.[Type],0) AS ProcessType,
			CASE WHEN IO.ObjetivoId IS NULL THEN 0 ELSE 1 END AS ObjetivoId,
			'' AS ObjetivoName,
			ISNULL(OEMP.Name,'') AS ObjetivoResponsibleName,
			ISNULL(OEMP.LastName,'') AS ObjetivoResponsableLastName,
			ISNULL(C.[Description],'') AS ProcesoResponsible,
			I.StartDate AS StartDate,
			I.EndDate AS EndDate,
			I.MetaComparer AS MetaComparer,
			I.Meta AS Meta,
			I.AlarmaComparer AS AlarmaComparer,
			I.Alarma AS Alarma
		
		FROM Indicador I WITH(NOLOCK)
		LEFT JOIN Proceso PR WITH(NOLOCK)
			LEFT JOIN Cargos C WITH(NOLOCK)
			ON	C.Id = PR.CargoId
		ON	PR.Id = I.ProcessId
		AND PR.CompanyId = I.CompanyId
		AND I.ProcessId > 0
	
		LEFT JOIN Employee OEMP WITH(NOLOCK)
		ON	OEMP.Id = I.ResponsableId
		LEFT JOIN IndicadorObjetivo IO WITH(NOLOCK)
		ON	IO.IndicadorId = I.Id



		WHERE
			I.CompanyId = @CompanyId
		AND I.Active = 1
		AND	(@DateFrom IS NULL OR I.StartDate >= @DateFrom)
		AND (@DateTo IS NULL OR I.StartDate <= @DateTo)
		AND
		(
			@ProcessId IS NULL OR PR.Id = @ProcessId OR @ProcessId = 0
		)
		AND
		(
			@ObjetivoId IS NULL OR IO.ObjetivoId = @ObjetivoId OR @ObjetivoId = 0
		)
		AND 
		(
			@ProcessTypeId IS NULL OR PR.[Type] = @ProcessTypeId
		)
		AND
		(
			@IndicadorType = 0
			OR
			(@IndicadorType = 1 AND PR.Id > 0)
			OR
			(@IndicadorType = 2 AND IO.ObjetivoId > 0)
		)
		AND
		(
			@Status = 0
			OR
			(I.EndDate > GETDATE() OR I.EndDate IS NULL AND @Status = 1)
			OR
			(I.EndDate < GETDATE() AND @Status = 2)
		)
	)
	AS DATA
	LEFT JOIN IndicadorRegistro IR
	ON IR.IndicadorId = Data.IndicadorId

	ORDER BY Data.IndicadorId, IR.Date DESC
	
END







GO

/****** Object:  StoredProcedure [dbo].[JobPosition_Delete]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[JobPosition_Delete]
	@JobPositionId bigint,
	@CompanyId int,
	@Extradata nvarchar(200),
    @UserId int
AS
BEGIN
	UPDATE Cargos SET 
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @JobPositionId
	AND CompanyId = @CompanyId

	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		9,
		@JobPositionId,
		3,
		GETDATE(),
		@ExtraData
    )
END





GO

/****** Object:  StoredProcedure [dbo].[JobPosition_GetByCompany]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[JobPosition_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		C.Id,
		C.Description,
		ISNULL(C.Responsabilidades,'') AS Responsabilidades,
		ISNULL(C.Notas,'') AS Notas,
		ISNULL(C.FormacionAcademicaDeseada,'') AS FormacionAcademicaDeseada,
		ISNULL(C.FormacionEspecificaDesdeada,'') AS FormacionEspecificaDeseada,
		ISNULL(C.ExperienciaLaboralDeseada,'') AS ExperienciaLaboralDeseada,
		ISNULL(C.HabilidadesDeseadas,'') AS HabilidadesDeseadas,
		C.DepartmentId,
		ISNULL(D.Name,'') AS DepartmentName,
		ISNULL(C2.Id, 0) AS ResponsableId,
		ISNULL(C2.Description,'') AS ResponsableDescription,
		D.Deleted AS DepartmentActive
	FROM Cargos C WITH(NOLOCK)
	INNER JOIN Department D WITH(NOLOCK)		
	ON	D.Id = C.DepartmentId
	AND D.CompanyId = C.CompanyId
	LEFT JOIN Cargos C2 WITH(NOLOCK)
	ON	C2.Id = C.ResponsableId
	ANd	C2.Active = 1
	WHERE 
		C.CompanyId = @CompanyId
	AND C.Active = 1
	ORDER BY C.Description ASC
END





GO

/****** Object:  StoredProcedure [dbo].[JobPosition_GetByDepartment]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[JobPosition_GetByDepartment]
	@DepartmentId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		C.Id,
		C.Description,		
		D.Id AS DepartmentId,
		D.Name AS DepartmentName,
		ISNULL(C2.Id,0) AS ResponsableId,
		ISNULL(C2.Description,'') AS ResponsableDescription
	FROM Cargos C WITH(NOLOCK)
	INNER JOIN Department D WITH(NOLOCK) 
	ON	D.CompanyId = C.CompanyId
	AND D.Id = @DepartmentId
	LEFT JOIN Cargos C2 WITH(NOLOCK)
	ON	C.ResponsableId = C2.Id
	AND C2.CompanyId = C.CompanyId
	WHERE
		C.CompanyId = @CompanyId
	AND C.DepartmentId = @DepartmentId
END





GO

/****** Object:  StoredProcedure [dbo].[JobPosition_GetByEmployee]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[JobPosition_GetByEmployee]
	@EmployeeId int,
	@CompanyId int
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		C.Id,
		C.Description,
		C.DepartmentId
	FROM Cargos C WITH(NOLOCK)
	INNER JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
	ON	ECA.CargoId = C.Id
	WHERE
		ECA.EmployeeId = @EmployeeId
	AND C.CompanyId = @CompanyId
	ORDER BY
		C.Description ASC
END





GO

/****** Object:  StoredProcedure [dbo].[JobPosition_GetById]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[JobPosition_GetById]
	@Id int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		C.Id,
		C.Description,
		ISNULL(C.Responsabilidades,'') AS Responsabilidades,
		ISNULL(C.Notas,'') AS Notas,
		ISNULL(C.FormacionAcademicaDeseada,'') AS FormacionAcademicaDeseada,
		ISNULL(C.FormacionEspecificaDesdeada,'') AS FormacionEspecificaDeseada,
		ISNULL(C.ExperienciaLaboralDeseada,'') AS ExperienciaLaboralDeseada,
		ISNULL(C.HabilidadesDeseadas,'') AS HabilidadesDeseadas,
		ISNULL(C.ResponsableId, 0) AS ResponsableId,
		ISNULL(C.Description, '') AS ResponsableDescription,
		C.DepartmentId,
		D.Name AS DepartmentName,
		C.ModifiedOn,
		AU.Id AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName
	FROM Cargos C WITH(NOLOCK)
	INNER JOIN Department D WITH(NOLOCK)
	ON	D.Id = C.DepartmentId
	AND D.CompanyId = C.CompanyId	
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON AU.Id = C.ModifiedBy	
	LEFT JOIN Cargos C2 WITH(NOLOCK)
	ON	C2.Id= C.ResponsableId
	WHERE 
		C.CompanyId = @CompanyId
	AND C.Id = @Id
END





GO

/****** Object:  StoredProcedure [dbo].[JobPosition_GetEmployees]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[JobPosition_GetEmployees]
	@JobPositionId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
		E.Id,
		E.Name,
		ISNULL(E.LastName,'') AS LastName,
		E.NIF,
		E.Email,
		E.Phone
	FROM Employee E WITH(NOLOCK)
	INNER JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
	ON	ECA.CargoId = @JobPositionId
	AND ECA.EmployeeId = E.Id
	AND ECA.CompanyId = E.CompanyId
	AND ECA.FechaBaja IS NULL
	WHERE
		E.Active = 1
	AND E.FechaBaja IS NULL
END





GO

/****** Object:  StoredProcedure [dbo].[JobPosition_GetEmployeesHistorical]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[JobPosition_GetEmployeesHistorical]
	@JobPositionId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		COUNT(E.Id)
	FROM Employee E WITH(NOLOCK)
	INNER JOIN EmployeeCargoAsignation ECA WITH(NOLOCK)
	ON	ECA.CargoId = @JobPositionId
	AND ECA.EmployeeId = E.Id
	AND ECA.CompanyId = E.CompanyId
	--AND ECA.FechaBaja IS NULL
	WHERE
		E.Active = 1
END





GO

/****** Object:  StoredProcedure [dbo].[JobPosition_Insert]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[JobPosition_Insert]
	@JobPositionId bigint out,
	@CompanyId int,
	@DepartmentId int,
	@ResponsableId bigint,
	@Description nvarchar(100),
	@Responsabilidades nvarchar(2000),
	@Notas nvarchar(2000),
	@FormacionAcademicaDeseada nvarchar(2000),
	@FormacionEspecificaDeseada nvarchar(2000),
	@ExperienciaLaboralDeseada nvarchar(2000),
	@HabilidadesDeseadas nvarchar(2000),
	@UserId int
AS
BEGIN
	INSERT INTO Cargos
	(
		CompanyId,
		DepartmentId,
		ResponsableId,
		Description,
		Responsabilidades,
		Notas,
		FormacionAcademicaDeseada,
		FormacionEspecificaDesdeada,
		ExperienciaLaboralDeseada,
		HabilidadesDeseadas,
		Active,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@DepartmentId,
		@ResponsableId,
		@Description,
		@Responsabilidades,
		@Notas,
		@FormacionAcademicaDeseada,
		@FormacionEspecificaDeseada,
		@ExperienciaLaboralDeseada,
		@HabilidadesDeseadas,
		1,
		@UserId,
		GETDATE()
	)
	
	SET @JobPositionId = @@IDENTITY

END





GO

/****** Object:  StoredProcedure [dbo].[JobPosition_Unlink]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[JobPosition_Unlink] 
	@EmployeeId int,
	@JobPositionId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM EmployeeCargoAsignation
	WHERE
		EmployeeId = @EmployeeId
	AND CargoId = @JobPositionId
END




GO

/****** Object:  StoredProcedure [dbo].[JobPosition_Update]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[JobPosition_Update]
	@JobPositionId bigint,
	@CompanyId int,
    @DepartmentId int,
    @ResponsableId bigint,
    @Description nvarchar(100),
    @Responsabilidades nvarchar(2000),
    @Notas nvarchar(2000),
    @FormacionAcademicaDeseada nvarchar(2000),
    @FormacionEspecificaDesdeada nvarchar(2000),
    @ExperienciaLaboralDeseada nvarchar(2000),
    @HabilidadesDeseadas nvarchar(2000),
    @ModifiedBy int
AS
BEGIN
	UPDATE Cargos SET 
		DepartmentId = @DepartmentId,
		ResponsableId = @ResponsableId,
		Description = @Description,
		Responsabilidades = @Responsabilidades,
		Notas = @Notas,
		FormacionAcademicaDeseada = @FormacionAcademicaDeseada,
		FormacionEspecificaDesdeada = @FormacionEspecificaDesdeada,
		ExperienciaLaboralDeseada = @ExperienciaLaboralDeseada,
		HabilidadesDeseadas = @HabilidadesDeseadas,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = GETDATE()
	WHERE
		Id = @JobPositionId
	AND CompanyId = @CompanyId


END





GO

/****** Object:  StoredProcedure [dbo].[Learning_AddAssistant]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Learning_AddAssistant]
	@LearningAssistantId int out,
	@LearningId int,
	@CompanyId int,
	@EmployeeId bigint,
	@JobPositionId bigint,
	@UserId int
AS
BEGIN
	SELECT
		@LearningAssistantId = LA.Id 
	FROM LearningAssistant LA WITH(NOLOCK)
	WHERE
		LA.LearningId = @LearningId
	AND LA.CompanyId = @CompanyId
	AND LA.EmployeeId = @EmployeeId
	
	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO LearningAssistant
		(
			LearningId,
			CompanyId,
			EmployeeId,
			CargoId,
			Completed,
			Success,
			CreatedBy,
			CreatedOn,
			ModifiedBy,
			ModifiedOn,
			Active
		)
		VALUES
		(
			@LearningId,
			@CompanyId,
			@EmployeeId,
			@JobPositionId,
			null,
			null,
			@UserId,
			GETDATE(),
			@UserId,
			GETDATE(),
			1
		)
		
		SET @LearningAssistantId = @@IDENTITY;
		
		INSERT INTO ActivityLog
		(
			ActivityId,
			UserId,
			CompanyId,
			TargetType,
			TargetId,
			ActionId,
			DateTime,
			ExtraData
		)
		VALUES
		(
			NEWID(),
			@UserId,
			@CompanyId,
			11,
			@LearningId,
			4,
			GETDATE(),
			'Add Assistant ' + CAST(@LearningAssistantId AS NVARCHAR(32)) 
		)
	END

END





GO

/****** Object:  StoredProcedure [dbo].[Learning_AssistantDelete]    Script Date: 24/10/2018 20:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
CREATE PROCEDURE [dbo].[Learning_AssistantDelete]
	@LearningAssistantId int,
	@LearningId int,
	@CompanyId int,
	@UserId int
AS
BEGIN
	DELETE FROM LearningAssistant
	WHERE
		Id = @LearningAssistantId
	AND CompanyId = @CompanyId
	AND LearningId = @LearningId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		11,
		@LearningId,
		5,
		GETDATE(),
		'AssistantId:' + CAST(@LearningAssistantId AS nvarchar(6))
    )
END





GO

/****** Object:  StoredProcedure [dbo].[Learning_Delete]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Learning_Delete]
	@LearningId int,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE Learning SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningId
	ANd	CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    SELECT
		NEWID(),
		@UserId,
		@CompanyId,
		11,
		LA.Id,
		5,
		GETDATE(),
		@Reason
	FROM LearningAssistant LA
	WHERE
		LearningId = @LearningId
	AND CompanyId = @CompanyId
	AND Active = 1
		
	
	UPDATE LearningAssistant SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		LearningId = @LearningId
	AND CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		11,
		@LearningId,
		6,
		GETDATE(),
		@Reason
    )
END





GO

/****** Object:  StoredProcedure [dbo].[Learning_Filter]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Learning_Filter]
	@YearFrom datetime,
	@YearTo datetime,
	@Mode int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		L.Id,
		L.Description,
		L.DateStimatedDate,
		YEAR(L.DateStimatedDate),
		L.Amount,
		L.[Status],
		L.RealFinish
	FROM Learning L WITH(NOLOCK)
	WHERE
		L.CompanyId = @CompanyId
	AND L.Active = 1
	AND (@YearFrom IS NULL OR L.DateStimatedDate >= @YearFrom)
	AND (@YearTo IS NULL OR L.DateStimatedDate <= @YearTo)
	AND (
			@Mode = 3
			OR
			(L.Status = 0 AND @Mode = 0)
			OR
			(L.Status = 1 AND @Mode = 1)
			OR
			(L.Status = 2 AND @Mode = 2)
		)
END




GO

/****** Object:  StoredProcedure [dbo].[Learning_GetAll]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Learning_GetAll]
	@CompanyId int,
	@Year int,
	@Status int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		L.Id,
		L.Description,
		LA.Id,
		E.Id,
		ISNULL(E.Name,'') AS Name,
		ISNULL(E.LastName,'') AS LastName,
		'' AS SecondLastName,
		LA.Completed,
		LA.Success,
		L.DateStimatedDate
	FROM Learning L WITH(NOLOCK)
	INNER JOIN LearningAssistant LA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	E.Id = LA.EmployeeId
		AND E.CompanyId = LA.CompanyId
	ON	LA.LearningId = L.Id
	AND	LA.CompanyId = L.CompanyId
	WHERE
		L.CompanyId = @CompanyId
	AND (@Year IS NULL OR L.Year = @Year)
	AND (@Status IS NULL OR L.Status = @Status)
END





GO

/****** Object:  StoredProcedure [dbo].[Learning_GetAssistance]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Learning_GetAssistance]
	@LearningId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		LA.Id,
		E.Id,
		ISNULL(E.Name,'') AS Name,
		ISNULL(E.LastName,'') AS LastName,
		LA.Completed,
		LA.Success,
		L.DateStimatedDate,
		ISNULL(LA.CargoId,0) AS JobPositionId,
		ISNULL(C.Description,'') AS JobPositionDescription
	FROM Learning L WITH(NOLOCK)
	INNER JOIN LearningAssistant LA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	E.Id = LA.EmployeeId
		AND E.CompanyId = LA.CompanyId
	ON	LA.LearningId = L.Id
	AND	LA.CompanyId = L.CompanyId
	LEFT JOIN Cargos C WITH(NOLOCK)
	ON	C.Id = LA.CargoId
	AND C.CompanyId = La.CompanyId
	WHERE
		L.CompanyId = @CompanyId
	AND L.Id = @LearningId
END





GO

/****** Object:  StoredProcedure [dbo].[Learning_GetById]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Learning_GetById]
	@LearningId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		L.Id,
		L.CompanyId,
		ISNULL(L.Description,'') AS Description,
		L.DateStimatedDate,
		L.Hours,
		L.Amount,
		L.Master,
		ISNULL(L.Notes,'') AS Notes,
		L.Status,
		L.Year,
		L.RealStart,
		L.RealFinish,
		ISNULL(L.Objetivo,'') AS Objetivo,
		ISNULL(L.Metodologia,'') AS Metodologia,
		L.ModifiedOn,
		L.ModifiedBy AS ModifiedByUserId,
		ISNULL(AU.[Login],'')
	FROM Learning L WITH(NOLOCK)
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = L.ModifiedBy
	WHERE
		L.CompanyId = @CompanyId
	AND L.Id = @LearningId
END





GO

/****** Object:  StoredProcedure [dbo].[Learning_Insert]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Learning_Insert]
	@LearningId int out,
	@CompanyId int,
	@Description nvarchar(100),
	@Status int,
	@DateStimatedDate date,
	@RealStart date,
	@RealFinish date,
	@Master nvarchar(100),
	@Hours int,
	@Amount numeric(18,3),
	@Notes text,
	@UserId int,
	@Year int,
	@Objetivo text,
	@Metodologia text
AS
BEGIN
	IF @RealFinish IS NOT NULL
	BEGIN
		SET @Status = 1
	END
	
	INSERT INTO Learning
	(
		CompanyId,
		Description,
		Status,
		DateStimatedDate,
		RealStart,
		RealFinish,
		Master,
		Hours,
		Amount,
		Notes,
		ModifiedBy,
		ModifiedOn,
		Year,
		Active,
		Objetivo,
		Metodologia
	)
	VALUES
	(
		@CompanyId,
		@Description,
		@Status,
		@DateStimatedDate,
		@RealStart,
		@RealFinish,
		@Master,
		@Hours,
		@Amount,
		@Notes,
		@UserId,
		GETDATE(),
		@Year,
		1,
		@Objetivo,
		@Metodologia
	)
	
	SET @LearningId = @@IDENTITY	

END





GO

/****** Object:  StoredProcedure [dbo].[Learning_Update]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Learning_Update]
	@LearningId int,
	@CompanyId int,
	@Description nvarchar(100),
	@Status int,
	@DateStimatedDate date,
	@RealStart date,
	@RealFinish date,
	@Master nvarchar(100),
	@Hours int,
	@Amount numeric(18,3),
	@Notes text,
	@UserId int,
	@Year int,
	@Objetivo text,
	@Metodologia text
AS
BEGIN
	IF @RealFinish IS NOT NULL AND @Status = 0
	BEGIN
		SET @Status = 1
		DELETE FROM LearningAssistant WHERE LearningId = @LearningId
	END

	UPDATE Learning SET
		Description = @Description,
		Status = @Status,
		DateStimatedDate = @DateStimatedDate,
		RealStart = @RealStart,
		RealFinish = @RealFinish,
		Master = @Master,
		Hours = @Hours,
		Amount = @Amount,
		Notes = @Notes,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE(),
		Year = @Year,
		Objetivo = @Objetivo,
		Metodologia = @Metodologia
	WHERE
		Id = @LearningId
	AND CompanyId = @CompanyId
	
	

END





GO

/****** Object:  StoredProcedure [dbo].[LearningAssistant_Complete]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[LearningAssistant_Complete]
	@LearningAssitantId int,
	@CompanyId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE LearningAssistant SET
		Completed = 1,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningAssitantId
	AND CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		12,
		@LearningAssitantId,
		3,
		GETDATE(),
		''
	)
	
	DECLARE @LearningId int;
	
	SELECT
		@LearningId =La.LearningId
	FROM LearningAssistant LA WITH(NOLOCK)
	WHERE
		LA.Id = @LearningAssitantId
	AND La.CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		11,
		@LearningId,
		4,
		GETDATE(),
		'Assistant ' + CAST(@LearningAssitantId AS NVARCHAR(32)) + ' - Complete'
	)


END





GO

/****** Object:  StoredProcedure [dbo].[LearningAssistant_CompleteFail]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[LearningAssistant_CompleteFail]
	@LearningAssitantId int,
	@CompanyId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE LearningAssistant SET
		Completed = 0,
		Success = NULL,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningAssitantId
	AND CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		12,
		@LearningAssitantId,
		3,
		GETDATE(),
		''
	)
	
	DECLARE @LearningId int;
	
	SELECT
		@LearningId =La.LearningId
	FROM LearningAssistant LA WITH(NOLOCK)
	WHERE
		LA.Id = @LearningAssitantId
	AND La.CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		11,
		@LearningId,
		4,
		GETDATE(),
		'Assistant ' + CAST(@LearningAssitantId AS NVARCHAR(32)) + ' - Complete'
	)


END





GO

/****** Object:  StoredProcedure [dbo].[LearningAssistant_Reset]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[LearningAssistant_Reset]
	@LearningAssitantId int,
	@CompanyId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE LearningAssistant SET
		Completed = 1,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningAssitantId
	AND CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[LearningAssistant_SetStatus]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[LearningAssistant_SetStatus]
	@LearningAssitantId int,
	@CompanyId int,
	@UserId int,
	@Completed bit,
	@Success bit
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE LearningAssistant SET
		Completed = @Completed,
		Success = @Success,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningAssitantId
	AND CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[LearningAssistant_Success]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[LearningAssistant_Success]
	@LearningAssitantId int,
	@CompanyId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE LearningAssistant SET
		Completed = 1,
		Success = 1,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningAssitantId
	AND CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		12,
		@LearningAssitantId,
		4,
		GETDATE(),
		''
	)
	
	DECLARE @LearningId int;
	
	SELECT
		@LearningId =La.LearningId
	FROM LearningAssistant LA WITH(NOLOCK)
	WHERE
		LA.Id = @LearningAssitantId
	AND La.CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		11,
		@LearningId,
		4,
		GETDATE(),
		'Assistant ' + CAST(@LearningAssitantId AS NVARCHAR(32)) + ' - Success'
	)


END





GO

/****** Object:  StoredProcedure [dbo].[LearningAssistant_SuccessFail]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[LearningAssistant_SuccessFail]
	@LearningAssitantId int,
	@CompanyId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE LearningAssistant SET
		Completed = 1,
		Success = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningAssitantId
	AND CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		12,
		@LearningAssitantId,
		4,
		GETDATE(),
		''
	)
	
	DECLARE @LearningId int;
	
	SELECT
		@LearningId =La.LearningId
	FROM LearningAssistant LA WITH(NOLOCK)
	WHERE
		LA.Id = @LearningAssitantId
	AND La.CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		11,
		@LearningId,
		4,
		GETDATE(),
		'Assistant ' + CAST(@LearningAssitantId AS NVARCHAR(32)) + ' - Success'
	)


END





GO

/****** Object:  StoredProcedure [dbo].[LearningAssistant_Unevaluated]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[LearningAssistant_Unevaluated]
	@LearningAssitantId int,
	@CompanyId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE LearningAssistant SET
		Completed = NULL,
		Success = NULL,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningAssitantId
	AND CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		12,
		@LearningAssitantId,
		3,
		GETDATE(),
		''
	)
	
	DECLARE @LearningId int;
	
	SELECT
		@LearningId =La.LearningId
	FROM LearningAssistant LA WITH(NOLOCK)
	WHERE
		LA.Id = @LearningAssitantId
	AND La.CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		DateTime,
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		11,
		@LearningId,
		4,
		GETDATE(),
		'Assistant ' + CAST(@LearningAssitantId AS NVARCHAR(32)) + ' - Complete'
	)


END





GO

/****** Object:  StoredProcedure [dbo].[Log_Login]    Script Date: 24/10/2018 20:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Log_Login]
	@UserName nvarchar(50),
	@Ip nvarchar(50),
	@Result int,
	@UserId int,
	@CompanyCode nvarchar(10),
	@CompanyId int
AS
BEGIN
	INSERT INTO Logins
           ([Id]
           ,[UserName]
           ,[Date]
           ,[IP]
           ,[Result]
           ,[UserId]
           ,[CompanyCode]
           ,[CompanyId])
     VALUES
           (NEWID()
           ,@UserName
           ,GETDATE()
           ,@Ip
           ,@Result
           ,@UserId
           ,@CompanyCode
           ,@CompanyId)
END





GO

/****** Object:  StoredProcedure [dbo].[MeasureUnit_GetByCompany]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[MeasureUnit_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT
		J.Id,
		J.CompanyId,
		J.Description,
		J.Active,
		J.ModifiedOn,
		J.ModifiedBy AS ModifiedByUserId,
		E.Id AS ModifiedByEmployeeId,
		ISNULL(E.Name,'') AS ModifiedByName,
		ISNULL(E.LastName,'') AS ModifiedByLastName,
		P1.Type
	FROM MeasureUnit J WITH(NOLOCK)
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	E.Id = EUA.EmployeeId
		AND E.CompanyId = EUA.CompanyId
	ON	EUA.UserId = J.ModifiedBy
	LEFT JOIN 
	(
		SELECT P.Type FROM Proceso P WITH(NOLOCK)
	) P1
	ON P1.Type = J.Id
	
	WHERE
		J.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Objetivo_Activate]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_Activate]
	@ObjetivoId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE Objetivo SET
		Active = 1,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @ObjetivoId
	AND CompanyId  = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Objetivo_Anulate]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_Anulate]
	@ObjetivoId int,
	@CompanyId int,
	@EndDate datetime,
	@EndReason nvarchar(500),
	@EndResponsible int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Objetivo SET
		EndDate = @EndDate,
		EndReason = @EndReason,
		ResponsibleClose = @EndResponsible,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @ObjetivoId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Objetivo_Filter]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[Objetivo_Filter]
	@CompanyId int,
	@DateFrom datetime,
	@DateTo datetime,
	@Status int
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		O.Id AS ObjetivoId,
		O.Name,
		O.ResponsibleId,
		ISNULL(R.Name,'') AS ResponsibleName,
		ISNULL(R.LastName,'') AS ResponsibleLastname,
		O.StartDate,
		O.PreviewEndDate,
		O.EndDate,
		O.Active
	FROM Objetivo O WITH(NOLOCK)
	LEFT JOIN Employee R WITH(NOLOCK)
	ON	R.Id = O.ResponsibleId
	
	WHERE
		O.CompanyId = @CompanyId
	AND O.Active = 1
	-- AND	(@DateFrom IS NULL OR O.PreviewEndDate >= @DateFrom)
	-- AND (@DateTo IS NULL OR O.PreviewEndDate <= @DateTo)
	AND	(@DateFrom IS NULL OR (O.EndDate >= @DateFrom) OR (O.EndDate IS NULL AND O.PreviewEndDate >= @DateFrom))
	AND (@DateTo IS NULL OR O.StartDate <= @DateTo)
	
	AND
	(
		@Status = 0
		OR
		(O.EndDate > GETDATE() OR O.EndDate IS NULL AND @Status = 1)
		OR
		(
		O.EndDate < GETDATE() AND @Status = 2
		)
	)
	
END







GO

/****** Object:  StoredProcedure [dbo].[Objetivo_GeById]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_GeById]
	@ObjetivoId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		O.Id,
		O.Name,
		O.[Description],
		O.Methodology,
		O.Resources,
		O.Notes,
		O.VinculatedToIndicator,
		O.IndicatorId,
		O.RevisionId,
		O.StartDate,
		O.PreviewEndDate,
		O.EndDate,
		O.ResponsibleId,
		O.ResponsibleClose,
		O.CreatedBy,
		CB.[Login] AS CreatedByName,
		O.CreatedOn,
		O.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		O.ModifiedOn,
		O.Active,
		ISNULL(R.Name,'') AS ResponsibleName,
		ISNULL(R.LastName,'') AS ResponsibleLastname,
		ISNULL(RC.Name,'') AS ResponsibleCloseName,
		ISNULL(RC.LastName,'') AS ResponsibleCloseLastame,
		ISNULL(O.EndReason,'') AS EndReason
	FROM Objetivo O WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = O.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = O.ModifiedBy
	INNER JOIN Employee R WITH(NOLOCK)
	ON	R.Id = O.ResponsibleId
	LEFT JOIN Employee RC WITH(NOLOCK)
	ON	RC.Id = O.ResponsibleClose

	WHERE
		O.CompanyId = @CompanyId
	AND O.Id = @ObjetivoId
END


GO

/****** Object:  StoredProcedure [dbo].[Objetivo_GetActive]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_GetActive]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		O.Id,
		O.Name,
		O.[Description],
		O.Methodology,
		O.Resources,
		O.Notes,
		O.VinculatedToIndicator,
		O.IndicatorId,
		O.RevisionId,
		O.StartDate,
		O.PreviewEndDate,
		O.EndDate,
		O.ResponsibleId,
		O.ResponsibleClose,
		O.CreatedBy,
		CB.[Login] AS CreatedByName,
		O.CreatedOn,
		O.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		O.ModifiedOn,
		O.Active,
		ISNULL(R.Name,'') AS ResponsibleName,
		ISNULL(R.LastName,'') AS ResponsibleLastname,
		ISNULL(RC.Name,'') AS ResponsibleCloseName,
		ISNULL(RC.LastName,'') AS ResponsibleCloseLastame,
		O.MetaComparer,
		O.Meta,
		ISNULL(O.EndReason,'') AS EndReason
	FROM Objetivo O WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = O.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = O.ModifiedBy
	INNER JOIN Employee R WITH(NOLOCK)
	ON	R.Id = O.ResponsibleId
	LEFT JOIN Employee RC WITH(NOLOCK)
	ON	RC.Id = O.ResponsibleClose

	WHERE
		O.CompanyId = @CompanyId
	AND O.Active = 1
END


GO

/****** Object:  StoredProcedure [dbo].[Objetivo_GetAll]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_GetAll]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		O.Id,
		O.Name,
		O.[Description],
		O.Methodology,
		O.Resources,
		O.Notes,
		O.VinculatedToIndicator,
		O.IndicatorId,
		O.RevisionId,
		O.StartDate,
		O.PreviewEndDate,
		O.EndDate,
		O.ResponsibleId,
		O.ResponsibleClose,
		O.CreatedBy,
		CB.[Login] AS CreatedByName,
		O.CreatedOn,
		O.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		O.ModifiedOn,
		O.Active,
		ISNULL(R.Name,'') AS ResponsibleName,
		ISNULL(R.LastName,'') AS ResponsibleLastname,
		ISNULL(RC.Name,'') AS ResponsibleCloseName,
		ISNULL(RC.LastName,'') AS ResponsibleCloseLastame,
		O.MetaComparer,
		O.Meta,
		ISNULL(O.EndReason,'') AS EndReason
	FROM Objetivo O WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = O.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = O.ModifiedBy
	INNER JOIN Employee R WITH(NOLOCK)
	ON	R.Id = O.ResponsibleId
	LEFT JOIN Employee RC WITH(NOLOCK)
	ON	RC.Id = O.ResponsibleClose

	WHERE
		O.CompanyId = @CompanyId
END


GO

/****** Object:  StoredProcedure [dbo].[Objetivo_GetAvailable]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_GetAvailable]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		O.Id,
		O.Name,
		O.[Description],
		O.Methodology,
		O.Resources,
		O.Notes,
		O.VinculatedToIndicator,
		O.IndicatorId,
		O.RevisionId,
		O.StartDate,
		O.PreviewEndDate,
		O.EndDate,
		O.ResponsibleId,
		O.ResponsibleClose,
		O.CreatedBy,
		CB.[Login] AS CreatedByName,
		O.CreatedOn,
		O.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		O.ModifiedOn,
		O.Active,
		ISNULL(R.Name,'') AS ResponsibleName,
		ISNULL(R.LastName,'') AS ResponsibleLastname,
		ISNULL(RC.Name,'') AS ResponsibleCloseName,
		ISNULL(RC.LastName,'') AS ResponsibleCloseLastame,
		'',
		0,
		ISNULL(O.EndReason,'') AS EndReason
	FROM Objetivo O WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = O.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = O.ModifiedBy
	INNER JOIN Employee R WITH(NOLOCK)
	ON	R.Id = O.ResponsibleId
	LEFT JOIN Employee RC WITH(NOLOCK)
	ON	RC.Id = O.ResponsibleClose

	/*LEFT JOIN Indicador I WITH(NOLOCK)
	ON	I.ObjetivoId = O.Id
	AND	I.Active = 1*/

	WHERE
		O.CompanyId = @CompanyId
	AND O.Active = 1
	--AND I.Id IS NULL
END


GO

/****** Object:  StoredProcedure [dbo].[Objetivo_GetById]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_GetById]
	@ObjetivoId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		O.Id,
		O.Name,
		O.[Description],
		O.Methodology,
		O.Resources,
		O.Notes,
		O.VinculatedToIndicator,
		O.IndicatorId,
		O.RevisionId,
		O.StartDate,
		O.PreviewEndDate,
		O.EndDate,
		O.ResponsibleId,
		O.ResponsibleClose,
		O.CreatedBy,
		CB.[Login] AS CreatedByName,
		O.CreatedOn,
		O.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		O.ModifiedOn,
		O.Active,
		ISNULL(R.Name,'') AS ResponsibleName,
		ISNULL(R.LastName,'') AS ResponsibleLastname,
		ISNULL(RC.Name,'') AS ResponsibleCloseName,
		ISNULL(RC.LastName,'') AS ResponsibleCloseLastame,
		O.MetaComparer,
		O.Meta,
		ISNULL(O.EndReason,'') AS EndReason
	FROM Objetivo O WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = O.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = O.ModifiedBy
	LEFT JOIN Employee R WITH(NOLOCK)
	ON	R.Id = O.ResponsibleId
	LEFT JOIN Employee RC WITH(NOLOCK)
	ON	RC.Id = O.ResponsibleClose

	WHERE
		O.CompanyId = @CompanyId
	AND O.Id = @ObjetivoId
END


GO

/****** Object:  StoredProcedure [dbo].[Objetivo_Inactivate]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_Inactivate]
	@ObjetivoId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE Objetivo SET
		Active = 0,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @ObjetivoId
	AND CompanyId  = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Objetivo_Insert]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_Insert]
	@ObjetivoId int output,
	@Name nvarchar(100),
	@Description nvarchar(2000),
	@ResponsibleId int,
	@StartDate datetime,
	@VinculatedToIndicator bit,
	@IndicatorId int,
	@RevisionId int,
	@Methodology nvarchar(2000),
	@Resources nvarchar(2000),
	@Notes nvarchar(2000),
	@PreviewEndDate datetime,
	@EndDate datetime,
	@ResponsibleClose int,
	@CompanyId int,
	@MetaComparer nvarchar(10),
	@Meta decimal(18,6),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

INSERT INTO [dbo].[Objetivo]
           ([Name]
           ,[Description]
           ,[ResponsibleId]
           ,[StartDate]
           ,[VinculatedToIndicator]
           ,[IndicatorId]
           ,[RevisionId]
           ,[Methodology]
           ,[Resources]
           ,[Notes]
           ,[PreviewEndDate]
           ,[EndDate]
           ,[ResponsibleClose]
           ,[CompanyId]
		   ,[MetaComparer]
		   ,[Meta]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
	 (
		@Name,
		@Description,
		@ResponsibleId,
		@StartDate,
		@VinculatedToIndicator,
		@IndicatorId,
		@RevisionId,
		@Methodology,
		@Resources,
		@Notes,
		@PreviewEndDate,
		@EndDate,
		@ResponsibleClose,
		@CompanyId,
		@MetaComparer,
		@Meta,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		1
	 )

	 SET @ObjetivoId = @@IDENTITY

	 

	IF @IndicatorId IS NOT NULL
	BEGIN
		INSERT INTO IndicadorObjetivo (IndicadorId, ObjetivoId, CompanyId, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Active)
		VALUES
		(@IndicatorId, @ObjetivoId, @CompanyId, @ApplicationUserId, GETDATE(), @ApplicationUserId, GETDATE(), 1)
	END

END



GO

/****** Object:  StoredProcedure [dbo].[Objetivo_Restore]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_Restore]
	@ObjetivoId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Objetivo SET
		EndDate = NULL,
		EndReason = NULL,
		ResponsibleClose = NULL,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()

	WHERE	
		Id = @ObjetivoId
	AND	CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Objetivo_Update]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_Update]
	@ObjetivoId int,
	@Name nvarchar(100),
	@Description nvarchar(2000),
	@ResponsibleId int,
	@StartDate datetime,
	@VinculatedToIndicator bit,
	@IndicatorId int,
	@RevisionId int,
	@Methodology nvarchar(2000),
	@Resources nvarchar(2000),
	@Notes nvarchar(2000),
	@PreviewEndDate datetime,
	@EndDate datetime,
	@ResponsibleClose int,
	@CompanyId int,
	@MetaComparer nvarchar(10),
	@Meta decimal(18,6),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE Objetivo SET
		Name = @Name,
		[Description] = @Description,
		ResponsibleId = @ResponsibleId,
		StartDate = @StartDate,
		Methodology = @Methodology,
		Resources = @Resources,
		Notes = @Notes,
		PreviewEndDate = @PreviewEndDate,
		--EndDate = @EndDate,
		VinculatedToIndicator = @VinculatedToIndicator,
		IndicatorId = @IndicatorId,
		RevisionId = @RevisionId,
		--ResponsibleClose = @ResponsibleClose,
		MetaComparer = @MetaComparer,
		Meta = @Meta,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @ObjetivoId
	AND CompanyId  = @CompanyId

	DELETE FROM IndicadorObjetivo WHERe ObjetivoId = @ObjetivoId

	IF @IndicatorId IS NOT NULL
	BEGIN
		INSERT INTO IndicadorObjetivo (IndicadorId, ObjetivoId, CompanyId, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Active)
		VALUES
		(@IndicatorId, @ObjetivoId, @CompanyId, @ApplicationUserId, GETDATE(), @ApplicationUserId, GETDATE(), 1)
	END

END



GO

/****** Object:  StoredProcedure [dbo].[ObjetivoRegistro_Activate]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ObjetivoRegistro_Activate]
	@ObjetivoRegistroId int output,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].ObjetivoRegistro SET 
       [Active] = 1
      ,[ModifiedBy] = @ApplicationUserId
      ,[ModifiedOn] = GETDATE()
	WHERE
		Id = @ObjetivoRegistroId
	AND CompanyId = @CompanyId



END



GO

/****** Object:  StoredProcedure [dbo].[ObjetivoRegistro_GetActive]    Script Date: 24/10/2018 20:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [dbo].[ObjetivoRegistro_GetActive]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		OBR.Id,
		OBR.CompanyId,
		OBR.ObjetivoId,
		OBR.[Date],
		OBR.[Value],
		OBR.Comments,
		OBR.ResponsibleId,
		EMP.Name AS ResponsableFirstName,
		EMP.LastName AS ResponsableLastName,
		OBR.CreatedBy,
		CB.[Login] AS CreatedByName,
		OBR.CreatedOn,
		OBR.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		OBR.ModifiedOn,
		OBR.Active,
		OBR.Meta,
		OBR.MetaComparer
	FROM ObjetivoRegistro OBR WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = OBR.ResponsibleId
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = OBR.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = OBR.ModifiedBy

	WHERE
		OBR.CompanyId = @CompanyId
	AND OBR.Active = 1

END





GO

/****** Object:  StoredProcedure [dbo].[ObjetivoRegistro_GetAll]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [dbo].[ObjetivoRegistro_GetAll]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		OBR.Id,
		OBR.CompanyId,
		OBR.ObjetivoId,
		OBR.[Date],
		OBR.[Value],
		OBR.Comments,
		OBR.ResponsibleId,
		EMP.Name AS ResponsableFirstName,
		EMP.LastName AS ResponsableLastName,
		OBR.CreatedBy,
		CB.[Login] AS CreatedByName,
		OBR.CreatedOn,
		OBR.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		OBR.ModifiedOn,
		OBR.Active,
		OBR.Meta,
		OBR.MetaComparer
	FROM ObjetivoRegistro OBR WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = OBR.ResponsibleId
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = OBR.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = OBR.ModifiedBy

	WHERE
		OBR.CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [issususer].[ObjetivoRegistro_GetAll]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [issususer].[ObjetivoRegistro_GetAll]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		OBR.Id,
		OBR.CompanyId,
		OBR.ObjetivoId,
		OBR.Fecha,
		OBR.Valor,
		OBR.Comentari,
		OBR.ResponsableId,
		EMP.Name AS ResponsableFirstName,
		EMP.LastName AS ResponsableLastName,
		OBR.CreatedBy,
		CB.[Login] AS CreatedByName,
		OBR.CreatedOn,
		OBR.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		OBR.ModifiedOn,
		OBR.Active
	FROM ObjetivoRegistro OBR WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = OBR.ResponsableId
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = OBR.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = OBR.ModifiedBy

	WHERE
		OBR.CompanyId = @CompanyId

END
GO

/****** Object:  StoredProcedure [dbo].[ObjetivoRegistro_GetByObjetivo]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [dbo].[ObjetivoRegistro_GetByObjetivo]
	@CompanyId int,
	@ObjetivoId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		OBR.Id,
		OBR.CompanyId,
		OBR.ObjetivoId,
		OBR.[Date],
		OBR.Value,
		OBR.Comments,
		OBR.ResponsibleId,
		EMP.Name AS ResponsableFirstName,
		EMP.LastName AS ResponsableLastName,
		OBR.CreatedBy,
		CB.[Login] AS CreatedByName,
		OBR.CreatedOn,
		OBR.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		OBR.ModifiedOn,
		OBR.Active,
		OBR.Meta,
		OBR.MetaComparer
	FROM ObjetivoRegistro OBR WITH(NOLOCK)
	INNER JOIN Objetivo OB WITH(NOLOCK)
	ON	OB.Id = OBR.ObjetivoId
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = OBR.ResponsibleId
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = OBR.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = OBR.ModifiedBy

	WHERE
		OBR.ObjetivoId = @ObjetivoId
	AND OBR.Active = 1
	--AND	OBR.CompanyId = @CompanyId
	--AND OBR.[Date] >= StartDate
END



GO

/****** Object:  StoredProcedure [dbo].[ObjetivoRegistro_Inactivate]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ObjetivoRegistro_Inactivate]
	@ObjetivoRegistroId int output,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].ObjetivoRegistro SET 
       [Active] = 0
      ,[ModifiedBy] = @ApplicationUserId
      ,[ModifiedOn] = GETDATE()
	WHERE
		Id = @ObjetivoRegistroId
	AND CompanyId = @CompanyId



END



GO

/****** Object:  StoredProcedure [dbo].[ObjetivoRegistro_Insert]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ObjetivoRegistro_Insert]
	@ObjetivoRegistroId int output,
	@CompanyId int,
	@ObjetivoId int,
	@Fecha datetime,
	@Valor decimal (18,6),
	@Meta decimal (18,6),
	@MetaComparer nvarchar(10),
	@Comentari nvarchar(500),
	@ResponsibleId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].ObjetivoRegistro
	(
		[CompanyId],
		[ObjetivoId],
		[Date],
		[Value],
		[MetaComparer],
		[Meta],
		[Comments],
		[ResponsibleId],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	VALUES
	(
		@CompanyId,
		@ObjetivoId,
		@Fecha,
		@Valor,
		@MetaComparer,
		@Meta,
		@Comentari,
		@ResponsibleId,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		1
	)

	SET @ObjetivoRegistroId = @@IDENTITY



END



GO

/****** Object:  StoredProcedure [dbo].[ObjetivoRegistro_Update]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ObjetivoRegistro_Update]
	@ObjetivoRegistroId int output,
	@CompanyId int,
	@ObjetivoId int,
	@Fecha datetime,
	@Valor decimal (18,6),
	@MetaComparer nvarchar(10),
	@Meta decimal (18,6),
	@Comentari nvarchar(500),
	@ResponsibleId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].ObjetivoRegistro SET 
       [ObjetivoId] = @ObjetivoId
      ,[Date] = @Fecha
      ,[Value] = @Valor
	  ,[MetaComparer] = @MetaComparer
	  ,[Meta] = @Meta
      ,[Comments] = @Comentari
      ,[ResponsibleId] = @ResponsibleId
      ,[ModifiedBy] = @ApplicationUserId
      ,[ModifiedOn] = GETDATE()
	WHERE
		Id = @ObjetivoRegistroId
	AND CompanyId = @CompanyId



END



GO

/****** Object:  StoredProcedure [dbo].[ProbabilitySeverityRange_Activate]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ProbabilitySeverityRange_Activate]
	@Id bigint,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [dbo].[ProbabilitySeverityRange]
	SET
		Active = 1,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @Id
	AND	CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		5,
		@Id,
		3,
		GETDATE(),
		@Reason
    )
	
	
END



GO

/****** Object:  StoredProcedure [dbo].[ProbabilitySeverityRange_Delete]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ProbabilitySeverityRange_Delete]
	@Id bigint,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [dbo].[ProbabilitySeverityRange]
	SET
		Active = 0,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @Id
	AND	CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		5,
		@Id,
		3,
		GETDATE(),
		@Reason
    )
	
	
END



GO

/****** Object:  StoredProcedure [dbo].[ProbabilitySeverityRange_GetActive]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ProbabilitySeverityRange_GetActive]
	@CompanyId int
AS
	SELECT
	P.Id,
	P.Description,
	P.Code,
	P.Type,
	P.CreatedOn,
	P.CreatedBy,
	CB.Login As CreatedByName,
	P.ModifiedOn,
	P.ModifiedBy,
	MB.Login As ModifiedByName,
	P.Active
	From ProbabilitySeverityRange P With (Nolock)
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = P.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = P.ModifiedBy

	Where P.CompanyId = @CompanyId and P.Active = 1
	Order By P.Type, P.Code
RETURN 0



GO

/****** Object:  StoredProcedure [dbo].[ProbabilitySeverityRange_GetAll]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ProbabilitySeverityRange_GetAll]
	@CompanyId int
AS
	SELECT
	P.Id,
	P.Description,
	P.Code,
	P.Type,
	P.CreatedOn,
	P.CreatedBy,
	CB.Login As CreatedByName,
	P.ModifiedOn,
	P.ModifiedBy,
	MB.Login As ModifiedByName,
	P.Active
	From ProbabilitySeverityRange P With (Nolock)
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = P.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = P.ModifiedBy

	Where P.CompanyId = @CompanyId
	Order By P.Type, P.Code
RETURN 0



GO

/****** Object:  StoredProcedure [dbo].[ProbabilitySeverityRange_GetBAR]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ProbabilitySeverityRange_GetBAR]
	@CompanyId int
AS
	SELECT Distinct
	P.Id,
	P.Description,
	Case when R.Limit Is Null Then CAST(1 AS BIT) Else CAST(0 AS BIT) end As Deletable

	From ProbabilitySeverityRange P With (Nolock) 
	Left Join Rules R With (Nolock)
	On R.Limit = P.Id

	Where P.CompanyId = @CompanyId And P.Active = 1
	Order By P.Description
RETURN 0



GO

/****** Object:  StoredProcedure [dbo].[ProbabilitySeverityRange_GetById]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ProbabilitySeverityRange_GetById]
	@CompanyId int,
	@Id bigint
AS
	SELECT
	P.Id,
	P.Description,
	P.Code,
	P.Type,
	P.CreatedOn,
	P.CreatedBy,
	CB.Login As CreatedByName,
	P.ModifiedOn,
	P.ModifiedBy,
	MB.Login As ModifiedByName,
	P.Active
	From ProbabilitySeverityRange P With (Nolock)
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = P.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = P.ModifiedBy

	Where P.CompanyId = @CompanyId and P.Id = @Id
	Order By P.Type, P.Code
RETURN 0



GO

/****** Object:  StoredProcedure [dbo].[ProbabilitySeverityRange_Insert]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[ProbabilitySeverityRange_Insert]
	@Id bigint out,
	@CompanyId int,
	@Description nvarchar(50),
	@Code int,
	@Type int,
	@UserId int
AS
BEGIN
	INSERT INTO [dbo].[ProbabilitySeverityRange]
	(
           CompanyId,
           Description,
           Code,
           Type,
           CreatedBy,
           CreatedOn,
           ModifiedBy,
           ModifiedOn,
           Active
	)
    VALUES
    (
		@CompanyId,
		@Description,
		@Code,
		@Type,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE(),
		1
	)
		   
	SET @Id = @@IDENTITY
END





GO

/****** Object:  StoredProcedure [dbo].[ProbabilitySeverityRange_Update]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ProbabilitySeverityRange_Update]
	@Id int,
	@CompanyId int,
	@Description nvarchar(50),
	@Code int,
	@Type int,
	@UserId int
AS
BEGIN
	UPDATE [dbo].[ProbabilitySeverityRange]
	SET Description = @Description,
		Code = @Code,
		Type = @Type,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @Id
	AND CompanyId = @CompanyId

END





GO

/****** Object:  StoredProcedure [dbo].[Process_Desactive]    Script Date: 24/10/2018 20:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[Process_Desactive]
	@Id int out,
	@CompanyId int,
	@UserId int
AS
BEGIN
	UPDATE Proceso SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id
	AND	CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Process_GetById]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Process_GetById]
	@Id bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
    
	SELECT 
		P.Id,
		P.CompanyId,
		P.Type,
		ISNULL(P.Inicio,'') AS Inicio,
		ISNULL(P.Desarrollo,'') AS Desarrollo,
		ISNULL(P.Fin,'') AS Fin,
		ISNULL(P.Description,'') AS Description,
		P.CargoId,
		P.ModifiedOn,
		P.ModifiedBy AS ModifiedByUserId,
		UA.Login AS ModifiedByUserName,
		P.Active AS Active,
		1 AS Deletable
	FROM Proceso P WITH(NOLOCK)
	INNER JOIN ApplicationUser UA WITH(NOLOCK)
	ON	UA.Id = P.ModifiedBy
	
	WHERE
		P.Id = @Id
	AND	P.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Process_Insert]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[Process_Insert]
	@Id int out,
	@CompanyId int,
	@JobPositionId int,
	@Description nvarchar(150),
	@Type int,
	@Start nvarchar(2000),
	@Work nvarchar(2000),
	@End nvarchar(2000),
	@UserId int
AS
BEGIN
	INSERT INTO Proceso
	(
		CompanyId,
		CargoId,
		Type,
		Inicio,
		Desarrollo,
		Fin,
		Description,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn,
		Active
	)
	VALUES
	(
		@CompanyId,
		@JobPositionId,
		@Type,
		@Start,
		@Work,
		@End,
		@Description,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE(),
		1
	)

	SET @Id = @@IDENTITY
END





GO

/****** Object:  StoredProcedure [dbo].[Process_Update]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Process_Update]
	@Id int,
	@Description nvarchar(150),
	@CompanyId int,
	@JobPositionId int,
	@Type int,
	@Start nvarchar(2000),
	@Work nvarchar(2000),
	@End nvarchar(2000),
	@UserId int
AS
BEGIN
	UPDATE Proceso SET
		CargoId = @JobPositionId,
		Type = @Type,
		Inicio = @Start,
		Desarrollo = @Work,
		Fin = @End,
		Description = @Description,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @Id
	AND CompanyId = @CompanyId

END





GO

/****** Object:  StoredProcedure [dbo].[ProcessType_GetByCompany]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ProcessType_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT
		J.Id,
		J.CompanyId,
		J.Description,
		J.Active,
		J.ModifiedOn,
		J.ModifiedBy AS ModifiedByUserId,
		J.ModifiedBy AS ModifiedByEmployeeId,
		AU.[Login] AS ModifiedByName,
		'' AS ModifiedByLastName,
		P1.Type
	FROM ProcessType J WITH(NOLOCK)
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.id = J.ModifiedBy
	LEFT JOIN 
	(
		SELECT P.Type FROM Proceso P WITH(NOLOCK)
	) P1
	ON P1.Type = J.Id
	
	WHERE
		J.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[ProcessType_Insert]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ProcessType_Insert]
	@ProcessTypeId int out,
	@CompanyId int,
	@Description nvarchar(50),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO ProcessType
    (
		CompanyId,
		Description,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@Description,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)
	
	SET @ProcessTypeId = @@IDENTITY	

END





GO

/****** Object:  StoredProcedure [dbo].[ProcessType_Update]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[ProcessType_Update]
	@ProcessTypeId int,
	@CompanyId int,
	@Description nvarchar(50),
	@Active bit,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE ProcessType SET
		Description = @Description,
		Active = @Active,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @ProcessTypeId
	AND CompanyId = @CompanyId

END





GO

/****** Object:  StoredProcedure [dbo].[Provider_Delete]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Provider_Delete]
	@ProviderId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE Provider SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @ProviderId
	AND	CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		21,
		@ProviderId,
		2,
		GETDATE(),
		''
    )

END





GO

/****** Object:  StoredProcedure [dbo].[Provider_GetByCompany]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Provider_GetByCompany]
	@CompanyId int
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT
		J.Id,
		J.CompanyId,
		J.Description,
		J.Active,
		J.ModifiedOn,
		J.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByName,
		--'' AS ModifiedByLastName,
		CASE WHEN ECA.Id IS NULL THEN 0 ELSE 1 END AS InCalibrationAct,
		CASE WHEN ECD.Id IS NULL THEN 0 ELSE 1 END AS InCalibrationDefinition,
		CASE WHEN EMA.Id IS NULL THEN 0 ELSE 1 END AS InMaintenanceAct,
		CASE WHEN EMD.Id IS NULL THEN 0 ELSE 1 END AS InMaintenanceDefinition,
		CASE WHEN R.Id IS NULL THEN 0 ELSE 1 END AS InRepair,
		CASE WHEN I.Id IS NULL THEN 0 ELSE 1 END AS InIncident,
		CASE WHEN IA.Id IS NULL THEN 0 ELSE 1 END AS InIncidentAction		
	FROM Provider J WITH(NOLOCK)
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = J.ModifiedBy
	LEFT JOIN EquipmentCalibrationAct ECA WITH(NOLOCK)
	ON	ECA.ProviderId = J.Id
	AND	ECA.Active = 1
	LEFT JOIN EquipmentCalibrationDefinition ECD WITH(NOLOCK)
	ON	ECD.ProviderId = J.Id
	AND ECD.Active = 1
	LEFT JOIN EquipmentMaintenanceAct EMA WITH(NOLOCK)
	ON	EMA.ProviderId = J.Id
	AND	EMA.Active = 1
	LEFT JOIN EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
	ON	EMD.ProviderId = J.Id
	AND EMD.Active = 1
	LEFT JOIN EquipmentRepair R WITH(NOLOCK)
	ON	R.ProviderId = J.Id
	AND R.Active = 1
	LEFT JOIN Incident I WITH(NOLOCK)
	ON	I.ProviderId = J.Id
	AND I.Active = 1
	LEFT JOIN IncidentAction IA WITH(NOLOCK)
	ON	IA.ProviderId = J.Id
	AND	IA.Active = 1
	
	WHERE
		J.CompanyId = @CompanyId

	ORDER BY
		J.Description ASC
END





GO

/****** Object:  StoredProcedure [dbo].[Provider_GetById]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Provider_GetById]
	@ProviderId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT
		J.Id,
		J.CompanyId,
		J.Description,
		J.Active,
		J.ModifiedOn,
		J.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		CASE WHEN ECA.Id IS NULL THEN 0 ELSE 1 END AS InCalibrationAct,
		CASE WHEN ECD.Id IS NULL THEN 0 ELSE 1 END AS InCalibrationDefinition,
		CASE WHEN EMA.Id IS NULL THEN 0 ELSE 1 END AS InMaintenanceAct,
		CASE WHEN EMD.Id IS NULL THEN 0 ELSE 1 END AS InMaintenanceDefinition,
		CASE WHEN R.Id IS NULL THEN 0 ELSE 1 END AS InRepair,
		CASE WHEN I.Id IS NULL THEN 0 ELSE 1 END AS InIncident,
		CASE WHEN IA.Id IS NULL THEN 0 ELSE 1 END AS InIncidentAction	
	FROM Provider J WITH(NOLOCK)
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = J.ModifiedBy
	LEFT JOIN EquipmentCalibrationAct ECA WITH(NOLOCK)
	ON	ECA.ProviderId = J.Id
	AND	ECA.Active = 1
	LEFT JOIN EquipmentCalibrationDefinition ECD WITH(NOLOCK)
	ON	ECD.ProviderId = J.Id
	AND ECD.Active = 1
	LEFT JOIN EquipmentMaintenanceAct EMA WITH(NOLOCK)
	ON	EMA.ProviderId = J.Id
	AND	EMA.Active = 1
	LEFT JOIN EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
	ON	EMD.ProviderId = J.Id
	AND EMD.Active = 1
	LEFT JOIN EquipmentRepair R WITH(NOLOCK)
	ON	R.ProviderId = J.Id
	AND R.Active = 1
	LEFT JOIN Incident I WITH(NOLOCK)
	ON	I.ProviderId = J.Id
	AND I.Active = 1
	LEFT JOIN IncidentAction IA WITH(NOLOCK)
	ON	IA.ProviderId = J.Id
	AND	IA.Active = 1
	
	WHERE
		J.Id = @ProviderId
	AND	J.CompanyId = @CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Provider_GetCosts]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Provider_GetCosts]
	@ProviderId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		TOTAL.Id,
		TOTAL.Item,
		TOTAL.Date,
		TOTAL.Operation,
		TOTAL.Cost,
		TOTAL.Responsable,
		EMP.Name,
		EMP.LastName,
		TOTAL.Active,
		TOTAL.EquipmentId,
		E.Description AS EquipmentDescription,
		E.Code
	FROM
	(
		SELECT
			ECA.Id,
			ECA.CompanyId,
			ECA.Date,
			'Calibration' AS Item,
			ECA.EquipmentCalibrationType AS Type,
			ECA.Operation,
			ECA.Responsable,
			ECA.Cost,
			ECA.Active,
			ECA.EquipmentId
		FROM EquipmentCalibrationAct ECA WITH(NOLOCK)
		WHERE 
			ECA.ProviderId = @ProviderId
		AND ECA.CompanyId = @CompanyId
		AND ECA.Active = 1
		
		UNION	

		SELECT
			EVA.Id,
			EVA.CompanyId,
			EVA.Date,
			'Verification' AS Item,
			EVA.EquipmentVerificationType AS Type,
			EVA.Operation,
			EVA.Responsable,
			EVA.Cost,
			EVA.Active,
			EVA.EquipmentId
		FROM EquipmentVerificationAct EVA WITH(NOLOCK)
		WHERE 
			EVA.ProviderId = @ProviderId
		AND EVA.CompanyId = @CompanyId
		AND EVA.Active = 1
		
		UNION	
		
		SELECT
			EMA.Id,
			EMA.CompanyId,
			EMA.Date,
			'Maintenance' AS Item,
			EMD.Type,
			EMA.Operation AS Operation,
			EMA.ResponsableId,
			EMA.Cost,
			EMA.Active,
			EMA.EquipmentId
		FROM EquipmentMaintenanceAct EMA WITH(NOLOCK)
		INNER JOIN EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
		ON	EMD.Id = EMA.EquipmentMaintenanceDefinitionId
		AND EMD.CompanyId = EMA.CompanyId
		WHERE
			EMA.ProviderId = @ProviderId
		AND EMA.CompanyId = @CompanyId
		AND EMA.Active = 1
		
		UNION	
		
		SELECT
			R.Id,
			R.CompanyId,
			R.Date,
			'Repair' AS Item,
			R.RepairType AS Type,
			CAST(R.Description AS nvarchar(50)) AS Operation,
			R.ResponsableId,
			R.Cost,
			R.Active,
			R.EquipmentId
		FROM EquipmentRepair R WITH(NOLOCK)
		WHERE 
			R.ProviderId = @ProviderId
		AND R.CompanyId = @CompanyId
		AND R.Active = 1
	) TOTAL
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	TOTAL.Responsable = EMP.Id
	AND	TOTAL.CompanyId = EMP.CompanyId
	INNER JOIN Equipment E WITH(NOLOCK)
	ON	TOTAL.EquipmentId = E.Id
	AND TOTAL.CompanyId = E.CompanyId
	
	ORDER BY TOTAL.Date DESC
END





GO

/****** Object:  StoredProcedure [dbo].[Provider_GetDefinitions]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Provider_GetDefinitions]
	@ProviderId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		TOTAL.Id,
		TOTAL.Item,
		TOTAL.Periodicity,
		TOTAL.Operation,
		TOTAL.Cost,
		TOTAL.Responsable,
		EMP.Name,
		EMP.LastName,
		TOTAL.Active,
		TOTAL.EquipmentId,
		E.Description AS EquipmentDescription,
		E.Code 
	FROM
	(
		SELECT
			ECD.Id,
			ECD.CompanyId,
			ECD.Periodicity,
			'Calibration' AS Item,
			ECD.Operation,
			ECD.Responsable,
			ECD.Cost,
			ECD.Active,
			ECD.EquipmentId
		FROM EquipmentCalibrationDefinition ECD WITH(NOLOCK)
		WHERE 
			ECD.ProviderId = @ProviderId
		AND ECD.CompanyId = @CompanyId
		AND ECD.Active = 1
		
		UNION	
		
		SELECT
			EVD.Id,
			EVD.CompanyId,
			EVD.Periodicity,
			'Verification' AS Item,
			EVD.Operation,
			EVD.Responsable,
			EVD.Cost,
			EVD.Active,
			EVD.EquipmentId
		FROM EquipmentVerificationDefinition EVD WITH(NOLOCK)
		WHERE 
			EVD.ProviderId = @ProviderId
		AND EVD.CompanyId = @CompanyId
		AND EVD.Active = 1
		
		UNION	
		
		SELECT
			EMD.Id,
			EMD.CompanyId,
			EMD.Periodicity,
			'Maintenance' AS Item,
			EMD.Operation AS Operation,
			EMD.ResponsableId,
			EMD.Cost,
			EMD.Active,
			EMD.EquipmentId
		FROM EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
		WHERE
			EMD.ProviderId = @ProviderId
		AND EMD.CompanyId = @CompanyId
		AND EMD.Active = 1
	) TOTAL
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	TOTAL.Responsable = EMP.Id
	AND	TOTAL.CompanyId = EMP.CompanyId
	INNER JOIN Equipment E WITH(NOLOCK)
	ON	TOTAL.EquipmentId = E.Id
	AND TOTAL.CompanyId = E.CompanyId
END





GO

/****** Object:  StoredProcedure [dbo].[Provider_GetIncidentActions]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Provider_GetIncidentActions]
	@ProviderId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		Item.*
	FROM
	(
		SELECT
			'Incident' AS ItemType,
			I.Id,
			I.Description,
			I.WhatHappendOn,
			I.CausesOn,
			I.ActionsOn,
			I.ClosedOn,
			-1 AS Origin,
			ISNULL(IA.Id,'') AS AssociantedId,
			ISNULL(IA.Description,'') AS AssociatedDescription,
			I.Code AS IncidentCode,
			ISNULL(IA.Number,0) AS ActionCode
		FROM Incident I WITH(NOLOCK)
		LEFT JOIN IncidentAction IA WITH(NOLOCK)
		ON	IA.IncidentId = I.Id
		AND	IA.CompanyId = I.CompanyId
		AND IA.Active = 1
		WHERE
			I.ProviderId = @ProviderId
		AND	I.CompanyId = @CompanyId
		AND I.Active = 1
		
		UNION
		
		
		SELECT
			'Action' AS ItemType,
			IA.Id,
			IA.Description,
			IA.WhatHappendOn,
			IA.CausesOn,
			IA.ActionsOn,
			IA.ClosedOn,
			IA.Origin,
			ISNULL(I.Id,0) AS AssociantedId,
			ISNULL(I.Description,'') AS AssociatedDescription,
			ISNULL(I.Code,0) AS IncidentCode,
			IA.Number AS ActionCode
		FROM IncidentAction IA WITH(NOLOCK)
		LEFT JOIN Incident I WITH(NOLOCK)
		ON	I.Id = IA.IncidentId
		AND I.CompanyId = Ia.CompanyId
		WHERE
			IA.ProviderId = @ProviderId
		AND	IA.CompanyId = @CompanyId
		AND IA.Active = 1
	) AS Item
	
END





GO

/****** Object:  StoredProcedure [dbo].[Provider_Insert]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Provider_Insert]
	@ProviderId bigint output,
	@CompanyId int,
	@Description nvarchar(100),
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Provider
	(
		CompanyId,
		Description,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@Description,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)
	
	SET @ProviderId = @@IDENTITY
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		21,
		@ProviderId,
		1,
		GETDATE(),
		@Description
    )

END





GO

/****** Object:  StoredProcedure [dbo].[Provider_Update]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Provider_Update]
	@ProviderId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@UserId int
AS
BEGIN



    UPDATE Provider SET
		Description = @Description,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @ProviderId
	AND	CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		21,
		@ProviderId,
		2,
		GETDATE(),
		@Description
    )

END





GO

/****** Object:  StoredProcedure [dbo].[Revision_Activate]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Revision_Activate]
	@Id int,
    @ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [dbo].[Revision] SET		
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn]= GETDATE(),
		[Active] = 1
	WHERE
		Id = @ID
END


GO

/****** Object:  StoredProcedure [dbo].[Revision_GeById]    Script Date: 24/10/2018 20:20:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Revision_GeById]
	@RevisionId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		R.Id,
		R.Type,
		R.MonthDay,
		R.MonthDayOrder,
		R.MonthDayWeek,
		R.WeekDays,
		R.DaysPeriode,
		R.Laboral,
		R.CreatedBy,
		CB.[Login] AS CreatedByName,
		R.CreatedOn,
		R.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		R.ModifiedOn,
		R.Active
	FROM Revision R WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = R.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = R.ModifiedBy

	WHERE
		R.Id = @RevisionId
END


GO

/****** Object:  StoredProcedure [dbo].[Revision_GetById]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Revision_GetById]
	@RevisionId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	/*SELECT
		R.Id,
		R.RevisionType,
		R.RevisionMonthSubtype,
		R.PeriodeNumber,
		R.WeekDays,
		R.CreatedBy,
		CB.[Login] AS CreatedByName,
		R.CreatedOn,
		R.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		R.ModifiedOn,
		R.Active
	FROM Revision R WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = R.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = R.ModifiedBy*/



END



GO

/****** Object:  StoredProcedure [dbo].[Revision_Inactivate]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Revision_Inactivate]
	@Id int,
    @ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [dbo].[Revision] SET		
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn]= GETDATE(),
		[Active] = 0
	WHERE
		Id = @ID
END


GO

/****** Object:  StoredProcedure [dbo].[Revision_Insert]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Revision_Insert]
	@Id int output,
	@Type int,
    @MonthDay int,
    @MonthDayOrder int,
    @MonthDayWeek int,
    @WeekDays nvarchar(7),
    @DaysPeriode int,
    @Laboral bit,
    @ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Revision]
	(
		[Type],
		[MonthDay],
		[MonthDayOrder],
		[MonthDayWeek],
		[WeekDays],
		[DaysPeriode],
		[Laboral],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	VALUES
	(
		@Type,
        @MonthDay,
        @MonthDayOrder,
        @MonthDayWeek,
        @WeekDays,
        @DaysPeriode,
        @Laboral,
        @ApplicationUserId,
        GETDATE(),
        @ApplicationUserId,
        GETDATE(),
		1
	)

	SET @Id = @@IDENTITY
END


GO

/****** Object:  StoredProcedure [dbo].[Revision_Update]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Revision_Update]
	@Id int,
	@Type int,
    @MonthDay int,
    @MonthDayOrder int,
    @MonthDayWeek int,
    @WeekDays nvarchar(7),
    @DaysPeriode int,
    @Laboral bit,
    @ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [dbo].[Revision] SET
		[Type] = @Type,
		[MonthDay] =@MonthDay,
		[MonthDayOrder] =@MonthDayOrder,
		[MonthDayWeek] = @MonthDayWeek,
		[WeekDays] = @WeekDays,
		[DaysPeriode] = @DaysPeriode,
		[Laboral] = @Laboral,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn]= GETDATE()
	WHERE
		Id = @ID
END


GO

/****** Object:  StoredProcedure [dbo].[Rules_Active]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Rules_Active]
	@RulesId int,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Rules SET
		Active = 1,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @RulesId
	AND	CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		5,
		@RulesId,
		3,
		GETDATE(),
		@Reason
    )
	
	
END



GO

/****** Object:  StoredProcedure [dbo].[Rules_Delete]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Rules_Delete]
	@RulesId int,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Rules SET
		Active = 0,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @RulesId
	AND	CompanyId = @CompanyId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		5,
		@RulesId,
		3,
		GETDATE(),
		@Reason
    )
	
	
END



GO

/****** Object:  StoredProcedure [dbo].[Rules_GetActive]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Rules_GetActive]
	@CompanyId int
AS
BEGIN
	SELECT DISTINCT
	R.Id,
	R.Description,
	R.Notes,
	R.Limit,
	R.CreatedOn,
	R.CreatedBy,
	CB.Login As CreatedByName,
	R.ModifiedOn,
	R.ModifiedBy,
	MB.Login As ModifiedByName,
	R.Active,
	CASE WHEN BR.Id IS NULL THEN 1 ELSE 0 END AS CanBeDeleted
	From Rules R With (Nolock) 
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = R.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = R.ModifiedBy
	LEFT JOIN BusinessRisk BR WITH(NOLOCK)
	ON	BR.RuleId = R.Id
				  

	WHERE
		R.CompanyId = @CompanyId 
	AND R.Active = 1

	ORDER BY 
		R.[Description]
END




GO

/****** Object:  StoredProcedure [dbo].[Rules_GetAll]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Rules_GetAll]
	@CompanyId int
AS
BEGIN
	SELECT DISTINCT
		R.Id,
		R.Description,
		R.Notes,
		R.Limit,
		R.CreatedOn,
		R.CreatedBy,
		CB.Login As CreatedByName,
		R.ModifiedOn,
		R.ModifiedBy,
		MB.Login As ModifiedByName,
		R.Active,
		CASE WHEN BR.Id IS NULL THEN 1 ELSE 0 END AS CanBeDeleted
	From Rules R With (Nolock) 
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = R.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = R.ModifiedBy
	LEFT JOIN BusinessRisk BR WITH(NOLOCK)
	ON	BR.RuleId = R.Id
				  

	WHERE
		R.CompanyId = @CompanyId
END



GO

/****** Object:  StoredProcedure [dbo].[Rules_GetBAR]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Rules_GetBAR]
	@CompanyId int
AS
	SELECT Distinct
	R.Id,
	R.Description,
	isNull(CAST(R.Notes As nvarchar(500)), '') As Notes,
	R.Limit,
	Case when B.RuleId Is Null Then CAST(1 AS BIT) Else CAST(0 AS BIT) end As Deletable

	From Rules R With (Nolock) 
	Left Join BusinessRisk B With (Nolock)
	On B.RuleId = R.Id

	Where R.CompanyId = @CompanyId And R.Active = 1
	Order By R.Description
RETURN 0



GO

/****** Object:  StoredProcedure [dbo].[Rules_GetById]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Rules_GetById]
	@CompanyId int,
	@Id bigint
AS
	SELECT DISTINCT
	R.Id,
	R.Description,
	R.Notes,
	R.Limit,
	R.CreatedOn,
	R.CreatedBy,
	CB.Login As CreatedByName,
	R.ModifiedOn,
	R.ModifiedBy,
	MB.Login As ModifiedByName,
	R.Active
	From Rules R With (Nolock) 
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = R.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = R.ModifiedBy

	Where R.CompanyId = @CompanyId And R.Id = @Id
RETURN 0



GO

/****** Object:  StoredProcedure [dbo].[Rules_Insert]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Rules_Insert]
	@Id bigint out,
	@CompanyId int,
	@Description nvarchar(50),
	@Notes nvarchar(500),
	@Limit int,
	@UserId int
AS
BEGIN
	INSERT INTO Rules
	(
		CompanyId,
		Description,
        Notes,
        Limit,
        CreatedBy,
        CreatedOn,
        ModifiedBy,
        ModifiedOn,
        Active
	)
    VALUES
	(
		@CompanyId,
        @Description,
        @Notes,
        @Limit,
        @UserId,
        GETDATE(),
        @UserId,
        GETDATE(),
        1
	)
	SET @Id = @@IDENTITY
END






GO

/****** Object:  StoredProcedure [dbo].[Rules_SetLimit]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Rules_SetLimit]
	@Id bigint out,
	@CompanyId int,
	@Limit int,
	@UserId int
AS
BEGIN
	UPDATE Rules SET 
		Limit = @Limit,
		ModifiedBy = @UserId, 
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id AND CompanyId = @CompanyId
END






GO

/****** Object:  StoredProcedure [dbo].[Rules_Update]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[Rules_Update]
	@Id bigint out,
	@CompanyId int,
	@Description nvarchar(50),
	@Notes nvarchar(500),
	@Limit int,
	@UserId int
AS
BEGIN
	UPDATE Rules SET 
		CompanyId = @CompanyId,
		Description = @Description,
		Notes = @Notes,
		Limit = @Limit,
		ModifiedBy = @UserId, 
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id AND CompanyId = @CompanyId
END






GO

/****** Object:  StoredProcedure [dbo].[ScheduleTask_GetByEmployee]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ScheduleTask_GetByEmployee] 
	@EmployeeId int,
	@CompanyId int
AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;

		SELECT
			TOTAL.OperationType,
			TOTAL.Id,
			TOTAL.ItemId,
			TOTAL.[Description],
			TOTAL.Vto,
			TOTAL.Operation,
			TOTAL.Code,
			TOTAL.Action,
			TOTAL.Responsable,
			TOTAL.ProviderId,
			E.Name AS EmployeeName,
			E.LastName AS EmployeeLastName,
			P.[Description] AS Provider,
			TOTAL.[Type]
		FROM
		(

		SELECT
			'C' AS OperationType,
			ECA.Id,
			E.Id AS ItemId,
			E.Code + '-' + E.[Description] AS Description,
			ECA.Vto,
			ECA.Operation,
			ECA.EquipmentCalibrationType AS Code,
			ECD.Id AS Action,
			ECD.Responsable,
			ECD.ProviderId,
			ECD.[Type]
		FROM EquipmentCalibrationAct ECA
		INNER JOIN Equipment E
		ON	E.Id = ECA.EquipmentId
		AND	E.Active = 1
		AND E.CompanyId = ECA.CompanyId
		AND ECA.Active = 1
		INNER JOIN EquipmentCalibrationDefinition ECD
		ON	ECD.EquipmentId = ECA.EquipmentId
		AND	ECD.Type = ECA.EquipmentCalibrationType
		AND ECD.Active = 1
		WHERE
			ECA.CompanyId = @CompanyId
		AND E.Active = 1
		AND E.EndDate IS NULL								   

		UNION 

		SELECT
			'V' AS OperationType,
			EVA.Id,
			E.Id AS ItemId,
			E.Code + '-' + E.[Description] AS Description,
			EVA.Vto,
			EVA.Operation,
			EVA.EquipmentVerificationType,
			EVD.Id AS Action,
			EVD.Responsable,
			EVD.ProviderId,
			EVD.VerificationType AS Type
		FROM EquipmentVerificationAct EVA
		INNER JOIN Equipment E
		ON	E.Id = EVA.EquipmentId
		AND	E.Active = 1
		AND E.CompanyId = EVA.CompanyId
		AND EVA.Active = 1
		INNER JOIN EquipmentVerificationDefinition EVD
		ON	EVD.EquipmentId = EVA.EquipmentId
		AND	EVD.VerificationType = EVA.EquipmentVerificationType
		AND EVD.Active = 1
		WHERE
			EVA.CompanyId = @CompanyId
		AND E.Active = 1
		AND E.EndDate IS NULL								   

		UNION 

		SELECT
			'M' AS OperationType,
			EMA.Id,
			E.Id AS ItemId,
			E.Code + '-' + E.[Description] AS Description,
			EMA.Vto,
			EMA.Operation,
			EMA.Id,
			EMD.Id AS Action,
			EMD.ResponsableId,
			EMA.ProviderId,
			EMD.[Type]
		FROM EquipmentMaintenanceAct EMA
		INNER JOIN Equipment E
		ON	E.Id = EMA.EquipmentId
		AND	E.Active = 1
		AND E.CompanyId = EMA.CompanyId
		AND EMA.Active = 1
		INNER JOIN EquipmentMaintenanceDefinition EMD
		ON  EMA.EquipmentMaintenanceDefinitionId = EMD.Id
		WHERE
			EMA.CompanyId = @CompanyId
		AND E.Active = 1
		AND E.EndDate IS NULL									 

		UNION

		SELECT
			'A' AS OperationType,
			IA.Id,
			IA.Id AS ItemId,
			IA.[Description],
			IA.ActionsSchedule,
			IA.[Description],
			0,
			0,
			IA.ActionsExecuter,
			0,
			0 AS Type
		FROM IncidentAction IA WITH(NOLOCK)
		WHERE
								
			ClosedOn IS NULL
		AND IA.Active = 1

		UNION

		SELECT
			'A' AS OperationType,
			IA.Id,
			IA.Id AS ItemId,
			IA.Description,
			IA.ActionsSchedule,
			IA.Description,
			0,
			0,
			IA.ClosedExecutor,
			0,
			0
		FROM IncidentAction IA WITH(NOLOCK)
		WHERE
							   
			ClosedExecutorOn IS NULL

		UNION 

		SELECT
			'I' AS OperationType,
			I.Id,
			I.Id AS ItemId,
			I.Description,
			I.ActionsSchedule,
			I.Description,
			0,
			0,
			I.ActionsExecuter,
			0,
			0 AS Type
		FROM Incident I WITH(NOLOCK)
		WHERE
								
			ClosedOn IS NULL

		) AS TOTAL

		INNER JOIN Employee E WITH(NOLOCK)
		ON	E.Id = TOTAL.Responsable

		LEFT JOIN Provider P WITH(NOLOCK)
		ON P.Id = TOTAL.ProviderId


		ORDER BY TOTAL.ItemId, TOTAL.OPeration
END
GO

/****** Object:  StoredProcedure [dbo].[Severity_GetAll]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Severity_GetAll] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		Id,
		[Description],
		Code
	FROM ProbabilitySeverityRange WITH(NOLOCK)
	WHERE
		Active = 1
	AND Type = 1

	ORDER BY Code
END




GO

/****** Object:  StoredProcedure [dbo].[Unidad_Activate]    Script Date: 24/10/2018 20:20:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Unidad_Activate]
	@UnidadId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Unidad SET
		Active = 1,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @UnidadId
	AND CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Unidad_GetActive]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Unidad_GetActive]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT
		U.Id,
		U.CompanyId,
		U.[Description],
		U.CreatedBy,
		CB.[Login] AS CreatedByName,
		U.CreatedOn,
		U.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		U.ModifiedOn,
		U.Active,
		CASE WHEN I.Id IS NULL THEN 1 ELSE 0 END
	FROM Unidad U WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = U.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = U.ModifiedBy
	LEFT JOIN Indicador I WITH(NOLOCK)
	ON	I.UnidadId = U.Id

	WHERE
		U.CompanyId = @CompanyId
	AND U.Active = 1

END



GO

/****** Object:  StoredProcedure [dbo].[Unidad_GetAll]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Unidad_GetAll]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT distinct
		U.Id,
		U.CompanyId,
		U.[Description],
		U.CreatedBy,
		CB.[Login] AS CreatedByName,
		U.CreatedOn,
		U.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		U.ModifiedOn,
		U.Active,
		CASE WHEN I.Id IS NULL THEN 1 ELSE 0 END AS Deletable
	FROM Unidad U WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = U.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = U.ModifiedBy
	LEFT JOIN Indicador I WITH(NOLOCK)
	ON	I.UnidadId = U.Id

	WHERE
		U.CompanyId = @CompanyId

	ORDER By U.[Description]

END



GO

/****** Object:  StoredProcedure [dbo].[Unidad_GetById]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Unidad_GetById]
	@UnidadId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		U.Id,
		U.CompanyId,
		U.[Description],
		U.CreatedBy,
		CB.[Login] AS CreatedByName,
		U.CreatedOn,
		U.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		U.ModifiedOn,
		U.Active,
		CASE WHEN I.Id IS NULL THEN 1 ELSE 0 END AS Deletable
	FROM Unidad U WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = U.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = U.ModifiedBy
	LEFT JOIN Indicador I WITH(NOLOCK)
	ON	I.UnidadId = U.Id

	WHERE
		U.Id = @UnidadId
	AND U.CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Unidad_Inactivate]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Unidad_Inactivate]
	@UnidadId int,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Unidad SET
		Active = 0,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @UnidadId
	AND CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[Unidad_Insert]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Unidad_Insert]
	@UnidadId int output,
	@Description nvarchar(50),
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[Unidad]
           ([CompanyId]
           ,[Description]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@CompanyId
           ,@Description
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

	SET @UnidadId = @@IDENTITY

END



GO

/****** Object:  StoredProcedure [dbo].[Unidad_Update]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Unidad_Update]
	@UnidadId int,
	@CompanyId int,
	@Description nvarchar(50),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Unidad SET
		[Description] = @Description,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @UnidadId
	AND CompanyId = @CompanyId

END



GO

/****** Object:  StoredProcedure [dbo].[UploadFiled_Inactive]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[UploadFiled_Inactive]
	@Id bigint,
	@CompanyId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM UploadFiles
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId
END


GO

/****** Object:  StoredProcedure [dbo].[UploadFiles_GetById]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UploadFiles_GetById]
	@Id bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		UF.Id,
		UF.CompanyId,
		UF.ItemLinked,
		UF.ItemId,
		UF.[FileName],
		UF.[Description],
		UF.Extension,
		UF.CreatedBy,
		CB.[Login],
		UF.CreatedOn,
		UF.ModifiedBy,
		MB.[Login],
		UF.ModifiedOn,
		UF.Active
	FROM UploadFiles UF WITH(NOLOCK)
	INNER JOIN ApplicationUser CB With (Nolock)
	On CB.Id = UF.CreatedBy
	INNER JOIN ApplicationUser MB With (Nolock)
	On MB.Id = UF.ModifiedBy
	WHERE
		UF.Id = @Id
END


GO

/****** Object:  StoredProcedure [dbo].[UploadFiles_GetByItem]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UploadFiles_GetByItem]
	@ItemLinked int,
	@ItemId bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		UF.Id,
		UF.CompanyId,
		UF.ItemLinked,
		UF.ItemId,
		UF.[FileName],
		UF.[Description],
		UF.Extension,
		UF.CreatedBy,
		CB.[Login],
		UF.CreatedOn,
		UF.ModifiedBy,
		MB.[Login],
		UF.ModifiedOn,
		UF.Active
	FROM UploadFiles UF WITH(NOLOCK)
	INNER JOIN ApplicationUser CB With (Nolock)
	On CB.Id = UF.CreatedBy
	INNER JOIN ApplicationUser MB With (Nolock)
	On MB.Id = UF.ModifiedBy
	WHERE
		ItemLinked = @ItemLinked
	AND ItemId = @ItemId
	AND UF.Active = 1
END


GO

/****** Object:  StoredProcedure [dbo].[UploadFiles_Insert]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Juan Castilla Calderón
-- Create date: 12/10/2016
-- Description:	Inserta un adjunto
-- =============================================
CREATE PROCEDURE [dbo].[UploadFiles_Insert]
	@Id bigint output,
	@CompanyId bigint,
	@ItemLinked int,
	@ItemId bigint,
	@FileName nvarchar(250),
	@Description nvarchar(100),
	@Extension nvarchar(10),
	@ApplicationUserId int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


INSERT INTO [dbo].[UploadFiles]
	(
		[CompanyId],
		[ItemLinked],
		[ItemId],
		[FileName],
		[Description],
		[Extension],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	VALUES
	(
		@CompanyId,
		@ItemLinked,
		@ItemId,
		@FileName,
		@Description,
		@Extension,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		1
	)

	SET @Id = @@IDENTITY
END


GO

/****** Object:  StoredProcedure [dbo].[User_GetByCompanyId]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[User_GetByCompanyId]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		AU.Id,
		AU.[Login],
		AU.[Status],
		AU.[Language],
		ISNULL(AU.Email,'') AS UserEmail,
		AUSGM.SecurityGroupId,
		ISNULL(E.Id,0) AS EmployeeId,
		ISNULL(E.Name,'') AS EmployeeName,
		ISNULL(E.LastName,'') AS EmployeeLastName,
		ISNULL(E.Email,'') AS EmployeeEmail,
		AU.PrimaryUser,
		AU.[Admin]
	FROM ApplicationUser AU WITH(NOLOCK)
	LEFT JOIN ApplicationUserSecurityGroupMembership AUSGM WITH(NOLOCK)
	ON	AU.Id = AUSGM.ApplicationUserId
	AND	AU.CompanyId = AUSGM.CompanyId
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	E.Id = EUA.EmployeeId
	ON	 EUA.UserId = AU.Id
		
	WHERE
		AU.CompanyId = @CompanyId
	AND AU.Status <> 0
		
	ORDER BY AU.[Login] ASC
END





GO

/****** Object:  StoredProcedure [dbo].[User_GetById]    Script Date: 24/10/2018 20:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[User_GetById]
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		AU.Id,
		AU.[Login],
		AU.[Status],
		AU.[Language],
		AUSGM.SecurityGroupId,
		ISNULL(E.Id, 0) AS EmployeeId,
		ISNULL(MSCG.Label,'') AS GreenLabel,
		ISNULL(MSCG.Icon,'') AS GreenIcon,
		ISNULL(MSCG.Link,'') AS GreenLink,
		ISNULL(MSCB.Label,'') AS BlueLabel,
		ISNULL(MSCB.Icon,'') AS BlueIcon,
		ISNULL(MSCB.Link,'') AS BlueLink,
		ISNULL(MSCR.Label,'') AS RedLabel,
		ISNULL(MSCR.Icon,'') AS RedIcon,
		ISNULL(MSCR.Link,'') AS RedLink,
		ISNULL(MSCY.Label,'') AS YellowLabel,
		ISNULL(MSCY.Icon,'') AS YellowIcon,
		ISNULL(MSCY.Link,'') AS YellowLink,
		ISNULL(E.Name, AU.[Login]) AS EmployeeName,
		ISNULL(E.LastName,'') AS EmployeeLastName,
		AU.ShowHelp,
		USC.ShorcutGreen AS GreenId,
		USC.ShorcutBlue AS BlueId,
		USC.ShortcutYellow AS YellowId,
		USC.ShortcutRed AS RedId,
		ISNULL(AU.Avatar,'avatar2.png') AS Avatar,
		AU.Email,
		ISNULL(AU.PrimaryUser,0) AS PrimaryUser,
		AU.CompanyId AS CompanyId,
		ISNULL(AU.[Admin],0)
	FROM ApplicationUser AU WITH(NOLOCK)
	LEFT JOIN ApplicationUserSecurityGroupMembership AUSGM WITH(NOLOCK)
	ON	AU.Id = AUSGM.ApplicationUserId
	AND	AU.CompanyId = AUSGM.CompanyId
	LEFT JOIN UserShortCuts USC WITH(NOLOCK)
	ON	USC.ApplicationUserId = AU.Id
	AND USC.CompanyId = AU.CompanyId
	LEFT JOIN MenuShortCuts MSCG WITH(NOLOCK)
	ON	MSCG.ID = USC.ShorcutGreen
	LEFT JOIN MenuShortCuts MSCB WITH(NOLOCK)
	ON	MSCB.ID = USC.ShorcutBlue
	LEFT JOIN MenuShortCuts MSCY WITH(NOLOCK)
	ON	MSCY.ID = USC.ShortcutYellow
	LEFT JOIN MenuShortCuts MSCR WITH(NOLOCK)
	ON	MSCR.ID = USC.ShortcutRed
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	EUA.EmployeeId = E.Id
	ON EUA.UserId = AU.Id
	
		
	WHERE
		AU.Id = @UserId;
END





GO

