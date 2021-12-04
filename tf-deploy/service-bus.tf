resource "azurerm_servicebus_namespace" "service_bus" {
  name                = local.service_bus_name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  location            = azurerm_resource_group.core_resource_group.location
  sku                 = "Basic"
}

resource "azurerm_servicebus_queue" "player_connected_queue" {
  name                = "player_connected_queue"
  resource_group_name = azurerm_resource_group.core_resource_group.name
  namespace_name      = azurerm_servicebus_namespace.service_bus.name

  enable_partitioning = true
}

resource "azurerm_key_vault_secret" "service_bus_connection_string" {
  name         = local.service_bus_connection_string_secret
  value        = azurerm_servicebus_namespace.service_bus.default_primary_connection_string
  key_vault_id = azurerm_key_vault.key_vault.id
}
