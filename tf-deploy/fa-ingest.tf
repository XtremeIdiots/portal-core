resource "azurerm_storage_account" "ingest_function_app_storage_account" {
  name                = local.ingest_app_storage_name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  location            = azurerm_resource_group.core_resource_group.location

  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_function_app" "ingest_function_app" {
  name                = local.ingest_function_app_name
  location            = azurerm_resource_group.core_resource_group.location
  resource_group_name = azurerm_resource_group.core_resource_group.name
  app_service_plan_id = azurerm_app_service_plan.function_app_service_plan.id

  storage_account_name       = azurerm_storage_account.ingest_function_app_storage_account.name
  storage_account_access_key = azurerm_storage_account.ingest_function_app_storage_account.primary_access_key

  version = "~4"

  identity {
    type = "SystemAssigned"
  }

  site_config {
    ftps_state      = "Disabled"
    min_tls_version = "1.2"
  }

  auth_settings {
    enabled          = true
    default_provider = "AzureActiveDirectory"

    active_directory {
      client_id = azuread_application.events_function_app_application.application_id
    }
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY"     = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.app_insights_instrumentation_key_secret)
    "WEBSITE_RUN_FROM_PACKAGE"           = 1
    "service-bus-connection-string"      = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.service_bus_connection_string_secret)
    "apim-base-url"                      = azurerm_api_management.api_management.gateway_url,
    "apim-subscription-key"              = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.apim_ingest_funcapp_subscription_secret_name)
    "webapi-portal-application-audience" = local.repository_web_api_audience
  }
}

data "azurerm_function_app_host_keys" "ingest_function_app_host_key" {
  name                = azurerm_function_app.ingest_function_app.name
  resource_group_name = azurerm_resource_group.core_resource_group.name
}
