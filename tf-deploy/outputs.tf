output "function_app_name" {
  value     = azurerm_function_app.function_app.name
  sensitive = false
}

output "sql_server_name" {
  value     = azurerm_mssql_server.sql_server.name
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