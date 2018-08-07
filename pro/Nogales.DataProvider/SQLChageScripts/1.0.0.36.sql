USE [NPICOMPANY01]
GO

/****** Object:  View [dbo].[BI_View_CUSTCOLLECTOR]    Script Date: 8/1/2018 1:27:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[BI_View_CUSTCOLLECTOR]
AS

--SELECT        custno, company, 
--                         CASE WHEN company LIKE '[A-E,a-e]%' THEN 'Humberto Hernandez' 
--							WHEN company LIKE '[F-K,f-k,M,m]%' THEN 'Enrique Pineda' 
--							WHEN company LIKE '[N-T,n-t,#,0-9]%' THEN 'Claudia Campos' 
--							WHEN company LIKE '[U-Z,u-z,L,l]%'
--                          THEN 'Open Position' END AS CollectorName
--FROM            dbo.arcust
	WITH CTE_COllector
	AS (
		SELECT 
			ca.LetterName
			, c.CollectorId
			, c.CollectorName 
			, c.Ordinance
		FROM TESTNogalesDashboard.dbo.[CollectorAssignment] ca
		LEFT JOIN TESTNogalesDashboard.dbo.[Collector] c
		ON ca.CollectorId = c.CollectorId
	)


	SELECT a.custno
	, a.company
	, c.CollectorName
	, c.Ordinance
	FROM arcust a
	JOIN CTE_COllector c
	ON SUBSTRING(a.company,1,1)   = c.LetterName COLLATE SQL_Latin1_General_CP1_CI_AS  

GO

/****** Object:  StoredProcedure [dbo].[BI_FI_GetCollectionInfoDashboardData]    Script Date: 8/1/2018 3:50:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*

exec [BI_FI_GetCollectionInfoDashboardData] '2018/06/01','2018/06/05','2018/05/01','2018/05/05','2017/06/01','2017/06/05'

*/

ALTER PROCEDURE [dbo].[BI_FI_GetCollectionInfoDashboardData] 
@currentStart DATETIME
,@currentEnd DATETIME
,@priorStart DATETIME
,@priorEnd DATETIME
,@historicalStart DATETIME
,@historicalEnd DATETIME


AS 
BEGIN 
DECLARE  @currentStart_P DATETIME
		,@currentEnd_P DATETIME
		,@priorStart_P DATETIME
		,@priorEnd_P DATETIME
		,@historicalStart_P DATETIME
		,@historicalEnd_P DATETIME

SET @currentStart_P = @currentStart
SET @currentEnd_P = @currentEnd
SET @priorStart_P = @priorStart
SET @priorEnd_P = @priorEnd
SET @historicalStart_P = @historicalStart
SET @historicalEnd_P = @historicalEnd


;WITH CTE_Core 
    AS (	SELECT am.invdte
				, am.pnet
				, am.pterms
				, DATEADD(Day,am.pnet,am.invdte) AS dueDate
				, am.dtepaid
				, am.invamt
				, am.paidamt
				, am.balance
				, DATEDIFF(Day,am.dtepaid,DATEADD(Day,am.pnet,am.invdte)) AS overDueDate
				, CASE WHEN DATEDIFF(Day,am.dtepaid,DATEADD(Day,am.pnet,am.invdte))>=0 THEN 100 ELSE 0 END AS payOnTime
				, CASE WHEN am.invamt=0 THEN 0 ELSE ((am.paidamt/am.invamt)*100) END AS totalCollection
				, cc.CollectorName as collector
				, cc.Ordinance as ordinance
			FROM BI_View_ARMAS am
			INNER JOIN BI_View_CUSTCOLLECTOR cc ON cc.custno = am.custno
			WHERE ((am.invdte BETWEEN @currentStart_P AND @currentEnd_P) 
					OR (am.invdte BETWEEN @priorStart_P AND @priorEnd_P) 
					OR (am.invdte BETWEEN @historicalStart_P AND @historicalEnd_P))
		)
	,CTE_Current AS (SELECT
							c.collector				AS Collector,
							c.ordinance				AS Ordinance,
							c.pnet					AS PNet,
							c.pTerms				AS PTerms,
							Sum(c.invamt)           AS InvoiceAmount,
							Sum(c.paidamt)			AS PaidAmount,
							AVG(c.payOnTime )		AS PaymentOnTime,
							AVG(c.totalCollection)  AS TotalCollection,
							'current' AS Period 
					FROM   CTE_Core c 
					WHERE c.invdte BETWEEN  @currentStart_P AND @currentEnd_P 
					GROUP  BY c.collector, c.pnet, c.pTerms, c.ordinance
					)
	,CTE_Prior AS (SELECT
							c.collector				AS Collector,
							c.ordinance				AS Ordinance,
							c.pnet					AS PNet,
							c.pTerms				AS PTerms,
							Sum(c.invamt)           AS InvoiceAmount,
							Sum(c.paidamt)			AS PaidAmount,
							AVG(c.payOnTime )		AS PaymentOnTime,
							AVG(c.totalCollection)  AS TotalCollection,
							'prior' AS Period 
					FROM   CTE_Core c 
					WHERE c.invdte BETWEEN  @priorStart_P AND @priorEnd_P
					GROUP  BY c.collector, c.pnet, c.pTerms, c.ordinance
				)
	,CTE_Historical AS (SELECT
								c.collector				AS Collector,
							c.ordinance					AS Ordinance,
								c.pnet					AS PNet,
								c.pTerms				AS PTerms,
								Sum(c.invamt)           AS InvoiceAmount,
								Sum(c.paidamt)			AS PaidAmount,
								AVG(c.payOnTime )		AS PaymentOnTime,
								AVG(c.totalCollection)  AS TotalCollection,
								'historical' AS Period 
						FROM   CTE_Core c 
						WHERE  c.invdte BETWEEN  @historicalStart_P AND @historicalEnd_P
						GROUP  BY c.collector, c.pnet, c.pTerms, c.ordinance
						)

	SELECT c1.Period, c1.Collector, c1.Ordinance, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Current c1
	UNION ALL
	SELECT c1.Period, c1.Collector, c1.Ordinance, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Prior c1
	UNION ALL 
	SELECT c1.Period, c1.Collector, c1.Ordinance, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Historical c1
