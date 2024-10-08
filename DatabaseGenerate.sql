USE [master];

DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
FROM sys.dm_exec_sessions
WHERE database_id  = db_id('OT_Assessment_DB')

EXEC(@kill);

-- DROP
ALTER DATABASE [OT_Assessment_DB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [OT_Assessment_DB];

-- CREATE
CREATE DATABASE [OT_Assessment_DB];

-- USE
USE OT_Assessment_DB;
GO

CREATE TABLE accounts (
    accountId UNIQUEIDENTIFIER PRIMARY KEY,               -- Unique identifier for each account
    username VARCHAR(255) UNIQUE,             -- Ensure each username is unique
    totalAmountSpend DECIMAL(18, 6)           -- Total amount the user has spent
);

-- Index for performance
CREATE INDEX idx_accounts_username ON accounts(username);

-- Create Tables
CREATE TABLE wagers (
    wagerId UNIQUEIDENTIFIER PRIMARY KEY,
    theme VARCHAR(255),
    transactionId UNIQUEIDENTIFIER NOT NULL,               -- Link to the transactions table
    brandId UNIQUEIDENTIFIER ,
    accountId UNIQUEIDENTIFIER  NOT NULL,                   -- Link to the accounts table
    externalReferenceId UNIQUEIDENTIFIER,
    transactionTypeId UNIQUEIDENTIFIER,
    createdDateTime DATETIME,
    numberOfBets INT,
    countryCode VARCHAR(10),
    sessionData TEXT,
    duration BIGINT,
    FOREIGN KEY (accountId) REFERENCES accounts(accountId),  -- Reference to accounts table
);

-- Indexes for performance
CREATE INDEX idx_wagers_accountId ON wagers(accountId);


CREATE TABLE transactions (
    transactionId UNIQUEIDENTIFIER PRIMARY KEY,            -- Unique identifier for each transaction
    wagerId UNIQUEIDENTIFIER NOT NULL,                     -- Link to the wagers table
    gameName VARCHAR(255),
    provider VARCHAR(255),
    amount DECIMAL(18, 6),
    createdDateTime DATETIME,
    FOREIGN KEY (wagerId) REFERENCES wagers(wagerId)
);

-- Index for performance
CREATE INDEX idx_transactions_wagerId ON transactions(wagerId);
