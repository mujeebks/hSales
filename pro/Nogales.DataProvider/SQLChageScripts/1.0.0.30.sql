--CREATE TABLE [dbo].[UserDisplayModuleAccess](
--	[UserId] [nvarchar](128) NOT NULL,
--	[ModuleId] [int] NOT NULL
--) ON [PRIMARY]


GO


CREATE PROCEDURE [dbo].[BI_USR_AddRemoveUserDisplayModuleAccess]

@UserId nvarchar(128),
@ModuleId INT	
AS
BEGIN
	IF DB_NAME() like 'TEST%'
	BEGIN
		IF NOT EXISTS ( SELECT * FROM TESTNogalesDashboard.dbo.UserDisplayModuleAccess 
					   WHERE UserId = @UserId
					   )
			BEGIN
				INSERT INTO TESTNogalesDashboard.dbo.UserDisplayModuleAccess (

				 UserId
				,ModuleId

				) 

				VALUES(@UserId,@ModuleId)
			END
		ELSE
			BEGIN 
				UPDATE TESTNogalesDashboard.dbo.UserDisplayModuleAccess 
				SET UserId = @UserId,
					ModuleId = @ModuleId
				WHERE UserId = @UserId
	
			END
	END
	BEGIN
		IF NOT EXISTS ( SELECT * FROM NogalesDashboard.dbo.UserDisplayModuleAccess 
					   WHERE UserId = @UserId
					   )
			BEGIN
				INSERT INTO NogalesDashboard.dbo.UserDisplayModuleAccess (

				 UserId
				,ModuleId

				) 

				VALUES(@UserId,@ModuleId)
			END
		ELSE
			BEGIN 
				UPDATE NogalesDashboard.dbo.UserDisplayModuleAccess 
				SET UserId = @UserId,
					ModuleId = @ModuleId
				WHERE UserId = @UserId
	
			END
	END
END

GO

CREATE PROCEDURE [dbo].[BI_USR_GetUserAssignedDisplayModules]

@UserId nvarchar(128)
	
AS
BEGIN
	IF DB_NAME() like 'TEST%'
	BEGIN
	
		SELECT  ModuleId, M.Name AS ModuleName
		FROM TESTNogalesDashboard.dbo.UserDisplayModuleAccess UMA
		INNER JOIN TESTNogalesDashboard.dbo.AspNetUsers ANU ON UMA.UserId=ANU.Id
		INNER JOIN TESTNogalesDashboard.dbo.Module M ON M.Id=UMA.ModuleId
		WHERE ANU.Id=@UserId
	END
	ELSE
	BEGIN
		SELECT  ModuleId, M.Name AS ModuleName
		FROM NogalesDashboard.dbo.UserDisplayModuleAccess UMA
		INNER JOIN NogalesDashboard.dbo.AspNetUsers ANU ON UMA.UserId=ANU.Id
		INNER JOIN NogalesDashboard.dbo.Module M ON M.Id=UMA.ModuleId
		WHERE ANU.Id=@UserId
	END
	
END

GO

CREATE PROCEDURE [dbo].[BI_USR_RemoveUserDisplayModuleAccess]

@UserId nvarchar(128)
AS
BEGIN
	IF DB_NAME() like 'TEST%'
	BEGIN
		IF EXISTS ( SELECT * FROM TESTNogalesDashboard.dbo.UserDisplayModuleAccess 
					   WHERE UserId = @UserId
					   )
		BEGIN
			DELETE FROM TESTNogalesDashboard.dbo.UserDisplayModuleAccess
			WHERE UserId = @UserId
		END
	END
	ELSE
	BEGIN
		IF EXISTS ( SELECT * FROM NogalesDashboard.dbo.UserDisplayModuleAccess 
					   WHERE UserId = @UserId
					   )
		BEGIN
			DELETE FROM NogalesDashboard.dbo.UserDisplayModuleAccess
			WHERE UserId = @UserId
		END
	END
END


GO



