resource "azurerm_api_management_certificate" "apim_repository_client_cert" {
  name                = local.apim_repository_client_cert_secret_name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name

  key_vault_secret_id = azurerm_key_vault_certificate.apim_repository_client_cert.versionless_secret_id
}
