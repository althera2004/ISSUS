IF OBJECT_ID('[dbo].[AuditoryPlanning]', 'U')  IS NOT NULL DROP TABLE [dbo].[AuditoryPlanning]


CREATE TABLE [dbo].[AuditoryPlanning](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[AuditoryId] [bigint]  NOT NULL,
	[Date] [datetime]  NOT NULL,
	[Hour] [int]  NOT NULL,
	[Duration] [int]  NOT NULL,
	[ProcessId] [bigint]  NOT NULL,
	[Auditor] [int]  NOT NULL,
	[Audited] [int]  NOT NULL,
	[SendMail] [bit]  NOT NULL,
	[ProviderEmail] [nvarchar](150)  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]