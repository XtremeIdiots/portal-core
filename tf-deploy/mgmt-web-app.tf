resource "azurerm_storage_account" "mgmgt_web_app_storage_account" {
  name                = local.mgmt_web_app_storage_name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  location            = azurerm_resource_group.core_resource_group.location

  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service" "mgmt_web_app" {
  name                = local.mgmt_web_app_name
  location            = azurerm_resource_group.core_resource_group.location
  resource_group_name = azurerm_resource_group.core_resource_group.name
  app_service_plan_id = azurerm_app_service_plan.web_app_service_plan.id

  storage_account_name       = azurerm_storage_account.mgmt_web_app_storage_account.name
  storage_account_access_key = azurerm_storage_account.mgmt_web_app_storage_account.primary_access_key

  identity {
    type = "SystemAssigned"
  }

  site_config {
    ftps_state               = "Disabled"
    min_tls_version          = "1.2"
    dotnet_framework_version = "v6.0"
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.app_insights_instrumentation_key_secret)
    "WEBSITE_RUN_FROM_PACKAGE"       = 1
    "apim-base-url"                  = azurerm_api_management.api_management.gateway_url,
    "apim-subscription-key"          = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.apim_mgmt_web_app_subscription_secret_name)
  }
}
