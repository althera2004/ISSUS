
/****** Object:  StoredProcedure [dbo].[Auditory_Filter]    Script Date: 10/1/2020 13:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Se ha añadido un replace para evitar que las " deformen el json de respuesta.

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Auditory_Filter]
	@CompanyId int,
	@From datetime,
	@To datetime,
	@TypeInterna bit,
	@TypeExterna bit,
	@TypeProveedor bit,
	@Status0 bit,
	@Status1 bit,
	@Status2 bit,
	@Status3 bit,
	@Status4 bit,
	@Status5 bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		A.Id,
		REPLACE( A.[Nombre], '"', '\"') as Nombre, --A.Nombre,
		A.[Status],
		A.PlannedOn,
		A.ValidatedOn,
		A.Amount,
		A.[Type],
		A.Active
	FROM Auditory A WITH(NOLOCK)
	WHERE
		A.CompanyId = @CompanyId
	AND A.Active = 1
	AND 
	(
		(@TypeInterna = 1 AND A.[Type] = 0)
		OR
		(@TypeExterna = 1 AND A.[Type] = 1)
		OR
		(@TypeProveedor = 1 AND A.[Type] = 2)
	)
	AND 
	(
		(@Status0 = 1 AND A.[Status] = 0)
		OR
		(@Status1 = 1 AND A.[Status] = 1)
		OR
		(@Status2 = 1 AND A.[Status] = 2)
		OR
		(@Status3 = 1 AND A.[Status] = 3)
		OR
		(@Status4 = 1 AND A.[Status] = 4)
		OR
		(@Status5 = 1 AND A.[Status] = 5)
	)

END

