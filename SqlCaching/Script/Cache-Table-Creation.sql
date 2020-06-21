CREATE TABLE [dbo].[Cache] (
    [Key]                            NVARCHAR (250) NOT NULL,
    [Value]                          NVARCHAR (MAX) NOT NULL,
    [Created]                        datetimeoffset        NOT NULL,
    [LastAccess]                     datetimeoffset       NOT NULL,
    [SlidingExpirationTimeInMinutes] INT            NULL,
    [AbsoluteExpirationTime]         datetimeoffset        NULL,
    [ObjectType]                     NVARCHAR (250) NOT NULL, 
    CONSTRAINT [PK_Cache] PRIMARY KEY ([Key])
);