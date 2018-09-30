CREATE TABLE [dbo].[Cache] (
    [Key]                            NVARCHAR (250) NOT NULL,
    [Value]                          NVARCHAR (MAX) NOT NULL,
    [Created]                        DATETIME       NOT NULL,
    [LastAccess]                     DATETIME       NOT NULL,
    [SlidingExpirationTimeInMinutes] INT            NULL,
    [AbsoluteExpirationTime]         DATETIME       NULL,
    [ObjectType]                     NVARCHAR (250) NOT NULL, 
    CONSTRAINT [PK_Cache] PRIMARY KEY ([Key])
);