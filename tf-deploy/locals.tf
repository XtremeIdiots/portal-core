locals {
  // Core Resource Group
  core_rg_name = format("rg-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  // API Management
  apim_name   = format("apim-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  apim_logger = format("apim-logger-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  // Secret Names
  apim_b3bot_subscription_secret_name              = format("%s-b3bot-subscription-key", local.apim_name)
  apim_ingest_funcapp_subscription_secret_name     = format("%s-ingest-funcapp-subscription-key", local.apim_name)
  apim_mgmt_web_app_subscription_secret_name       = format("%s-mgmt-webapp-subscription-key", local.apim_name)
  apim_admin_web_app_subscription_secret_name      = format("%s-admin-webapp-subscription-key", local.apim_name)
  apim_public_web_app_subscription_secret_name     = format("%s-public-webapp-subscription-key", local.apim_name)
  apim_repository_web_api_subscription_secret_name = format("%s-repository-webapi-subscription-key", local.apim_name)

  // App Insights
  app_insights_name                       = format("ai-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  app_insights_instrumentation_key_secret = format("%s-instrumentation-key", local.app_insights_name)

  // App Service Plans
  function_app_service_plan_name = format("asp-%s-%s-%s-%s", var.workload, var.env, var.region, "01")
  web_app_service_plan_name      = format("asp-%s-%s-%s-%s", var.workload, var.env, var.region, "02")

  // Function Apps
  ingest_function_app_name     = format("fa-%s-ingest-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  repository_function_app_name = format("fa-%s-repository-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  events_function_app_name     = format("fa-%s-events-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  ingest_app_storage_name      = format("safa%s%s%s%s", var.workload, var.env, var.region, "01")
  repository_app_storage_name  = format("safa%s%s%s%s", var.workload, var.env, var.region, "02")
  events_app_storage_name      = format("safa%s%s%s%s", var.workload, var.env, var.region, "03")

  // Web Apps
  mgmt_web_app_name   = format("web-%s-mgmt-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  admin_web_app_name  = format("web-%s-admin-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  public_web_app_name = format("web-%s-public-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  repository_web_api  = format("webapi-%s-repository-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  // Key Vault
  key_vault_name = format("kv-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  // Service Bus
  service_bus_name                     = format("sb-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  service_bus_connection_string_secret = format("%s-connection-string", local.service_bus_name)

  // SQL Server
  sql_server_name                  = format("sql-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  sql_server_admin_username        = format("%s-admin", local.sql_server_name)
  sql_server_connstring_secret     = format("%s-connection-string", local.sql_server_name)
  sql_server_admin_username_secret = format("%s-username", local.sql_server_name)
  sql_server_admin_password_secret = format("%s-password", local.sql_server_name)

  // SQL Database
  sql_database_name = "portal"

  ## Azure Mgmt Resources

  // Log Analytics
  log_analytics_rg_name = "rg-log-mgmt-eastus-01"
  log_analytics_name    = "log-mgmt-eastus-01"
}
