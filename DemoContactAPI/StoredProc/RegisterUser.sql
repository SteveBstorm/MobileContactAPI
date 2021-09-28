CREATE PROCEDURE [dbo].[RegisterUser]
	@Email VARCHAR(50),
	@Password VARCHAR(50),
	@IsAdmin BIT
AS
BEGIN
	DECLARE @Salt VARCHAR(100);
	SET @Salt = CONCAT(NEWID(), NEWID());

	DECLARE @secretKey VARCHAR(50);
	SET @secretKey = [dbo].[GetSecretKey]();

	DECLARE @Password_Hash VARBINARY(64);
	SET @Password_Hash = HASHBYTES('SHA2_512', CONCAT(@Salt, @Password, @secretKey, @Salt))

	INSERT INTO [Users] (Email, [Password], IsAdmin, Salt)
	VALUES (@Email, @Password_Hash, @IsAdmin, @Salt)
END

