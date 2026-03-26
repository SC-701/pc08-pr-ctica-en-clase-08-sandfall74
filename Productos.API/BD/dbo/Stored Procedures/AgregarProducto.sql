CREATE PROCEDURE [dbo].[AgregarProducto]
    -- Parámetros que coinciden con las columnas de la tabla Producto
    @Id UNIQUEIDENTIFIER,
    @IdSubCategoria UNIQUEIDENTIFIER,
    @Nombre VARCHAR(MAX),
    @Descripcion VARCHAR(MAX),
    @Precio DECIMAL(18, 2),
    @Stock INT,
    @CodigoBarras VARCHAR(MAX)
AS
BEGIN
    -- SET NOCOUNT ON evita que se envíen mensajes de "filas afectadas" 
    -- que pueden interferir con el rendimiento o con aplicaciones cliente.
    SET NOCOUNT ON;

    begin transaction
    INSERT INTO [dbo].[Producto]
           ([Id]
           ,[IdSubCategoria]
           ,[Nombre]
           ,[Descripcion]
           ,[Precio]
           ,[Stock]
           ,[CodigoBarras])
     VALUES
           (@Id
           ,@IdSubCategoria
           ,@Nombre
           ,@Descripcion
           ,@Precio
           ,@Stock
           ,@CodigoBarras);
           select @Id as 'Id';
           commit transaction
END