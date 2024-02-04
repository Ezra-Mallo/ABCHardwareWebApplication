sp_helpuser
USE emallo1

CREATE SCHEMA ABCHardware AUTHORIZATION dbo

SELECT sys.schemas.name AS SchemaName,
	   sys.database_principals.name as PrincipalName
FROM sys.schemas
	INNER JOIN sys.database_principals
	    ON sys.schemas.principal_id = sys.database_principals.principal_id --ANSI 92 standard

--OR--

SELECT sys.schemas.name AS SchemaName,
	   sys.database_principals.name as PrincipalName
FROM sys.schemas, sys.database_principals
WHERE  sys.schemas.principal_id = sys.database_principals.principal_id --ANSI 92 standard

/*
DROP SCHEMA CAN only happen when the Schema has no content
*/
ALTER SCHEMA dbo TRANSFER ABCHardware.Customer

DROP SCHEMA ABCHardware



Drop table ABCHardware.SaleItem
Drop table ABCHardware.Sale
Drop table ABCHardware.Item
Drop table ABCHardware.Customer

DROP PROCEDURE ABCHardware.AddItem
DROP PROCEDURE ABCHardware.UpdateItem


--1--
CREATE TABLE ABCHardware.Customer
(
	CustomerID		Integer		IDENTITY(1,1),
	FirstName		VARCHAR(25)	NOT NULL,
	LastName		VARCHAR(25)	NOT NULL,
	Address			VARCHAR(25) NOT NULL,	
	City			VARCHAR(25) NOT NULL,	
	Province		VARCHAR(25) NOT NULL,	
	PostalCode		VARCHAR(7)  NOT NULL
)
ALTER TABLE ABCHardware.Customer
	ADD CONSTRAINT PK_Customer PRIMARY KEY (CustomerID),
		CONSTRAINT CK_PostalCode CHECK (PostalCode LIKE '[A-Z][0-9][A-Z] [0-9][A-Z][0-9]');





--2--
CREATE TABLE ABCHardware.Item
(
	ItemCode	VARCHAR(6)  NOT NULL,
	Description	VARCHAR(50) NOT NULL,
	UnitPrice	MONEY		NOT NULL,
	StockBal	INT			NOT NULL,
	StockFlag   BIT			NOT NULL
)
ALTER TABLE ABCHardware.Item
    ADD CONSTRAINT PK_Items PRIMARY KEY (ItemCode),		
		CONSTRAINT CK_Items_ItemCode CHECK (ItemCode LIKE '[A_Z][0-9][0-9][0-9][0-9][0-9]'),
		CONSTRAINT CK_Items_UnitPrice CHECK (UnitPrice >= 0.00)





--3--
CREATE TABLE ABCHardware.Sale
(
	SaleNumber		INT			IDENTITY(1,1) NOT NULL,
	SaleDate		DATETIME	DEFAULT (GETDATE()) NOT NULL,
	Salesperson		VARCHAR(25)	NOT NULL,
	CustomerID		Integer		NOT NULL,	
	SubTotal        MONEY       NOT NULL,
	GST             MONEY       NOT NULL,
	SaleTotal       MONEY       NOT NULL
)
ALTER TABLE ABCHardware.Sale
	ADD CONSTRAINT PK_Sale PRIMARY KEY (SaleNumber),
		CONSTRAINT FK_Customer FOREIGN KEY (CustomerID) REFERENCES ABCHardware.Customer(CustomerID),
		CONSTRAINT CK_SaleNumber CHECK (SaleNumber >= 1 AND SaleNumber <= 999999999),
		CONSTRAINT CK_SubTotal CHECK (SubTotal >= 0.00),
		CONSTRAINT CK_GST CHECK (GST >= 0.00),
		CONSTRAINT CK_SaleTotal CHECK (SaleTotal >= 0.00);





--4--
CREATE TABLE ABCHardware.SaleItem
(
	SaleNumber	INT NOT NULL,
	ItemCode	VARCHAR(6)  NOT NULL,
	Quantity	INT         NOT NULL,
	ItemTotal	MONEY       NOT NULL
)
ALTER TABLE ABCHardware.SaleItem
    ADD CONSTRAINT PK_SaleItem PRIMARY KEY (SaleNumber, ItemCode),
    CONSTRAINT FK_SaleItem_SaleNumber FOREIGN KEY (SaleNumber) REFERENCES ABCHardware.Sale(SaleNumber),
    CONSTRAINT FK_SaleItem_ItemCode FOREIGN KEY (ItemCode) REFERENCES ABCHardware.Item(ItemCode),
	CONSTRAINT CK_SaleItem_Quantity CHECK (Quantity >= 0),
    CONSTRAINT CK_SaleItem_ItemTotal CHECK (ItemTotal >= 0.00)





