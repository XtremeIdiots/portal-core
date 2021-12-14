CREATE TABLE [dbo].[Players](
	[Id] [uniqueidentifier] NOT NULL,
	[GameType] NVARCHAR(50) NOT NULL,
	[Username] [nvarchar](max) NULL,
	[Guid] [nvarchar](50) NULL,
	[FirstSeen] [datetime] NOT NULL,
	[LastSeen] [datetime] NOT NULL,
	[IpAddress] [nvarchar](50) NULL, 
    CONSTRAINT [PK_Players] PRIMARY KEY ([Id]),
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
