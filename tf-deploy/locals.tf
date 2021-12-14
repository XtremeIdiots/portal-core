locals {
  // Core Resource Group
  core_rg_name = format("rg-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  // API Management
  apim_name   = format("apim-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  apim_logger = format("apim-logger-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  // App Insights
  app_insights_name                       = format("ai-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  app_insights_instrumentation_key_secret = format("%s-instrumentation-key", local.app_insights_name)

  // Function Apps
  function_app_service_plan_name = format("asp-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  ingest_function_app_name     = format("fa-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  repository_function_app_name = format("fa-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  ingest_app_storage_name      = format("safa%s%s%s%s", var.workload, var.env, var.region, var.instance)
  repository_app_storage_name  = format("safa%s%s%s%s", var.workload, var.env, var.region, var.instance)

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
