CREATE OR ALTER TRIGGER [CreateAccountLedgerTrigger]
ON [Account]
FOR INSERT
AS
BEGIN
	DECLARE @accountId int;
	SET @accountId = (SELECT id FROM INSERTED);

	DECLARE @newLedgerTableName nvarchar(max);
	SET @newLedgerTableName = N'AccountLedger_' + convert(nvarchar(max), @accountId);

	DECLARE @createTableSql nvarchar(max);
	SET @createTableSql = N'
		CREATE TABLE ' + @newLedgerTableName + N' (
			[Id] bigint NOT NULL PRIMARY KEY IDENTITY,
			[Transaction] money NOT NULL,
			[TimeStamp] datetimeoffset(7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
			[Description] nvarchar(max)
		);
	';
	EXEC (@createTableSql);

	DECLARE @createAccountTrigger nvarchar(max);
	SET @createAccountTrigger = N'
		CREATE TRIGGER [AccountId_'+ convert(nvarchar(6), @accountId) + N'_InsertDeleteTrigger]
		ON [' + @newLedgerTableName + N']
		FOR INSERT, DELETE
		AS
		BEGIN
			DECLARE @transaction money;
			DECLARE @action smallint;
			DECLARE @id bigint;

			DECLARE @ins int;
			SET @ins = (SELECT count(0) FROM INSERTED);
			IF(@ins > 0) BEGIN
				SET @transaction = (SELECT [Transaction] FROM INSERTED);
				SET @action = 1;
				SET @id = (SELECT [Id] FROM INSERTED);
			END
			ELSE BEGIN
				SET @transaction = (SELECT ([Transaction] * -1) FROM DELETED);
				SET @action = 3;
				SET @id = (SELECT [Id] FROM DELETED);
			END

			UPDATE [Account]
			SET [Money] = [Money] + @transaction
			WHERE [Id] = ' + convert(nvarchar(6), @accountId) + N';

			INSERT INTO [AuditLog]([AccountId], [LedgerId], [Action], [Transaction])
			VALUES(' + convert(nvarchar(6), @accountId) + N', @id, @action, @transaction);
		END
	';
	EXEC (@createAccountTrigger);

	SET @createAccountTrigger = N'
		CREATE TRIGGER [AccountId_'+ convert(nvarchar(6), @accountId) + N'_UpdateTrigger]
		ON [' + @newLedgerTableName + N']
		FOR UPDATE
		AS
		BEGIN
			DECLARE @id bigint;
			SET @id = (SELECT [Id] FROM INSERTED);

			DECLARE @transaction money;
			SET @transaction = (SELECT [Transaction] FROM INSERTED);

			DECLARE @oldTransaction money;
			SET @oldTransaction = (SELECT [Transaction] FROM DELETED);

			UPDATE [Account]
			SET [Money] = [Money] + (@transaction - @oldTransaction)
			WHERE [Id] = '+ convert(nvarchar(6), @accountId) + N';

			INSERT INTO [AuditLog]([AccountId], [LedgerId], [Action], [Transaction])
			VALUES(' + convert(nvarchar(6), @accountId) + N', @id, 2, @transaction);
		END
	';
	EXEC (@createAccountTrigger);
END