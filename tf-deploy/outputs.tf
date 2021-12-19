output "events-func_name" {
  value     = azurerm_function_app.events_function_app.name
  sensitive = false
}

output "ingest-func_name" {
  value     = azurerm_function_app.ingest_function_app.name
  sensitive = false
}

output "repository-func_name" {
  value     = azurerm_function_app.repository_function_app.name
  sensitive = false
}

output "mgmt-web_name" {
  value     = azurerm_app_service.mgmt_web_app.name
  sensitive = false
}

output "mgmt-web_resource_group" {
  value     = azurerm_app_service.mgmt_web_app.resource_group_name
  sensitive = false
}

output "admin-web_name" {
  value     = azurerm_app_service.admin_web_app.name
  sensitive = false
}

output "admin-web_resource_group" {
  value     = azurerm_app_service.admin_web_app.resource_group_name
  sensitive = false
}

output "public-web_name" {
  value     = azurerm_app_service.public_web_app.name
  sensitive = false
}

output "public-web_resource_group" {
  value     = azurerm_app_service.public_web_app.resource_group_name
  sensitive = false
}

output "repository-webapi_name" {
  value     = azurerm_app_service.repository_web_api.name
  sensitive = false
}

output "repository-webapi_resource_group" {
  value     = azurerm_app_service.repository_web_api.resource_group_name
  sensitive = false
}

output "sql_server_domain_name" {
  value     = azurerm_mssql_server.sql_server.fully_qualified_domain_name
  sensitive = true
}

output "sql_database_name" {
  value     = azurerm_mssql_database.portal_database.name
  sensitive = true
}

output "sql_admin_username" {
  value     = azurerm_key_vault_secret.sql_server_admin_username.value
  sensitive = true
}

output "sql_admin_password" {
  value     = azurerm_key_vault_secret.sql_server_admin_password.value
  sensitive = true
}
