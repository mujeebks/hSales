USE [NPICOMPANY01]
GO

/****** Object:  StoredProcedure [dbo].[BI_USR_AssignCustomerPrefix]    Script Date: 7/31/2018 8:39:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BI_USR_AssignCustomerPrefix]
	@collectorAssignmentId INT,
	@collectorId INT NULL
AS
BEGIN
	IF DB_NAME() like 'TEST%'
		BEGIN
			UPDATE TESTNogalesDashboard.dbo.CollectorAssignment
			SET
			CollectorId = @collectorId
			WHERE CollectorAssignmentId = @collectorAssignmentId
		END
	ELSE 
		BEGIN
			UPDATE NogalesDashboard.dbo.CollectorAssignment
			SET
			CollectorId = @collectorId
			WHERE CollectorAssignmentId = @collectorAssignmentId
			
		END
	
END

GO
/****** Object:  StoredProcedure [dbo].[BI_USR_DeleteCollector]    Script Date: 7/31/2018 8:39:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BI_USR_DeleteCollector]
	 @collectorId INT
AS
BEGIN
	DECLARE @ordinance INT
	IF DB_NAME() like 'TEST%'
		BEGIN
			SELECT @ordinance=Ordinance FROM TESTNogalesDashboard.dbo.Collector WHERE CollectorId=@collectorId

			UPDATE TESTNogalesDashboard.dbo.CollectorAssignment
			SET CollectorId=NULL
			WHERE  CollectorId=@collectorId

			DELETE FROM TESTNogalesDashboard.dbo.Collector WHERE CollectorId=@collectorId

			UPDATE TESTNogalesDashboard.dbo.Collector 
			SET Ordinance= Ordinance-1
			WHERE Ordinance > @ordinance
		END
	ELSE 
		BEGIN
			SELECT @ordinance=Ordinance FROM NogalesDashboard.dbo.Collector WHERE CollectorId=@collectorId

			UPDATE NogalesDashboard.dbo.CollectorAssignment
			SET CollectorId=NULL
			WHERE  CollectorId=@collectorId

			DELETE FROM NogalesDashboard.dbo.Collector WHERE CollectorId=@collectorId	

			UPDATE NogalesDashboard.dbo.Collector 
			SET Ordinance= Ordinance-1
			WHERE Ordinance > @ordinance		
		END


	
END

GO
/****** Object:  StoredProcedure [dbo].[BI_USR_GetAllCollectors]    Script Date: 7/31/2018 8:39:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BI_USR_GetAllCollectors] 
AS
BEGIN
	
	IF DB_NAME() like 'TEST%'
		BEGIN
			SELECT c.CollectorId
			, c.CollectorName
			, c.Ordinance
			, ca.CollectorAssignmentId
			, ca.LetterName
			FROM TESTNogalesDashboard.dbo.Collector c	
			LEFT JOIN TESTNogalesDashboard.dbo.CollectorAssignment ca
			ON c.CollectorId = ca.CollectorId	
		END
	ELSE 
		BEGIN
			SELECT c.CollectorId
			, c.CollectorName
			, c.Ordinance
			, ca.CollectorAssignmentId
			, ca.LetterName
			FROM NogalesDashboard.dbo.Collector c	
			LEFT JOIN NogalesDashboard.dbo.CollectorAssignment ca
			ON c.CollectorId = ca.CollectorId	
		END

END

GO
/****** Object:  StoredProcedure [dbo].[BI_USR_GetAllUnAssignedCustomerPrefixes]    Script Date: 7/31/2018 8:39:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BI_USR_GetAllUnAssignedCustomerPrefixes] 
AS
BEGIN
	IF DB_NAME() like 'TEST%'
		BEGIN
			SELECT ca.CollectorAssignmentId
			, ca.LetterName
		FROM TESTNogalesDashboard.dbo.CollectorAssignment ca	
		WHERE ca.CollectorId is NULL
		END
	ELSE 
		BEGIN
			SELECT ca.CollectorAssignmentId
			, ca.LetterName
			FROM NogalesDashboard.dbo.CollectorAssignment ca	
			WHERE ca.CollectorId is NULL
		END
	

	
END


/*

exec BI_USR_GetAllUnAssignedCustomerPrefixes

*/
GO
/****** Object:  StoredProcedure [dbo].[BI_USR_InsertCollector]    Script Date: 7/31/2018 8:39:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BI_USR_InsertCollector]
	 @collectorName varchar(150)
AS
BEGIN
	DECLARE @ordinance INT
	IF DB_NAME() like 'TEST%'
		BEGIN
			SELECT @ordinance= (MAX(Ordinance)+1) FROM TESTNogalesDashboard.dbo.Collector
	
			INSERT INTO TESTNogalesDashboard.dbo.Collector 
			(
				CollectorName,
				Ordinance
			)
			VALUES
			(
				@collectorName,
				@ordinance
			)
		END
	ELSE 
		BEGIN
			
			SELECT @ordinance= (MAX(Ordinance)+1) FROM NogalesDashboard.dbo.Collector
	
			INSERT INTO NogalesDashboard.dbo.Collector 
			(
				CollectorName,
				Ordinance
			)
			VALUES
			(
				@collectorName,
				@ordinance
			)
		END

	
END

GO
/****** Object:  StoredProcedure [dbo].[BI_USR_IsExistCollector]    Script Date: 7/31/2018 8:39:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC  [dbo].[BI_USR_IsExistCollector] 'Humberto Hernandez',0

CREATE PROCEDURE [dbo].[BI_USR_IsExistCollector]
	@collectorName VARCHAR(150),
	@collectorId INT =0
AS
BEGIN

	IF DB_NAME() like 'TEST%'
		BEGIN
			SELECT CollectorId 
			FROM TESTNogalesDashboard.dbo.Collector 
			WHERE CollectorName=@collectorName 
				AND (CollectorId <> @collectorId OR @collectorId = 0)
		END
	ELSE 
		BEGIN
			SELECT CollectorId 
			FROM NogalesDashboard.dbo.Collector 
			WHERE CollectorName=@collectorName 
				AND (CollectorId <> @collectorId OR @collectorId = 0)
		END

END

GO
/****** Object:  StoredProcedure [dbo].[BI_USR_UpdateCollector]    Script Date: 7/31/2018 8:39:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC BI_USR_UpdateCollector 5,'Humberto Hernandez1',4

CREATE PROCEDURE [dbo].[BI_USR_UpdateCollector]
	 @collectorId INT,
	 @collectorName VARCHAR(150),
	 @newOrder INT
AS
BEGIN
	DECLARE @oldOrder INT
	DECLARE @newOrderCollectorId INT

	IF DB_NAME() like 'TEST%'
		BEGIN
			SELECT @oldOrder=Ordinance FROM TESTNogalesDashboard.dbo.Collector WHERE CollectorId=@collectorId
			SELECT @newOrderCollectorId=CollectorId FROM  TESTNogalesDashboard.dbo.Collector WHERE Ordinance=@newOrder

			UPDATE TESTNogalesDashboard.dbo.Collector SET
				CollectorName = @collectorName,
				Ordinance = @newOrder
			WHERE CollectorId = @collectorId

			IF (@oldOrder <> @newOrder)
			BEGIN
				IF (@newOrder > @oldOrder)
					BEGIN
						UPDATE TESTNogalesDashboard.dbo.Collector 
						SET Ordinance = Ordinance-1
						WHERE Ordinance <= @newOrder AND CollectorId <> @collectorId		
					END
				ELSE
					BEGIN
						UPDATE TESTNogalesDashboard.dbo.Collector 
						SET Ordinance = Ordinance+1
						WHERE Ordinance >= @newOrder AND CollectorId <> @collectorId
					END
			END
		END
	ELSE 
		BEGIN
			SELECT @oldOrder=Ordinance FROM NogalesDashboard.dbo.Collector WHERE CollectorId=@collectorId
			SELECT @newOrderCollectorId=CollectorId FROM NogalesDashboard.dbo.Collector WHERE Ordinance=@newOrder

			UPDATE NogalesDashboard.dbo.Collector SET
				CollectorName = @collectorName,
				Ordinance = @newOrder
			WHERE CollectorId = @collectorId

			IF (@oldOrder <> @newOrder)
			BEGIN
				IF (@newOrder > @oldOrder)
					BEGIN
						UPDATE NogalesDashboard.dbo.Collector 
						SET Ordinance = Ordinance-1
						WHERE Ordinance <= @newOrder AND CollectorId <> @collectorId		
					END
				ELSE
					BEGIN
						UPDATE NogalesDashboard.dbo.Collector 
						SET Ordinance = Ordinance+1
						WHERE Ordinance >= @newOrder AND CollectorId <> @collectorId	
					END
			END
			
		END

	
END

GO

/****** Object:  StoredProcedure [dbo].[BI_USR_UpdateCollector]    Script Date: 8/1/2018 10:45:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[BI_USR_UpdateCollector]
	 @collectorId INT,
	 @collectorName VARCHAR(150),
	 @newOrder INT
AS
BEGIN
	DECLARE @oldOrder INT
	DECLARE @newOrderCollectorId INT

	IF DB_NAME() like 'TEST%'
		BEGIN
			SELECT @oldOrder=Ordinance FROM TESTNogalesDashboard.dbo.Collector WHERE CollectorId=@collectorId
			SELECT @newOrderCollectorId=CollectorId FROM  TESTNogalesDashboard.dbo.Collector WHERE Ordinance=@newOrder

			UPDATE TESTNogalesDashboard.dbo.Collector SET
				CollectorName = @collectorName,
				Ordinance = @newOrder
			WHERE CollectorId = @collectorId

			IF (@oldOrder <> @newOrder)
			BEGIN
				IF (@newOrder > @oldOrder)
					BEGIN
						UPDATE TESTNogalesDashboard.dbo.Collector 
						SET Ordinance = Ordinance-1
						WHERE Ordinance <= @newOrder AND Ordinance > @oldOrder AND CollectorId <> @collectorId		
					END
				ELSE
					BEGIN
						UPDATE TESTNogalesDashboard.dbo.Collector 
						SET Ordinance = Ordinance+1
						WHERE Ordinance >= @newOrder AND Ordinance < @oldOrder AND CollectorId <> @collectorId
					END
			END
		END
	ELSE 
		BEGIN
			SELECT @oldOrder=Ordinance FROM NogalesDashboard.dbo.Collector WHERE CollectorId=@collectorId
			SELECT @newOrderCollectorId=CollectorId FROM NogalesDashboard.dbo.Collector WHERE Ordinance=@newOrder

			UPDATE NogalesDashboard.dbo.Collector SET
				CollectorName = @collectorName,
				Ordinance = @newOrder
			WHERE CollectorId = @collectorId

			IF (@oldOrder <> @newOrder)
			BEGIN
				IF (@newOrder > @oldOrder)
					BEGIN
						UPDATE NogalesDashboard.dbo.Collector 
						SET Ordinance = Ordinance-1
						WHERE Ordinance <= @newOrder AND Ordinance > @oldOrder AND CollectorId <> @collectorId		
					END
				ELSE
					BEGIN
						UPDATE NogalesDashboard.dbo.Collector 
						SET Ordinance = Ordinance+1
						WHERE Ordinance >= @newOrder AND Ordinance < @oldOrder AND CollectorId <> @collectorId	
					END
			END
			
		END

	
END
