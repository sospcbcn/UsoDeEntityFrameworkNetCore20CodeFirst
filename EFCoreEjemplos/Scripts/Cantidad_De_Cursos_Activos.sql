SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================================================================================
-- Esta Función Recibirá EstudianteId como parámetro y comprobará todos los Cursos Asignados a ese Estudiante y que estén Activos
-- ===============================================================================================================================
CREATE FUNCTION [dbo].[Cantidad_De_Cursos_Activos]
(
	-- Add the parameters for the function here
	@EstudianteId int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result int

	-- Add the T-SQL statements to compute the return value here
	SELECT @result = count(*)
	from EstudiantesCursos
	where EstudianteId = @EstudianteId and Activo = 'true'

	-- Return the result of the function
	RETURN @result

END
GO


