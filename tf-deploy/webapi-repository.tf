resource "azurerm_app_service" "repository_web_api" {
  name                = local.repository_web_api_name
  location            = azurerm_resource_group.core_resource_group.location
  resource_group_name = azurerm_resource_group.core_resource_group.name
  app_service_plan_id = azurerm_app_service_plan.web_app_service_plan.id

  https_only = true

  identity {
    type = "SystemAssigned"
  }

  site_config {
    ftps_state               = "Disabled"
    min_tls_version          = "1.2"
    dotnet_framework_version = "v6.0"
    # This is required to be set to support the shared app service plan
    use_32_bit_worker_process = true
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.app_insights_instrumentation_key_secret)
    "WEBSITE_RUN_FROM_PACKAGE"       = 1
    "ASPNETCORE_ENVIRONMENT"         = var.env == "dev" ? "Development" : "Production"
    "minTlsVersion"                  = "1.2"
    "sql-connection-string"          = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.sql_server_connstring_identity_secret)
    "apim-base-url"                  = azurerm_api_management.api_management.gateway_url
    "apim-subscription-key"          = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.apim_repository_web_api_subscription_secret_name)
    "AzureAd:TenantId"               = data.azurerm_client_config.current.tenant_id
    "AzureAd:ClientId"               = azuread_application.repository_api_application.application_id
    "AzureAd:Audience"               = local.repository_api_application_audience
    "gameservers-keyvault-endpoint"  = azurerm_key_vault.game_servers_key_vault.vault_uri
  }
}
