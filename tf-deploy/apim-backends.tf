resource "azurerm_api_management_backend" "events_funcapp_backend" {
  name                = local.events_function_app_name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name

  protocol    = "http"
  url         = format("https://%s/api/", azurerm_function_app.events_function_app.default_hostname)
  resource_id = azurerm_function_app.events_function_app.id

  credentials {
    query = {
      "code" = data.azurerm_function_app_host_keys.events_function_app_host_key.default_function_key
    }
  }
}

resource "azurerm_api_management_backend" "repository_funcapp_backend" {
  name                = local.repository_function_app_name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name

  protocol    = "http"
  url         = format("https://%s/api/", azurerm_function_app.repository_function_app.default_hostname)
  resource_id = azurerm_function_app.repository_function_app.id

  credentials {
    query = {
      "code" = data.azurerm_function_app_host_keys.repository_function_app_host_key.default_function_key
    }
  }
}
