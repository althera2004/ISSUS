

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IncidentActionZombie_Insert]
	@Id bigint output,
	@CompanyId int,
	@ActionType int,
	@Description nvarchar(100),
	@WhatHappend nvarchar(2000),
	@WhatHappendBy int,
	@WhatHappendOn datetime,
	@AuditoryId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[IncidentActionZombie]
           ([CompanyId]
           ,[ActionType]
           ,[Description]
           ,[WhatHappend]
           ,[WhatHappendBy]
           ,[WhatHappendOn]
           ,[AuditoryId])
     VALUES
           (@CompanyId
           ,@ActionType
           ,@Description
           ,@WhatHappend
           ,@WhatHappendBy
           ,@WhatHappendOn
           ,@AuditoryId)

	SET @Id = @@IDENTITY

END

