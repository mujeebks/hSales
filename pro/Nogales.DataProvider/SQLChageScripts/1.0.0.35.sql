USE [NogalesDashboard]
GO


/****** Object:  Table [dbo].[Collector]    Script Date: 7/31/2018 9:15:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Collector](
	[CollectorId] [int] IDENTITY(1,1) NOT NULL,
	[CollectorName] [varchar](150) NOT NULL,
	[Ordinance] [int] NOT NULL,
 CONSTRAINT [PK_Collector] PRIMARY KEY CLUSTERED 
(
	[CollectorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CollectorAssignment]    Script Date: 7/31/2018 9:15:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollectorAssignment](
	[CollectorAssignmentId] [int] NOT NULL,
	[CollectorId] [int] NULL,
	[LetterName] [char](1) NOT NULL,
 CONSTRAINT [PK_CollectorAssignment] PRIMARY KEY CLUSTERED 
(
	[CollectorAssignmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[CollectorAssignment]  WITH CHECK ADD  CONSTRAINT [FK_Collector_CollectorAssignment] FOREIGN KEY([CollectorId])
REFERENCES [dbo].[Collector] ([CollectorId])
GO
ALTER TABLE [dbo].[CollectorAssignment] CHECK CONSTRAINT [FK_Collector_CollectorAssignment]
GO

--VALUES



INSERT INTO Collector VALUES ('Humberto Hernandez',1)
INSERT INTO Collector VALUES ('Enrique Pineda',2)
INSERT INTO Collector VALUES ('Claudia Campos',3)
INSERT INTO Collector VALUES ('Open Position',4)


INSERT INTO CollectorAssignment VALUES (1,1,'A')
INSERT INTO CollectorAssignment VALUES (2,1,'B')
INSERT INTO CollectorAssignment VALUES (3,1,'C')
INSERT INTO CollectorAssignment VALUES (4,1,'D')
INSERT INTO CollectorAssignment VALUES (5,1,'E')

INSERT INTO CollectorAssignment VALUES (6,2,'F')
INSERT INTO CollectorAssignment VALUES (7,2,'G')
INSERT INTO CollectorAssignment VALUES (8,2,'H')
INSERT INTO CollectorAssignment VALUES (9,2,'I')
INSERT INTO CollectorAssignment VALUES (10,2,'J')
INSERT INTO CollectorAssignment VALUES (11,2,'K')
INSERT INTO CollectorAssignment VALUES (13,2,'M')



INSERT INTO CollectorAssignment VALUES (14,3,'N')
INSERT INTO CollectorAssignment VALUES (15,3,'O')
INSERT INTO CollectorAssignment VALUES (16,3,'P')
INSERT INTO CollectorAssignment VALUES (17,3,'Q')
INSERT INTO CollectorAssignment VALUES (18,3,'R')
INSERT INTO CollectorAssignment VALUES (19,3,'S')
INSERT INTO CollectorAssignment VALUES (20,3,'T')


INSERT INTO CollectorAssignment VALUES (12,4,'L')
INSERT INTO CollectorAssignment VALUES (21,4,'U')
INSERT INTO CollectorAssignment VALUES (22,4,'V')
INSERT INTO CollectorAssignment VALUES (23,4,'W')
INSERT INTO CollectorAssignment VALUES (24,4,'X')
INSERT INTO CollectorAssignment VALUES (25,4,'Y')
INSERT INTO CollectorAssignment VALUES (26,4,'Z')


INSERT INTO CollectorAssignment VALUES (27,3,'0')
INSERT INTO CollectorAssignment VALUES (28,3,'1')
INSERT INTO CollectorAssignment VALUES (29,3,'2')
INSERT INTO CollectorAssignment VALUES (30,3,'3')
INSERT INTO CollectorAssignment VALUES (31,3,'4')
INSERT INTO CollectorAssignment VALUES (32,3,'5')
INSERT INTO CollectorAssignment VALUES (33,3,'6')
INSERT INTO CollectorAssignment VALUES (34,3,'7')
INSERT INTO CollectorAssignment VALUES (35,3,'8')
INSERT INTO CollectorAssignment VALUES (36,3,'9')