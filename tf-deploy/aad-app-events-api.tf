resource "random_uuid" "events_api_player_generator_uuid" {
}

resource "random_uuid" "events_api_server_generator_uuid" {
}

resource "azuread_application" "events_api_application" {
  display_name     = local.events_api_application_name
  owners           = [local.application_owner_object_id, data.azuread_client_config.current.object_id]
  sign_in_audience = "AzureADMyOrg"
  identifier_uris  = [local.events_api_application_audience]

  app_role {
    allowed_member_types = ["Application"]
    display_name         = "PlayerEventGenerator"
    description          = "Player event generators can submit player events to the API"
    enabled              = true
    id                   = random_uuid.events_api_player_generator_uuid.result
    value                = "PlayerEventGenerator"
  }

  app_role {
    allowed_member_types = ["Application"]
    display_name         = "ServerEventGenerator"
    description          = "Server event generators can submit server events to the API"
    enabled              = true
    id                   = random_uuid.events_api_server_generator_uuid.result
    value                = "ServerEventGenerator"
  }
}

resource "azuread_service_principal" "events_api_application_service_principal" {
  application_id = azuread_application.events_api_application.application_id
  use_existing   = true
}

resource "azuread_app_role_assignment" "events_api_b3bot_client_player_generator_role_assignment" {
  app_role_id         = random_uuid.events_api_player_generator_uuid.result
  principal_object_id = azuread_service_principal.b3bots_client_application_service_principal.object_id
  resource_object_id  = azuread_service_principal.events_api_application_service_principal.object_id
}

resource "azuread_app_role_assignment" "events_api_b3bot_client_server_generator_role_assignment" {
  app_role_id         = random_uuid.events_api_server_generator_uuid.result
  principal_object_id = azuread_service_principal.b3bots_client_application_service_principal.object_id
  resource_object_id  = azuread_service_principal.events_api_application_service_principal.object_id
}
