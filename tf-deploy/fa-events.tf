resource "azurerm_storage_account" "events_function_app_storage_account" {
  name                = local.events_app_storage_name
  resource_group_name = azurerm_resource_group.core_resource_group.name
  location            = azurerm_resource_group.core_resource_group.location

  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_function_app" "events_function_app" {
  name                = local.events_function_app_name
  location            = azurerm_resource_group.core_resource_group.location
  resource_group_name = azurerm_resource_group.core_resource_group.name
  app_service_plan_id = azurerm_app_service_plan.function_app_service_plan.id

  storage_account_name       = azurerm_storage_account.events_function_app_storage_account.name
  storage_account_access_key = azurerm_storage_account.events_function_app_storage_account.primary_access_key

  version    = "~4"
  https_only = true

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
      client_id         = azuread_application.events_api_application.application_id
      allowed_audiences = [local.events_api_application_audience]
    }
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.app_insights_instrumentation_key_secret)
    "WEBSITE_RUN_FROM_PACKAGE"       = 1
    "service-bus-connection-string"  = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.service_bus_connection_string_secret)
  }
}

data "azurerm_function_app_host_keys" "events_function_app_host_key" {
  name                = azurerm_function_app.events_function_app.name
  resource_group_name = azurerm_resource_group.core_resource_group.name
}
