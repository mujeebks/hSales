
ALTER VIEW [dbo].[BI_View_ARMAS]
AS

SELECT  a1.invno,  a1.route AS route,  a1.invdte AS invdte,  a1.artype AS artype,  a1.tosw AS tosw,  a1.salesmn AS salesmn
		,  a1.ortaker AS ortaker,  a1.terr AS terr, a1.arstat , a1.custno
		, a1.trckdr , a1.trcklc, a1.rtref
					from armast a1
UNION ALL

SELECT  a1.invno,  a1.route AS route,  a1.invdte AS invdte,  a1.artype AS artype,  a1.tosw AS tosw,  a1.salesmn AS salesmn
		,  a1.ortaker AS ortaker,  a1.terr AS terr, a1.arstat , a1.custno
		, a1.trckdr , a1.trcklc, a1.rtref
					from arymst a1 
					

GO



ALTER view [dbo].[BI_View_ARTRAN]
as
select  at.invno,transeq ,at.invdte 
					,qtyshp	,extprice ,at.custno,at.arstat,item
				,at.salesmn,cost,price,binno,descrip, at.sono, am.artype,at.umeasur,at.adduser
				,at.tqtyshp
					from artran at inner join armast am on at.invno=am.invno
					--where at.invdte > DATEADD(year,-1,getdate())
union all

select  at.invno,transeq ,at.invdte 
					,qtyshp	,extprice ,at.custno,at.arstat,item
				,at.salesmn,cost,price,binno,descrip, at.sono, am.artype,at.umeasur,at.adduser
				,at.tqtyshp
					from arytrn at inner join arymst am on at.invno=am.invno
GO

/*

exec [BI_TR_GetDriverTripsDasboardData] '2018/06/01','2018/06/06','2018/05/01','2018/05/06','2017/06/01','2017/06/06'

*/

CREATE  PROCEDURE [dbo].[BI_TR_GetDriverTripsDashboardData] 
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
    AS (SELECT am.invdte, 
            am.trckdr, 
            am.trcklc, 
            am.route, 
			CASE WHEN am.route BETWEEN 0 AND 99 THEN 'Local' 
				 WHEN am.route BETWEEN 201 AND 299 THEN 'Local' 
				 WHEN am.route BETWEEN 101 AND 199 THEN 'Out of Town' 
				 ELSE '' 
			END  as routeType,
            am.invno, 
            am.custno, 
            am.rtref, 
            Sum(at.tqtyshp) Cases 
        FROM   BI_View_ARMAS am 
			INNER JOIN BI_View_ARTRAN at ON at.invno = am.invno 
            --INNER JOIN artran at ON at.invno = am.invno 
        WHERE  am.trckdr <> '' 
			AND ( (am.invdte BETWEEN @currentStart_P AND @currentEnd_P) 
				OR (am.invdte BETWEEN @priorStart_P AND @priorEnd_P) 
				OR (am.invdte BETWEEN @historicalStart_P AND @historicalEnd_P) 
				)
            AND Isnumeric(am.route) > 0 
        GROUP  BY am.invdte, am.trckdr, am.trcklc, am.route, am.invno, am.custno, am.rtref
		)
	,CTE_Current AS (
						SELECT Cast(c.invdte AS DATE)   invdate, 
								c.trckdr                 AS DriverCode, 
								Isnull(sc.descrip, '')   AS DriverName, 
								c.routeType				 AS RouteType, 
								COUNT(DISTINCT c.route)			AS NoOfTrips,
								Count(DISTINCT c.custno) AS NoOfStops, 
								Sum(c.cases)             AS Cases,
								'current' AS Period 
						FROM   CTE_Core c 
								LEFT JOIN npiprosys.dbo.sycrlst sc ON sc.chrvl = c.trckdr 
										AND sc.ruleid = 'DRIVER  ' 
						WHERE c.invdte BETWEEN  @currentStart_P AND @currentEnd_P 
								AND c.routeType <> ''
						GROUP  BY c.invdte, c.trckdr, sc.descrip, c.routeType
					)
	,CTE_Prior AS (
					SELECT Cast(c.invdte AS DATE)   invdate, 
							c.trckdr                 AS DriverCode, 
							Isnull(sc.descrip, '')   AS DriverName, 
							c.routeType				 AS RouteType, 
							COUNT(DISTINCT c.route)			AS NoOfTrips,
							Count(DISTINCT c.custno) AS NoOfStops, 
							Sum(c.cases)             AS Cases,
							'prior' AS Period 
					FROM   CTE_Core c 
							LEFT JOIN npiprosys.dbo.sycrlst sc ON sc.chrvl = c.trckdr 
									AND sc.ruleid = 'DRIVER  ' 
					WHERE c.invdte BETWEEN  @priorStart_P AND @priorEnd_P
							AND c.routeType <> ''
					GROUP  BY c.invdte, c.trckdr, sc.descrip, c.routeType
					)
	,CTE_Historical AS (SELECT Cast(c.invdte AS DATE)   invdate, 
								c.trckdr                 AS DriverCode, 
								Isnull(sc.descrip, '')   AS DriverName, 
								c.routeType				 AS RouteType, 
								COUNT(DISTINCT c.route)			AS NoOfTrips,
								Count(DISTINCT c.custno) AS NoOfStops, 
								Sum(c.cases)             AS Cases,
								'historical' AS Period 
						FROM   CTE_Core c 
								LEFT JOIN npiprosys.dbo.sycrlst sc ON sc.chrvl = c.trckdr 
										AND sc.ruleid = 'DRIVER  ' 
						WHERE c.invdte BETWEEN  @historicalStart_P AND @historicalEnd_P
								AND c.routeType <> ''
						GROUP  BY c.invdte, c.trckdr, sc.descrip,  c.routeType
						)

	SELECT c1.Period, c1.DriverCode, c1.DriverName, c1.RouteType
		, SUM(c1.NoOfTrips) NoOfTrips, SUM(c1.NoOfStops) NoOfStops, SUM(c1.Cases) Cases
	FROM CTE_Current c1
	GROUP BY c1.Period, c1.DriverCode, c1.DriverName, c1.RouteType
	UNION ALL
	SELECT c1.Period, c1.DriverCode, c1.DriverName, c1.RouteType
		, SUM(c1.NoOfTrips) NoOfTrips, SUM(c1.NoOfStops) NoOfStops, SUM(c1.Cases) Cases
	FROM CTE_Prior c1
	GROUP BY c1.Period, c1.DriverCode, c1.DriverName, c1.RouteType
	UNION ALL 
	SELECT c1.Period, c1.DriverCode, c1.DriverName, c1.RouteType
		, SUM(c1.NoOfTrips) NoOfTrips, SUM(c1.NoOfStops) NoOfStops, SUM(c1.Cases) Cases
	FROM CTE_Historical c1
	GROUP BY c1.Period, c1.DriverCode, c1.DriverName, c1.RouteType

