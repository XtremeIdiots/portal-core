resource "azuread_group_member" "principal_member_of_sql_server_admins" {
  group_object_id  = azuread_group.sql_server_admins.id
  member_object_id = var.override_principal_object_id == "" ? data.azurerm_client_config.current.object_id : var.override_principal_object_id 
}

resource "azuread_group_member" "repository_function_app_member_of_portal_database_writers" {
  group_object_id  = azuread_group.sql_portal_database_writers_group.id
  member_object_id = azurerm_function_app.repository_function_app.identity[0].principal_id
}

resource "azuread_group_member" "repository_webapi_member_of_portal_database_writers" {
  group_object_id  = azuread_group.sql_portal_database_writers_group.id
  member_object_id = azurerm_app_service.repository_web_api.identity[0].principal_id
}