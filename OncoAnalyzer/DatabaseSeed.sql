drop table if exists Users;

CREATE TABLE Users
(
 Id INT PRIMARY KEY IDENTITY(1,1),
 Username NVARCHAR(50) NOT NULL UNIQUE,
 PasswordHash NVARCHAR(255) NOT NULL,
 Role NVARCHAR(50) NOT NULL
);

DECLARE @Password NVARCHAR(MAX);
DECLARE @PasswordHash VARBINARY(MAX);
DECLARE @PasswordHashBase64 NVARCHAR(MAX);

SET @Password = 'admin123';
SET @PasswordHash = HASHBYTES('SHA2_256', @Password);
SET @PasswordHashBase64 = CAST('' AS XML).value('xs:base64Binary(sql:variable("@PasswordHash"))','VARCHAR(MAX)');
INSERT INTO Users(Username, PasswordHash, Role)
VALUES ('admin', @PasswordHashBase64, 'Admin');


SET @Password = 'doctor123'
SET @PasswordHash = HASHBYTES('SHA2_256', @Password);
SET @PasswordHashBase64 = CAST('' AS XML).value('xs:base64Binary(sql:variable("@PasswordHash"))','VARCHAR(MAX)');
INSERT INTO Users(Username, PasswordHash, Role)
VALUES ('doctor', @PasswordHashBase64, 'Doctor');

SET @Password = 'staff123'
SET @PasswordHash = HASHBYTES('SHA2_256', @Password);
SET @PasswordHashBase64 = CAST('' AS XML).value('xs:base64Binary(sql:variable("@PasswordHash"))','VARCHAR(MAX)');
INSERT INTO Users(Username, PasswordHash, Role)
VALUES('staff', @PasswordHashBase64, 'Staff');

SELECT Username, PasswordHash, Role FROM Users;