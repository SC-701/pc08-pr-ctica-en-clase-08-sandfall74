-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ObtenerProducto]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.[Id],
        P.[IdSubCategoria],
        P.[Nombre],
        P.[Descripcion],
        P.[Precio],
        P.[Stock],
        P.[CodigoBarras],
        S.[Nombre] AS SubCategoria,
        C.[Nombre] AS Categoria
    FROM [dbo].[Producto] AS P
    INNER JOIN [dbo].[SubCategorias] AS S ON P.[IdSubCategoria] = S.[Id]
    INNER JOIN [dbo].[Categorias] AS C ON S.[IdCategoria] = C.[Id]
    WHERE P.[Id] = @Id;
END