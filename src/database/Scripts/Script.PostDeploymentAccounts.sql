/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

PRINT 'Environment Var: $(env)'

IF (NOT EXISTS(SELECT * FROM sys.database_principals WHERE [name] = 'sg-sql-portal-dev-eastus-01-admins'))  
BEGIN  
	CREATE USER [sg-sql-portal-dev-eastus-01-admins] FROM EXTERNAL PROVIDER
	ALTER ROLE [db_datawriter] ADD MEMBER [sg-sql-portal-dev-eastus-01-admins]
	SELECT * FROM sys.database_principals WHERE [name] = 'sg-sql-portal-dev-eastus-01-admins'
END  
