-- Sample queries for test database

-- Get all users
SELECT * FROM users ORDER BY created_at DESC;

-- Count total users
SELECT COUNT(*) as total_users FROM users;

-- Find user by email
SELECT * FROM users WHERE email = $1;
