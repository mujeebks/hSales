
ALTER view [dbo].[BI_View_ARMAS]
as

SELECT  a1.invno,  a1.route AS route,  a1.invdte AS invdte,  a1.artype AS artype,  a1.tosw AS tosw,  a1.salesmn AS salesmn
		,  a1.ortaker AS ortaker,  a1.terr AS terr, a1.arstat , a1.custno, a1.trckdr AS trckdr , a1.trcklc AS trcklc, a1.rtref AS rtref
		, a1.invamt AS invamt, a1.paidamt AS paidamt, a1.balance AS balance, a1.pterms AS pterms, a1.pnet AS pnet, a1.dtepaid AS dtepaid
					from armast a1
union all
SELECT  a1.invno,  a1.route AS route,  a1.invdte AS invdte,  a1.artype AS artype,  a1.tosw AS tosw,  a1.salesmn AS salesmn
		,  a1.ortaker AS ortaker,  a1.terr AS terr, a1.arstat , a1.custno, a1.trckdr AS trckdr , a1.trcklc AS trcklc, a1.rtref AS rtref
		, a1.invamt AS invamt, a1.paidamt AS paidamt, a1.balance AS balance, a1.pterms AS pterms, a1.pnet AS pnet, a1.dtepaid AS dtepaid
					from arymst a1 
					

GO

/*

exec [BI_FI_GetCollectorDetailsReport] '2018/04/01','2018/04/10','COD',0

*/

CREATE PROCEDURE [dbo].[BI_FI_GetCollectorDetailsReport]
	@currentStart DATETIME
	,@currentEnd DATETIME
	,@pterms nvarchar(20) =''
	,@collector int =0
AS
BEGIN

	DECLARE  @currentStart_P DATETIME
	,@currentEnd_P DATETIME
	,@pterms_P nvarchar(20)
	,@collector_P int

	SET @currentStart_P = @currentStart
	SET @currentEnd_P = @currentEnd
	SET @pterms_P = @pterms
	SET @collector_P = @collector

	SELECT  am.invno
			, am.invdte
			, am.custno
			, ac.company
			, am.salesmn
			, spm.SalesPersonDescription
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
			, DATEPART(dd,am.dtepaid) as collector
		FROM
		BI_View_ARMAS am
		INNER JOIN arcust ac ON ac.custno = am.custno
		INNER JOIN SalesPersonMapping spm ON spm.AssignedPersonCode = am.salesmn 
					AND am.invdte BETWEEN spm.startdate AND Isnull(spm.enddate, @currentEnd)
		WHERE (am.invdte BETWEEN @currentStart_P AND @currentEnd_P )
			AND (@pterms_P='' OR (@pterms_P <>'' AND  am.pterms=@pterms_P))
			AND (@collector_P =0 OR (@collector_P >0 AND  DATEPART(dd,am.dtepaid)=@collector_P))
						
END


/*

exec [BI_FI_GetCollectorDetailsReport] '2017/06/01','2018/06/01','NET 10'

*/

GO

GO
/*

exec [BI_FI_GetCollectionInfoDashboardData] '2018/06/01','2018/06/11','2018/05/01','2018/05/11','2017/06/01','2017/06/11'

*/

CREATE PROCEDURE [dbo].[BI_FI_GetCollectionInfoDashboardData] 
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
				, DATEPART(dd,am.dtepaid) as collector
			FROM
			BI_View_ARMAS am
			WHERE ((am.invdte BETWEEN @currentStart_P AND @currentEnd_P) 
					OR (am.invdte BETWEEN @priorStart_P AND @priorEnd_P) 
					OR (am.invdte BETWEEN @historicalStart_P AND @historicalEnd_P) 
					)
		)
	,CTE_Current AS (SELECT
							c.collector				AS Collector,
							c.pnet					AS PNet,
							c.pTerms				AS PTerms,
							Sum(c.invamt)           AS InvoiceAmount,
							Sum(c.paidamt)			AS PaidAmount,
							AVG(c.payOnTime )		AS PaymentOnTime,
							AVG(c.totalCollection)  AS TotalCollection,
							'current' AS Period 
					FROM   CTE_Core c 
					WHERE c.invdte BETWEEN  @currentStart_P AND @currentEnd_P 
					GROUP  BY c.collector, c.pnet, c.pTerms
					)
	,CTE_Prior AS (SELECT
							c.collector				AS Collector,
							c.pnet					AS PNet,
							c.pTerms				AS PTerms,
							Sum(c.invamt)           AS InvoiceAmount,
							Sum(c.paidamt)			AS PaidAmount,
							AVG(c.payOnTime )		AS PaymentOnTime,
							AVG(c.totalCollection)  AS TotalCollection,
							'prior' AS Period 
					FROM   CTE_Core c 
					WHERE c.invdte BETWEEN  @priorStart_P AND @priorEnd_P
					GROUP  BY c.collector, c.pnet, c.pTerms
				)
	,CTE_Historical AS (SELECT
								c.collector				AS Collector,
								c.pnet					AS PNet,
								c.pTerms				AS PTerms,
								Sum(c.invamt)           AS InvoiceAmount,
								Sum(c.paidamt)			AS PaidAmount,
								AVG(c.payOnTime )		AS PaymentOnTime,
								AVG(c.totalCollection)  AS TotalCollection,
								'historical' AS Period 
						FROM   CTE_Core c 
						WHERE  c.invdte BETWEEN  @historicalStart_P AND @historicalEnd_P
						GROUP  BY c.collector, c.pnet, c.pTerms
						)

	SELECT c1.Period, c1.Collector, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Current c1
	UNION ALL
	SELECT c1.Period, c1.Collector, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Prior c1
	UNION ALL 
	SELECT c1.Period, c1.Collector, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Historical c1
