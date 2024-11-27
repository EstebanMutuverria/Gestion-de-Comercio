CREATE DATABASE COMERCIO_DB;

GO

USE COMERCIO_DB;

GO

------------------------------
----------- TABLAS -----------
------------------------------

CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario NVARCHAR(50) NOT NULL,
    Contrasena NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(20) CHECK (Rol IN ('Administrador', 'Vendedor')) NOT NULL,
    FotoPerfil NVARCHAR(255),
    Estado BIT DEFAULT 1
);


-- Insert Admin
INSERT INTO Usuario (NombreUsuario, Contrasena, Rol)
VALUES ('admin', 'admin', 'Administrador');

CREATE TABLE Cliente (
    IdCliente INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(15),
    Direccion NVARCHAR(200),
    DNI NVARCHAR(20),                  
    CUIT NVARCHAR(20),                  
    TipoPersona NVARCHAR(10) CHECK (TipoPersona IN ('Fisica', 'Juridica')),
    Estado BIT DEFAULT 1
);

CREATE TABLE Proveedor (
    IdProveedor INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(15),
    Direccion NVARCHAR(200),
    DNI NVARCHAR(20),                  
    CUIT NVARCHAR(20),                  
    TipoPersona NVARCHAR(10) CHECK (TipoPersona IN ('Fisica', 'Juridica')),
    Estado BIT DEFAULT 1
);

CREATE TABLE Marca (
    IdMarca INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL
);

CREATE TABLE Categoria (
    IdCategoria INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL
);

CREATE TABLE Producto (
    IdProducto INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    IdMarca INT FOREIGN KEY REFERENCES Marca(IdMarca),
    IdTipoProducto INT FOREIGN KEY REFERENCES Categoria(IdCategoria),
    StockActual INT NOT NULL,
    StockMinimo INT NOT NULL,
    PorcentajeGanancia DECIMAL(5,2) NOT NULL,
    FechaVencimiento DATE NULL,
    Precio Money NULL
);


CREATE TABLE Compra (
    IdCompra INT PRIMARY KEY IDENTITY(1,1),
    IdProveedor INT FOREIGN KEY REFERENCES Proveedor(IdProveedor),
    FechaCompra DATETIME NOT NULL,
    Estado BIT DEFAULT 1
);

CREATE TABLE DetalleCompra (
    IdDetalleCompra INT PRIMARY KEY IDENTITY(1,1),
    IdCompra INT FOREIGN KEY REFERENCES Compra(IdCompra),
    IdProducto INT FOREIGN KEY REFERENCES Producto(IdProducto),
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL
);

CREATE TABLE Venta (
    IdVenta INT PRIMARY KEY IDENTITY(1,1),
    IdCliente INT FOREIGN KEY REFERENCES Cliente(IdCliente),
    FechaVenta DATETIME NOT NULL,
    NumeroFactura NVARCHAR(50) NOT NULL UNIQUE,
    Estado BIT DEFAULT 1
);

CREATE TABLE DetalleVenta (
    IdDetalleVenta INT PRIMARY KEY IDENTITY(1,1),
    IdVenta INT FOREIGN KEY REFERENCES Venta(IdVenta),
    IdProducto INT FOREIGN KEY REFERENCES Producto(IdProducto),
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    PrecioTotal DECIMAL(10,2) NOT NULL
);

------------------------------
------------ VIEWS -----------
------------------------------

GO

CREATE OR ALTER VIEW VW_productosGrid as
SELECT p.IdProducto, p.Nombre, c.IdCategoria, c.Nombre as NombreCategoria,
m.IdMarca, m.Nombre as NombreMarca,
p.StockActual, p.StockMinimo, p.PorcentajeGanancia, p.FechaVencimiento, p.Precio
FROM Producto p
LEFT JOIN Categoria c on c.IdCategoria = p.IdTipoProducto
LEFT JOIN Marca m on m.IdMarca = p.IdMarca

GO

CREATE OR ALTER VIEW VW_productosGridDDL as
SELECT Distinct
p.IdProducto,
p.Nombre,
c.IdCategoria,
c.Nombre as NombreCategoria,
m.IdMarca,
m.Nombre as NombreMarca,
p.StockActual,
p.StockMinimo,
p.PorcentajeGanancia,
p.FechaVencimiento  
FROM Producto p
LEFT JOIN Categoria c on c.IdCategoria = p.IdTipoProducto
LEFT JOIN Marca m on m.IdMarca = p.IdMarca
INNER JOIN DetalleCompra DetComp on DetComp.IdProducto = P.IdProducto 

GO

