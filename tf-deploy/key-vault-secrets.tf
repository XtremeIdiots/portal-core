resource "azurerm_key_vault_secret" "service_bus_connection_string" {
  name         = local.service_bus_connection_string_secret
  value        = azurerm_servicebus_namespace.service_bus.default_primary_connection_string
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "application_insights_instrumentation_key" {
  name         = local.app_insights_instrumentation_key_secret
  value        = azurerm_application_insights.application_insights.instrumentation_key
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "sql_server_admin_username" {
  name         = local.sql_server_admin_username_secret
  value        = local.sql_server_admin_username
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "sql_server_admin_password" {
  name         = local.sql_server_admin_password_secret
  value        = random_password.sql_server_admin_password.result
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "sql_server_sqlauth_connection_string" {
  name         = local.sql_server_connstring_sqlauth_secret
  value        = format("Server=tcp:%s,1433;Initial Catalog=%s;Persist Security Info=False;User ID=%s;Password=%s;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", azurerm_mssql_server.sql_server.fully_qualified_domain_name, local.sql_database_name, local.sql_server_admin_username, random_password.sql_server_admin_password.result)
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "sql_server_identity_connection_login" {
  name         = local.sql_server_connstring_identity_secret
  value        = format("Server=tcp:%s,1433;Initial Catalog=%s;Persist Security Info=False;Authentication=Active Directory Device Code Flow;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", azurerm_mssql_server.sql_server.fully_qualified_domain_name, local.sql_database_name)
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "b3bot_subscription_key" {
  name         = local.apim_b3bot_subscription_secret_name
  value        = azurerm_api_management_subscription.b3bot_subscription.primary_key
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "ingest_funcapp_subscription_key" {
  name         = local.apim_ingest_funcapp_subscription_secret_name
  value        = azurerm_api_management_subscription.ingest_funcapp_subscription.primary_key
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "mgmt_web_app_subscription_key" {
  name         = local.apim_mgmt_web_app_subscription_secret_name
  value        = azurerm_api_management_subscription.mgmt_web_app_subscription.primary_key
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "admin_web_app_subscription_key" {
  name         = local.apim_admin_web_app_subscription_secret_name
  value        = azurerm_api_management_subscription.admin_web_app_subscription.primary_key
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "public_web_app_subscription_key" {
  name         = local.apim_public_web_app_subscription_secret_name
  value        = azurerm_api_management_subscription.public_web_app_subscription.primary_key
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "repository_webapi_subscription_key" {
  name         = local.apim_repository_web_api_subscription_secret_name
  value        = azurerm_api_management_subscription.repository_web_api_subscription.primary_key
  key_vault_id = azurerm_key_vault.key_vault.id

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "b3bot_client_application_secret" {
  name            = local.b3bot_client_application_secret_name
  value           = azuread_application_password.b3bots_client_application_password.value
  key_vault_id    = azurerm_key_vault.key_vault.id
  expiration_date = azuread_application_password.b3bots_client_application_password.end_date

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}

resource "azurerm_key_vault_secret" "mgmt_web_app_application_secret" {
  name            = local.mgmt_web_app_application_secret_name
  value           = azuread_application_password.mgmt_web_app_application_password.value
  key_vault_id    = azurerm_key_vault.key_vault.id
  expiration_date = azuread_application_password.mgmt_web_app_application_password.end_date

  depends_on = [
    azurerm_key_vault_access_policy.principal_key_vault_access_policy
  ]
}
