--Step 1
-- create database
CREATE DATABASE [ConfigManager]


--Step 2
-- change owner to configuser
USE [ConfigManager]
GO
ALTER AUTHORIZATION ON DATABASE::[ConfigManager] TO [configuser]	--don't forget json config file...
GO

--Step 3
-- set recovery model to simple 
ALTER DATABASE [ConfigManager] SET RECOVERY SIMPLE 
GO

--Step 4
USE [ConfigManager]
GO
/****** Object:  Table [dbo].[Parameter]    Script Date: 6/17/2020 11:58:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parameter](
	[Guid] [nvarchar](50) NOT NULL,
	[Username] [nvarchar](50) NULL,
	[Application] [nvarchar](50) NULL,
	[KeyName] [nvarchar](50) NULL,
	[KeyValue] [nvarchar](500) NULL,
	[InsertDateTime] [bigint] NULL,
	[Notes] [nvarchar](50) NULL,
	[LastProcessDateTime] [bigint] NULL,
	[IsDeleted] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 6/17/2020 11:58:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[UserGuid] [nvarchar](50) NOT NULL,
	[Password] [varchar](max) NULL,
	[IsActive] [int] NOT NULL,
	[InsertDateTime] [bigint] NOT NULL,
	[LastProcessDateTime] [bigint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Parameter] ([Guid], [Username], [Application], [KeyName], [KeyValue], [InsertDateTime], [Notes], [LastProcessDateTime], [IsDeleted]) VALUES (N'e71841eb-fd19-4e78-b79a-8b293f27ade3', N'ConfigManagerService@gmail.com', N'ConfigManagerService', N'Debug', N'eWBb2p1Zi4x1G8dbmb5vnA==', 1580382002, N'', 1580382019, 0)
GO
INSERT [dbo].[Parameter] ([Guid], [Username], [Application], [KeyName], [KeyValue], [InsertDateTime], [Notes], [LastProcessDateTime], [IsDeleted]) VALUES (N'e71841eb-fd19-4e78-b79a-8b293f27ade3', N'ConfigManagerService@gmail.com', N'ConfigManagerService', N'MultiThreadOperation', N'TW1mZ/5HG88=', 1591022315, N'', 1591022315, 0)
GO
INSERT [dbo].[Parameter] ([Guid], [Username], [Application], [KeyName], [KeyValue], [InsertDateTime], [Notes], [LastProcessDateTime], [IsDeleted]) VALUES (N'e71841eb-fd19-4e78-b79a-8b293f27ade3', N'ConfigManagerService@gmail.com', N'ConfigManagerService', N'NetTcpAddress', N'dnPAVUlQSxJQf2TPuxffKqspWT9FYF7e2AxmKqPh88VlNrUvTqclNPp6ybFjofXx', 1591022459, N'', 1591022459, 0)
GO
INSERT [dbo].[Parameter] ([Guid], [Username], [Application], [KeyName], [KeyValue], [InsertDateTime], [Notes], [LastProcessDateTime], [IsDeleted]) VALUES (N'e71841eb-fd19-4e78-b79a-8b293f27ade3', N'ConfigManagerService@gmail.com', N'ConfigManagerService', N'HttpAddress', N'zPorBN6tPRp9APVaiDi/2w6NxEBzjLMnzlL6c0E0+vFoc4OyQcA7nbyROQXcxAbk', 1591022476, N'', 1591022476, 0)
GO
INSERT [dbo].[Parameter] ([Guid], [Username], [Application], [KeyName], [KeyValue], [InsertDateTime], [Notes], [LastProcessDateTime], [IsDeleted]) VALUES (N'e71841eb-fd19-4e78-b79a-8b293f27ade3', N'ConfigManagerService@gmail.com', N'ConfigManagerService', N'SqlConnectionString', N'me3SRRas9QGHcjqWCbr0bwW7bfN6KS4aiqAkMUSRpUwxVwh1LZasz5hZPfFx0HtL6W1wHPOBbQdsYpyTviTuNXLheyQHEkD3OzgHyBB6qP79/P3DI1M+sVjmihXge0GjVZwosXUo70EDJGFMkbrThw==', 1591028614, N'', 1591028614, 0)
GO
INSERT [dbo].[Parameter] ([Guid], [Username], [Application], [KeyName], [KeyValue], [InsertDateTime], [Notes], [LastProcessDateTime], [IsDeleted]) VALUES (N'e71841eb-fd19-4e78-b79a-8b293f27ade3', N'ConfigManagerService@gmail.com', N'ConfigManagerService', N'Prm1', N'oysPbWPXvOU=', 1591358118, N'', 1591358118, 0)
GO
INSERT [dbo].[Parameter] ([Guid], [Username], [Application], [KeyName], [KeyValue], [InsertDateTime], [Notes], [LastProcessDateTime], [IsDeleted]) VALUES (N'e71841eb-fd19-4e78-b79a-8b293f27ade3', N'ConfigManagerService@gmail.com', N'ConfigManagerService', N'Prm2', N'EoO+exLhwuk=', 1591443659, N'', 1591443659, 0)
GO
INSERT [dbo].[Permissions] ([UserGuid], [Password], [IsActive], [InsertDateTime], [LastProcessDateTime]) VALUES (N'e71841eb-fd19-4e78-b79a-8b293f27ade3', N'gXtZabfgqz5jz+f9+9vubw==', 1, 1591094085, 1591094085)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AplicationSettings_User_Application_Key_Index]    Script Date: 6/17/2020 11:58:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_AplicationSettings_User_Application_Key_Index] ON [dbo].[Parameter]
(
	[Guid] ASC,
	[Application] ASC,
	[KeyName] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserGuid_Index]    Script Date: 6/17/2020 11:58:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_UserGuid_Index] ON [dbo].[Permissions]
(
	[UserGuid] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Parameter] ADD  CONSTRAINT [DF_Table_1_Guid]  DEFAULT (newid()) FOR [Application]
GO
ALTER TABLE [dbo].[Parameter] ADD  CONSTRAINT [DF_AplicationSettings_InsertDateTime]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getdate())) FOR [InsertDateTime]
GO
ALTER TABLE [dbo].[Parameter] ADD  CONSTRAINT [DF_AplicationSettings_Notes]  DEFAULT ('') FOR [Notes]
GO
ALTER TABLE [dbo].[Parameter] ADD  CONSTRAINT [DF_AplicationSettings_LastProcessDateTime]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getdate())) FOR [LastProcessDateTime]
GO
ALTER TABLE [dbo].[Parameter] ADD  CONSTRAINT [DF_AplicationSettings_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [DF_Permissions_Password]  DEFAULT ('') FOR [Password]
GO
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [DF_Permissions_Permissipn]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [DF_Permissions_InsertDateTime]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getdate())) FOR [InsertDateTime]
GO
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [DF_Permissions_LastProcessDateTime]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getdate())) FOR [LastProcessDateTime]
GO
