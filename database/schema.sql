-- ============================================================
-- TaskManagerDb - Database Schema
-- ============================================================

-- Create database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TaskManagerDb')
BEGIN
    CREATE DATABASE TaskManagerDb;
END
GO

USE TaskManagerDb;
GO

-- ============================================================
-- Users Table
-- ============================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Users' AND xtype = 'U')
BEGIN
    CREATE TABLE Users (
        Id            INT IDENTITY(1,1) PRIMARY KEY,
        Username      NVARCHAR(100)  NOT NULL UNIQUE,
        PasswordHash  NVARCHAR(255)  NOT NULL,
        CreatedAt     DATETIME2      NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- ============================================================
-- Tasks Table
-- ============================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Tasks' AND xtype = 'U')
BEGIN
    CREATE TABLE Tasks (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Title       NVARCHAR(200)  NOT NULL,
        Description NVARCHAR(1000) NULL,
        Status      NVARCHAR(50)   NOT NULL DEFAULT 'Pending',
        CreatedAt   DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt   DATETIME2      NULL,
        UserId      INT            NOT NULL,

        CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT CK_Tasks_Status CHECK (Status IN ('Pending', 'InProgress', 'Done'))
    );
END
GO

-- ============================================================
-- Seed: Default admin user
-- Password: admin123
-- BCrypt hash generated with work factor 11
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, PasswordHash)
    VALUES (
        'admin',
        '$2a$11$zE5gGiVlMiHi9G3PQO5eGOBqIiHCzK5.PkiIVu/9EF4uWZ1T1GnAS'
    );
END
GO
