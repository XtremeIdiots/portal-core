resource "azurerm_api_management_named_value" "events_function_app_key" {
  name                = "events_function_app_key"
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name
  display_name        = local.events_function_app_key_secret_name
  secret              = true

  value_from_key_vault {
    secret_id = azurerm_key_vault_secret.events_function_app_key.versionless_id
  }
}

resource "azurerm_api_management_named_value" "repository_function_app_key" {
  name                = "repository_function_app_key"
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name
  display_name        = local.repository_function_app_key_secret_name
  secret              = true

  value_from_key_vault {
    secret_id = azurerm_key_vault_secret.repository_function_app_key.versionless_id
  }
}
