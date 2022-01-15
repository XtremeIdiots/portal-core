resource "azuread_group" "mgmt_web_app_users_group" {
  display_name     = local.mgmt_web_app_users_group
  owners           = [local.application_owner_object_id, data.azuread_client_config.current.object_id]
  security_enabled = true
}

resource "azuread_group" "sql_server_admins" {
  display_name     = local.sql_server_admins_group
  owners           = [local.application_owner_object_id, data.azuread_client_config.current.object_id]
  security_enabled = true
}

resource "azuread_group" "sql_portal_database_readers_group" {
  display_name     = local.sql_portal_database_readers_group
  owners           = [local.application_owner_object_id, data.azuread_client_config.current.object_id]
  security_enabled = true
}

resource "azuread_group" "sql_portal_database_writers_group" {
  display_name     = local.sql_portal_database_writers_group
  owners           = [local.application_owner_object_id, data.azuread_client_config.current.object_id]
  security_enabled = true
}
