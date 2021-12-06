output "function_app_name" {
  value     = azurerm_function_app.function_app.name
  sensitive = false
}

output "sql_database_connection_string" {
  value     = azurerm_key_vault_secret.sql_server_connection_string.value
  sensitive = true
}