END

/*

exec [BI_TR_GetDriverTripsDasboardData] '2018/06/01','2018/06/06','2018/05/01','2018/05/06','2017/06/01','2017/06/06'

*/
GO

/*

exec [BI_TR_GetDriverTripsDayReport] '2018/06/01','2018/06/06','ANDALV'

*/

CREATE PROCEDURE [dbo].[BI_TR_GetDriverTripsDayReport] 
@currentStart DATETIME
,@currentEnd DATETIME
,@driverCode nvarchar(20) =''
,@route nvarchar(20) =''

AS 
BEGIN 
DECLARE  @currentStart_P DATETIME
		,@currentEnd_P DATETIME
		,@driverCode_P nvarchar(20)
		,@route_P nvarchar(20)

SET @currentStart_P = @currentStart
SET @currentEnd_P = @currentEnd
SET @driverCode_P = @driverCode
SET @route_P = @route

;WITH CTE_Core 
    AS (
	SELECT am.invdte, 
            am.trckdr, 
            am.trcklc, 
            am.route, 
			CASE WHEN am.route BETWEEN 0 AND 99 THEN 'Local' 
				 WHEN am.route BETWEEN 201 AND 299 THEN 'Local' 
				 WHEN am.route BETWEEN 101 AND 199 THEN 'Out of Town' 
				 ELSE '' 
			END  as routeType,
            am.invno, 
            am.custno, 
            am.rtref, 
            Sum(at.tqtyshp) Cases 
        FROM   BI_View_ARMAS am 
			INNER JOIN BI_View_ARTRAN at ON at.invno = am.invno 
            --INNER JOIN artran at ON at.invno = am.invno 
        WHERE  am.trckdr <> '' 
			AND (@driverCode_P='' OR @driverCode_P <> '' AND am.trckdr=@driverCode_P)
			AND (@route_P ='' OR @route_P <> '' AND am.route= @route_P)
			AND am.invdte BETWEEN @currentStart_P AND @currentEnd_P
            AND Isnumeric(am.route) > 0 
        GROUP  BY am.invdte, am.trckdr, am.trcklc, am.route, am.invno, am.custno, am.rtref
		)
		SELECT Cast(c.invdte AS DATE)   InvoiceDate, 
								c.trckdr                 AS DriverCode, 
								Isnull(sc.descrip, '')   AS DriverName, 
								c.routeType				 AS RouteType, 
								c.Route					 AS Route,
								COUNT(DISTINCT c.route)			AS NoOfTrips,
								Count(DISTINCT c.custno) AS NoOfStops, 
								Sum(c.cases)             AS Cases
						FROM   CTE_Core c 
								LEFT JOIN npiprosys.dbo.sycrlst sc ON sc.chrvl = c.trckdr 
										AND sc.ruleid = 'DRIVER  ' 
						WHERE  c.routeType <> ''
						GROUP  BY c.invdte, c.trckdr, sc.descrip, c.routeType, c.route
	