CREATE OR ALTER VIEW VW_CategoriasGrid AS
SELECT IdCategoria, Nombre FROM Categoria

GO

CREATE OR ALTER VIEW VW_MarcasGrid AS
SELECT IdMarca, Nombre FROM Marca

GO

CREATE OR ALTER VIEW VW_verDetalleProd AS
SELECT p.IdProducto, p.Nombre, c.IdCategoria, c.Nombre as NombreCategoria,
m.IdMarca, m.Nombre as NombreMarca,
p.StockActual, p.StockMinimo, p.PorcentajeGanancia  
FROM Producto p
LEFT JOIN Categoria c on c.IdCategoria = p.IdTipoProducto
LEFT JOIN Marca m on m.IdMarca = p.IdMarca

GO

CREATE VIEW vw_IngresarCompra
AS
SELECT IdProveedor, FechaCompra FROM Compra;

------------------------------
------ STORED PROCEDURES -----
------------------------------

GO

CREATE OR ALTER PROCEDURE SP_insertProducto (@Nombre NVARCHAR(100), @IdMarca INT, @IdCategoria INT, @StockActual INT, @StockMinimo INT, @PorcentajeGanancia DECIMAL, @FechaVencimiento DATE, @Precio money)
AS 
BEGIN
BEGIN TRY
BEGIN TRANSACTION
INSERT INTO PRODUCTO (Nombre, IdMarca, IdTipoProducto, StockActual, StockMinimo, PorcentajeGanancia, FechaVencimiento, Precio)
VALUES(@Nombre, @IdMarca, @IdCategoria, @StockActual, @StockMinimo, @PorcentajeGanancia, @FechaVencimiento, @Precio)
COMMIT TRANSACTION
END TRY
BEGIN CATCH
  ROLLBACK TRANSACTION
  RAISERROR('No fue posible agregar el producto',16,1)
END CATCH
END

GO

CREATE OR ALTER PROCEDURE sp_DeleteCategoria
    @IdCategoria INT
AS
BEGIN
 BEGIN TRY
    IF @IdCategoria NOT in(SELECT IdTipoProducto FROM Producto WHERE IdTipoProducto = @IdCategoria)
    BEGIN
        DELETE FROM Categoria WHERE IdCategoria = @IdCategoria;
        PRINT 'Categoría eliminada correctamente.';
    END
    ELSE
    BEGIN
        RAISERROR('No se puede eliminar la categoría porque está referenciada en productos.',16,1)
    END
    END TRY
    BEGIN CATCH
      RAISERROR('Error al eliminar la categoria',16,1)
    END CATCH
END

GO

CREATE OR ALTER PROCEDURE sp_DeleteMarca
    @IdMarca INT
AS
BEGIN
 BEGIN TRY
    IF @IdMarca NOT in(SELECT IdMarca FROM Producto WHERE IdMarca= @IdMarca)
    BEGIN
        DELETE FROM Marca WHERE IdMarca = @IdMarca;
        PRINT 'Marca eliminada correctamente.';
    END
    ELSE
    BEGIN
        RAISERROR('No se puede eliminar la marca porque está referenciada en productos.',16,1)
    END
    END TRY
    BEGIN CATCH
      RAISERROR('Error al eliminar la marca',16,1)
    END CATCH
END

GO

CREATE OR ALTER PROCEDURE SP_ModifyCategoria (@IdCategoria INT, @Nombre NVARCHAR(100))
AS 
BEGIN
BEGIN TRY
UPDATE Categoria set Nombre = @Nombre where IdCategoria = @IdCategoria
END TRY
BEGIN CATCH
  RAISERROR('No fue posible Modificar la categoría',16,1)
END CATCH
END

GO

CREATE OR ALTER PROCEDURE SP_ModifyMarca (@IdMarca INT, @Nombre NVARCHAR(100))
AS 
BEGIN
BEGIN TRY
UPDATE Marca set Nombre = @Nombre where IdMarca = @IdMarca
END TRY
BEGIN CATCH
  RAISERROR('No fue posible Modificar la marca',16,1)
END CATCH
END

GO

CREATE PROCEDURE sp_InsertarCompra
    @IdProveedor INT,
    @FechaCompra DATETIME,
	@Estado bit
AS
BEGIN
    INSERT INTO Compra (IdProveedor, FechaCompra,Estado)
    VALUES (@IdProveedor, @FechaCompra,@Estado);

    SELECT SCOPE_IDENTITY() AS IdCompra;
END

GO

