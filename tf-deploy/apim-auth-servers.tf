resource "azurerm_api_management_authorization_server" "events_function_app_application_auth_server" {
  name                         = local.events_function_app_application_name
  api_management_name          = azurerm_api_management.api_management.name
  resource_group_name          = azurerm_api_management.api_management.resource_group_name
  display_name                 = local.events_function_app_application_name
  authorization_methods        = ["GET", "POST"]
  authorization_endpoint       = format("https://login.microsoftonline.com/%s/oauth2/v2.0/authorize", data.azurerm_client_config.current.tenant_id)
  client_id                    = azuread_application.events_function_app_application.application_id
  client_registration_endpoint = "https://login.microsoftonline.com"

  grant_types = [
    "authorizationCode",
  ]
}