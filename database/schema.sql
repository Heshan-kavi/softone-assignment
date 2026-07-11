-- ============================================================
-- TaskManagerDb - Database Schema (Guid IDs)
-- ============================================================

-- Drop and recreate to apply schema changes
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'TaskManagerDb')
BEGIN
    ALTER DATABASE TaskManagerDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TaskManagerDb;
END
GO

CREATE DATABASE TaskManagerDb;
GO

USE TaskManagerDb;
GO

-- ============================================================
-- Users Table
-- ============================================================
CREATE TABLE Users (
    Id            UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username      NVARCHAR(100)  NOT NULL UNIQUE,
    PasswordHash  NVARCHAR(255)  NOT NULL,
    CreatedAt     DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt     DATETIME2      NOT NULL DEFAULT GETUTCDATE()
);
GO

-- ============================================================
-- Tasks Table
-- ============================================================
CREATE TABLE Tasks (
    Id          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title       NVARCHAR(200)  NOT NULL,
    Description NVARCHAR(1000) NULL,
    Priority    NVARCHAR(50)   NOT NULL DEFAULT 'Low',
    Status      NVARCHAR(50)   NOT NULL DEFAULT 'Todo',
    IsCompleted BIT            NOT NULL DEFAULT 0,
    DueDate     DATETIME2      NULL,
    CreatedAt   DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt   DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    UserId      UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT FK_Tasks_Users  FOREIGN KEY (UserId)   REFERENCES Users(Id),
    CONSTRAINT CK_Tasks_Status   CHECK (Status   IN ('Todo', 'InProgress', 'Done')),
    CONSTRAINT CK_Tasks_Priority CHECK (Priority IN ('Low', 'Medium', 'High'))
);
GO

-- ============================================================
-- Seed: Default admin user
-- Username : admin
-- Password : admin123  (BCrypt hash, work factor 11)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Id, Username, PasswordHash, CreatedAt, UpdatedAt)
    VALUES (
        NEWID(),
        'admin',
        '$2a$11$zE5gGiVlMiHi9G3PQO5eGOBqIiHCzK5.PkiIVu/9EF4uWZ1T1GnAS',
        GETUTCDATE(),
        GETUTCDATE()
    );
END
GO

-- ============================================================
-- Note: The application also seeds the admin user automatically
-- via DbSeeder on first startup if no users exist.
-- Running this script is sufficient to set up a complete DB.
-- ============================================================
