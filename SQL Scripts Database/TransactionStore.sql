

CREATE DATABASE TransactionStore
    COLLATE SQL_Latin1_General_CP1_CI_AS;
GO

USE TransactionStore;

CREATE TABLE [PaymentTransaction] (
    PaymentTransactionId bigint IDENTITY(1,1),
    TransactionId nvarchar(50) NOT NULL, 
    Amount decimal(19,4) NOT NULL,
    CurrencyCode nvarchar(3) NOT NULL,
    TransactionDt datetime2(7) NOT NULL,
    StatusRaw nvarchar(50) NOT NULL,
    Status nvarchar(10) NOT NULL,
    CreatedDt datetime2(7) NOT NULL,
    UpdatedDt datetime2(7) NOT NULL,

    PRIMARY KEY (PaymentTransactionId),
    CONSTRAINT TransactionID_UNIQ UNIQUE(TransactionId) 
);

-- DROP TABLE [PaymentTransaction];

