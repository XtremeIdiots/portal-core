resource "azurerm_key_vault" "key_vault" {
  name                = local.key_vault_name
  location            = azurerm_resource_group.core_resource_group.location
  resource_group_name = azurerm_resource_group.core_resource_group.name
  tenant_id           = data.azurerm_client_config.current.tenant_id

  sku_name = "standard"
}

resource "azurerm_key_vault_access_policy" "principal_key_vault_access_policy" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id

  certificate_permissions = [
    "Create",
    "Delete",
    "Get",
    "List",
    "Update"
  ]

  secret_permissions = [
    "Get",
    "List",
    "Set",
    "Delete",
    "Recover"
  ]
}

resource "azurerm_key_vault_access_policy" "api_management_key_vault_access_policy" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_api_management.api_management.identity[0].principal_id

  secret_permissions = [
    "Get"
  ]
}

resource "azurerm_key_vault_access_policy" "events_function_app_key_vault_access_policy" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_function_app.events_function_app.identity[0].principal_id

  secret_permissions = [
    "Get"
  ]
}

resource "azurerm_key_vault_access_policy" "ingest_function_app_key_vault_access_policy" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_function_app.ingest_function_app.identity[0].principal_id

  secret_permissions = [
    "Get"
  ]
}

resource "azurerm_key_vault_access_policy" "repository_function_app_key_vault_access_policy" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_function_app.repository_function_app.identity[0].principal_id

  secret_permissions = [
    "Get"
  ]
}

resource "azurerm_key_vault_access_policy" "bansync_function_app_key_vault_access_policy" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_function_app.bansync_function_app.identity[0].principal_id

  secret_permissions = [
    "Get"
  ]
}

resource "azurerm_key_vault_access_policy" "mgmt_web_app_key_vault_access_policy" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_app_service.mgmt_web_app.identity[0].principal_id

  secret_permissions = [
    "Get"
  ]
}

resource "azurerm_key_vault_access_policy" "admin_web_app_key_vault_access_policy" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_app_service.admin_web_app.identity[0].principal_id

  secret_permissions = [
    "Get"
  ]
}

resource "azurerm_key_vault_access_policy" "public_web_app_key_vault_access_policy" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_app_service.public_web_app.identity[0].principal_id

  secret_permissions = [
    "Get"
  ]
}

resource "azurerm_key_vault_access_policy" "repository_webapi_key_vault_access_policy" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_app_service.repository_web_api.identity[0].principal_id

  secret_permissions = [
    "Get"
  ]
}

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

resource "azurerm_key_vault_secret" "sql_server_connection_string" {
  name         = local.sql_server_connstring_secret
  value        = format("Server=tcp:%s,1433;Initial Catalog=%s;Persist Security Info=False;User ID=%s;Password=%s;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", azurerm_mssql_server.sql_server.fully_qualified_domain_name, local.sql_database_name, local.sql_server_admin_username, random_password.sql_server_admin_password.result)
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
