
/*
declare @p1 dbo.StringTable
exec BI_GetSalesOrder_NoBin @SalesPerson=@p1,@date='2018-06-01 00:00:00',@sono=N'1351711'
*/

CREATE PROCEDURE [dbo].[BI_WH_GetSalesOrderNoBinReport] 
	 @SalesPerson StringTable readonly,  
	 @date datetime,
	 @sono varchar(50)
AS
BEGIN

	if((select count(*) from @SalesPerson)>0)
	begin
		select distinct 
		d1.sono, ic.item itemcode,ic.itmdesc item, binno,CAST(ROUND(tqtyshp, 0) AS INT) tqtyshp,sodate [date],
		--so.picker puller,sw.alphaname name,
		case when so.picker='' then so.gropicker else so.picker end  puller,
		case when so.picker='' then sw1.alphaname  else sw.alphaname  end		name,
		d1.salesmn SalesPerson,
		isnull((select top 1 d.SalesPersonDescription from SalesPersonMapping d where d.AssignedPersonCode=d1.salesmn and EndDate is null),d1.salesmn) SalesPersonName
		from NPICOMPANY01..sotran d1 WITH(NOLOCK) 
		INNER JOIN NPICOMPANY01..icitem ic WITH(NOLOCK) ON d1.item  = ic.item
		inner join somast so on so.sono=d1.sono 
		left join SOWPKR sw on sw.userid=so.picker and sw.loctid='ONSITE'
		left join SOWPKR sw1 on sw.userid=so.gropicker  and  sw1.loctid='ONSITE'
		inner join @SalesPerson slp on slp.Code=d1.salesmn
		where tqtyshp <>0 and (binno='' or binno='_OVERSHIP      ') 
		AND d1.sono<>''  
		and (@sono ='' or (d1.sono<>'' and ltrim(rtrim(d1.sono)) = @sono)) 
		and convert(date,sodate)= convert(date,@date)
	end
	else
	begin
		select distinct 
		d1.sono, ic.item itemcode,ic.itmdesc item, binno,CAST(ROUND(tqtyshp, 0) AS INT) tqtyshp,sodate [date],
		case when so.picker='' then so.gropicker else so.picker end  puller,
		case when so.picker='' then sw1.alphaname  else sw.alphaname  end		name,
		d1.salesmn SalesPerson,
		isnull((select top 1 d.SalesPersonDescription from SalesPersonMapping d where d.AssignedPersonCode=d1.salesmn and EndDate is null),d1.salesmn) SalesPersonName
		from NPICOMPANY01..sotran d1 WITH(NOLOCK) 
		INNER JOIN NPICOMPANY01..icitem ic WITH(NOLOCK) ON d1.item  = ic.item
		inner join somast so on so.sono=d1.sono 
		left join SOWPKR sw on sw.userid=so.picker  and  sw.loctid='ONSITE'
		left join SOWPKR sw1 on sw1.userid=so.gropicker  and  sw1.loctid='ONSITE'
		where tqtyshp <>0 and (binno='' or binno='_OVERSHIP      ') 
		AND (@sono ='' or (d1.sono<>'' and ltrim(rtrim(d1.sono)) = @sono)) and d1.sono<>''  
		and convert(date,sodate)= convert(date,@date)
	end
						
END

GO
/*
declare @p1 dbo.StringTable
exec BI_GetSalesOrder_NoBin @SalesPerson=@p1,@date='2018-06-01 00:00:00',@sono=N'1351711'
*/


GO 


GO



/*

exec BI_WH_GetPickerProductivityDashboardData  '2018/06/01','2018/06/11','2018/05/01','2018/05/11','2017/06/01','2017/06/11'

*/

CREATE PROCEDURE [dbo].[BI_WH_GetPickerProductivityDashboardData]
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


;WITH CTE_CORE(UserId, TaskNo, MinutesWorked,TaskDate)
            AS
            (
			    SELECT st.userid, st.taskno ,SUM(datediff(MINUTE ,st.sdtime,st.edtime)) as MinutesWorked
						,cast(st.sdtime as date) as TaskDate 
				FROM sottrk st 
                WHERE st.sdtime between @currentStart_P AND @currentEnd_P AND st.edtime between @currentStart_P AND @currentEnd_P
				  OR st.sdtime between @priorStart_P AND @priorEnd_P AND st.edtime between @priorStart_P AND @priorEnd_P
				  OR st.sdtime between @historicalStart_P AND @historicalEnd_P AND st.edtime between @historicalStart_P AND @historicalEnd_P
                GROUP BY st.userid,st.taskno,cast(st.sdtime as date)
            )
