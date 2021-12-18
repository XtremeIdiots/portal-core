resource "azuread_application" "mgmt_web_app_application" {
  display_name      = local.mgmt_web_app_name
  owners            = [data.azuread_client_config.current.object_id]
  ssign_in_audience = "AzureADMyOrg"

  web {
    logout_url    = "https://localhost:44321/signout-oidc"
    redirect_uris = ["https://localhost:44321/signin-oidc"]

    implicit_grant {
      access_token_issuance_enabled = true
      id_token_issuance_enabled     = true
    }
  }
}

resource "azurerm_app_service" "mgmt_web_app" {
  name                = local.mgmt_web_app_name
  location            = azurerm_resource_group.core_resource_group.location
  resource_group_name = azurerm_resource_group.core_resource_group.name
  app_service_plan_id = azurerm_app_service_plan.web_app_service_plan.id

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
