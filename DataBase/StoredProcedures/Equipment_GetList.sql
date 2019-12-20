





CREATE PROCEDURE [dbo].[Equipment_GetList]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT
		E.Id,
		E.Code,
		E.Description,
		ISNULL(E.Location,'') AS Location,
		E.Resposable AS ResponsableId,
		EMP.Name AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		E.IsCalibration,
		E.IsVerification,
		E.IsMaintenance,
		E.EndDate,
		CASE WHEN UF.Id IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS Adjuntos,
		ISNULL(Costes.Coste,0) AS Coste
    FROM Equipment E WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = E.Resposable
	LEFT JOIN UploadFiles UF WITH(NOLOCK)
	ON	UF.ItemId = E.Id
	AND UF.ItemLinked = 11
	AND UF.Active = 1
	LEFT JOIN 
	(
			SELECT
				SUM(Data.Cost) AS Coste,
				EquipmentId
			FROM
			(
				select
				 SUM(ISNULL(Cost,0)) AS Cost,
					EquipmentId
				FROM EquipmentCalibrationAct
				WHERE Active = 1
				GROUP BY EquipmentId

				UNION

				select
				 SUM(ISNULL(Cost,0)) AS Cost,
					EquipmentId
				FROM EquipmentVerificationAct
				WHERE Active = 1
				GROUP BY EquipmentId

				UNION

				select
				 SUM(ISNULL(Cost,0)) AS Cost,
					EquipmentId
				FROM EquipmentMaintenanceAct
				WHERE Active = 1
				GROUP BY EquipmentId

				UNION

				select
				 SUM(ISNULL(Cost,0)) AS Cost,
					EquipmentId
				FROM EquipmentRepair
				WHERE Active = 1
				GROUP BY EquipmentId
			) AS Data
			GROUP BY Data.EquipmentId
		) Costes
	ON Costes.EquipmentId = E.Id
    WHERE
		E.CompanyId = @CompanyId
	AND E.Active = 1
END






