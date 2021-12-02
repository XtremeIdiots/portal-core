resource "azurerm_application_insights" "application_insights" {
  name                = local.app_insights_name
  location            = azurerm_resource_group.core_resource_group.location
  resource_group_name = azurerm_resource_group.core_resource_group.name
  workspace_id        = data.azurerm_log_analytics_workspace.log_analytics_workspace.id

  application_type    = "web"
}