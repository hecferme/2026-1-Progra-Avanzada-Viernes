-- SQL Server DDL for library schema
-- Drops (safe to re-run)
IF OBJECT_ID('dbo.BookAuthors','U') IS NOT NULL DROP TABLE dbo.BookAuthors;
IF OBJECT_ID('dbo.BookThemes','U') IS NOT NULL DROP TABLE dbo.BookThemes;
IF OBJECT_ID('dbo.Borrows','U') IS NOT NULL DROP TABLE dbo.Borrows;
IF OBJECT_ID('dbo.BookCopies','U') IS NOT NULL DROP TABLE dbo.BookCopies;
IF OBJECT_ID('dbo.Users','U') IS NOT NULL DROP TABLE dbo.Users;
IF OBJECT_ID('dbo.Authors','U') IS NOT NULL DROP TABLE dbo.Authors;
IF OBJECT_ID('dbo.Themes','U') IS NOT NULL DROP TABLE dbo.Themes;
IF OBJECT_ID('dbo.Books','U') IS NOT NULL DROP TABLE dbo.Books;

GO

-- Books
CREATE TABLE dbo.Books (
    id INT IDENTITY(1,1) PRIMARY KEY,
    title NVARCHAR(400) NOT NULL,
    isbn NVARCHAR(50) NULL,
    published_date DATE NULL
);

GO

-- Themes
CREATE TABLE dbo.Themes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(200) NOT NULL UNIQUE
);

GO

-- Authors
CREATE TABLE dbo.Authors (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(300) NOT NULL
);

GO

-- Users
-- max_concurrent_borrows: starts at 1 (default), decreases by penalty_weight when books are lost
-- If value < 0, user is banned from borrowing
CREATE TABLE dbo.Users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    full_name NVARCHAR(300) NOT NULL,
    email NVARCHAR(320) NULL,
    max_concurrent_borrows INT NOT NULL DEFAULT(1)
);

GO

-- Book copies
-- Status: 0 = LOST, 1 = BORROWED, 2 = AVAILABLE
CREATE TABLE dbo.BookCopies (
    id INT IDENTITY(1,1) PRIMARY KEY,
    book_id INT NOT NULL REFERENCES dbo.Books(id) ON DELETE CASCADE,
    barcode NVARCHAR(100) NULL UNIQUE,
    status TINYINT NOT NULL DEFAULT(2),
    penalty_weight INT NOT NULL DEFAULT(0),
    CONSTRAINT CK_BookCopies_Status CHECK (status IN (0,1,2))
);

GO

-- Borrows
-- Status: 0 = LOST, 1 = ACTIVE, 2 = RETURNED
CREATE TABLE dbo.Borrows (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL REFERENCES dbo.Users(id),
    book_copy_id INT NOT NULL REFERENCES dbo.BookCopies(id),
    borrow_date DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    return_date DATETIME2 NULL,
    status TINYINT NOT NULL DEFAULT(1),
    CONSTRAINT CK_Borrows_Status CHECK (status IN (0,1,2))
);

GO

-- Many-to-many: BookThemes
CREATE TABLE dbo.BookThemes (
    book_id INT NOT NULL REFERENCES dbo.Books(id) ON DELETE CASCADE,
    theme_id INT NOT NULL REFERENCES dbo.Themes(id) ON DELETE CASCADE,
    CONSTRAINT PK_BookThemes PRIMARY KEY (book_id, theme_id)
);

GO

-- Many-to-many: BookAuthors
CREATE TABLE dbo.BookAuthors (
    book_id INT NOT NULL REFERENCES dbo.Books(id) ON DELETE CASCADE,
    author_id INT NOT NULL REFERENCES dbo.Authors(id) ON DELETE CASCADE,
    CONSTRAINT PK_BookAuthors PRIMARY KEY (book_id, author_id)
);

GO

-- Indexes for lookups
CREATE INDEX IX_BookCopies_BookId ON dbo.BookCopies(book_id);
CREATE INDEX IX_Borrows_CopyId ON dbo.Borrows(book_copy_id);
CREATE INDEX IX_Borrows_UserId ON dbo.Borrows(user_id);

GO

