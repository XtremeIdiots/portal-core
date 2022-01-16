CREATE TABLE [dbo].[Players](
	[Id] [uniqueidentifier] NOT NULL DEFAULT newsequentialid(),
	[GameType] NVARCHAR(50) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[FirstSeen] [datetime] NOT NULL,
	[LastSeen] [datetime] NOT NULL,
	[IpAddress] [nvarchar](50) NULL, 
    CONSTRAINT [PK_Players] PRIMARY KEY ([Id]),
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
