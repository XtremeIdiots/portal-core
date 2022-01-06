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

resource "azuread_service_principal" "repository_webapi_application_service_principal" {
  application_id = azuread_application.repository_webapi_application.application_id
  use_existing   = true
}

resource "azuread_app_role_assignment" "ingest_function_app_repository_role_assignment" {
  app_role_id         = random_uuid.webapi_serviceaccount_uuid.result
  principal_object_id = azurerm_function_app.ingest_function_app.identity[0].principal_id
  resource_object_id  = azuread_service_principal.repository_webapi_application_service_principal.object_id
}