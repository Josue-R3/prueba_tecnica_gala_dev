-- Reinicia la base de datos (en caso se levante de nuevo el contenedor)
IF DB_ID(N'TiendaDB') IS NOT NULL
BEGIN
    ALTER DATABASE TiendaDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TiendaDB;
END;
GO
CREATE DATABASE TiendaDB;
GO
USE TiendaDB;
GO





-- Tiendas  (soft-delete por estado)
CREATE TABLE Tiendas (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    Nombre     NVARCHAR(100) NOT NULL UNIQUE,
    Direccion  NVARCHAR(200) NOT NULL,
    Estado     BIT NOT NULL DEFAULT (1)          -- soft-delete
        CHECK (Estado IN (0,1))
);
CREATE INDEX IX_Tiendas_Estado ON Tiendas (Estado);






-- Empleados  (soft-delete por estado)
CREATE TABLE Empleados (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Nombre        NVARCHAR(50)  NOT NULL,
    Apellido      NVARCHAR(50)  NOT NULL,
    Correo        NVARCHAR(100) NOT NULL UNIQUE,
    Cargo         NVARCHAR(50)  NOT NULL,
    FechaIngreso  DATE          NOT NULL DEFAULT (GETDATE()),
    Estado        BIT           NOT NULL DEFAULT (1)
        CHECK (Estado IN (0,1)),
    TiendaId      INT           NOT NULL
        FOREIGN KEY REFERENCES Tiendas(Id)
);
CREATE INDEX IX_Empleados_TiendaId ON Empleados (TiendaId);
CREATE INDEX IX_Empleados_Estado   ON Empleados (Estado);





-- Roles  (datos mínimo para autorización por JWT)
CREATE TABLE Roles (
    Id     TINYINT      PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);
INSERT INTO Roles (Id, Nombre) VALUES
(1, N'Admin'),
(2, N'Manager'),
(3, N'Empleado');






-- Usuarios  (password se guarda en hash, pendiente para capa de servicio)
CREATE TABLE Usuarios (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Usuario      NVARCHAR(50)  NOT NULL UNIQUE,
    Contrasenia  NVARCHAR(255) NOT NULL,
    RolId        TINYINT       NOT NULL
        FOREIGN KEY REFERENCES Roles(Id),
    EmpleadoId   INT NULL
        FOREIGN KEY REFERENCES Empleados(Id),
    Estado       BIT           NOT NULL DEFAULT (1),
    FechaCreado  DATETIME      NOT NULL DEFAULT (GETDATE())
);
CREATE INDEX IX_Usuarios_RolId      ON Usuarios (RolId);
CREATE INDEX IX_Usuarios_EmpleadoId ON Usuarios (EmpleadoId);




-- Datos de muestra
INSERT INTO Tiendas (Nombre, Direccion, Estado) VALUES
(N'Tienda Centro',  N'Av. Principal 123', 1),
(N'Tienda Norte',   N'Calle 5 y Av. 8',   1),
(N'Tienda Cerrada', N'Vía Antigua S/N',   0);

INSERT INTO Empleados (Nombre, Apellido, Correo, Cargo, FechaIngreso, Estado, TiendaId) VALUES
(N'Ana',   N'Pérez', N'ana@demo.com',   N'Cajera',     '2024-01-10', 1, 1),
(N'Luis',  N'Mora',  N'luis@demo.com',  N'Supervisor', '2023-09-02', 1, 1),
(N'Marta', N'Ríos',  N'mrios@demo.com', N'Bodega',     '2022-05-21', 0, 3);

INSERT INTO Usuarios (Usuario, Contrasenia, RolId, EmpleadoId) VALUES
(N'admin',    N'Admin123',   1, NULL),
(N'manager',  N'Manager123', 2, NULL),
(N'ana.login',N'Ana123',     3, 1);
GO
