

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Oportunity_Insert]
	@Id int out,
	@CompanyId int,
	@Description nvarchar(100),
	@Code bigint,
	@ItemDescription nvarchar(2000),
	@StartControl nvarchar(2000),
	@Notes nvarchar(2000),
	@ApplyAction bit,
	@DateStart datetime,
	@Causes nvarchar(2000),
	@Cost int,
	@Impact int,
	@Result int,
	@ProcessId bigint,
	@RuleId bigint,
	@PreviousOportunityId bigint,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @Code < 1
	BEGIN
		SELECT @Code = ISNULL(MAX(Code) + 1, 1) FROM Oportunity WHERE CompanyId = @CompanyId
	END

    -- Insert statements for procedure here
	INSERT INTO [dbo].[Oportunity]
           ([CompanyId]
           ,[Description]
           ,[Code]
           ,[ItemDescription]
           ,[StartControl]
           ,[Notes]
           ,[ApplyAction]
           ,[DateStart]
           ,[Causes]
           ,[Cost]
           ,[Impact]
           ,[Result]
           ,[ProcessId]
           ,[RuleId]
		   ,[PreviousOportunityId]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@CompanyId
           ,@Description
           ,@Code
           ,@ItemDescription
           ,@StartControl
           ,@Notes
           ,@ApplyAction
           ,@DateStart
           ,@Causes
           ,@Cost
           ,@Impact
           ,@Result
           ,@ProcessId
           ,@RuleId
		   ,@PreviousOportunityId
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

	SET @Id = @@IDENTITY

END