END

/*

exec [BI_TR_GetDriverTripsDayReport] '2018/06/01','2018/06/06','ANDALV'

*/


GO


/*

exec [BI_TR_GetDriverTripsDetailsReport] '2018/05/01','2018/05/30','','000'

*/

CREATE PROCEDURE [dbo].[BI_TR_GetDriverTripsDetailsReport] 
@currentStart DATETIME
,@currentEnd DATETIME
,@driverCode nvarchar(20) =''
,@route nvarchar(20) =''

AS 
BEGIN 
DECLARE  @currentStart_P DATETIME
		,@currentEnd_P DATETIME
		,@driverCode_P nvarchar(20)
		,@route_P nvarchar(20)
SET @currentStart_P = @currentStart
SET @currentEnd_P = @currentEnd
SET @driverCode_P = @driverCode
SET @route_P = @route

	SELECT am.invdte as InvoiceDate, 
            am.trckdr as DriverCode, 
            am.trcklc as TruckCode, 
			ISNULL(sc.descrip,'') AS DriverName,
            am.route as Route, 
			CASE WHEN am.route BETWEEN 0 AND 99 THEN 'Local' 
				 WHEN am.route BETWEEN 201 AND 299 THEN 'Local' 
				 WHEN am.route BETWEEN 101 AND 199 THEN 'Out of Town' 
				 ELSE '' 
			END  as RouteType,
			CASE WHEN am.route BETWEEN 0 AND 99 THEN 'First Run' 
				 WHEN am.route BETWEEN 201 AND 299 THEN 'Second Run' 
				 ELSE '' 
			END  as RouteRun,
            am.invno as InvoiceNumber, 
            am.custno as CustomerCode, 
			ac.company as CustomerName,
			ac.address1 as Address,
			ac.city as City,
			ac.addrstate as State,
			ac.zip as Zip,
            am.rtref as Reference, 
            Sum(at.tqtyshp) CasesDelivered 
        FROM   BI_View_ARMAS am 
			INNER JOIN BI_View_ARTRAN at ON at.invno = am.invno 
            --INNER JOIN artran at ON at.invno = am.invno 
			INNER JOIN arcust ac ON am.custno = ac.custno 
			LEFT JOIN NPIPROSYS.dbo.sycrlst sc ON sc.chrvl = am.trckdr AND sc.ruleid = 'DRIVER  ' 
        WHERE  am.trckdr <> '' 
			AND (@driverCode_P='' OR @driverCode_P <> '' AND am.trckdr=@driverCode_P)
			AND (@route_P ='' OR @route_P <> '' AND am.route= @route_P)
			AND am.invdte BETWEEN @currentStart_P AND @currentEnd_P
            AND Isnumeric(am.route) > 0 
        GROUP  BY am.invdte, am.trckdr, am.trcklc, am.route, am.invno, am.custno, am.rtref
				  ,ac.company,ac.address1,ac.city,ac.addrstate,ac.zip, sc.descrip
		ORDER BY am.invdte, am.route, am.custno		
	
END

/*

exec [BI_TR_GetDriverTripsDetailsReport] '2018/06/01','2018/06/06','ANDALV'

*/
GO

/*

exec [BI_TR_GetDriverTripsDayAndDetailsReport] '2018/06/01','2018/06/06','ANDALV'

*/

CREATE  PROCEDURE [dbo].[BI_TR_GetDriverTripsDayAndDetailsReport] 
@currentStart DATETIME
,@currentEnd DATETIME
,@driverCode nvarchar(20) =''
,@route nvarchar(20) =''

AS 

BEGIN

DECLARE  @currentStart_P DATETIME
		,@currentEnd_P DATETIME
		,@driverCode_P nvarchar(20)
		,@route_P nvarchar(20)

SET @currentStart_P = @currentStart
SET @currentEnd_P = @currentEnd
SET @driverCode_P = @driverCode
SET @route_P = @route


