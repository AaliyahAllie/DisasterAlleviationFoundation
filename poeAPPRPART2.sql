-- ===============================
-- Identity Tables (for Login/Registration)
-- ===============================

CREATE TABLE AspNetRoles (
    Id NVARCHAR(256) NOT NULL PRIMARY KEY,
    Name NVARCHAR(256) NULL,
    NormalizedName NVARCHAR(256) NULL UNIQUE,
    ConcurrencyStamp NVARCHAR(MAX) NULL
);

CREATE TABLE AspNetUsers (
    Id NVARCHAR(256) NOT NULL PRIMARY KEY,
    UserName NVARCHAR(256) NULL,
    NormalizedUserName NVARCHAR(256) NULL UNIQUE,
    Email NVARCHAR(256) NULL,
    NormalizedEmail NVARCHAR(256) NULL,
    EmailConfirmed BIT NOT NULL DEFAULT 0,
    PasswordHash NVARCHAR(MAX) NULL,
    SecurityStamp NVARCHAR(MAX) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL,
    PhoneNumber NVARCHAR(50) NULL,
    PhoneNumberConfirmed BIT NOT NULL DEFAULT 0,
    TwoFactorEnabled BIT NOT NULL DEFAULT 0,
    LockoutEnd DATETIMEOFFSET NULL,
    LockoutEnabled BIT NOT NULL DEFAULT 1,
    AccessFailedCount INT NOT NULL DEFAULT 0
);

CREATE TABLE AspNetUserRoles (
    UserId NVARCHAR(256) NOT NULL,
    RoleId NVARCHAR(256) NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRoles_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserRoles_Role FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserClaims (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId NVARCHAR(256) NOT NULL,
    ClaimType NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL,
    CONSTRAINT FK_UserClaims_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserLogins (
    LoginProvider NVARCHAR(128) NOT NULL,
    ProviderKey NVARCHAR(128) NOT NULL,
    ProviderDisplayName NVARCHAR(MAX) NULL,
    UserId NVARCHAR(256) NOT NULL,
    PRIMARY KEY (LoginProvider, ProviderKey),
    CONSTRAINT FK_UserLogins_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserTokens (
    UserId NVARCHAR(256) NOT NULL,
    LoginProvider NVARCHAR(128) NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    Value NVARCHAR(MAX) NULL,
    PRIMARY KEY (UserId, LoginProvider, Name),
    CONSTRAINT FK_UserTokens_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

CREATE TABLE AspNetRoleClaims (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    RoleId NVARCHAR(256) NOT NULL,
    ClaimType NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL,
    CONSTRAINT FK_RoleClaims_Role FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);

-- ===============================
-- Custom Tables for Your Project
-- ===============================

-- Disaster Incidents
CREATE TABLE Disasters (
    DisasterId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Location NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    DateReported DATETIME DEFAULT GETDATE()
);

-- Volunteers (linked to Users if needed)
CREATE TABLE Volunteers (
    VolunteerId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    UserId NVARCHAR(256) NULL,
    CONSTRAINT FK_Volunteer_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

-- Donations
CREATE TABLE Donations (
    DonationId INT IDENTITY(1,1) PRIMARY KEY,
    DonorName NVARCHAR(100) NOT NULL,
    ResourceType NVARCHAR(50) NOT NULL,
    Quantity INT NOT NULL,
    DateDonated DATETIME NOT NULL DEFAULT GETDATE(),
    UserId NVARCHAR(450) NOT NULL,
    CreatedBy NVARCHAR(100) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Received'
);


-- Task Assignments
CREATE TABLE TaskAssignments (
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
    TaskName NVARCHAR(100) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Pending',
    DisasterId INT NOT NULL,
    VolunteerId INT NOT NULL,
    CONSTRAINT FK_Task_Disaster FOREIGN KEY (DisasterId) REFERENCES Disasters(DisasterId),
    CONSTRAINT FK_Task_Volunteer FOREIGN KEY (VolunteerId) REFERENCES Volunteers(VolunteerId)
);
-- Disaster table
ALTER TABLE Disasters
    DROP COLUMN DateReported;

ALTER TABLE Disasters
    ADD ReportedAt DATETIME DEFAULT GETDATE();

ALTER TABLE Disasters
    ADD UserId NVARCHAR(256) NULL;

ALTER TABLE Disasters
    ADD CONSTRAINT FK_Disaster_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id);

    USE DisasterAlleviationDatabase;
GO

EXEC sp_columns Disasters;

ALTER TABLE Disasters
ADD ReportedAt DATETIME DEFAULT GETDATE();

ALTER TABLE Disasters
ADD UserId NVARCHAR(256) NULL;

-- Optional: add foreign key to AspNetUsers
ALTER TABLE Disasters
ADD CONSTRAINT FK_Disaster_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id);


-- Drop the incorrect column first if it exists
ALTER TABLE Disasters
DROP COLUMN UserId;

-- Add column with correct type
ALTER TABLE Disasters
ADD UserId NVARCHAR(256) NULL;

-- Add the foreign key
ALTER TABLE Disasters
ADD CONSTRAINT FK_Disaster_User
FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id);
ALTER TABLE Disasters
DROP COLUMN UserId;

ALTER TABLE Disasters
ADD UserId NVARCHAR(256) NULL;

ALTER TABLE Disasters
ADD CONSTRAINT FK_Disaster_User
FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id);

EXEC sp_columns AspNetUsers;
