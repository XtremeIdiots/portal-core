resource "azuread_application" "events_function_app_application" {
  display_name     = local.events_function_app_application_name
  owners           = [data.azuread_client_config.current.object_id]
  sign_in_audience = "AzureADMyOrg"
  identifier_uris  = [local.events_function_app_audience]
}

resource "azuread_service_principal" "events_function_app_application_service_principal" {
  application_id = azuread_application.events_function_app_application.application_id
  use_existing   = true
}
