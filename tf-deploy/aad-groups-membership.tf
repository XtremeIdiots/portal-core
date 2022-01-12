resource "azuread_group_member" "principal_member_of_sql_server_admins" {
  group_object_id  = azuread_group.sql_server_admins.id
  member_object_id = var.override_principal_object_id == "" ? data.azurerm_client_config.current.object_id : var.override_principal_object_id 
}