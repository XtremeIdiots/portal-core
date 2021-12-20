resource "azuread_group_member" "apim_member_of_repository_service_writers" {
  group_object_id  = azuread_group.repository_service_writers.id
  member_object_id = azurerm_api_management.api_management.identity[0].principal_id
}
