resource "azurerm_app_service_plan" "function_app_service_plan" {
  name                = local.function_app_service_plan_name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  location            = azurerm_resource_group.core_resource_group.location
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}