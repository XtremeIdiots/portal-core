CREATE TABLE [dbo].[GameServers]
(
	[Id] NVARCHAR(50) NOT NULL , 
    [Title] [nvarchar](50) NOT NULL,
    [GameType] NVARCHAR(50) NOT NULL,
    [IpAddress] NVARCHAR(50) NOT NULL,
    [QueryPort] [int] NOT NULL, 
    [HasFtp] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_GameServers] PRIMARY KEY ([Id]),
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
