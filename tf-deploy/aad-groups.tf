resource "azuread_group" "mgmt_web_app_users_group" {
  display_name     = local.mgmt_web_app_users_group
  owners           = [local.application_owner_object_id, data.azuread_client_config.current.object_id]
  security_enabled = true
}