, CTE_Current AS 
			(
				SELECT 'current' as period , a.UserId
						, SUM(a.MinutesWorked) as MinutesWorked
						, SUM(a.QtyPicked) as QtyPicked
				FROM (
						SELECT c.UserId, c.TaskDate, c.TaskNo, c.MinutesWorked
								, SUM(sh.tqtypicked) as QtyPicked
						FROM CTE_CORE c
							INNER JOIN sowdtl sh on sh.taskno = c.TaskNo
						WHERE c.TaskDate  BETWEEN @currentStart_P AND @currentEnd_P 
						GROUP BY c.UserId,c.TaskDate,c.MinutesWorked,c.TaskNo
					) a
				GROUP BY a.UserId
			) 
			
, CTE_Prior AS 
			(
				SELECT 'prior' as period , a.UserId
						, SUM(a.MinutesWorked) as MinutesWorked
						, SUM(a.QtyPicked) as QtyPicked
				FROM (
						SELECT c.UserId, c.TaskDate, c.TaskNo, c.MinutesWorked
								, SUM(sh.tqtypicked) as QtyPicked
						FROM CTE_CORE c
							INNER JOIN sowdtl sh on sh.taskno = c.TaskNo
						WHERE c.TaskDate  BETWEEN @priorStart_P AND @priorEnd_P 
						GROUP BY c.UserId,c.TaskDate,c.MinutesWorked,c.TaskNo
					) a
				GROUP BY a.UserId
			) 
, CTE_Historical AS 
			(
				SELECT 'historical' as period , a.UserId
						, SUM(a.MinutesWorked) as MinutesWorked
						, SUM(a.QtyPicked) as QtyPicked
				FROM (
						SELECT c.UserId, c.TaskDate, c.TaskNo, c.MinutesWorked
								, SUM(sh.tqtypicked) as QtyPicked
						FROM CTE_CORE c
							INNER JOIN sowdtl sh on sh.taskno = c.TaskNo
						WHERE c.TaskDate  BETWEEN @historicalStart_P AND @historicalEnd_P 
						GROUP BY c.UserId,c.TaskDate,c.MinutesWorked,c.TaskNo
					) a
				GROUP BY a.UserId
			)
,CTE_Union AS
			(
				SELECT * FROM CTE_Current
				UNION ALL
				SELECT * FROM CTE_Prior
				UNION ALL
				SELECT * FROM CTE_Historical
			)
SELECT h.*, u.alphaname as UserName 
FROM CTE_Union h
		 LEFT JOIN sowpkr u on u.userid = h.UserId AND u.loctid ='ONSITE'
END



/*

exec BI_WH_GetPickerProductivityDashboardData  '2018/06/01','2018/06/11','2018/05/01','2018/05/11','2017/06/01','2017/06/11'

*/


GO
/*

exec [BI_WH_GetPickerProductivityDayReport] '2018/05/01','2018/05/30','0426'

*/

CREATE PROCEDURE [dbo].[BI_WH_GetPickerProductivityDayReport] 
@currentStart DATETIME
,@currentEnd DATETIME
,@userId nvarchar(20) =''

AS 
BEGIN 
DECLARE  @currentStart_P DATETIME
		,@currentEnd_P DATETIME
		,@userId_P nvarchar(20)

SET @currentStart_P = @currentStart
SET @currentEnd_P = @currentEnd
SET @userId_P = @userId


;WITH CTE_CORE
            AS
            (
			    SELECT st.userid, st.taskno 
						,SUM(datediff(MINUTE ,st.sdtime,st.edtime)) as MinutesWorked
						,cast(st.sdtime as date) as TaskDate 
						, min(sdtime) as StartTime
						, max(edtime) as EndTime
				FROM sottrk st 
                WHERE st.sdtime between @currentStart_P AND @currentEnd_P AND st.edtime between @currentStart_P AND @currentEnd_P
					AND (@userId_P='' OR (@userId_P <>'' AND st.userid = @userId_P))
                GROUP BY st.userid,st.taskno,cast(st.sdtime as date)
            )
			SELECT a.UserId, a.TaskDate
						,Min(a.StartTime) as StartTime
						,Max(a.EndTime) as EndTime
						, SUM(a.QtyPicked) as QtyPicked
						, cast(SUM(a.MinutesWorked) as decimal)/60 as HoursWorked
						,CASE WHEN (cast(SUM(a.MinutesWorked) as decimal)/60) >0 
							  THEN SUM(a.QtyPicked)/(cast(SUM(a.MinutesWorked) as decimal)/60 ) 
							  ELSE 0
							  END AS AvgPiecesPicked

				FROM (
						SELECT c.UserId, c.TaskDate, c.TaskNo, c.MinutesWorked
								, SUM(sh.tqtypicked) as QtyPicked
								,Min(c.StartTime) as StartTime
								,Max(c.EndTime) aS EndTime
						FROM CTE_CORE c
							INNER JOIN sowdtl sh on sh.taskno = c.TaskNo
						GROUP BY c.UserId,c.TaskDate,c.MinutesWorked,c.TaskNo
					) a
					GROUP BY a.UserId,a.TaskDate
