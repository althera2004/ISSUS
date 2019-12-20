







CREATE PROCEDURE [dbo].[Indicator_Filter]
	@CompanyId int,
	@IndicadorType int,
	@DateFrom datetime,
	@DateTo datetime,
	@ProcessId int,
	@ProcessTypeId int,
	@ObjetivoId int,
	@Status int
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		Data.IndicadorId,
		Data.Description,
		Data.ProcessId,
		Data.ProcessDescription,
		Data.ProcessType,
		Data.ObjetivoId,
		Data.ObjetivoName,
		Data.ObjetivoResponsibleName,
		Data.ObjetivoResponsableLastName,
		Data.ProcesoResponsible,
		Data.StartDate,
		Data.EndDate,
		Data.MetaComparer,
		Data.Meta,
		Data.AlarmaComparer,
		Data.Alarma,
		IR.Value,
		IR.Date
	FROM
	(
		SELECT DISTINCT
			I.Id AS IndicadorId,
			I.Descripcion AS Description,
			ISNULL(I.ProcessId,0) AS ProcessId,
			ISNULL(PR.[Description],'') AS ProcessDescription,
			ISNULL(PR.[Type],0) AS ProcessType,
			CASE WHEN IO.ObjetivoId IS NULL THEN 0 ELSE 1 END AS ObjetivoId,
			'' AS ObjetivoName,
			ISNULL(OEMP.Name,'') AS ObjetivoResponsibleName,
			ISNULL(OEMP.LastName,'') AS ObjetivoResponsableLastName,
			ISNULL(C.[Description],'') AS ProcesoResponsible,
			I.StartDate AS StartDate,
			I.EndDate AS EndDate,
			I.MetaComparer AS MetaComparer,
			I.Meta AS Meta,
			I.AlarmaComparer AS AlarmaComparer,
			I.Alarma AS Alarma
		
		FROM Indicador I WITH(NOLOCK)
		LEFT JOIN Proceso PR WITH(NOLOCK)
			LEFT JOIN Cargos C WITH(NOLOCK)
			ON	C.Id = PR.CargoId
		ON	PR.Id = I.ProcessId
		AND PR.CompanyId = I.CompanyId
		AND I.ProcessId > 0
	
		LEFT JOIN Employee OEMP WITH(NOLOCK)
		ON	OEMP.Id = I.ResponsableId
		LEFT JOIN IndicadorObjetivo IO WITH(NOLOCK)
			INNER JOIN Objetivo O WITH(NOLOCK)
			ON	O.Id = IO.ObjetivoId
			AND O.Active = 1
		ON	IO.IndicadorId = I.Id
		AND IO.Active = 1



		WHERE
			I.CompanyId = @CompanyId
		AND I.Active = 1
		AND	(@DateFrom IS NULL OR I.StartDate >= @DateFrom)
		AND (@DateTo IS NULL OR I.StartDate <= @DateTo)
		AND
		(
			@ProcessId IS NULL OR PR.Id = @ProcessId OR @ProcessId = 0
		)
		AND
		(
			@ObjetivoId IS NULL OR IO.ObjetivoId = @ObjetivoId OR @ObjetivoId = 0
		)
		AND 
		(
			@ProcessTypeId IS NULL OR PR.[Type] = @ProcessTypeId
		)
		AND
		(
			@IndicadorType = 0
			OR
			(@IndicadorType = 1 AND PR.Id > 0)
			OR
			(@IndicadorType = 2 AND IO.ObjetivoId > 0)
		)
		AND
		(
			@Status = 0
			OR
			(I.EndDate > GETDATE() OR I.EndDate IS NULL AND @Status = 1)
			OR
			(I.EndDate < GETDATE() AND @Status = 2)
		)
	)
	AS DATA
	LEFT JOIN IndicadorRegistro IR
	ON	IR.IndicadorId = Data.IndicadorId
	AND IR.Active = 1

	ORDER BY Data.IndicadorId, IR.Date DESC
	
END