--5--
INSERT INTO ABCHardware.Customer
    (FirstName, LastName,  Address, City, Province, PostalCode)
VALUES
	('Ezra','Mallo', '128th str, 160 Ave','Edmonton','Alberta','T6L 1L6'),
	('Philip','Udeh', '129th str, 160 Ave','Edmonton','Alberta','T6L 1L6')





--6--
INSERT INTO ABCHardware.Item
    (ItemCode, Description, UnitPrice,  StockBal, StockFlag)
VALUES
	('A00001', 'Book', 20.00, 100, 1),
	('A00002', 'Pen', 10, 100, 1)





--7--
INSERT INTO ABCHardware.Sale
    (Salesperson,  CustomerID, SubTotal, GST, SaleTotal)
VALUES
	('Uche',  1, 300, 15.00, 100)





--8--
INSERT INTO ABCHardware.SaleItem
    (SaleNumber, ItemCode, Quantity,  ItemTotal)
VALUES
	(1, 'A00001', 2, 5)





SELECT * FROM ABCHardware.Customer
SELECT * FROM ABCHardware.Item
SELECT * FROM ABCHardware.Sale
SELECT * FROM ABCHardware.SaleItem






--9--
CREATE PROCEDURE ABCHardware.AddItem(@ItemCode VARCHAR(6) = NULL, @Description VARCHAR(50) = NULL,
									 @UnitPrice	MONEY = NULL,  @StockBal INT = NULL,
									 @StockFlag BIT	= NULL)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @ItemCode IS NULL
        RAISERROR('AddItem  - required parameter: @ItemCode',16,1)
    ELSE
        IF @Description IS NULL
            RAISERROR('AddItem  - required parameter: @Description',16,1)
        ELSE
            IF @UnitPrice IS NULL
                RAISERROR('AddItem  - required parameter: @UnitPrice',16,1)
            ELSE	     
                IF @StockBal  IS NULL
                    RAISERROR('AddItem - required parameter: @StockBal ',16,1)
				ELSE
					IF @StockFlag IS NULL
						RAISERROR('AddItem - required parameter: @StockFlag',16,1)
					ELSE				
						BEGIN
							INSERT INTO ABCHardware.Item
								(ItemCode, Description, UnitPrice,  StockBal, StockFlag)
							VALUES
								(@ItemCode, @Description, @UnitPrice,  @StockBal, @StockFlag)
				  
							IF @@ERROR = 0
								SET @ReturnCode = 0
							ELSE
								RAISERROR('AddItem - INSERT Error: Items table.',16,1)
						END

	RETURN @ReturnCode





execute ABCHardware.AddItem 'A00004', 'Mathset', 10, 100, 1
SELECT * FROM ABCHardware.Item





--10--
CREATE PROCEDURE ABCHardware.UpdateItem(@ItemCode VARCHAR(6) = NULL, @Description VARCHAR(50) = NULL,
										@UnitPrice	MONEY = NULL,  @StockBal INT = NULL,
										@StockFlag BIT	= NULL)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @ItemCode IS NULL
        RAISERROR('UpdateItem  - required parameter: @ItemCode',16,1)
    ELSE
        IF @Description IS NULL
            RAISERROR('UpdateItem  - required parameter: @Description',16,1)
        ELSE
            IF @UnitPrice IS NULL
                RAISERROR('UpdateItem  - required parameter: @UnitPrice',16,1)
            ELSE	     
                IF @StockBal  IS NULL
                    RAISERROR('UpdateItem - required parameter: @StockBal ',16,1)
				ELSE
					IF @StockFlag IS NULL
						RAISERROR('UpdateItem - required parameter: @StockFlag',16,1)
					ELSE				
						BEGIN
							UPDATE ABCHardware.Item
							SET
								Description = @Description,
								UnitPrice = @UnitPrice,
								StockBal = @StockBal,
								StockFlag = @StockFlag
							WHERE 
								ItemCode = @ItemCode


							IF @@ERROR = 0
								SET @ReturnCode = 0
							ELSE
								RAISERROR('UpdateItem - Update Error: Items table.',16,1)
						END

	RETURN @ReturnCode





SELECT * FROM ABCHardware.Item
execute ABCHardware.UpdateItem 'A00003', 'Mathsets', 12, 100, 1








--11--
CREATE PROCEDURE ABCHardware.DeleteItem(@ItemCode VARCHAR(6) = NULL)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @ItemCode IS NULL
        RAISERROR('DeleteItem  - required parameter: @ItemCode',16,1)
    ELSE        			
		BEGIN
			DELETE FROM ABCHardware.Item
			WHERE 
				ItemCode = @ItemCode

			IF @@ERROR = 0
				SET @ReturnCode = 0
			ELSE
				RAISERROR('DeleteItem - DELETE Error: Items table.',16,1)
		END

	RETURN @ReturnCode





