





CREATE PROCEDURE [dbo].[Application_GetItems]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
    *
    FROM ApplicationItem WITH(NOLOCK)
    ORDER BY Id
END






