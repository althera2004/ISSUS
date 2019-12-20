

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IncidentActionZombie_Update]
	@Id bigint,
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
	UPDATE IncidentActionZombie SET
		[ActionType] = @ActionType,
		[Description] = @Description,
		[WhatHappend] = @WhatHappend,
		[WhatHappendBy] = @WhatHappendBy,
		[WhatHappendOn] = @WhatHappendOn
	WHERE
		Id = @Id
	AND AuditoryId = @AuditoryId
	AND CompanyId = @CompanyId
           


END