exec [BI_TR_GetDriverTripsDayReport] @currentStart_P ,@currentEnd_P, @driverCode_P ,@route_P
exec [BI_TR_GetDriverTripsDetailsReport] @currentStart_P ,@currentEnd_P, @driverCode_P ,@route_P
END




GO

/*

exec [BI_TR_GetAllDriversTripsConsolidatedReport] '2018/05/01','2018/05/30'

*/

CREATE PROCEDURE [dbo].[BI_TR_GetAllDriversTripsConsolidatedReport] 
@currentStart DATETIME
,@currentEnd DATETIME

AS 
BEGIN 
DECLARE  @currentStart_P DATETIME
		,@currentEnd_P DATETIME

SET @currentStart_P = @currentStart
SET @currentEnd_P = @currentEnd


;WITH CTE_Core 
    AS (
	SELECT am.invdte, 
            am.trckdr, 
            am.trcklc, 
            am.route, 
			CASE WHEN am.route BETWEEN 0 AND 99 THEN 'Local' 
				 WHEN am.route BETWEEN 201 AND 299 THEN 'Local' 
				 WHEN am.route BETWEEN 101 AND 199 THEN 'Out of Town' 
				 ELSE '' 
			END  as routeType,
            am.invno, 
            am.custno, 
            am.rtref, 
            Sum(at.tqtyshp) Cases 
        FROM   BI_View_ARMAS am 
			INNER JOIN BI_View_ARTRAN at ON at.invno = am.invno 
            --INNER JOIN artran at ON at.invno = am.invno 
        WHERE  am.trckdr <> '' 
			AND am.invdte BETWEEN @currentStart_P AND @currentEnd_P
            AND Isnumeric(am.route) > 0 
        GROUP  BY am.invdte, am.trckdr, am.trcklc, am.route, am.invno, am.custno, am.rtref
		)
		,CTE_DAY AS (SELECT Cast(c.invdte AS DATE)   invdate, 
								c.trckdr                 AS DriverCode, 
								c.routeType				 AS RouteType, 
								COUNT(DISTINCT c.route)			AS NoOfTrips,
								Count(DISTINCT c.custno) AS NoOfStops, 
								Sum(c.cases)             AS Cases
						FROM   CTE_Core c 
								
						WHERE  c.routeType <> ''
						GROUP  BY c.invdte, c.trckdr, c.routeType
		)

		SELECT c.DriverCode
				,Isnull(sc.descrip, '')   AS DriverName 
				,c.RouteType
				,SUM(c.NoOfTrips)		  AS NoOfTrips
				,SUM(c.NoOfStops)         AS NoOfStops
				,SUM(c.Cases)			  AS Cases
		FROM CTE_DAY c
				LEFT JOIN npiprosys.dbo.sycrlst sc ON  c.DriverCode = sc.chrvl
										AND sc.ruleid = 'DRIVER  ' 
		GROUP BY c.DriverCode, sc.descrip, c.RouteType 
	
END

/*

exec [BI_TR_GetAllDriversTripsConsolidatedReport] '2018/05/01','2018/05/30'

*/
GO




/*

exec [BI_TR_GetAllRoutesConsolidatedReport] '2018/05/01','2018/05/30'

*/

CREATE PROCEDURE [dbo].[BI_TR_GetAllRoutesConsolidatedReport] 
@currentStart DATETIME
,@currentEnd DATETIME

AS 
BEGIN 
DECLARE  @currentStart_P DATETIME
		,@currentEnd_P DATETIME

SET @currentStart_P = @currentStart
SET @currentEnd_P = @currentEnd


;WITH CTE_Core 
    AS (
	SELECT am.invdte, 
            am.trckdr, 
            am.trcklc, 
            am.route, 
			CASE WHEN am.route BETWEEN 0 AND 99 THEN 'Local' 
				 WHEN am.route BETWEEN 201 AND 299 THEN 'Local' 
				 WHEN am.route BETWEEN 101 AND 199 THEN 'Out of Town' 
				 ELSE '' 
			END  as routeType,
            am.invno, 
            am.custno, 
            am.rtref, 
            Sum(at.tqtyshp) Cases 
        FROM   BI_View_ARMAS am 		
			INNER JOIN BI_View_ARTRAN at ON at.invno = am.invno 
            --INNER JOIN artran at ON at.invno = am.invno 
        WHERE  am.trckdr <> '' 
			AND am.invdte BETWEEN @currentStart_P AND @currentEnd_P
            AND Isnumeric(am.route) > 0 
        GROUP  BY am.invdte, am.trckdr, am.trcklc, am.route, am.invno, am.custno, am.rtref
		)

		,CTE_DAY AS 
			(SELECT c.invdte				 AS	invdate, 
					c.route					 AS Route,
					c.routeType				 AS RouteType, 
					c.trckdr				 AS DriverCode,
					Count(DISTINCT c.custno) AS NoOfStops, 
					Sum(c.cases)             AS Cases
			FROM   CTE_Core c 
			WHERE  c.routeType <> ''
			GROUP  BY c.invdte, c.routeType, c.route, c.trckdr				 
		)
		SELECT 
				c.Route
				,c.RouteType
			    ,COUNT(DISTINCT c.DriverCode) as NoOfDrivers
				,SUM(c.NoOfStops) as NoOfStops
				,SUM(c.Cases) As Cases
		FROM CTE_DAY c
		GROUP BY c.Route, c.RouteType
		ORDER BY Route
