resource "azurerm_api_management_named_value" "events_function_app_key" {
  name                = local.events_function_app_key_secret_name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name
  display_name        = "Events Function App Key"
  secret              = true

  value_from_key_vault {
    secret_id = azurerm_key_vault_secret.events_function_app_key.versionless_id
  }
}

resource "azurerm_api_management_named_value" "repository_function_app_key" {
  name                = local.repository_function_app_key_secret_name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name
  display_name        = "Repository Function App Key"
  secret              = true

  value_from_key_vault {
    secret_id = azurerm_key_vault_secret.repository_function_app_key.versionless_id
  }
}