END

/*

exec [BI_WH_GetPickerProductivityDayReport] '2018/05/01','2018/05/30','0426'

*/

GO

GO

/*
exec BI_WH_GetPickerProductivityReportDetails '2018-06-02 00:00:00','2018-06-02 23:59:59','00:00:00','23:59:59', '3230'
*/

CREATE PROCEDURE [dbo].[BI_WH_GetPickerProductivityReportDetails]
@startDate datetime
,@endDate datetime
,@startTime TIME
,@endTime TIME
,@empId VARCHAR(20)

as
begin

	SET @startDate = @startDate + '00:00:00'
	SET @endDate = @endDate +'23:59:59'
	DECLARE @dayEndTime TIME;

	SET @dayEndTime = CASE 
			WHEN @endTime < @startTime
			THEN CAST('23:59:59' AS TIME)
			ELSE NULL
			END;

	WITH CTE_SOTTRK (UserId,TaskNo,MinutesWorked,StartTime,EndTime)
	AS (
		SELECT st.userid
			,st.taskno
			,SUM(datediff(MINUTE, st.sdtime, st.edtime)) AS MinutesWorked
			,min(sdtime) AS startTime
			,max(edtime) AS endTime
		FROM sottrk st
		WHERE st.sdtime BETWEEN @startDate AND @endDate
			AND st.edtime BETWEEN @startDate AND @endDate
			AND (st.userid = @empId OR '' = @empId)
			AND (@startTime IS NULL
				OR @startTime = ''
				OR (CAST(st.sdtime AS TIME) BETWEEN @startTime AND @endTime)
				OR (@dayEndTime IS NOT NULL
					AND (
							CAST(st.sdtime AS TIME) BETWEEN @startTime AND @dayEndTime
						OR CAST(st.sdtime AS TIME) BETWEEN CAST('0:0:0' AS TIME) AND @endTime
						)
					)
				)
		GROUP BY st.userid,st.taskno
		)
		,CTE_Header (UserId,TaskNo,item,MinutesWorked,QtyPicked,StartTime,EndTime,SalesOrderNo)
	AS (
		SELECT st.UserId
			,st.TaskNo
			,sh.item
			,st.MinutesWorked
			,SUM(sh.tqtypicked) AS QtyPicked
			,MIN(st.StartTime)
			,max(st.EndTime)
			,sh.sono
		FROM CTE_SOTTRK st
		INNER JOIN sowdtl sh ON sh.taskno = st.TaskNo
		
		GROUP BY st.UserId
			,st.TaskNo
			,st.MinutesWorked,sh.item,sh.sono
			
		)
	SELECT h.UserId AS EMPID
		,u.alphaname AS NAME,h.item,i.itmdesc as ItemDesc, h.SalesOrderNo
		,cast(SUM(h.QtyPicked) AS DECIMAL(18, 2)) AS PiecePicked
		,cast(SUM(h.MinutesWorked) / 60.0 AS DECIMAL(18, 2)) HoursWorked
		,CAST(SUM(h.QtyPicked) / (SUM(h.MinutesWorked) / 60.0) AS DECIMAL(18, 2)) AS PiecesPerHour
		,cast(Min(h.StartTime) AS DATETIME) AS StartTime
		,cast(MaX(H.EndTime) AS DATETIME) AS EndTime
	FROM CTE_Header h
		INNER JOIN icitem i on i.item=h.item
		LEFT JOIN sowpkr u ON u.userid = h.UserId AND u.loctid = 'ONSITE'
	GROUP BY h.UserId,u.alphaname,h.item,i.itmdesc, h.SalesOrderNo
	HAVING SUM(h.MinutesWorked) <> 0
	ORDER BY h.UserId

	End

	GO

	
ALTER PROCEDURE [dbo].[BI_GetIgnoredShortReport] 
		 @shipDate datetime,
		 @routNo varchar(50)='',
		 @buyerName varchar(50)=''
AS
BEGIN
   SELECT * FROM SOShortExclusion
    WHERE rqdate = @shipDate
        AND (@routNo ='' or( route <>'' AND route = @routNo))
        AND (@buyerName ='' OR(buyer <>'' AND buyer = @buyerName ))
    ORDER BY description
END

GO