END

/*

exec [BI_TR_GetAllRoutesConsolidatedReport] '2018/05/01','2018/05/30'

*/

GO

ALTER PROCEDURE [dbo].[BI_USR_AddUserCategoryAccess]
@UserId nvarchar(128),
@CategoryId INT	
AS
BEGIN

IF DB_NAME() like 'TEST%'
	BEGIN
		IF NOT EXISTS ( SELECT * FROM TESTNogalesDashboard.dbo.UserCategoryAccess  
						   WHERE UserId = @UserId
							   AND CategoryId = @CategoryId
					)
		BEGIN			   	
			INSERT INTO TESTNogalesDashboard.dbo.UserCategoryAccess (UserId,CategoryId) 
			VALUES(@UserId,@CategoryId)
		END
	END
ELSE 
	BEGIN
		IF NOT EXISTS ( SELECT * FROM NogalesDashboard.dbo.UserCategoryAccess  
					   WHERE UserId = @UserId
						   AND CategoryId = @CategoryId
				)
		BEGIN			   	
			INSERT INTO NogalesDashboard.dbo.UserCategoryAccess (UserId,CategoryId) 
			VALUES(@UserId,@CategoryId)
		END
	END
END

GO

ALTER	 PROCEDURE [dbo].[BI_USR_AddUserModuleAccess]
@UserId nvarchar(128),
@ModuleId INT	
AS
BEGIN

	IF DB_NAME() like 'TEST%'
	BEGIN
		IF NOT EXISTS ( SELECT * FROM TESTNogalesDashboard.dbo.UserModuleAccess 
					   WHERE UserId = @UserId
					   AND ModuleId = @ModuleId
					   )
			BEGIN
				INSERT INTO TESTNogalesDashboard.dbo.UserModuleAccess (UserId,ModuleId) 
				VALUES(@UserId,@ModuleId)
			END
	END
	ELSE 
	BEGIN
		IF NOT EXISTS ( SELECT * FROM NogalesDashboard.dbo.UserModuleAccess 
				   WHERE UserId = @UserId
				   AND ModuleId = @ModuleId
				   )
		BEGIN
			INSERT INTO NogalesDashboard.dbo.UserModuleAccess (UserId,ModuleId) 
			VALUES(@UserId,@ModuleId)
		END
	END
END

GO
ALTER PROCEDURE [dbo].[BI_USR_GetAllCategories]
	
AS
BEGIN
IF DB_NAME() like 'TEST%'
	BEGIN
		SELECT  C.Id AS Id,C.Name AS Name
		FROM TESTNogalesDashboard.dbo.Category C
	END
ELSE 
	BEGIN
		SELECT  C.Id AS Id,C.Name AS Name
		FROM NogalesDashboard.dbo.Category C
	END
END

GO
ALTER PROCEDURE [dbo].[BI_USR_GetAllModules]
	
AS
BEGIN
IF DB_NAME() like 'TEST%'
	BEGIN
		SELECT C.Id AS Id, C.Name as Name  
		FROM TESTNogalesDashboard.dbo.Module C
	END
ELSE 
	BEGIN
		SELECT C.Id AS Id, C.Name as Name  
		FROM NogalesDashboard.dbo.Module C
	END	
END

GO
ALTER PROCEDURE [dbo].[BI_USR_GetUserAssignedCategories]
@UserId nvarchar(128)
AS
BEGIN

IF DB_NAME() like 'TEST%'
	BEGIN
		SELECT  C.Id AS CategoryId,C.Name AS CategoryName
		FROM TESTNogalesDashboard.dbo.UserCategoryAccess UCA
		INNER JOIN TESTNogalesDashboard.dbo.AspNetUsers ANU ON UCA.UserId=ANU.Id
		INNER JOIN TESTNogalesDashboard.dbo.Category C ON C.Id=UCA.CategoryId
		WHERE ANU.Id=@UserId
	END
