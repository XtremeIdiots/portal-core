resource "azuread_application" "mgmt_web_app_application" {
  display_name     = local.mgmt_web_app_name
  owners           = [data.azuread_client_config.current.object_id]
  sign_in_audience = "AzureADMyOrg"

  web {
    logout_url = format("https://%s.azurewebsites.net/signout-oidc", local.mgmt_web_app_name)
    redirect_uris = [
      format("https://%s.azurewebsites.net/signin-oidc", local.mgmt_web_app_name)
    ]

    implicit_grant {
      access_token_issuance_enabled = true
      id_token_issuance_enabled     = true
    }
  }
}
