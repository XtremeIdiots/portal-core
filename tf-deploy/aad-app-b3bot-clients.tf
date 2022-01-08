resource "azuread_application" "b3bots_client_application" {
  display_name = local.b3bots_client_application_name
  owners       = [data.azuread_client_config.current.object_id]
}

resource "azuread_service_principal" "b3bots_client_application_service_principal" {
  application_id = azuread_application.b3bots_client_application.application_id
  use_existing   = true
}