ELSE 
	BEGIN
		SELECT  C.Id AS CategoryId,C.Name AS CategoryName
		FROM NogalesDashboard.dbo.UserCategoryAccess UCA
		INNER JOIN NogalesDashboard.dbo.AspNetUsers ANU ON UCA.UserId=ANU.Id
		INNER JOIN NogalesDashboard.dbo.Category C ON C.Id=UCA.CategoryId
		WHERE ANU.Id=@UserId
	END
END

GO
ALTER PROCEDURE [dbo].[BI_USR_GetUserAssignedModules]
@UserId nvarchar(128)
AS
BEGIN
IF DB_NAME() like 'TEST%'
	BEGIN
		SELECT  M.Id AS ModuleId,M.Name AS ModuleName
		FROM TESTNogalesDashboard.dbo.UserModuleAccess UMA
		INNER JOIN TESTNogalesDashboard.dbo.AspNetUsers ANU ON UMA.UserId=ANU.Id
		INNER JOIN TESTNogalesDashboard.dbo.Module M ON M.Id=UMA.ModuleId
		WHERE ANU.Id=@UserId
	END
ELSE 
	BEGIN
		SELECT  M.Id AS ModuleId,M.Name AS ModuleName
		FROM NogalesDashboard.dbo.UserModuleAccess UMA
		INNER JOIN NogalesDashboard.dbo.AspNetUsers ANU ON UMA.UserId=ANU.Id
		INNER JOIN NogalesDashboard.dbo.Module M ON M.Id=UMA.ModuleId
		WHERE ANU.Id=@UserId
	END	
END

GO
ALTER  PROCEDURE [dbo].[BI_USR_GetUserDetails]
@UserId nvarchar(128)
AS
BEGIN
IF DB_NAME() like 'TEST%'
	BEGIN
		SELECT U.FirstName,U.LastName,U.Email,U.Id,U.IsRestrictedModuleAccess,U.IsRestrictedCategoryAccess
		FROM TESTNogalesDashboard.dbo.AspNetUsers  U
		WHERE U.Id=@UserId
	END
ELSE 
	BEGIN
		SELECT U.FirstName,U.LastName,U.Email,U.Id,U.IsRestrictedModuleAccess,U.IsRestrictedCategoryAccess
		FROM NogalesDashboard.dbo.AspNetUsers  U
		WHERE U.Id=@UserId
	END	
END

GO
ALTER PROCEDURE [dbo].[BI_USR_RemoveUserCategoryAccess]
@UserId nvarchar(128),
@CategoryId INT	
AS
BEGIN
IF DB_NAME() like 'TEST%'
	BEGIN
		DELETE FROM TESTNogalesDashboard.dbo.UserCategoryAccess 
		WHERE UserId=@UserId AND CategoryId=@CategoryId
	END
ELSE 
	BEGIN
		DELETE FROM NogalesDashboard.dbo.UserCategoryAccess 
		WHERE UserId=@UserId AND CategoryId=@CategoryId
	END	
END

GO
ALTER PROCEDURE [dbo].[BI_USR_RemoveUserModuleAccess]
@UserId nvarchar(128),
@ModuleId INT	
AS
BEGIN
IF DB_NAME() like 'TEST%'
	BEGIN
		DELETE FROM TESTNogalesDashboard.dbo.UserModuleAccess 
		WHERE UserId=@UserId AND ModuleId=@ModuleId
	END
ELSE 
	BEGIN
		DELETE FROM NogalesDashboard.dbo.UserModuleAccess 
		WHERE UserId=@UserId AND ModuleId=@ModuleId
	END
END

GO


ALTER PROCEDURE [dbo].[BI_PF_CustomerServiceDetailReport]
   @currentStart datetime,
	@currentEnd datetime,
	@priorStart datetime,
	@priorEnd datetime,
	@historicalStart datetime,
	@historicalEnd datetime
AS 
BEGIN