END
GO



/****** Object:  StoredProcedure [dbo].[BI_FI_GetCollectionInfoDashboardData]    Script Date: 8/2/2018 1:16:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[BI_FI_GetCollectionInfoDashboardData] 
@currentStart DATETIME
,@currentEnd DATETIME
,@priorStart DATETIME
,@priorEnd DATETIME
,@historicalStart DATETIME
,@historicalEnd DATETIME


AS 
BEGIN 
DECLARE  @currentStart_P DATETIME
		,@currentEnd_P DATETIME
		,@priorStart_P DATETIME
		,@priorEnd_P DATETIME
		,@historicalStart_P DATETIME
		,@historicalEnd_P DATETIME

SET @currentStart_P = @currentStart
SET @currentEnd_P = @currentEnd
SET @priorStart_P = @priorStart
SET @priorEnd_P = @priorEnd
SET @historicalStart_P = @historicalStart
SET @historicalEnd_P = @historicalEnd


;WITH CTE_Core 
    AS (	SELECT am.invdte
				, am.pnet
				, am.pterms
				, DATEADD(Day,am.pnet,am.invdte) AS dueDate
				, am.dtepaid
				, am.invamt
				, am.paidamt
				, am.balance
				, DATEDIFF(Day,am.dtepaid,DATEADD(Day,am.pnet,am.invdte)) AS overDueDate
				, CASE WHEN DATEDIFF(Day,am.dtepaid,DATEADD(Day,am.pnet,am.invdte))>=0 THEN 100 ELSE 0 END AS payOnTime
				, CASE WHEN am.invamt=0 THEN 0 ELSE ((am.paidamt/am.invamt)*100) END AS totalCollection
				--, c.CollectorName as collector
				, CASE WHEN c.CollectorName IS NUll THEN 'UnAssigned' ELSE c.CollectorName END AS collector
				--, c.Ordinance as ordinance
				, CASE WHEN c.Ordinance IS NUll THEN 1000 ELSE c.Ordinance END AS ordinance
			FROM BI_View_ARMAS am
			--INNER JOIN BI_View_CUSTCOLLECTOR cc ON cc.custno = am.custno
			INNER JOIN arcust ac ON ac.custno = am.custno
			INNER JOIN TESTNogalesDashboard.dbo.[CollectorAssignment] ca ON SUBSTRING(ac.company,1,1) = ca.LetterName COLLATE SQL_Latin1_General_CP1_CI_AS
			LEFT JOIN TESTNogalesDashboard.dbo.[Collector] c ON ca.CollectorId = c.CollectorId
			WHERE ((am.invdte BETWEEN @currentStart_P AND @currentEnd_P) 
					OR (am.invdte BETWEEN @priorStart_P AND @priorEnd_P) 
					OR (am.invdte BETWEEN @historicalStart_P AND @historicalEnd_P))
		)



	,CTE_Current AS (SELECT
							c.collector				AS Collector,
							c.ordinance				AS Ordinance,
							c.pnet					AS PNet,
							c.pTerms				AS PTerms,
							Sum(c.invamt)           AS InvoiceAmount,
							Sum(c.paidamt)			AS PaidAmount,
							AVG(c.payOnTime )		AS PaymentOnTime,
							AVG(c.totalCollection)  AS TotalCollection,
							'current' AS Period 
					FROM   CTE_Core c 
					WHERE c.invdte BETWEEN  @currentStart_P AND @currentEnd_P 
					GROUP  BY c.collector, c.pnet, c.pTerms, c.ordinance
					)
	,CTE_Prior AS (SELECT
							c.collector				AS Collector,
							c.ordinance				AS Ordinance,
							c.pnet					AS PNet,
							c.pTerms				AS PTerms,
							Sum(c.invamt)           AS InvoiceAmount,
							Sum(c.paidamt)			AS PaidAmount,
							AVG(c.payOnTime )		AS PaymentOnTime,
							AVG(c.totalCollection)  AS TotalCollection,
							'prior' AS Period 
					FROM   CTE_Core c 
					WHERE c.invdte BETWEEN  @priorStart_P AND @priorEnd_P
					GROUP  BY c.collector, c.pnet, c.pTerms, c.ordinance
				)
	,CTE_Historical AS (SELECT
								c.collector				AS Collector,
								c.ordinance				AS Ordinance,
								c.pnet					AS PNet,
								c.pTerms				AS PTerms,
								Sum(c.invamt)           AS InvoiceAmount,
								Sum(c.paidamt)			AS PaidAmount,
								AVG(c.payOnTime )		AS PaymentOnTime,
								AVG(c.totalCollection)  AS TotalCollection,
								'historical' AS Period 
						FROM   CTE_Core c 
						WHERE  c.invdte BETWEEN  @historicalStart_P AND @historicalEnd_P
						GROUP  BY c.collector, c.pnet, c.pTerms, c.ordinance
						)

	SELECT c1.Period, c1.Collector, c1.Ordinance, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Current c1
	UNION ALL
	SELECT c1.Period, c1.Collector, c1.Ordinance, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Prior c1
	UNION ALL 
	SELECT c1.Period, c1.Collector, c1.Ordinance, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Historical c1
END




/****** Object:  StoredProcedure [dbo].[BI_FI_GetCollectionInfoDashboardData]    Script Date: 8/2/2018 1:16:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[BI_FI_GetCollectionInfoDashboardData] 
@currentStart DATETIME
,@currentEnd DATETIME
,@priorStart DATETIME
,@priorEnd DATETIME
,@historicalStart DATETIME
,@historicalEnd DATETIME


AS 
BEGIN 
DECLARE  @currentStart_P DATETIME
		,@currentEnd_P DATETIME
		,@priorStart_P DATETIME
		,@priorEnd_P DATETIME
		,@historicalStart_P DATETIME
		,@historicalEnd_P DATETIME

SET @currentStart_P = @currentStart
SET @currentEnd_P = @currentEnd
SET @priorStart_P = @priorStart
SET @priorEnd_P = @priorEnd
SET @historicalStart_P = @historicalStart
SET @historicalEnd_P = @historicalEnd


;WITH CTE_Core 
    AS (	SELECT am.invdte
				, am.pnet
				, am.pterms
				, DATEADD(Day,am.pnet,am.invdte) AS dueDate
				, am.dtepaid
				, am.invamt
				, am.paidamt
				, am.balance
				, DATEDIFF(Day,am.dtepaid,DATEADD(Day,am.pnet,am.invdte)) AS overDueDate
				, CASE WHEN DATEDIFF(Day,am.dtepaid,DATEADD(Day,am.pnet,am.invdte))>=0 THEN 100 ELSE 0 END AS payOnTime
				, CASE WHEN am.invamt=0 THEN 0 ELSE ((am.paidamt/am.invamt)*100) END AS totalCollection
				--, c.CollectorName as collector
				, CASE WHEN c.CollectorName IS NUll THEN 'Unassigned' ELSE c.CollectorName END AS collector
				--, c.Ordinance as ordinance
				, CASE WHEN c.Ordinance IS NUll THEN 1000 ELSE c.Ordinance END AS ordinance
			FROM BI_View_ARMAS am
			--INNER JOIN BI_View_CUSTCOLLECTOR cc ON cc.custno = am.custno
			INNER JOIN arcust ac ON ac.custno = am.custno
			INNER JOIN TESTNogalesDashboard.dbo.[CollectorAssignment] ca ON SUBSTRING(ac.company,1,1) = ca.LetterName COLLATE SQL_Latin1_General_CP1_CI_AS
			LEFT JOIN TESTNogalesDashboard.dbo.[Collector] c ON ca.CollectorId = c.CollectorId
			WHERE ((am.invdte BETWEEN @currentStart_P AND @currentEnd_P) 
					OR (am.invdte BETWEEN @priorStart_P AND @priorEnd_P) 
					OR (am.invdte BETWEEN @historicalStart_P AND @historicalEnd_P))
		)



	,CTE_Current AS (SELECT
							c.collector				AS Collector,
							c.ordinance				AS Ordinance,
							c.pnet					AS PNet,
							c.pTerms				AS PTerms,
							Sum(c.invamt)           AS InvoiceAmount,
							Sum(c.paidamt)			AS PaidAmount,
							AVG(c.payOnTime )		AS PaymentOnTime,
							AVG(c.totalCollection)  AS TotalCollection,
							'current' AS Period 
					FROM   CTE_Core c 
					WHERE c.invdte BETWEEN  @currentStart_P AND @currentEnd_P 
					GROUP  BY c.collector, c.pnet, c.pTerms, c.ordinance
					)
	,CTE_Prior AS (SELECT
							c.collector				AS Collector,
							c.ordinance				AS Ordinance,
							c.pnet					AS PNet,
							c.pTerms				AS PTerms,
							Sum(c.invamt)           AS InvoiceAmount,
							Sum(c.paidamt)			AS PaidAmount,
							AVG(c.payOnTime )		AS PaymentOnTime,
							AVG(c.totalCollection)  AS TotalCollection,
							'prior' AS Period 
					FROM   CTE_Core c 
					WHERE c.invdte BETWEEN  @priorStart_P AND @priorEnd_P
					GROUP  BY c.collector, c.pnet, c.pTerms, c.ordinance
				)
	,CTE_Historical AS (SELECT
								c.collector				AS Collector,
								c.ordinance				AS Ordinance,
								c.pnet					AS PNet,
								c.pTerms				AS PTerms,
								Sum(c.invamt)           AS InvoiceAmount,
								Sum(c.paidamt)			AS PaidAmount,
								AVG(c.payOnTime )		AS PaymentOnTime,
								AVG(c.totalCollection)  AS TotalCollection,
								'historical' AS Period 
						FROM   CTE_Core c 
						WHERE  c.invdte BETWEEN  @historicalStart_P AND @historicalEnd_P
						GROUP  BY c.collector, c.pnet, c.pTerms, c.ordinance
						)

	SELECT c1.Period, c1.Collector, c1.Ordinance, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Current c1
	UNION ALL
	SELECT c1.Period, c1.Collector, c1.Ordinance, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Prior c1
	UNION ALL 
	SELECT c1.Period, c1.Collector, c1.Ordinance, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Historical c1
END


/***** Object:  StoredProcedure [dbo].[BI_FI_GetCollectorDetailsReport]    Script Date: 08/02/2018 04:20:00 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
exec [BI_FI_GetCollectorDetailsReport] '2018/07/01','2018/07/25','COD CASH ONLY','Unassigned'

*/

