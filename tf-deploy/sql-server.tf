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

  azuread_administrator {
    login_username              = azuread_group.sql_server_admins.display_name
    object_id                   = azuread_group.sql_server_admins.object_id
    azuread_authentication_only = true
  }

  identity {
    type = "SystemAssigned"
  }

  public_network_access_enabled = true
}

resource "azurerm_mssql_firewall_rule" "azure_services_firewall_rule" {
  name             = "AllowAzureServices"
  server_id        = azurerm_mssql_server.sql_server.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}

resource "azurerm_mssql_firewall_rule" "dedicated_agent_pool_firewall_rule" {
  name             = "DedicatedAgentPool"
  server_id        = azurerm_mssql_server.sql_server.id
  start_ip_address = "51.79.83.13"
  end_ip_address   = "51.79.83.13"
}

resource "azurerm_mssql_database" "portal_database" {
  name      = local.sql_database_name
  server_id = azurerm_mssql_server.sql_server.id
  collation = "SQL_Latin1_General_CP1_CI_AS"
  sku_name  = "Basic"
}
