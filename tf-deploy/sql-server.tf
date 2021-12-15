resource "random_password" "sql_server_admin_password" {
  length  = 32
  special = true
}

resource "azurerm_mssql_server" "sql_server" {
  name                = local.sql_server_name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  location            = azurerm_resource_group.core_resource_group.location

  version = "12.0"

  administrator_login          = local.sql_server_admin_username
  administrator_login_password = random_password.sql_server_admin_password.result

  public_network_access_enabled = true
}

resource "azurerm_mssql_database" "portal_database" {
  name                        = local.sql_database_name
  server_id                   = azurerm_mssql_server.sql_server.id
  collation                   = "SQL_Latin1_General_CP1_CI_AS"
  max_size_gb                 = 32
  read_scale                  = false
  sku_name                    = "GP_S_Gen5_1"
  auto_pause_delay_in_minutes = 60
  min_capacity                = 0.5
  geo_backup_enabled          = false
}
