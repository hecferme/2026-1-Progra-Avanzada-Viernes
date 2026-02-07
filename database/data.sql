-- Seed data for library schema
-- Deletes (safe to re-run)
DELETE FROM dbo.Borrows;
DELETE FROM dbo.BookAuthors;
DELETE FROM dbo.BookThemes;
DELETE FROM dbo.BookCopies;
DELETE FROM dbo.Books;
DELETE FROM dbo.Authors;
DELETE FROM dbo.Themes;
DELETE FROM dbo.Users;

-- Insert authors
INSERT INTO dbo.Authors (name) VALUES
('George Orwell'), -- id = 1
('Aldous Huxley'); -- id = 2

-- Insert themes
INSERT INTO dbo.Themes (name) VALUES
('Dystopia'), -- id = 1
('Classic');  -- id = 2

-- Insert books
INSERT INTO dbo.Books (title, isbn, published_date) VALUES
('1984','9780451524935','1949-06-08'), -- id = 1
('Brave New World','9780060850524','1932-08-18'); -- id = 2

-- Link books to authors
INSERT INTO dbo.BookAuthors (book_id, author_id) VALUES
(1,1),
(2,2);

-- Link books to themes
INSERT INTO dbo.BookThemes (book_id, theme_id) VALUES
(1,1),
(1,2),
(2,1);

-- Insert book copies
-- Status: 0 = LOST, 1 = BORROWED, 2 = AVAILABLE
INSERT INTO dbo.BookCopies (book_id, barcode, status) VALUES
(1,'BC1984-1',2), -- id = 1 available
(1,'BC1984-2',2), -- id = 2 available
(2,'BCBNW-1',2);  -- id = 3 available

-- Insert users
INSERT INTO dbo.Users (full_name, email) VALUES
('Alice Cooper','alice@example.com'), -- id = 1
('Bob Smith','bob@example.com');     -- id = 2

-- Insert a borrow: user 1 borrows copy id 1
-- Borrow status defaults to ACTIVE (1) and trigger will mark copy BORROWED (1)
INSERT INTO dbo.Borrows (user_id, book_copy_id) VALUES (1,1);

-- Return that borrow (set to RETURNED). Trigger will set copy back to AVAILABLE.
UPDATE dbo.Borrows SET status = 2, return_date = SYSUTCDATETIME() WHERE id = 1;

-- Now borrow same copy again (allowed because it was returned)
INSERT INTO dbo.Borrows (user_id, book_copy_id) VALUES (2,1);

-- Example: mark a borrow as LOST (simulate lost copy)
-- First create another borrow of copy id 2
INSERT INTO dbo.Borrows (user_id, book_copy_id) VALUES (1,2);
-- Now mark that borrow LOST -> associated copy becomes LOST
-- The inserted borrow above will be id = 3 in a fresh DB; update it accordingly
UPDATE dbo.Borrows SET status = 0 WHERE id = 3;

-- End seed
