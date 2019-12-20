
CREATE PROCEDURE [dbo].[EquipmentMaintenanceDefinition_Insert]
	@EquipmentMaintenanceDefinitionId bigint output,
	@EquipmentId bigint,
	@CompanyId int,
	@Operation nvarchar(200),
	@Type int,
	@Periodicity int,
	@Accesories nvarchar(50),
	@Cost numeric(18,3),
	@UserId int,
	@FirstDate datetime
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO EquipmentMaintenanceDefinition
           ([EquipmentId]
           ,[CompanyId]
           ,[Operation]
           ,[Type]
           ,[Periodicity]
           ,[Accessories]
           ,[Cost]
		   ,[FirstDate]
           ,[Active]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn])
     VALUES
           (@EquipmentId
           ,@CompanyId
           ,@Operation
           ,@Type
           ,@Periodicity
           ,@Accesories
           ,@Cost
		   ,@FirstDate
           ,1
           ,@UserId
           ,GETDATE()
           ,@UserId
           ,GETDATE())
	
	SET @EquipmentMaintenanceDefinitionId = @@IDENTITY
	 
	 UPDATE Equipment SET
		IsMaintenance = 1
		WHERE
		Id = @EquipmentId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		16,
		@EquipmentMaintenanceDefinitionId,
		1,
		GETDATE(),
		@Operation
    )
END

