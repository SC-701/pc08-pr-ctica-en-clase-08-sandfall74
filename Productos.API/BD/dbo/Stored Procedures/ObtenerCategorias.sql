create procedure ObtenerCategorias
AS
begin
set NOCOUNT on
SELECT [Id]
      ,[Nombre]
  FROM [dbo].[Categorias]

END