resource "azurerm_api_management" "api_management" {
  name                = local.apim_name
  location            = azurerm_resource_group.core_resource_group.location
  resource_group_name = azurerm_resource_group.core_resource_group.name
  publisher_name      = "XtremeIdiots"
  publisher_email     = "admin@xtremeidiots.com"

  sku_name = "Consumption_0"

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_api_management_logger" "api_management_api_logger" {
  name                = local.apim_logger
  api_management_name = azurerm_api_management.api_management.name
  resource_group_name = azurerm_resource_group.core_resource_group.name

  application_insights {
    instrumentation_key = azurerm_application_insights.application_insights.instrumentation_key
  }
}