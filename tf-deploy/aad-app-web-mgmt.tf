resource "time_rotating" "mgmt_web_app_secret_rotation" {
  rotation_days = 365
}

resource "random_uuid" "mgmt_web_app_application_user_uuid" {
}

resource "azuread_application" "mgmt_web_app_application" {
  display_name     = local.mgmt_web_app_application_name
  owners           = [local.application_owner_object_id, data.azuread_client_config.current.object_id]
  sign_in_audience = "AzureADMyOrg"

  web {
    logout_url = format("https://%s.azurewebsites.net/signout-oidc", local.mgmt_web_app_name)
    redirect_uris = var.env == "box" ? [
      format("https://%s.azurewebsites.net/signin-oidc", local.mgmt_web_app_name),
      "https://localhost:7222/signin-oidc"
    ] : [
      format("https://%s.azurewebsites.net/signin-oidc", local.mgmt_web_app_name)
    ]

    implicit_grant {
      access_token_issuance_enabled = true
      id_token_issuance_enabled     = true
    }
  }

  app_role {
    allowed_member_types = ["User"]
    display_name         = "ApplicationUser"
    description          = "Application users can access the management portal"
    enabled              = true
    id                   = random_uuid.mgmt_web_app_application_user_uuid.result
    value                = "ApplicationUser"
  }
}

resource "azuread_service_principal" "mgmt_web_app_local_service_principal" {
  application_id = azuread_application.mgmt_web_app_application.application_id
  use_existing   = true
  owners         = [local.application_owner_object_id, data.azuread_client_config.current.object_id]

  app_role_assignment_required = true
}

resource "azuread_app_role_assignment" "mgmt_web_app_application_user_role_assignment" {
  app_role_id         = random_uuid.mgmt_web_app_application_user_uuid.result
  principal_object_id = azuread_group.mgmt_web_app_users_group.object_id
  resource_object_id  = azuread_service_principal.mgmt_web_app_local_service_principal.object_id
}

resource "azuread_application_password" "mgmt_web_app_application_password" {
  application_object_id = azuread_application.mgmt_web_app_application.object_id
  display_name          = "Mgmt Web App"

  rotate_when_changed = {
    rotation = time_rotating.mgmt_web_app_secret_rotation.id
  }
}
