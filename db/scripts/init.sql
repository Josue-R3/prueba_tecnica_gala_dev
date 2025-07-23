-- Elimina la base si ya existe (útil para reinicios locales)
IF DB_ID(N'TiendaDB') IS NOT NULL
BEGIN
    ALTER DATABASE TiendaDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TiendaDB;
END;
GO

-- Crea la base vacía
CREATE DATABASE TiendaDB;
GO
USE TiendaDB;
GO

-- Tabla Tiendas
CREATE TABLE Tiendas (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Nombre    NVARCHAR(100) NOT NULL,
    Direccion NVARCHAR(200) NOT NULL,
    Estado    BIT NOT NULL DEFAULT (1)         -- 1 = Activa, 0 = Inactiva
        CHECK (Estado IN (0,1)),
    CONSTRAINT UQ_Tiendas_Nombre UNIQUE (Nombre)
);
CREATE INDEX IX_Tiendas_Estado ON Tiendas (Estado);

-- Tabla Empleados
CREATE TABLE Empleados (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Nombre        NVARCHAR(50)  NOT NULL,
    Apellido      NVARCHAR(50)  NOT NULL,
    Correo        NVARCHAR(100) NOT NULL UNIQUE,
    Cargo         NVARCHAR(50)  NOT NULL,
    FechaIngreso  DATE          NOT NULL DEFAULT (GETDATE()),
    Estado        BIT           NOT NULL DEFAULT (1)       -- 1 = Activo
        CHECK (Estado IN (0,1)),
    TiendaId      INT           NOT NULL
        FOREIGN KEY REFERENCES Tiendas(Id)
);
CREATE INDEX IX_Empleados_TiendaId ON Empleados (TiendaId);
CREATE INDEX IX_Empleados_Estado   ON Empleados (Estado);

-- Datos de ejemplo
INSERT INTO Tiendas (Nombre, Direccion, Estado) VALUES
(N'Tienda Centro',  N'Av. Principal 123', 1),
(N'Tienda Norte',   N'Calle 5 y Av. 8',   1),
(N'Tienda Cerrada', N'Vía Antigua S/N',   0);

INSERT INTO Empleados (Nombre, Apellido, Correo, Cargo, FechaIngreso, Estado, TiendaId) VALUES
(N'Ana',   N'Pérez',  N'ana@demo.com',   N'Cajera',     '2024-01-10', 1, 1),
(N'Luis',  N'Mora',   N'luis@demo.com',  N'Supervisor', '2023-09-02', 1, 1),
(N'Marta', N'Ríos',   N'mrios@demo.com', N'Bodega',     '2022-05-21', 0, 3);
GO
