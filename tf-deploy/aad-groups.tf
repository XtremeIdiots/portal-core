resource "azuread_group" "mgmt_web_app_users_group" {
  display_name     = local.mgmt_web_app_users_group
  owners           = [data.azuread_client_config.current.object_id]
  security_enabled = true
}

