CREATE PROCEDURE sp_UpdateBook
	@MyId INT,
	@Page INT
	AS
	BEGIN

	IF(EXISTS(SELECT * FROM Books WHERE Id=@MyId))
	BEGIN
	   PRINT 'I UPDATED'

	   UPDATE Books
	   SET Pages=@Page
	   WHERE Id=@MyId
	END
	ELSE
	   BEGIN
		PRINT 'Book did not find'

		ROLLBACK TRANSACTION

	   END


	END