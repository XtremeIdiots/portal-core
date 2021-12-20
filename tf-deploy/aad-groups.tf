resource "azuread_group" "repository_service_writers" {
  display_name     = local.repository_service_writers
  owners           = [data.azuread_client_config.current.object_id]
  security_enabled = true
}