/*
exec [BI_WH_GetWarehouseShortReport] '2018/06/26'
*/
CREATE PROCEDURE [dbo].[BI_WH_GetWarehouseShortReport] 
		 @shipDate datetime,
		 @routNo varchar(50) ='',
		 @buyerName varchar(50)=''
AS
BEGIN
	select  sot.sono as 'SO Num'
                        ,sot.custno , cu.company as customer, cu.email as email , itm.buyer,sot.tprice as 'Mkt.Price',sot.[id_col] as 'Id'
                        ,sot.item as item, sot.descrip as description, sot.umeasur as UoM
                        ,sot.tcost as 'Trans.Cost', sot.cost
                        ,sot.rqdate, som.route as route
                        ,sot.origqtyord as QtyOrdered--, sot.qtyord
                        , loc.lonhand QtyOnHand
						, loc.lonhand - loc.lsoaloc as QtyAvailable
						,sot.binno,som.picker,sw.alphaname pickername
                        from sotran sot
                        inner join arcust cu on cu.custno = sot.custno
                        inner join somast som on som.sono = sot.sono
                        inner join icitem itm on itm.item = sot.item
                        inner join iciloc loc on loc.loctid = sot.loctid and loc.item = sot.item
                        LEFT JOIN SOShortExclusion soe on soe.id = sot.id_col
						left join SOWPKR sw on sw.userid=som.picker  and  sw.loctid='ONSITE'
                        where sot.rqdate =@shipDate   AND 
						sot.sostat = '' 
                            AND (sot.shipped <> '' OR sot.caspo <> '' OR sot.issub = 1  OR sot.ishere = 1 ) 
                            AND (@routNo='' or( som.route <>'' AND som.route = @routNo))
                            AND (@buyerName ='' OR(itm.buyer <>'' AND itm.buyer =  @buyerName ))
                            AND soe.Id IS NULL
                        order by sot.descrip
						
END


GO


CREATE PROCEDURE [dbo].[BI_WH_GetWarehouseShortReportbyIds]   
   @shipDate datetime,  
   @routNo varchar(50),  
   @buyerName varchar(50),  
   @selelectedIds IntTable readonly
AS  
BEGIN  
select   
        sot.sono as 'SO Num'  
        ,sot.custno , cu.company as customer, cu.email as email , itm.buyer,sot.tprice as 'Mkt.Price',sot.[id_col] as 'Id'  
        ,sot.item as item, sot.descrip as description, sot.umeasur as UoM  
        ,sot.tcost as 'Trans.Cost', sot.cost  
        ,sot.rqdate, som.route as route  
        ,sot.origqtyord as QtyOrdered--, sot.qtyord  
        , loc.lonhand QtyOnHand, loc.lonhand - loc.lsoaloc as QtyAvailable ,sot.binno,som.picker,sw.alphaname pickername
        from sotran sot  
        inner join arcust cu on cu.custno = sot.custno  
        inner join somast som on som.sono = sot.sono  
        inner join icitem itm on itm.item = sot.item  
        inner join iciloc loc on loc.loctid = sot.loctid and loc.item = sot.item  
        LEFT JOIN NPICOMPANY01..SOShortExclusion soe on soe.id = sot.id_col  
		left join SOWPKR sw on sw.userid=som.picker and  sw.loctid='ONSITE'
        inner join @selelectedIds s on s.Id=sot.id_col
        where sot.rqdate =@shipDate  
            AND sot.sostat = ''   
                                    AND (sot.shipped <> '' OR sot.caspo <> '' OR sot.issub = 1  OR sot.ishere = 1 )   
                                    AND (@routNo ='' or( som.route <>'' AND som.route = @routNo))  
                                    AND (@buyerName='' OR(itm.buyer <>'' AND itm.buyer = @buyerName ))  
                                    -- AND sot.[id_col] in (select id from CSVToTable(@selelectedIds))  
                                    AND soe.Id IS NULL  
        order by sot.descrip  
END  
