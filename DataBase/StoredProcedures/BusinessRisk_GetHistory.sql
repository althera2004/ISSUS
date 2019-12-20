




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