execute ABCHardware.DeleteItem 'A00004'	
SELECT * FROM ABCHardware.Item





--12--
CREATE PROCEDURE ABCHardware.AddCustomer(@FirstName	VARCHAR(25)	= NULL,	@LastName	 VARCHAR(25) = NULL, 
										 @Address	VARCHAR(25) = NULL,	@City		 VARCHAR(25) = NULL, 
										 @Province	VARCHAR(25) = NULL,	@PostalCode VARCHAR(7)  = NULL	)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @FirstName IS NULL
        RAISERROR('AddCustomer  - required parameter: @FirstName.',16,1)
    ELSE
        IF @LastName IS NULL
            RAISERROR('AddCustomer  - required parameter: @LastName.',16,1)
        ELSE	     
            IF @Address  IS NULL
                RAISERROR('AddCustomer - required parameter: @Address.',16,1)
			ELSE
				IF @City IS NULL
					RAISERROR('AddCustomer - required parameter: @City.',16,1)
				ELSE
					IF @Province IS NULL
						RAISERROR('AddCustomer - required parameter: @Province.',16,1)
					ELSE
						IF @PostalCode IS NULL
							RAISERROR('AddCustomer - required parameter: @PostalCode.',16,1)
						ELSE
							BEGIN						
							
								INSERT INTO ABCHardware.Customer
									(FirstName, LastName,  Address, City, Province, PostalCode)
								VALUES
									(@FirstName, @LastName,  @Address, @City, @Province, @PostalCode)
							
								IF @@ERROR = 0
									SET @ReturnCode = 0
								ELSE
									RAISERROR('AddCustomer  - INSERT Error: Customer table.',16,1)
							END

RETURN @ReturnCode





SELECT * FROM ABCHardware.Customer
execute ABCHardware.AddCustomer  'JohnPaul','Mallo', '130th str, 160 Ave','Edmonton','Alberta','T6L 1L6'
execute ABCHardware.AddCustomer  'Ifeanyi','Ani', '140th str, 160 Ave','Edmonton','Alberta','T6L 1L6'
execute ABCHardware.AddCustomer  'Ifeanyi','Ani', '140th str, 160 Ave','Edmonton','Alberta','T6L 1L6'





--13--
CREATE PROCEDURE ABCHardware.UpdateCustomer(@CustomerID INT,				@FirstName	VARCHAR(25)	= NULL,	
										    @LastName	VARCHAR(25) = NULL, @Address	VARCHAR(25) = NULL,	
										    @City		VARCHAR(25) = NULL, @Province	VARCHAR(25) = NULL,	
											@PostalCode VARCHAR(7)  = NULL)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @CustomerID IS NULL
        RAISERROR('UpdateCustomer  - required parameter: @CustomerID.',16,1)
    ELSE
		BEGIN						
			UPDATE ABCHardware.Customer
			SET
				FirstName = @FirstName,
				LastName = @LastName,
				Address = @Address,
				City = @City,
				Province = @Province,
				PostalCode = @PostalCode
			WHERE CustomerID = @CustomerID
			
			IF @@ERROR = 0
				SET @ReturnCode = 0
			ELSE
				RAISERROR('UpdateCustomer - Update Error: Customer table.',16,1)
		END

RETURN @ReturnCode





SELECT * FROM ABCHardware.Customer
execute ABCHardware.UpdateCustomer 3, 'JohnPaul','Mallo', '130th str, 170 Ave','Edmonton','Alberta','T6L 1L6'





--14--
CREATE PROCEDURE ABCHardware.DeleteCustomer(@CustomerID INT)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @CustomerID IS NULL
        RAISERROR('DeleteCustomer - required parameter: @CustomerID.',16,1)
    ELSE
		BEGIN						
			DELETE FROM ABCHardware.Customer
			WHERE CustomerID = @CustomerID
			
			IF @@ERROR = 0
				SET @ReturnCode = 0
			ELSE
				RAISERROR('DeleteCustomer - Delete Error: Customer table.',16,1)
		END

RETURN @ReturnCode





SELECT * FROM ABCHardware.Customer
execute ABCHardware.DeleteCustomer 5





-- AddItem - Adding an inventory item (2 marks)
-- UpdateItem - Updating an inventory item (2 marks)
-- DeleteItem - Deleting an inventory item (2 marks)
-- AddCustomer - Adding a customer (2 marks)
-- UpdateCustomer - Updating a customer (2 marks)
-- DeleteCustomer - Deleting a customer (2 marks)
o ProcessSale - Processing a sale (6 marks)
