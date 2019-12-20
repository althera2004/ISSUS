




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




