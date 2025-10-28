-- Disaster Incident Reporting
CREATE TABLE Disasters (
    DisasterId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200),
    Description NVARCHAR(MAX),
    Location NVARCHAR(100),
    DateReported DATETIME DEFAULT GETDATE(),
    Severity NVARCHAR(50),
    UserId NVARCHAR(450),
    CONSTRAINT FK_Disasters_Users FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

-- Optional: File Uploads for Disaster Evidence
CREATE TABLE DisasterFiles (
    FileId INT IDENTITY(1,1) PRIMARY KEY,
    DisasterId INT,
    FileName NVARCHAR(255),
    FilePath NVARCHAR(500),
    UploadedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_DisasterFiles_Disasters FOREIGN KEY (DisasterId) REFERENCES Disasters(DisasterId)
);

-- Resource Donations
CREATE TABLE Donations (
    DonationId INT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(450),
    Category NVARCHAR(50), -- Food, Clothing, Medical, Money
    Quantity INT,
    Description NVARCHAR(MAX),
    Status NVARCHAR(50) DEFAULT 'Pending', -- Pending / Distributed
    DateDonated DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Donations_Users FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

-- Volunteers
CREATE TABLE Volunteers (
    VolunteerId INT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(450),
    Skills NVARCHAR(200),
    Availability NVARCHAR(200),
    CONSTRAINT FK_Volunteers_Users FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

-- Tasks for Volunteers
CREATE TABLE VolunteerTasks (
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200),
    Description NVARCHAR(MAX),
    Location NVARCHAR(100),
    DateTime DATETIME,
    AssignedVolunteerId INT,
    CONSTRAINT FK_VolunteerTasks_Volunteers FOREIGN KEY (AssignedVolunteerId) REFERENCES Volunteers(VolunteerId)
);

-- Add primary key to existing table
ALTER TABLE Volunteers
ADD Name NVARCHAR(100) NOT NULL DEFAULT '',
    Age INT NOT NULL DEFAULT 18;

    select * from Volunteers;

    CREATE TABLE VolunteerAssignments
(
    AssignmentId INT IDENTITY(1,1) PRIMARY KEY,
    VolunteerId INT NOT NULL,
    TaskId INT NOT NULL,
    DisasterId INT NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    DateAssigned DATETIME NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT FK_VolunteerAssignments_Volunteers FOREIGN KEY (VolunteerId)
        REFERENCES Volunteers(VolunteerId) ON DELETE CASCADE,
    CONSTRAINT FK_VolunteerAssignments_VolunteerTasks FOREIGN KEY (TaskId)
        REFERENCES VolunteerTasks(TaskId) ON DELETE CASCADE,
    CONSTRAINT FK_VolunteerAssignments_Disasters FOREIGN KEY (DisasterId)
        REFERENCES Disasters(DisasterId) ON DELETE CASCADE
);

ALTER TABLE VolunteerAssignments
ADD  TaskDescription VARCHAR (250);

-- 1. Drop the foreign key first
ALTER TABLE VolunteerAssignments
DROP CONSTRAINT FK_VolunteerAssignments_VolunteerTasks;

-- 2. Drop the TaskId column
ALTER TABLE VolunteerAssignments
DROP COLUMN TaskId;

-- 3. Add TaskDescription column
ALTER TABLE VolunteerAssignments
ADD TaskDescription NVARCHAR(MAX) NOT NULL DEFAULT '';

-- Optional: you can also make it nullable if you want
-- ALTER TABLE VolunteerAssignments
-- ADD TaskDescription NVARCHAR(MAX) NULL;
ALTER TABLE VolunteerAssignments
DROP COLUMN TaskDescription;
