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

resource "azurerm_servicebus_queue" "chat_message_queue" {
  name                = "chat_message_queue"
  resource_group_name = azurerm_resource_group.core_resource_group.name
  namespace_name      = azurerm_servicebus_namespace.service_bus.name

  enable_partitioning = true
}

resource "azurerm_servicebus_queue" "map_change_queue" {
  name                = "map_change_queue"
  resource_group_name = azurerm_resource_group.core_resource_group.name
  namespace_name      = azurerm_servicebus_namespace.service_bus.name

  enable_partitioning = true
}

resource "azurerm_servicebus_queue" "server_register_queue" {
  name                = "server_register_queue"
  resource_group_name = azurerm_resource_group.core_resource_group.name
  namespace_name      = azurerm_servicebus_namespace.service_bus.name

  enable_partitioning = true
}