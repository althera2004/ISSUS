




CREATE PROCEDURE [dbo].[CostImpactRange_GetBAR]
	@CompanyId int
AS
	SELECT Distinct
	P.Id,
	P.Description,
	Case when R.Limit Is Null Then CAST(1 AS BIT) Else CAST(0 AS BIT) end As Deletable

	From CostImpactRange P With (Nolock) 
	Left Join Rules R With (Nolock)
	On R.Limit = P.Id

	Where P.CompanyId = @CompanyId And P.Active = 1
	Order By P.Description
RETURN 0