ALTER PROCEDURE [dbo].[BI_FI_GetCollectorDetailsReport]
	@currentStart DATETIME
	,@currentEnd DATETIME
	,@pterms nvarchar(20) =''
	,@collector  nvarchar(50) =''
AS
BEGIN

	DECLARE  @currentStart_P DATETIME
	,@currentEnd_P DATETIME
	,@pterms_P nvarchar(20)
	,@collector_P nvarchar(50)

	SET @currentStart_P = @currentStart
	SET @currentEnd_P = @currentEnd
	SET @pterms_P = @pterms
	SET @collector_P = @collector

	IF DB_NAME() like 'TEST%'
		BEGIN
				SELECT  am.invno
				, am.invdte
				, am.custno
				, ac.company
				, am.salesmn
				, spm.SalesPersonDescription
				, am.pnet
				, am.pterms
				, DATEADD(Day,am.pnet,am.invdte) AS dueDate
				, CASE WHEN am.dtepaid='1900-01-01 00:00:00.000' THEN null ELSE am.dtepaid END as dtepaid
				, am.invamt
				, am.paidamt
				, am.balance
				, CASE WHEN am.dtepaid='1900-01-01 00:00:00.000' OR am.dtepaid IS NULL THEN DATEDIFF(Day,DATEADD(Day,am.pnet,am.invdte),GETDATE())
						ELSE DATEDIFF(Day,DATEADD(Day,am.pnet,am.invdte),am.dtepaid) END AS overDueDate
				, CASE WHEN am.dtepaid='1900-01-01 00:00:00.000' THEN 0 WHEN DATEDIFF(Day,am.dtepaid,DATEADD(Day,am.pnet,am.invdte))>=0 THEN 100 ELSE 0 END AS payOnTime
				, CASE WHEN am.invamt=0 THEN 0 ELSE ((am.paidamt/am.invamt)*100) END AS totalCollection
				,ISNULL(c.CollectorName, 'Unassigned') as CollectorName
				--, DATEPART(dd,am.dtepaid) as collector
			FROM
			BI_View_ARMAS am
			--INNER JOIN arcust ac ON ac.custno = am.custno
			--INNER JOIN BI_View_CUSTCOLLECTOR cc ON cc.custno = am.custno
			INNER JOIN arcust ac ON ac.custno = am.custno
			INNER JOIN TESTNogalesDashboard.dbo.[CollectorAssignment] ca ON SUBSTRING(ac.company,1,1) = ca.LetterName COLLATE SQL_Latin1_General_CP1_CI_AS
			LEFT JOIN TESTNogalesDashboard.dbo.[Collector] c ON ca.CollectorId = c.CollectorId
			LEFT JOIN SalesPersonMapping spm ON spm.AssignedPersonCode = am.salesmn 
						AND am.invdte BETWEEN spm.startdate AND Isnull(spm.enddate, @currentEnd)
			WHERE (am.invdte BETWEEN @currentStart_P AND @currentEnd_P )
				AND (@pterms_P='' OR (@pterms_P <>'' AND  am.pterms=@pterms_P))
				AND (
						(@collector_P ='') 
					OR (@collector_P ='Unassigned' AND c.CollectorName is null) 
					OR (@collector_P <>'' AND  c.CollectorName=@collector_P)
					)
		END
	ELSE
		BEGIN
				SELECT  am.invno
				, am.invdte
				, am.custno
				, ac.company
				, am.salesmn
				, spm.SalesPersonDescription
				, am.pnet
				, am.pterms
				, DATEADD(Day,am.pnet,am.invdte) AS dueDate
				, CASE WHEN am.dtepaid='1900-01-01 00:00:00.000' THEN null ELSE am.dtepaid END as dtepaid
				, am.invamt
				, am.paidamt
				, am.balance
				, CASE WHEN am.dtepaid='1900-01-01 00:00:00.000' OR am.dtepaid IS NULL THEN DATEDIFF(Day,DATEADD(Day,am.pnet,am.invdte),GETDATE())
						ELSE DATEDIFF(Day,DATEADD(Day,am.pnet,am.invdte),am.dtepaid) END AS overDueDate
				, CASE WHEN am.dtepaid='1900-01-01 00:00:00.000' THEN 0 WHEN DATEDIFF(Day,am.dtepaid,DATEADD(Day,am.pnet,am.invdte))>=0 THEN 100 ELSE 0 END AS payOnTime
				, CASE WHEN am.invamt=0 THEN 0 ELSE ((am.paidamt/am.invamt)*100) END AS totalCollection
				,ISNULL(c.CollectorName, 'Unassigned') as CollectorName
				--, DATEPART(dd,am.dtepaid) as collector
			FROM
			BI_View_ARMAS am
			--INNER JOIN arcust ac ON ac.custno = am.custno
			--INNER JOIN BI_View_CUSTCOLLECTOR cc ON cc.custno = am.custno
			INNER JOIN arcust ac ON ac.custno = am.custno
			INNER JOIN NogalesDashboard.dbo.[CollectorAssignment] ca ON SUBSTRING(ac.company,1,1) = ca.LetterName COLLATE SQL_Latin1_General_CP1_CI_AS
			LEFT JOIN NogalesDashboard.dbo.[Collector] c ON ca.CollectorId = c.CollectorId
			LEFT JOIN SalesPersonMapping spm ON spm.AssignedPersonCode = am.salesmn 
						AND am.invdte BETWEEN spm.startdate AND Isnull(spm.enddate, @currentEnd)
			WHERE (am.invdte BETWEEN @currentStart_P AND @currentEnd_P )
				AND (@pterms_P='' OR (@pterms_P <>'' AND  am.pterms=@pterms_P))
				AND (
						(@collector_P ='') 
					OR (@collector_P ='Unassigned' AND c.CollectorName is null) 
					OR (@collector_P <>'' AND  c.CollectorName=@collector_P)
					)
		END
	
						
END

