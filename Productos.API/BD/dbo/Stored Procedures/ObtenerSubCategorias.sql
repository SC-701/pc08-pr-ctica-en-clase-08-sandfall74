create procedure ObtenerSubCategorias
	@IdCategoria uniqueidentifier
	as
	begin
	set nocount on;
SELECT [Id]
      ,[IdCategoria]
      ,[Nombre]
  FROM [dbo].[SubCategorias]
  where (IdCategoria= @IdCategoria)

end