-- Triggers implement the business rules described:
-- 1) When a borrow is inserted its status is ACTIVE (default) and the book copy status
--    is updated to BORROWED. A copy cannot be borrowed if it isn't AVAILABLE.
-- 2) A borrow may be updated only when its prior status is ACTIVE. If new state
--    is LOST -> associated book copy becomes LOST. If new state is RETURNED -> copy becomes AVAILABLE.

-- AFTER INSERT trigger: ensure copies are AVAILABLE and mark them BORROWED
CREATE TRIGGER trg_Borrows_AfterInsert
ON dbo.Borrows
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    -- Ensure every inserted borrow references an AVAILABLE copy (status = 2)
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN dbo.BookCopies bc ON i.book_copy_id = bc.id
        WHERE bc.status <> 2
    )
    BEGIN
        THROW 50001, 'One or more book copies are not AVAILABLE and cannot be borrowed.', 1;
    END

    -- Update referenced copies to BORROWED (1)
    UPDATE bc
    SET bc.status = 1
    FROM dbo.BookCopies bc
    JOIN inserted i ON bc.id = i.book_copy_id;
END;

GO

-- AFTER UPDATE trigger: only allow updates when previous status was ACTIVE (1),
-- and propagate state changes to the related BookCopies
CREATE TRIGGER trg_Borrows_AfterUpdate
ON dbo.Borrows
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    -- Ensure that only borrows that were ACTIVE (1) can be updated
    IF EXISTS (
        SELECT 1
        FROM deleted d
        WHERE d.status <> 1
    )
    BEGIN
        THROW 50002, 'Only borrows with prior status ACTIVE can be updated.', 1;
    END

    -- If new status is LOST (0), mark the associated copy LOST (0)
    UPDATE bc
    SET bc.status = 0
    FROM dbo.BookCopies bc
    JOIN inserted i ON bc.id = i.book_copy_id
    WHERE i.status = 0;

    -- If new status is RETURNED (2), mark the associated copy AVAILABLE (2)
    UPDATE bc
    SET bc.status = 2
    FROM dbo.BookCopies bc
    JOIN inserted i ON bc.id = i.book_copy_id
    WHERE i.status = 2;
END;

GO

-- AFTER UPDATE trigger: Apply penalty when a borrow status changes to LOST (0)
-- Deduct penalty_weight from user's max_concurrent_borrows
-- If result < 0, user becomes banned (cannot borrow any more books)
CREATE TRIGGER trg_Borrows_ApplyLostPenalty
ON dbo.Borrows
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Only process rows that are transitioning TO LOST status (0)
    -- and were previously ACTIVE (1)
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN deleted d ON i.id = d.id
        WHERE i.status = 0 AND d.status = 1
    )
    BEGIN
        -- Deduct penalty_weight from users who had books marked as LOST
        UPDATE dbo.Users
        SET max_concurrent_borrows = max_concurrent_borrows - bc.penalty_weight
        WHERE id IN (
            SELECT DISTINCT i.user_id
            FROM inserted i
            JOIN deleted d ON i.id = d.id
            JOIN dbo.BookCopies bc ON i.book_copy_id = bc.id
            WHERE i.status = 0 AND d.status = 1
        );
    END
END;

GO

-- Scalar function to calculate available concurrent borrows for a user
-- Returns the number of additional books a user can currently borrow
-- Returns 0 if user is banned (max_concurrent_borrows < 0)
CREATE FUNCTION dbo.fnGetAvailableConcurrentBorrows(@user_id INT)
RETURNS INT
AS
BEGIN
    DECLARE @available INT;
    
    SELECT @available = CASE 
                          WHEN u.max_concurrent_borrows < 0 THEN 0
                          ELSE u.max_concurrent_borrows - ISNULL(active_count, 0)
                        END
    FROM dbo.Users u
    LEFT JOIN (
        SELECT user_id, COUNT(*) as active_count
        FROM dbo.Borrows
        WHERE status = 1 -- ACTIVE borrows only
        GROUP BY user_id
    ) active ON u.id = active.user_id
    WHERE u.id = @user_id;
    
    RETURN ISNULL(@available, 0);
END;

GO

-- End of DDL