END

/*

exec [BI_FI_GetCollectionInfoDashboardData] '2018/06/01','2018/06/11','2018/05/01','2018/05/11','2017/06/01','2017/06/11'

*/


GO

CREATE VIEW [dbo].[BI_View_CUSTCOLLECTOR]
AS
SELECT        custno, company, 
                         CASE WHEN company LIKE '[A-E,a-e]%' THEN 'Humberto Hernandez' 
							WHEN company LIKE '[F-K,f-k,M,m]%' THEN 'Enrique Pineda' 
							WHEN company LIKE '[N-T,n-t,#,0-9]%' THEN 'Claudia Campos' 
							WHEN company LIKE '[U-Z,u-z,L,l]%'
                          THEN 'Open Position' END AS CollectorName
FROM            dbo.arcust


GO


/*

exec [BI_FI_GetCollectorDetailsReport] '2018/04/01','2018/04/10','COD',0

*/

ALTER PROCEDURE [dbo].[BI_FI_GetCollectorDetailsReport]
	@currentStart DATETIME
	,@currentEnd DATETIME
	,@pterms nvarchar(20) =''
	,@collector nvarchar(50) =''
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

	SELECT  am.invno
			, am.invdte
			, am.custno
			, cc.company
			, am.salesmn
			, spm.SalesPersonDescription
			, am.pnet
			, am.pterms
			, DATEADD(Day,am.pnet,am.invdte) AS dueDate
			, am.dtepaid
			, am.invamt
			, am.paidamt
			, am.balance
			, DATEDIFF(Day,DATEADD(Day,am.pnet,am.invdte),am.dtepaid) AS overDueDate
			, CASE WHEN DATEDIFF(Day,am.dtepaid,DATEADD(Day,am.pnet,am.invdte))>=0 THEN 100 ELSE 0 END AS payOnTime
			, CASE WHEN am.invamt=0 THEN 0 ELSE ((am.paidamt/am.invamt)*100) END AS totalCollection
			,cc.CollectorName
		FROM
		BI_View_ARMAS am
		INNER JOIN BI_View_CUSTCOLLECTOR cc ON cc.custno = am.custno
		LEFT JOIN SalesPersonMapping spm ON spm.AssignedPersonCode = am.salesmn 
					AND am.invdte BETWEEN spm.startdate AND Isnull(spm.enddate, @currentEnd)
		WHERE (am.invdte BETWEEN @currentStart_P AND @currentEnd_P )
			AND (@pterms_P='' OR (@pterms_P <>'' AND  am.pterms=@pterms_P))
			AND (@collector_P =0 OR (@collector_P >0 AND  cc.CollectorName=@collector_P))
						
END


/*

exec [BI_FI_GetCollectorDetailsReport] '2017/06/01','2018/06/01','NET 10'

*/

