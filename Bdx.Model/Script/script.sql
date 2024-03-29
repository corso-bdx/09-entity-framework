﻿USE BDX
GO
CREATE SCHEMA Bdx
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TABLE [Bdx].[Ruoli] (
	[Nome] [varchar](255) NOT NULL,
	[Descrizione] [varchar](255) NOT NULL,
	[Categoria] [varchar](255) NOT NULL
,CONSTRAINT [PK_Ruoli] PRIMARY KEY CLUSTERED 
(
	[Nome] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]
GO


CREATE TABLE [Bdx].[Utenti] (
	[NomeUtente] [varchar](255) NOT NULL,
	[Nome] [varchar](255) NOT NULL,
	[Cognome] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Salt] [varchar](255) NOT NULL,
	[NomeRuolo] [varchar](255) NOT NULL,
	[DataCreazione] [datetime] NOT NULL,
	[DataUltimoCambioPassword] [datetime] NOT NULL
,CONSTRAINT [PK_Utente] PRIMARY KEY CLUSTERED 
(
	[NomeUtente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
,CONSTRAINT [FK_Utenti_NomeRuolo] FOREIGN KEY ([NomeRuolo]) REFERENCES [Bdx].[Ruoli] ([Nome]) ON DELETE NO ACTION ON UPDATE NO ACTION

) ON [PRIMARY]
GO

CREATE TABLE [Bdx].[Accessi] (
	[ID] int NOT NULL IDENTITY(1,1),
	[NomeUtente] [varchar](255) NOT NULL,
	[Data] [datetime] NOT NULL
,CONSTRAINT [PK_Accessi] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
,CONSTRAINT [FK_Accessi_NomeUtente] FOREIGN KEY ([NomeUtente]) REFERENCES [Bdx].[Utenti] ([NomeUtente]) ON DELETE NO ACTION ON UPDATE NO ACTION
) ON [PRIMARY]
GO







