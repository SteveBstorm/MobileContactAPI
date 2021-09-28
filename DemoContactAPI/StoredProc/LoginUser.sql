CREATE PROCEDURE [dbo].[LoginUser]
	@Email VARCHAR(50),
	@Password VARCHAR(50)
AS
BEGIN
	DECLARE @Salt VARCHAR(100);
	SET @Salt = (SELECT Salt FROM Users WHERE Email = @Email)

	IF @Salt IS NOT NULL
	BEGIN

		DECLARE @secretKey VARCHAR(50);
		SET @secretKey = [dbo].[GetSecretKey]();

		DECLARE @Password_Hash VARBINARY(64);
		SET @Password_Hash = HASHBYTES('SHA2_512', CONCAT(@Salt, @Password, @secretKey, @Salt))
	
		SELECT Id, Email, IsAdmin FROM Users 
		WHERE [Password] = @Password_Hash AND Email = @Email
	END
END
