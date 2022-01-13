resource "azurerm_role_assignment" "sql_server_principal_directory_reader_role" {
  scope                = data.azurerm_subscription.current.id
  role_definition_name = "Directory Readers"
  principal_id         = azurerm_mssql_server.sql_server.identity[0].principal_id
}
