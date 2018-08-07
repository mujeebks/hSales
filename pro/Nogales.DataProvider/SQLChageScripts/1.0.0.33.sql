USE [NPICOMPANY01]
GO
/****** Object:  StoredProcedure [dbo].[BI_FI_GetCollectorDetailsReport]    Script Date: 7/26/2018 3:03:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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
			, CASE WHEN am.dtepaid='1900-01-01 00:00:00.000' OR am.dtepaid IS NULL THEN DATEDIFF(Day,DATEADD(Day,am.pnet,am.invdte),GETDATE())
					ELSE DATEDIFF(Day,DATEADD(Day,am.pnet,am.invdte),am.dtepaid) END AS overDueDate
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

