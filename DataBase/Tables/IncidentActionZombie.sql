IF OBJECT_ID('[dbo].[IncidentActionZombie]', 'U')  IS NOT NULL DROP TABLE [dbo].[IncidentActionZombie]


CREATE TABLE [dbo].[IncidentActionZombie](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[ActionType] [int]  NOT NULL,
	[Description] [nvarchar](100)  NOT NULL,
	[WhatHappend] [nvarchar](2000)  NOT NULL,
	[WhatHappendBy] [int]  NOT NULL,
	[WhatHappendOn] [datetime]  NOT NULL,
	[AuditoryId] [bigint]  NULL,
) ON [PRIMARY]