CREATE PROCEDURE sp_InsertarDetalleCompra
(
    @IdCompra INT,
    @IdProducto INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(18,2)
)
AS
BEGIN
    INSERT INTO DetalleCompra (IdCompra, IdProducto, Cantidad, PrecioUnitario) 
    VALUES (@IdCompra, @IdProducto, @Cantidad, @PrecioUnitario);

    UPDATE Producto
    SET Precio = dbo.fn_CalcularPrecioProducto(p.IdProducto)
    FROM Producto p
    INNER JOIN DetalleCompra dc 
    ON p.IdProducto = dc.IdProducto;
END;

GO

CREATE SEQUENCE NumeroFacturaSeq
START WITH 1
INCREMENT BY 1
NO CYCLE;

GO

CREATE OR ALTER PROCEDURE sp_GenerarVenta
@IdCliente INT,
@FechaVenta DATETIME,
@Estado BIT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @NumeroFactura NVARCHAR(50);
        DECLARE @NumeroSecuencial NVARCHAR(10);

        SET @NumeroSecuencial = RIGHT('0000' + CAST(NEXT VALUE FOR NumeroFacturaSeq AS NVARCHAR), 4);

        SET @NumeroFactura = CONCAT('FAC-', YEAR(@FechaVenta), '-', @NumeroSecuencial);

        INSERT INTO Venta (IdCliente, FechaVenta, NumeroFactura, Estado)
        VALUES (@IdCliente, @FechaVenta, @NumeroFactura, @Estado);

        COMMIT TRANSACTION;

        SELECT SCOPE_IDENTITY() AS IdVenta, @NumeroFactura AS NumeroFactura;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

GO

CREATE OR ALTER FUNCTION fn_CalcularPrecioProducto
(
    @idProducto INT
)
RETURNS MONEY
AS
BEGIN
    DECLARE @PorcentajeGanancia DECIMAL(5,2);
    DECLARE @PrecioUltimaCompra MONEY;
    DECLARE @PrecioProductoActual MONEY;

    
        SELECT TOP 1
            @PorcentajeGanancia = p.PorcentajeGanancia,
            @PrecioUltimaCompra = dc.PrecioUnitario
        FROM
            Producto p
            INNER JOIN DetalleCompra dc ON p.IdProducto = dc.IdProducto
            INNER JOIN Compra c ON c.IdCompra = dc.IdCompra
        WHERE
            p.IdProducto = @idProducto
        ORDER BY
            c.FechaCompra DESC;

        SET @PrecioProductoActual = @PrecioUltimaCompra * (1 + @PorcentajeGanancia / 100);

    RETURN @PrecioProductoActual;
END;

GO


CREATE OR ALTER PROCEDURE sp_GenerarDetalleVenta
(
    @IdVenta INT,
    @IdProducto INT,
    @Cantidad INT
)
AS
BEGIN
    BEGIN TRY
        DECLARE @PrecioUnitario MONEY;

        SET @PrecioUnitario = dbo.fn_CalcularPrecioProducto(@IdProducto);

        IF @PrecioUnitario IS NULL
        BEGIN
            RAISERROR('No se pudo calcular el precio del producto.', 16, 1);
            RETURN;
        END

        INSERT INTO DetalleVenta(IdVenta, IdProducto, Cantidad, PrecioUnitario, PrecioTotal)
        VALUES (@IdVenta, @IdProducto, @Cantidad, @PrecioUnitario, @Cantidad * @PrecioUnitario);
    END TRY
    BEGIN CATCH
        RAISERROR('Ocurrió un error al generar el detalle de la venta.', 16, 1);
    END CATCH
END;

------------------------------
---------- TRIGGERS ---------
------------------------------

GO

CREATE TRIGGER trg_IngresarCompra
ON vw_IngresarCompra
INSTEAD OF INSERT
AS
BEGIN
    INSERT INTO Compra (IdProveedor, FechaCompra)
    SELECT 
        IdProveedor, 
        FechaCompra
    FROM 
        inserted;
END;

GO

CREATE TRIGGER trg_CompraStock
ON DetalleCompra
AFTER INSERT
AS
BEGIN
    UPDATE p
    SET p.StockActual = p.StockActual + dc.Cantidad
    FROM Producto p
    INNER JOIN Inserted dc ON p.IdProducto = dc.IdProducto;
END;

GO

CREATE TRIGGER trg_VentaStock
ON DetalleVenta
AFTER INSERT
AS
BEGIN
    UPDATE p
    SET p.StockActual = p.StockActual - dv.Cantidad
    FROM Producto p
    INNER JOIN Inserted dv ON p.IdProducto = dv.IdProducto;
END;

----------------------------------------------------------