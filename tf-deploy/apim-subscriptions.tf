resource "azurerm_api_management_subscription" "b3bot_subscription" {
  api_management_name = azurerm_api_management.api_management.name
  resource_group_name = azurerm_api_management.api_management.resource_group_name
  display_name        = "B3Bots"
  state               = "active"
  allow_tracing       = false
}

resource "azurerm_api_management_subscription" "ingest_funcapp_subscription" {
  api_management_name = azurerm_api_management.api_management.name
  resource_group_name = azurerm_api_management.api_management.resource_group_name
  display_name        = "Ingest Function App"
  state               = "active"
  allow_tracing       = false
}

resource "azurerm_api_management_subscription" "mgmt_web_app_subscription" {
  api_management_name = azurerm_api_management.api_management.name
  resource_group_name = azurerm_api_management.api_management.resource_group_name
  display_name        = "Mgmt Web App"
  state               = "active"
  allow_tracing       = false
}
