CREATE PROCEDURE [dbo].[EliminarProducto]
    -- Solo necesitamos el ID para identificar qué registro borrar
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    begin transaction
    DELETE FROM [dbo].[Producto]
    WHERE [Id] = @Id;
    select @Id as 'Id';
    commit transaction
END