resource "time_rotating" "b3bots_client_secret_rotation" {
  rotation_days = 365
}

resource "azuread_application" "b3bots_client_application" {
  display_name = local.b3bots_client_application_name
  owners       = [local.application_owner_object_id, data.azuread_client_config.current.object_id]

  required_resource_access {
    resource_app_id = azuread_application.events_api_application.application_id

    resource_access {
      id   = random_uuid.events_api_player_generator_uuid.result
      type = "Role"
    }

    resource_access {
      id   = random_uuid.events_api_server_generator_uuid.result
      type = "Role"
    }
  }
}

resource "azuread_service_principal" "b3bots_client_application_service_principal" {
  application_id = azuread_application.b3bots_client_application.application_id
  use_existing   = true
}

resource "azuread_application_password" "b3bots_client_application_password" {
  application_object_id = azuread_application.b3bots_client_application.object_id
  display_name          = "B3Bot Client"

  rotate_when_changed = {
    rotation = time_rotating.b3bots_client_secret_rotation.id
  }
}
