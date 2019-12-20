




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




