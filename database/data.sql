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
('Aldous Huxley'), -- id = 2
('J.R.R. Tolkien'), -- id = 3
('J.K. Rowling'), -- id = 4
('Stephen King'); -- id = 5

-- Insert themes
INSERT INTO dbo.Themes (name) VALUES
('Dystopia'), -- id = 1
('Classic'), -- id = 2
('Fantasy'), -- id = 3
('Science Fiction'); -- id = 4

-- Insert books
INSERT INTO dbo.Books (title, isbn, published_date) VALUES
('1984','9780451524935','1949-06-08'), -- id = 1
('Brave New World','9780060850524','1932-08-18'), -- id = 2
('The Lord of the Rings','9780544003415','1954-07-29'), -- id = 3
('Harry Potter and the Sorcerers Stone','9780439708180','1998-06-26'), -- id = 4
('The Shining','9780385123693','1977-01-28'); -- id = 5

-- Link books to authors
INSERT INTO dbo.BookAuthors (book_id, author_id) VALUES
(1,1),
(2,2),
(3,3),
(4,4),
(5,5);

-- Link books to themes
INSERT INTO dbo.BookThemes (book_id, theme_id) VALUES
(1,1),
(1,2),
(2,1),
(3,3),
(4,3),
(5,4);

-- Insert book copies with penalty weights
-- Heavy books (weight 3-4): dystopia/sci-fi novels
-- Medium books (weight 2): fantasy
-- Light books (weight 1): standard novels
INSERT INTO dbo.BookCopies (book_id, barcode, status, penalty_weight) VALUES
(1,'BC1984-1',2,2), -- id = 1, AVAILABLE
(1,'BC1984-2',2,2), -- id = 2, AVAILABLE (will become LOST)
(2,'BCBNW-1',2,3), -- id = 3, AVAILABLE
(2,'BCBNW-2',2,3), -- id = 4, AVAILABLE
(3,'BCLOTR-1',2,4), -- id = 5, AVAILABLE
(3,'BCLOTR-2',2,4), -- id = 6, AVAILABLE
(4,'BCHP-1',2,2), -- id = 7, AVAILABLE
(4,'BCHP-2',2,2), -- id = 8, AVAILABLE
(5,'BCSH-1',2,3); -- id = 9, AVAILABLE

-- Insert users
-- All start with max_concurrent_borrows = 1 (default)
INSERT INTO dbo.Users (full_name, email, max_concurrent_borrows) VALUES
('Alice Cooper','alice@example.com',1), -- id = 1, will lose copy 2 (weight 2) -> becomes banned
('Bob Smith','bob@example.com',1), -- id = 2, will return books normally
('Carol White','carol@example.com',1), -- id = 3, multiple returns
('David Brown','david@example.com',1), -- id = 4, normal borrower
('Eve Martin','eve@example.com',1); -- id = 5, will lose book 8 (weight 4) -> becomes banned

-- === Scenario: Demonstrate penalty system ===
-- User 1 (Alice): borrows copy 1 -> returns it
--                 borrows copy 2 -> LOSES it (weight 2, already = 1 max) -> becomes banned (-1)
-- User 2 (Bob):   borrows copy 3 -> returns it
--                 borrows copy 4 -> returns it
-- User 3 (Carol): borrows copy 5 -> returns it
-- User 4 (David): borrows copy 6 -> keeps it (active)
-- User 5 (Eve):   borrows copy 7 -> returns it
--                 borrows copy 8 -> LOSES it (weight 4, already = 1 max) -> becomes banned (-3)

-- Alice borrows copy 1, then returns it
INSERT INTO dbo.Borrows (user_id, book_copy_id) VALUES (1,1);
UPDATE dbo.Borrows SET status = 2, return_date = SYSUTCDATETIME() WHERE id = 1;

-- Alice borrows copy 2, then loses it -> penalty applied by trigger
INSERT INTO dbo.Borrows (user_id, book_copy_id) VALUES (1,2);
UPDATE dbo.Borrows SET status = 0 WHERE id = 2; -- Mark LOST: trigger applies penalty

-- Bob borrows copy 3 and returns it
INSERT INTO dbo.Borrows (user_id, book_copy_id) VALUES (2,3);
UPDATE dbo.Borrows SET status = 2, return_date = SYSUTCDATETIME() WHERE id = 3;

-- Bob borrows copy 4 and returns it
INSERT INTO dbo.Borrows (user_id, book_copy_id) VALUES (2,4);
UPDATE dbo.Borrows SET status = 2, return_date = SYSUTCDATETIME() WHERE id = 4;

-- Carol borrows copy 5 and returns it
INSERT INTO dbo.Borrows (user_id, book_copy_id) VALUES (3,5);
UPDATE dbo.Borrows SET status = 2, return_date = SYSUTCDATETIME() WHERE id = 5;

-- David borrows copy 6 (active, not returned)
INSERT INTO dbo.Borrows (user_id, book_copy_id) VALUES (4,6);

-- Eve borrows copy 7 and returns it
INSERT INTO dbo.Borrows (user_id, book_copy_id) VALUES (5,7);
UPDATE dbo.Borrows SET status = 2, return_date = SYSUTCDATETIME() WHERE id = 7;

-- Eve borrows copy 8, then loses it -> penalty applied by trigger
INSERT INTO dbo.Borrows (user_id, book_copy_id) VALUES (5,8);
UPDATE dbo.Borrows SET status = 0 WHERE id = 8; -- Mark LOST: trigger applies penalty

-- End seed
