resource "azurerm_api_management_subscription" "b3bot_subscription" {
  api_management_name = azurerm_api_management.api_management.name
  resource_group_name = azurerm_api_management.api_management.resource_group_name
  display_name        = "B3Bots"
  api_id              = azurerm_api_management_api.apim_player_ingest.id
  state               = "active"
}

resource "azurerm_api_management_subscription" "ingest_funcapp_subscription" {
  api_management_name = azurerm_api_management.api_management.name
  resource_group_name = azurerm_api_management.api_management.resource_group_name
  display_name        = "Ingest Function App"
  state               = "active"
}