declare  @startDate_P datetime,
		 @endDate_P datetime,
		 @previousStart_P datetime,
		 @previousEnd_P datetime,
		 @historicalStart_p datetime,
		 @historicalEnd_p datetime
		 

		set  @startDate_P       = @currentStart 
		set	 @endDate_P 		= @currentEnd 
		set	 @previousStart_P 	= @priorStart 
		set	 @previousEnd_P 	= @priorEnd 
		set  @historicalStart_p = @historicalStart
		set  @historicalEnd_p   = @historicalEnd_p 
	;WITH CTE_CoreCurrent
	AS 
	(
	select * 
	from (
	SELECT 'current' as Period
	,extprice - (qtyshp*cost) as Profit
	,extprice
	,qtyshp,cost,spm.Speciality as Category	
	,CASE WHEN i.descrip IS NULL THEN 'Produce' ELSE i.descrip END AS Commodity
	,spm.AssignedPersonCode
	,spm.AssignedPersonDescription 
	,AT.adduser 
				FROM BI_View_ARTRAN at 
					INNER JOIN arcust ac ON ac.custno = at.custno AND ac.custno <> 'ICADJ' 
					INNER JOIN SalesPersonMapping spm on spm.AssignedPersonCode = at.salesmn 
					--and spm.EndDate is null
						AND at.invdte BETWEEN spm.startdate AND Isnull(spm.enddate, @endDate_P)
					LEFT JOIN ICIBIN AS ICB ON ICB.item = at.item AND ICB.binno = at.binno   
					INNER JOIN BI_View_ICITEM i ON i.item = at.item 
				WHERE (at.invdte BETWEEN @startDate_P AND @endDate_P)
						AND at.arstat NOT IN ( 'X', 'V' ) 
						AND at.item <> '' 
						AND at.salesmn <> '' 
						AND Ltrim(Rtrim(ac.speclty)) NOT IN('OOT', 'LOSS PROD') 
						AND ac.company not like '%DONATION%' 
						AND ac.company not like '%DUMP%'
						AND Ltrim(Rtrim(spm.speciality)) NOT IN('OSS')
						 AND spm.Speciality='FOOD SERVICE'
						)temp
    ),
	CTE_CorePrior AS ( select * from (
	SELECT 'prior' as Period,
	extprice - (qtyshp*cost) as Profit
			--,(SUM(cast(extprice- (qtyshp*cost) as numeric (36,2))))  AS Profit
			,extprice,qtyshp,cost,spm.Speciality as Category ,CASE WHEN i.descrip IS NULL THEN 'Produce' ELSE i.descrip END AS Commodity ,
			spm.AssignedPersonCode,
			spm.AssignedPersonDescription
			,AT.adduser 
				FROM BI_View_ARTRAN at 
					INNER JOIN arcust ac ON ac.custno = at.custno AND ac.custno <> 'ICADJ' 
					INNER JOIN SalesPersonMapping spm on spm.AssignedPersonCode = at.salesmn 
					AND at.invdte BETWEEN spm.startdate AND Isnull(spm.enddate, @previousEnd_P)
					LEFT JOIN ICIBIN AS ICB ON ICB.item = at.item AND ICB.binno = at.binno   
					INNER JOIN BI_View_ICITEM i ON i.item = at.item 
				WHERE (at.invdte BETWEEN @previousStart_P AND @previousEnd_P)
						AND at.arstat NOT IN ( 'X', 'V' ) 
						AND at.item <> '' 
						AND at.salesmn <> '' 
						AND Ltrim(Rtrim(ac.speclty)) NOT IN('OOT', 'LOSS PROD') 
						AND ac.company not like '%DONATION%' 
						AND ac.company not like '%DUMP%'
						AND Ltrim(Rtrim(spm.speciality)) NOT IN('OSS')
					    AND spm.Speciality='FOOD SERVICE'
						)t2
),

	CTE_CoreHistorical AS ( select * from (
	SELECT 'historical' as Period,
			--,(SUM(cast(extprice- (qtyshp*cost) as numeric (36,2))))  AS Profit
			extprice - (qtyshp*cost) as Profit
			,extprice,qtyshp,cost,spm.Speciality as Category,CASE WHEN i.descrip IS NULL THEN 'Produce' ELSE i.descrip END AS Commodity ,
			spm.AssignedPersonCode,
			spm.AssignedPersonDescription
			,AT.adduser 
				FROM BI_View_ARTRAN at 
					INNER JOIN arcust ac ON ac.custno = at.custno AND ac.custno <> 'ICADJ' 
					INNER JOIN SalesPersonMapping spm on spm.AssignedPersonCode = at.salesmn 
						AND at.invdte BETWEEN spm.startdate AND Isnull(spm.enddate, @historicalEnd_p)
					LEFT JOIN ICIBIN AS ICB ON ICB.item = at.item AND ICB.binno = at.binno   
					INNER JOIN BI_View_ICITEM i ON i.item = at.item 
				WHERE (at.invdte BETWEEN @historicalStart_p AND @historicalEnd_p)
						AND at.arstat NOT IN ( 'X', 'V' ) 
						AND at.item <> '' 
						AND at.salesmn <> '' 
						AND Ltrim(Rtrim(ac.speclty)) NOT IN('OOT', 'LOSS PROD') 
						AND ac.company not like '%DONATION%' 
						AND ac.company not like '%DUMP%'
						 AND spm.Speciality='FOOD SERVICE'
						AND Ltrim(Rtrim(spm.speciality)) NOT IN('OSS')
						)t2
)
		SELECT * FROM
		(SELECT * FROM CTE_CoreCurrent
		UNION ALL
		SELECT * FROM CTE_CorePrior
		UNION ALL
		SELECT * FROM CTE_CoreHistorical
		)t
