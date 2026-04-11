-- Migration: AddVenueToRegistration
-- Generated: 2026-04-11
-- Description: Adds Venue column to Registrations table and makes ClassSlot nullable
-- Existing records will be set to 'SriAlAminCherasSelatan' venue

-- Step 1: Make ClassSlot column nullable
IF EXISTS (SELECT * FROM [sys].[columns] WHERE [object_id] = OBJECT_ID(N'[Registrations]') AND [name] = N'ClassSlot')
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Registrations]') AND [c].[name] = N'ClassSlot');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Registrations] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Registrations] ALTER COLUMN [ClassSlot] nvarchar(100) NULL;
    PRINT 'ClassSlot column is now nullable';
END
GO

-- Step 2: Add Venue column with default value (this sets existing rows to default)
IF NOT EXISTS (SELECT * FROM [sys].[columns] WHERE [object_id] = OBJECT_ID(N'[Registrations]') AND [name] = N'Venue')
BEGIN
    ALTER TABLE [Registrations] ADD [Venue] nvarchar(max) NOT NULL DEFAULT N'SriAlAminCherasSelatan';
    PRINT 'Venue column added with default value SriAlAminCherasSelatan';
END
GO

-- Step 3: Update any records that might have empty venue (safety check)
IF EXISTS (SELECT * FROM [sys].[columns] WHERE [object_id] = OBJECT_ID(N'[Registrations]') AND [name] = N'Venue')
BEGIN
    UPDATE [Registrations]
    SET [Venue] = 'SriAlAminCherasSelatan'
    WHERE [Venue] = '';
    PRINT 'Updated existing records to SriAlAminCherasSelatan venue';
END
GO

-- Step 4: Record migration in history (if using EF migrations history)
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260411085955_AddVenueToRegistration')
    BEGIN
        INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
        VALUES (N'20260411085955_AddVenueToRegistration', N'10.0.1');
        PRINT 'Migration recorded in __EFMigrationsHistory';
    END
END
GO

PRINT 'Migration completed successfully!';