GO
/*

exec [BI_FI_GetCollectionInfoDashboardData] '2018/06/01','2018/06/11','2018/05/01','2018/05/11','2017/06/01','2017/06/11'

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
			FROM BI_View_ARMAS am
			INNER JOIN BI_View_CUSTCOLLECTOR cc ON cc.custno = am.custno
			WHERE ((am.invdte BETWEEN @currentStart_P AND @currentEnd_P) 
					OR (am.invdte BETWEEN @priorStart_P AND @priorEnd_P) 
					OR (am.invdte BETWEEN @historicalStart_P AND @historicalEnd_P))
		)
	,CTE_Current AS (SELECT
							c.collector				AS Collector,
							c.pnet					AS PNet,
							c.pTerms				AS PTerms,
							Sum(c.invamt)           AS InvoiceAmount,
							Sum(c.paidamt)			AS PaidAmount,
							AVG(c.payOnTime )		AS PaymentOnTime,
							AVG(c.totalCollection)  AS TotalCollection,
							'current' AS Period 
					FROM   CTE_Core c 
					WHERE c.invdte BETWEEN  @currentStart_P AND @currentEnd_P 
					GROUP  BY c.collector, c.pnet, c.pTerms
					)
	,CTE_Prior AS (SELECT
							c.collector				AS Collector,
							c.pnet					AS PNet,
							c.pTerms				AS PTerms,
							Sum(c.invamt)           AS InvoiceAmount,
							Sum(c.paidamt)			AS PaidAmount,
							AVG(c.payOnTime )		AS PaymentOnTime,
							AVG(c.totalCollection)  AS TotalCollection,
							'prior' AS Period 
					FROM   CTE_Core c 
					WHERE c.invdte BETWEEN  @priorStart_P AND @priorEnd_P
					GROUP  BY c.collector, c.pnet, c.pTerms
				)
	,CTE_Historical AS (SELECT
								c.collector				AS Collector,
								c.pnet					AS PNet,
								c.pTerms				AS PTerms,
								Sum(c.invamt)           AS InvoiceAmount,
								Sum(c.paidamt)			AS PaidAmount,
								AVG(c.payOnTime )		AS PaymentOnTime,
								AVG(c.totalCollection)  AS TotalCollection,
								'historical' AS Period 
						FROM   CTE_Core c 
						WHERE  c.invdte BETWEEN  @historicalStart_P AND @historicalEnd_P
						GROUP  BY c.collector, c.pnet, c.pTerms
						)

	SELECT c1.Period, c1.Collector, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Current c1
	UNION ALL
	SELECT c1.Period, c1.Collector, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Prior c1
	UNION ALL 
	SELECT c1.Period, c1.Collector, c1.PNet, c1.PTerms, c1.InvoiceAmount, c1.PaidAmount, c1.PaymentOnTime, c1.TotalCollection
	FROM CTE_Historical c1
END

/*

exec [BI_FI_GetCollectionInfoDashboardData] '2018/06/01','2018/06/11','2018/05/01','2018/05/11','2017/06/01','2017/06/11'

*/



/*
exec [BI_FI_GetCollectorDetailsReport] '2018/07/01','2018/07/25','COD CASH ONLY','Humberto Hernandez'

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

	SELECT  am.invno
			, am.invdte
			, am.custno
			, cc.company
			, am.salesmn
			, spm.SalesPersonDescription
			, am.pnet
			, am.pterms
			, DATEADD(Day,am.pnet,am.invdte) AS dueDate
			, CASE WHEN am.dtepaid='1900-01-01 00:00:00.000' THEN null ELSE am.dtepaid END as dtepaid
			, am.invamt
			, am.paidamt
			, am.balance
			, CASE WHEN am.dtepaid='1900-01-01 00:00:00.000' THEN 0 ELSE DATEDIFF(Day,DATEADD(Day,am.pnet,am.invdte),am.dtepaid) END AS overDueDate
			, CASE WHEN am.dtepaid='1900-01-01 00:00:00.000' THEN 0 WHEN DATEDIFF(Day,am.dtepaid,DATEADD(Day,am.pnet,am.invdte))>=0 THEN 100 ELSE 0 END AS payOnTime
			, CASE WHEN am.invamt=0 THEN 0 ELSE ((am.paidamt/am.invamt)*100) END AS totalCollection
			,cc.CollectorName
			--, DATEPART(dd,am.dtepaid) as collector
		FROM
		BI_View_ARMAS am
		--INNER JOIN arcust ac ON ac.custno = am.custno
		INNER JOIN BI_View_CUSTCOLLECTOR cc ON cc.custno = am.custno
		LEFT JOIN SalesPersonMapping spm ON spm.AssignedPersonCode = am.salesmn 
					AND am.invdte BETWEEN spm.startdate AND Isnull(spm.enddate, @currentEnd)
		WHERE (am.invdte BETWEEN @currentStart_P AND @currentEnd_P )
			AND (@pterms_P='' OR (@pterms_P <>'' AND  am.pterms=@pterms_P))
			AND (@collector_P ='' OR (@collector_P <>'' AND  cc.CollectorName=@collector_P))
						
END


/*

exec [BI_FI_GetCollectorDetailsReport] '2017/06/01','2018/06/01','NET 10'

*/

