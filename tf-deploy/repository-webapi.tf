resource "random_uuid" "webapi_allaccess_uuid" {
}

resource "random_uuid" "webapi_gamesadmin_uuid" {
}

resource "random_uuid" "webapi_serviceaccount_uuid" {
}

resource "azuread_application" "repository_webapi_application" {
  display_name     = local.repository_web_api_name
  owners           = [data.azuread_client_config.current.object_id]
  sign_in_audience = "AzureADMyOrg"
  identifier_uris  = [format("api://%s", local.repository_web_api_name)]

  api {
    oauth2_permission_scope {
      admin_consent_description  = "AllAccess"
      admin_consent_display_name = "AllAccess"
      enabled                    = true
      id                         = random_uuid.webapi_allaccess_uuid.result
      type                       = "Admin"
      value                      = "AllAccess"
    }

    oauth2_permission_scope {
      admin_consent_description  = "GamesAdmin"
      admin_consent_display_name = "GamesAdmin"
      enabled                    = true
      id                         = random_uuid.webapi_gamesadmin_uuid.result
      type                       = "Admin"
      value                      = "GamesAdmin"
    }
  }

  app_role {
    allowed_member_types = ["Application"]
    description          = "Service Accounts can access/manage all data aspects"
    display_name         = "ServiceAccount"
    enabled              = true
    id                   = random_uuid.webapi_serviceaccount_uuid.result
    value                = "ServiceAccount"
  }

  web {
    logout_url = format("https://%s.azurewebsites.net/signout-oidc", local.repository_web_api_name)
    redirect_uris = [
      format("https://%s.azurewebsites.net/signin-oidc", local.repository_web_api_name)
    ]

    implicit_grant {
      access_token_issuance_enabled = true
      id_token_issuance_enabled     = true
    }
  }

  optional_claims {
    access_token {
      name = "groups"
    }

    id_token {
      name = "groups"
    }
  }
}

resource "azurerm_app_service" "repository_web_api" {
  name                = local.repository_web_api_name
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
    # This is required to be set to support the shared app service plan
    use_32_bit_worker_process = true
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.app_insights_instrumentation_key_secret)
    "WEBSITE_RUN_FROM_PACKAGE"       = 1
    "sql-connection-string"          = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.sql_server_connstring_secret)
    "apim-base-url"                  = azurerm_api_management.api_management.gateway_url
    "apim-subscription-key"          = format("@Microsoft.KeyVault(VaultName=%s;SecretName=%s)", local.key_vault_name, local.apim_repository_web_api_subscription_secret_name)
    "AzureAd:TenantId"               = data.azurerm_client_config.current.tenant_id
    "AzureAd:ClientId"               = azuread_application.repository_webapi_application.application_id
    "AzureAd:Audience"               = azuread_application.repository_webapi_application.application_id
    "ASPNETCORE_ENVIRONMENT"         = var.env == "dev" ? "Development" : "Production"
  }
}
