select * from ABCHardware.Sale

select * from ABCHardware.Customer
drop PROCEDURE ABCHardware.AddSale


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
			SET @NewSaleNumber = @@IDENTITY

			IF @@ERROR = 0
				SET @ReturnCode = 0
			ELSE
				RAISERROR('AddSale - INSERT Error: Sale table.', 16, 1)
		END

    RETURN @ReturnCode
END







DECLARE @NewSaleNumber INT;

EXEC ABCHardware.AddSale 'John Doe', 1,100.00, 5.00, 105.00, @NewSaleNumber = @NewSaleNumber OUTPUT;

-- Use @NewSaleNumber as needed





--4--
CREATE TABLE ABCHardware.SaleItem
(
	SaleNumber	INT NOT NULL,
	ItemCode	VARCHAR(6)  NOT NULL,
	Quantity	INT         NOT NULL,
	ItemTotal	MONEY       NOT NULL
)



	ItemCode	VARCHAR(7)  NOT NULL,
	Description	VARCHAR(100) NOT NULL,
	UnitPrice	MONEY		NOT NULL,
	StockBal	INT			NOT NULL,
	StockFlag   BIT			NOT NULL
