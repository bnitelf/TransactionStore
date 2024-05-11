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

CREATE TABLE [PaymentTransactionUploadLog] (
    Id bigint IDENTITY(1,1),
    RequestId nvarchar(36) NOT NULL, -- in case we want to store multiple message per RequestId so don't make it unique.
    CreateDt datetime2(7) NOT NULL,
    ErrorMessage nvarchar(max) NOT NULL,

    PRIMARY KEY (Id)
);

-- DROP TABLE [PaymentTransaction];
-- DROP TABLE [PaymentTransactionUploadLog];

