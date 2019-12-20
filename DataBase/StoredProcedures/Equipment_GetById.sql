





CREATE PROCEDURE [dbo].[Equipment_GetById]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
	E.Id,
	E.CompanyId,
	E.Code,
	E.Description,
	E.TradeMark,
	E.Model,
	E.SerialNumber,
	E.Location,
	E.MeasureRange,
	E.ScaleDivision,
	E.MeasureUnit,
	ISNULL(ESD.Description,''),
	E.Resposable,
	RESP.Name,
	RESP.LastName,
	E.IsCalibration,
	E.IsVerification,
	E.IsMaintenance,
	ISNULL(E.Notes,'') AS Notes,
	ISNULL(E.Image,'images/noimage.png') AS Image,
	E.Observations,
	E.ModifiedBy AS ModifiedByUserId,
	AU.[Login] AS ModifiedByUserName,
	E.ModifiedOn,
	E.StartDate,
	E.EndDate,
	E.EndResponsible,
	E.EndReason,
	ISNULL(ENDEMP.Name,'') AS EndResponsibleName,
	ISNULL(ENDEMP.LastName,'') AS EndResponsibleLastName
	FROM Equipment E WITH(NOLOCK)
	LEFt JOIN ApplicationUser AU WITH(NOLOCK)
	ON AU.Id = E.ModifiedBy
	INNER JOIN Employee RESP WITH(NOLOCK)
	ON RESP.Id = E.Resposable
	LEFT JOIN EquipmentScaleDivision ESD WITH(NOLOCK)
	ON	ESD.Id = E.MeasureUnit
	LEFT JOIN Employee ENDEMP WITH(NOLOCK)
	ON	 ENDEMP.Id = E.EndResponsible
	
	WHERE
		E.Id = @EquipmentId
	AND E.CompanyId = @CompanyId
END






