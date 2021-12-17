CREATE TABLE [dbo].[ChatMessages]
(
	[Id] UNIQUEIDENTIFIER NOT NULL  DEFAULT newsequentialid(), 
    [GameServerId] NVARCHAR(50) NOT NULL, 
    [PlayerId] UNIQUEIDENTIFIER NOT NULL, 
    [Username] NVARCHAR(50) NOT NULL, 
    [Message] NVARCHAR(MAX) NOT NULL, 
    [Type] NVARCHAR(50) NOT NULL, 
    [Timestamp] DATETIME NOT NULL, 
    CONSTRAINT [PK_ChatMessages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ChatMessages_GameServer] FOREIGN KEY ([GameServerId]) REFERENCES [dbo].[GameServers] ([Id]),
    CONSTRAINT [FK_ChatMessages_Player] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Players] ([Id])
)
