USE [tas2]
GO

/****** Object:  Table [tas].[Tran_ShiftPatternUpdates]    Script Date: 30/01/2026 21:53:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [tas].[Tran_ShiftPatternUpdates](
	[AutoID] [INT] NOT NULL,
	[DateX] [DATETIME] NULL,
	[TxID] [INT] NULL,
	[EmpNo] [INT] NULL,
	[Effective_ShiftPatCode] [CHAR](2) NULL,
	[Effective_ShiftPointer] [INT] NULL,
	[Effective_ShiftCode] [CHAR](10) NULL,
	[Effective_BusinessUnit] [CHAR](12) NULL
) ON [PRIMARY]
GO


