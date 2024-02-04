sp_help ABCHardware;

sp_help ABCHardware 

sp_helpuser
 
Create Database FinalProjectDB
Use FinalProjectDB


USE emallo1

CREATE SCHEMA ABCHardware AUTHORIZATION dbo


SELECT sys.schemas.name AS SchemaName,
	   sys.database_principals.name as PrincipalName
FROM sys.schemas
	INNER JOIN sys.database_principals
	    ON sys.schemas.principal_id = sys.database_principals.principal_id --ANSI 92 standard


--same as 

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
	CustomerID		INT IDENTITY(1,1),
	FirstName		VARCHAR(25)	NOT NULL,
	LastName		VARCHAR(25)	NOT NULL,
	Address			VARCHAR(50) NOT NULL,	
	City			VARCHAR(25) NOT NULL,	
	Province		VARCHAR(25) NOT NULL,	
	PostalCode		VARCHAR(7)  NOT NULL
)
ALTER TABLE ABCHardware.Customer
	ADD CONSTRAINT PK_Customer PRIMARY KEY (CustomerID),
		CONSTRAINT CK_PostalCode CHECK (PostalCode LIKE '[A-Z][0-9][A-Z] [0-9][A-Z][0-9]');


		select * from ABCHardware.Item


--2--
CREATE TABLE ABCHardware.Item
(
	ItemCode	VARCHAR(7)  NOT NULL,
	Description	VARCHAR(100) NOT NULL,
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
	


--4--
CREATE TABLE ABCHardware.SalePersons
(
	Salesperson		VARCHAR(25)	NOT NULL,
	Password VARCHAR(10) NOT NULL
)
ALTER TABLE ABCHardware.SalePersons
    ADD CONSTRAINT PK_SalePerson PRIMARY KEY (Salesperson)
	




INSERT INTO ABCHardware.Customer
    (FirstName, LastName,  Address, City, Province, PostalCode)
VALUES
	('Ezra','Mallo', '128th str, 160 Ave','Edmonton','Alberta','T6L 1L6'),
	('Philip','Udeh', '129th str, 160 Ave','Edmonton','Alberta','T6L 1L6')


INSERT INTO ABCHardware.Item
    (ItemCode, Description, UnitPrice,  StockBal, StockFlag)
VALUES
	('A00001', 'Book', 20.00, 100, 1),
	('A00002', 'Pen', 10, 100, 1)





INSERT INTO ABCHardware.Sale
    (Salesperson,  CustomerID, SubTotal, GST, SaleTotal)
VALUES
	('Uche',  1, 300, 15.00, 100)
	

INSERT INTO ABCHardware.SaleItem
    (SaleNumber, ItemCode, Quantity,  ItemTotal)
VALUES
	(1, 'A00001', 2, 5)


INSERT INTO ABCHardware.SalePersons
    (Salesperson, Password)
VALUES
	('Ada','12345'),
	('Uche','12345'),



SELECT * FROM ABCHardware.Customer
SELECT * FROM ABCHardware.Item
SELECT * FROM ABCHardware.Sale
SELECT * FROM ABCHardware.SaleItem
SELECT * FROM ABCHardware.SalePersons

delete from ABCHardware.Customer where CustomerID=5

DROP PROCEDURE ABCHardware.AddItem
--9--
CREATE PROCEDURE ABCHardware.AddItem(@ItemCode VARCHAR(6) = NULL, @Description VARCHAR(100) = NULL,
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


execute ABCHardware.AddItem 'A00003', 'Mathset', 10, 100, 1








--10--
CREATE PROCEDURE ABCHardware.UpdateItem(@ItemCode VARCHAR(6) = NULL, @Description VARCHAR(100) = NULL,
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
								RAISERROR('UpdateItem - INSERT Error: Items table.',16,1)
						END

	RETURN @ReturnCode


SELECT * FROM ABCHardware.Item


execute ABCHardware.UpdateItem 'A00003', 'Mathsets', 12, 100, 1
	

----
CREATE PROCEDURE ABCHardware.FindItem(@ItemCode VARCHAR(6) = NULL)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @ItemCode IS NULL
        RAISERROR('FindItem  - required parameter: @ItemCode',16,1)
    ELSE
		BEGIN
			SELECT ItemCode, Description, UnitPrice, StockBal, StockFlag 
			FROM ABCHardware.Item 
			WHERE ItemCode = @ItemCode


			IF @@ERROR = 0
				SET @ReturnCode = 0
			ELSE
				RAISERROR('FindItem - Select Error: Items table.',16,1)
		END

	RETURN @ReturnCode

EXEC ABCHardware.FindItem 'a00001'




--11--
CREATE PROCEDURE ABCHardware.DeleteItem(@ItemCode VARCHAR(6) = NULL)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @ItemCode IS NULL
        RAISERROR('DeleteItem  - required parameter: @ItemCode',16,1)
    ELSE        			
		BEGIN
			UPDATE ABCHardware.Item
			SET
				StockFlag = 0
			WHERE 
				ItemCode = @ItemCode


			IF @@ERROR = 0
				SET @ReturnCode = 0
			ELSE
				RAISERROR('DeleteItem - INSERT Error: Items table.',16,1)
		END

	RETURN @ReturnCode

execute ABCHardware.DeleteItem 'A00007'	
SELECT * FROM ABCHardware.Item










drop PROCEDURE ABCHardware.AddCustomer

--9--
CREATE PROCEDURE ABCHardware.AddCustomer(@FirstName	VARCHAR(25)	= NULL,	@LastName   VARCHAR(25) = NULL, 
										 @Address	VARCHAR(50) = NULL,	@City		VARCHAR(25) = NULL, 
										 @Province	VARCHAR(25) = NULL,	@PostalCode VARCHAR(7)  = NULL)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @FirstName IS NULL
        RAISERROR('AddCustomer  - required parameter: @FirstName',16,1)
    ELSE
        IF @LastName IS NULL
            RAISERROR('AddCustomer  - required parameter: @LastName',16,1)
        ELSE	     
            IF @Address  IS NULL
                RAISERROR('AddCustomer - required parameter: @Address ',16,1)
			ELSE
				IF @City  IS NULL
					RAISERROR('AddCustomer - required parameter: @City ',16,1)
				ELSE				     
					IF @Province  IS NULL
						RAISERROR('AddCustomer - required parameter: @Province ',16,1)
					ELSE

						IF @PostalCode  IS NULL
							RAISERROR('AddCustomer - required parameter: @PostalCode ',16,1)
						ELSE
											
					
							BEGIN						
							
								INSERT INTO ABCHardware.Customer
									(FirstName, LastName,  Address, City, Province, PostalCode)
								VALUES
									(@FirstName, @LastName,  @Address, @City, @Province, @PostalCode)
							
							
				
								IF @@ERROR = 0
									SET @ReturnCode = 0
								ELSE
									RAISERROR('AddItem - INSERT Error: Items table.',16,1)
							END

	RETURN @ReturnCode


execute ABCHardware.AddCustomer 'Amalu','Kenechukwu', '129th str, 165 Ave','Edmonton','Alberta','T6L 1L6'


Select * from ABCHardware.Customer

	(2, 'Philip','Udeh', '129th str, 160 Ave','Edmonton','Alberta','T6L 1L6')
	(3, 'JohnPaul','Mallo', '130th str, 160 Ave','Edmonton','Alberta','T6L 1L6')
	(4, 'Ifeanyi','Ani', '140th str, 160 Ave','Edmonton','Alberta','T6L 1L6')




DROP PROCEDURE ABCHardware.FindCustomer

--9--
CREATE PROCEDURE ABCHardware.FindCustomer(@CustomerID INT = NULL)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @CustomerID IS NULL
        RAISERROR('FindCustomer  - required parameter: @CustomerID.',16,1)
    ELSE
        BEGIN						
							
			SELECT CustomerID, FirstName, LastName,  Address, City, Province, PostalCode
			FROM ABCHardware.Customer
			WHERE CustomerID = @CustomerID 
							
							
				
			IF @@ERROR = 0
				SET @ReturnCode = 0
			ELSE
				RAISERROR('FindCustomer - Select Error: Items table.',16,1)
		END

	RETURN @ReturnCode


  
exec ABCHardware.FindCustomer 1

drop PROCEDURE ABCHardware.UpdateCustomer

CREATE PROCEDURE ABCHardware.UpdateCustomer(@CustomerID INT	= NULL,         @FirstName	VARCHAR(25)	= NULL,	
										    @LastName   VARCHAR(25) = NULL, @Address	VARCHAR(50) = NULL,	
										    @City		VARCHAR(25) = NULL, @Province	VARCHAR(25) = NULL,	
											@PostalCode VARCHAR(7)  = NULL)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @CustomerID IS NULL
        RAISERROR('UpdateCustomer  - required parameter: @CustomerID',16,1)
    ELSE    
		IF @FirstName IS NULL
			RAISERROR('UpdateCustomer  - required parameter: @Description',16,1)
		ELSE
			IF @LastName IS NULL
				RAISERROR('UpdateCustomer  - required parameter: @LastName',16,1)
			ELSE	     
				IF @Address  IS NULL
					RAISERROR('UpdateCustomer - required parameter: @Address ',16,1)
				ELSE
					IF @City  IS NULL
						RAISERROR('UpdateCustomer - required parameter: @City ',16,1)
					ELSE				     
						IF @Province  IS NULL
							RAISERROR('UpdateCustomer - required parameter: @Province ',16,1)
						ELSE
							IF @PostalCode  IS NULL
								RAISERROR('UpdateCustomer - required parameter: @PostalCode ',16,1)
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


exec ABCHardware.UpdateCustomer 2, 'Philip Obina','Udeh', '129th str, 160 Ave','Edmonton','Alberta','T6L 1L6'

Select * from ABCHardware.Customer

	(2, 'Philip','Udeh', '129th str, 160 Ave','Edmonton','Alberta','T6L 1L6')



CREATE PROCEDURE ABCHardware.DeleteCustomer(@CustomerID INT = NULL)
AS  
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @CustomerID IS NULL
        RAISERROR('FindCustomer  - required parameter: @CustomerID.',16,1)
    ELSE
        BEGIN						
							
			DELETE 
			FROM ABCHardware.Customer
			WHERE CustomerID = @CustomerID 
							
							
				
			IF @@ERROR = 0
				SET @ReturnCode = 0
			ELSE
				RAISERROR('FindCustomer - Select Error: Items table.',16,1)
		END

	RETURN @ReturnCode


CREATE PROCEDURE ABCHardware.AddSale(@Salesperson VARCHAR(25) = NULL, @CustomerID INT = NULL,
									 @SubTotal MONEY = NULL,  @GST MONEY  = NULL,
									 @SaleTotal MONEY = NULL, @NewSaleNumber  INT OUTPUT)
AS
BEGIN
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    IF @Salesperson IS NULL
        RAISERROR('AddSale - required parameter: @Salesperson.', 16, 1)
    ELSE IF @CustomerID IS NULL
        RAISERROR('AddSale - required parameter: @CustomerID.', 16, 1)
    ELSE IF @SubTotal IS NULL
        RAISERROR('AddSale - required parameter: @SubTotal.', 16, 1)
    ELSE IF @GST IS NULL
        RAISERROR('AddSale - required parameter: @GST.', 16, 1)
    ELSE IF @SaleTotal IS NULL
        RAISERROR('AddSale - required parameter: @SaleTotal.', 16, 1)
    ELSE
		BEGIN
			-- Inserting a new record into the Sale table
			INSERT INTO ABCHardware.Sale
				(Salesperson, CustomerID, SubTotal, GST, SaleTotal)
			VALUES
				(@Salesperson, @CustomerID, @SubTotal, @GST, @SaleTotal)

			-- Return the SaleNumber of the inserted record
			SET @NewSaleNumber = SCOPE_IDENTITY()

			IF @@ERROR = 0
				SET @ReturnCode = 0
			ELSE
				RAISERROR('AddSale - INSERT Error: Sale table.', 16, 1)
		END

    RETURN @ReturnCode
END




















  










-- AddItem - Adding an inventory item (2 marks)
-- UpdateItem - Updating an inventory item (2 marks)
-- DeleteItem - Deleting an inventory item (2 marks)
-- AddCustomer - Adding a customer (2 marks)
o UpdateCustomer - Updating a customer (2 marks)
o DeleteCustomer - Deleting a customer (2 marks)
o ProcessSale - Processing a sale (6 marks)
