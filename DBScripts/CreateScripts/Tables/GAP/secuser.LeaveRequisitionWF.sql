USE [JDE_PRODUCTION]
GO

/****** Object:  Table [secuser].[LeaveRequisitionWF]    Script Date: 08/03/2026 21:06:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [secuser].[LeaveRequisitionWF](
	[LeaveNo] [NUMERIC](18, 0) NOT NULL,
	[LeaveInstanceID] [VARCHAR](50) NULL,
	[LeaveReqTypeID] [INT] NULL,
	[LeaveReqTypeName] [VARCHAR](30) NULL,
	[LeaveEmpNo] [INT] NOT NULL,
	[LeaveEmpName] [VARCHAR](50) NOT NULL,
	[LeaveEmpEmail] [VARCHAR](150) NOT NULL,
	[LeaveEmpCostCenter] [VARCHAR](12) NULL,
	[LeaveRemarks] [VARCHAR](120) NULL,
	[LeaveJustification] [VARCHAR](100) NULL,
	[LeaveConstraints] [BIT] NULL,
	[LeaveReqStatusID] [INT] NULL,
	[LeaveReqStatusCode] [VARCHAR](10) NULL,
	[LeaveApprovalFlag] [CHAR](1) NULL,
	[LeaveVisaRequired] [BIT] NULL,
	[LeavePayAdv] [BIT] NULL,
	[LeaveIsFTMember] [BIT] NULL,
	[LeaveCreatedBy] [INT] NULL,
	[LeaveCreatedName] [VARCHAR](10) NULL,
	[LeaveCreatedEmail] [VARCHAR](150) NULL,
	[LeaveCreatedDate] [DATETIME] NULL,
	[LeaveModifiedBy] [INT] NULL,
	[LeaveModifiedName] [VARCHAR](10) NULL,
	[LeaveModifiedEmail] [VARCHAR](150) NULL,
	[LeaveModifiedDate] [DATETIME] NULL,
	[LeaveRetError] [INT] NULL,
	[LeaveSpecialApprover] [INT] NULL,
	[LeaveBypassSuperintendent] [BIT] NULL,
	[LeaveSpecialSupervisor] [INT] NULL,
	[LeaveIsNewWF] [BIT] NULL,
	[LeaveBypassCcManager] [BIT] NULL,
 CONSTRAINT [PK_LeaveRequisitionWF] PRIMARY KEY CLUSTERED 
(
	[LeaveNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [secuser].[LeaveRequisitionWF] ADD  CONSTRAINT [DF_LeaveRequisitionWF_LeaveConstraints]  DEFAULT ((0)) FOR [LeaveConstraints]
GO

ALTER TABLE [secuser].[LeaveRequisitionWF] ADD  CONSTRAINT [DF_LeaveRequisitionWF_LeaveVisaRequired]  DEFAULT ((0)) FOR [LeaveVisaRequired]
GO

ALTER TABLE [secuser].[LeaveRequisitionWF] ADD  CONSTRAINT [DF_LeaveRequisitionWF_LeavePayAdv]  DEFAULT ((0)) FOR [LeavePayAdv]
GO

ALTER TABLE [secuser].[LeaveRequisitionWF] ADD  CONSTRAINT [DF_LeaveRequisitionWF_LeaveIsFTMember]  DEFAULT ((0)) FOR [LeaveIsFTMember]
GO

ALTER TABLE [secuser].[LeaveRequisitionWF] ADD  CONSTRAINT [DF_LeaveRequisition1_LeaveRetError]  DEFAULT ((0)) FOR [LeaveRetError]
GO

ALTER TABLE [secuser].[LeaveRequisitionWF] ADD  DEFAULT ((0)) FOR [LeaveSpecialApprover]
GO

ALTER TABLE [secuser].[LeaveRequisitionWF] ADD  DEFAULT ((0)) FOR [LeaveBypassSuperintendent]
GO

ALTER TABLE [secuser].[LeaveRequisitionWF] ADD  DEFAULT ((0)) FOR [LeaveSpecialSupervisor]
GO

ALTER TABLE [secuser].[LeaveRequisitionWF] ADD  DEFAULT ((0)) FOR [LeaveIsNewWF]
GO


