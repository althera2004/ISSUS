





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






