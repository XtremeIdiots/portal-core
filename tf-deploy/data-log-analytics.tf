data "azurerm_log_analytics_workspace" "log_analytics_workspace" {
  name                = local.log_analytics_name
  resource_group_name = local.log_analytics_rg_name

  provider = azurerm.mgmt
}