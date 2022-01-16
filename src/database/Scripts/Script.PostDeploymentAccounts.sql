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
PRINT 'Environment Var: $(region)'
PRINT 'Environment Var: $(instance)'
PRINT 'Database Name (Predefined Var): $(DatabaseName)'

IF (NOT EXISTS(SELECT * FROM sys.database_principals WHERE [name] = 'sg-sql-portal-$(env)-$(region)-$(instance)-$(DatabaseName)-readers'))  
BEGIN  
	PRINT 'Adding user: sg-sql-portal-$(env)-$(region)-$(instance)-$(DatabaseName)-readers to [db_datareader]'
	CREATE USER [sg-sql-portal-$(env)-$(region)-$(instance)-$(DatabaseName)-readers] FROM EXTERNAL PROVIDER
	ALTER ROLE [db_datareader] ADD MEMBER [sg-sql-portal-$(env)-$(region)-$(instance)-$(DatabaseName)-readers]
	SELECT * FROM sys.database_principals WHERE [name] = 'sg-sql-portal-$(env)-$(region)-$(instance)-$(DatabaseName)-readers'
END  

IF (NOT EXISTS(SELECT * FROM sys.database_principals WHERE [name] = 'sg-sql-portal-$(env)-$(region)-$(instance)-$(DatabaseName)-writers'))  
BEGIN  
	PRINT 'Adding user: sg-sql-portal-$(env)-$(region)-$(instance)-$(DatabaseName)-writers to [db_datawriter]'
	CREATE USER [sg-sql-portal-$(env)-$(region)-$(instance)-$(DatabaseName)-writers] FROM EXTERNAL PROVIDER
	ALTER ROLE [db_datawriter] ADD MEMBER [sg-sql-portal-$(env)-$(region)-$(instance)-$(DatabaseName)-writers]
	SELECT * FROM sys.database_principals WHERE [name] = 'sg-sql-portal-$(env)-$(region)-$(instance)-$(DatabaseName)-writers'
END  