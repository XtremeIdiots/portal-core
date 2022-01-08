resource "random_uuid" "repository_api_all_access_uuid" {
}

resource "random_uuid" "repository_api_games_admin_uuid" {
}

resource "random_uuid" "repository_api_service_account_uuid" {
}

resource "azuread_application" "repository_api_application" {
  display_name     = local.repository_api_application_name
  owners           = [data.azuread_client_config.current.object_id]
  sign_in_audience = "AzureADMyOrg"
  identifier_uris  = [local.repository_api_application_audience]

  api {
    oauth2_permission_scope {
      admin_consent_description  = "AllAccess"
      admin_consent_display_name = "AllAccess"
      enabled                    = true
      id                         = random_uuid.repository_api_all_access_uuid.result
      type                       = "Admin"
      value                      = "AllAccess"
    }

    oauth2_permission_scope {
      admin_consent_description  = "GamesAdmin"
      admin_consent_display_name = "GamesAdmin"
      enabled                    = true
      id                         = random_uuid.repository_api_games_admin_uuid.result
      type                       = "Admin"
      value                      = "GamesAdmin"
    }
  }

  app_role {
    allowed_member_types = ["Application"]
    description          = "Service Accounts can access/manage all data aspects"
    display_name         = "ServiceAccount"
    enabled              = true
    id                   = random_uuid.repository_api_service_account_uuid.result
    value                = "ServiceAccount"
  }
}

resource "azuread_service_principal" "repository_api_application_service_principal" {
  application_id = azuread_application.repository_api_application.application_id
  use_existing   = true
}

resource "azuread_app_role_assignment" "repository_api_ingest_function_app_service_account_role_assingment" {
  app_role_id         = random_uuid.repository_api_service_account_uuid.result
  principal_object_id = azurerm_function_app.ingest_function_app.identity[0].principal_id
  resource_object_id  = azuread_service_principal.repository_api_application_service_principal.object_id
}
