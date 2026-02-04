IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260203125316_InitialCreate'
)
BEGIN
    CREATE TABLE [Registrations] (
        [Id] uniqueidentifier NOT NULL,
        [ReferenceNumber] nvarchar(30) NOT NULL,
        [StudentName] nvarchar(255) NOT NULL,
        [DateOfBirth] date NOT NULL,
        [Gender] nvarchar(max) NOT NULL,
        [SchoolYear] nvarchar(10) NOT NULL,
        [MusicExperience] nvarchar(max) NOT NULL,
        [GuardianName] nvarchar(255) NOT NULL,
        [GuardianPhone] nvarchar(15) NOT NULL,
        [GuardianEmail] nvarchar(255) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Postcode] nvarchar(5) NOT NULL,
        [City] nvarchar(100) NOT NULL,
        [State] nvarchar(50) NOT NULL,
        [EmergencyName] nvarchar(255) NOT NULL,
        [EmergencyPhone] nvarchar(15) NOT NULL,
        [EmergencyRelationship] nvarchar(100) NOT NULL,
        [ClassSlot] nvarchar(100) NOT NULL,
        [AgreeTerms] bit NOT NULL,
        [AgreeParticipation] bit NOT NULL,
        [AgreeMedia] bit NOT NULL,
        [SignatoryName] nvarchar(255) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Registrations] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260203125316_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Registrations_ReferenceNumber] ON [Registrations] ([ReferenceNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260203125316_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260203125316_InitialCreate', N'10.0.1');
END;

COMMIT;
GO

