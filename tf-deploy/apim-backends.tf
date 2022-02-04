resource "azurerm_api_management_backend" "events_funcapp_backend" {
  name                = azurerm_function_app.events_function_app.name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name

  protocol = "http"
  url      = format("https://%s/api/", azurerm_function_app.events_function_app.default_hostname)

  credentials {
    query = {
      "code" = "{{${local.events_function_app_key_secret_name}}}"
    }
  }
}

resource "azurerm_api_management_backend" "repository_funcapp_backend" {
  name                = azurerm_function_app.repository_function_app.name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name

  protocol = "http"
  url      = format("https://%s/api/", azurerm_function_app.repository_function_app.default_hostname)

  credentials {
    query = {
      "code" = "{{${local.repository_function_app_key_secret_name}}}"
    }
  }
}

resource "azurerm_api_management_backend" "repository_webapi_backend" {
  name                = azurerm_app_service.repository_web_api.name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name

  protocol = "http"
  url      = format("https://%s/api/", azurerm_app_service.repository_web_api.default_site_hostname)
}