END


GO
ALTER PROCEDURE [dbo].[BI_EP_GetEmployeeOPEXCOGSPayRollDetails]   
    @currentStart datetime,    
     @currentEnd datetime,    
     @historicalStart datetime,  
     @historicalEnd datetime,  
     @priorStart datetime,  
     @priorEnd  datetime,  
     @CogsAccountStart varchar(10),  
     @CogsAccountEnd varchar(10),  
     @OpexAccountStart varchar(10),  
    @opexAccountEnd varchar(10)  
  
AS  
BEGIN  
   
	  --OPEX COGS  
	  --If date prior date and current date not in overlapped state
declare @currentStart_P datetime,    
     @currentEnd_P datetime,    
     @historicalStart_P datetime,  
     @historicalEnd_P datetime,  
     @priorStart_P datetime,  
     @priorEnd_P  datetime,  
     @CogsAccountStart_P varchar(10),  
     @CogsAccountEnd_P varchar(10),  
     @OpexAccountStart_P varchar(10),  
     @opexAccountEnd_P varchar(10)  

	 set  @currentStart_P   = @currentStart     
     set @currentEnd_P    	= @currentEnd    
     set @historicalStart_P = @historicalStart   
     set @historicalEnd_P   = @historicalEnd   
     set @priorStart_P   	= @priorStart   
     set @priorEnd_P    	= @priorEnd    
     set @CogsAccountStart_P= @CogsAccountStart 
     set @CogsAccountEnd_P 	= @CogsAccountEnd 
     set @OpexAccountStart_P= @OpexAccountStart 
     set @opexAccountEnd_P 	= @opexAccountEnd 


IF DB_NAME() like 'TEST%'
	BEGIN
			;WITH cte_current 
		 AS (SELECT dateadded, 
					hourshund, 
					description, 
					NAME, 
					empid, 
					departmentname, 
					amount, 
					period 
			 FROM   (SELECT EP.date            AS DateAdded, 
							EP.hourshund       HoursHund, 
							PD.description, 
							E.NAME, 
							E.empid            AS EmpID, 
							EP.department      AS DepartmentName, 
							Isnull(EP.rate, 0) AS Amount, 
							'current'          AS Period 
					 FROM   TESTNogalesDashboard.dbo.employeepaymentdetails EP 
							INNER JOIN TESTNogalesDashboard.dbo.paymentdescription PD ON EP.paymentdescriptionid = PD.id 
							INNER JOIN TESTNogalesDashboard.dbo.employee E ON E.id = EP.employeeid 
					 WHERE  ep.date BETWEEN @currentStart_P AND @currentEnd_P)tem) 
		SELECT * 
		FROM   cte_current 
	END
ELSE 
	BEGIN
		;WITH cte_current 
		 AS (SELECT dateadded, 
					hourshund, 
					description, 
					NAME, 
					empid, 
					departmentname, 
					amount, 
					period 
			 FROM   (SELECT EP.date            AS DateAdded, 
							EP.hourshund       HoursHund, 
							PD.description, 
							E.NAME, 
							E.empid            AS EmpID, 
							EP.department      AS DepartmentName, 
							Isnull(EP.rate, 0) AS Amount, 
							'current'          AS Period 
					 FROM   NogalesDashboard.dbo.employeepaymentdetails EP 
							INNER JOIN NogalesDashboard.dbo.paymentdescription PD ON EP.paymentdescriptionid = PD.id 
							INNER JOIN NogalesDashboard.dbo.employee E ON E.id = EP.employeeid 
					 WHERE  ep.date BETWEEN @currentStart_P AND @currentEnd_P)tem) 
		SELECT * 
		FROM   cte_current 
	END
END


GO



GO
CREATE NONCLUSTERED INDEX NIX_arymst_invdte_trckdr
ON [dbo].[arymst] ([invdte],[trckdr])
INCLUDE ([invno],[custno],[route],[trcklc],[rtref])
GO

GO
CREATE NONCLUSTERED INDEX NIX_artran_invdte
ON [dbo].[artran] ([invdte])
INCLUDE ([invno],[tqtyshp])
GO


GO
CREATE NONCLUSTERED INDEX NIX_arytrn_invdte
ON [dbo].[arytrn] ([invdte])
INCLUDE ([invno],[tqtyshp])
GO

