




CREATE PROCEDURE [dbo].[Employee_GetImplications]
	@EmployeeId bigint,
	@CompanyId int
AS
BEGIN
	SELECT
		'E' AS ItemType,
		E.Id AS ItemId,
		E.Description AS ItemDescription,
		E.Resposable
	FROM Equipment E WITH(NOLOCK)
	WHERE
		E.active = 1
	AND E.CompanyId = @CompanyId
	AND E.Resposable = @EmployeeId

	UNION
	
	SELECT
		'ECD' AS ItemType,
		ECD.Id AS ItemId,
		ECD.Operation AS ItemDescription,
		ECD.Responsable
	FROM EquipmentCalibrationDefinition ECD WITH(NOLOCK)
	WHERE
		ECD.active = 1
	AND ECD.CompanyId = @CompanyId
	AND ECD.Responsable = @EmployeeId

	UNION
	
	SELECT
		'EVD' AS ItemType,
		EVD.Id AS ItemId,
		EVD.Operation AS ItemDescription,
		EVD.Responsable
	FROM EquipmentVerificationDefinition EVD WITH(NOLOCK)
	WHERE
		EVD.active = 1
	AND EVD.CompanyId = @CompanyId
	AND EVD.Responsable = @EmployeeId

	UNION
	
	SELECT
		'EMD' AS ItemType,
		EMD.Id AS ItemId,
		EMD.Operation AS ItemDescription,
		EMD.ResponsableId
	FROM EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
	WHERE
		EMD.active = 1
	AND EMD.CompanyId = @CompanyId
	AND EMD.ResponsableId = @EmployeeId	

	UNION
	
	SELECT
		'IAW' AS ItemType,
		IA.Id AS ItemId,
		IA.Description AS ItemDescription,
		IA.WhatHappendBy
	FROM IncidentAction IA WITH(NOLOCK)
	WHERE
		IA.active = 1
	AND IA.CompanyId = @CompanyId
	AND IA.WhatHappendBy = @EmployeeId

	UNION
	
	SELECT
		'IAC' AS ItemType,
		IA.Id AS ItemId,
		IA.Description AS ItemDescription,
		IA.WhatHappendBy
	FROM IncidentAction IA WITH(NOLOCK)
	WHERE
		IA.active = 1
	AND IA.CompanyId = @CompanyId
	AND IA.ActionsBy = @EmployeeId




END





