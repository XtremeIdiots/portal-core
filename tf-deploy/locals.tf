locals {
  // Core Resource Group
  core_rg_name = format("rg-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  // API Management
  apim_name   = format("apim-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  apim_logger = format("apim-logger-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  // App Insights
  app_insights_name = format("ai-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  // Function App
  function_app_name                   = format("fa-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  function_app_service_plan_name      = format("asp-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  function_app_storage_name           = format("safa%s%s%s%s", var.workload, var.env, var.region, var.instance)
  function_app_aad_client_id_name     = format("fa-%s-%s-%s-%s-client-id", var.workload, var.env, var.region, var.instance)
  function_app_aad_client_secret_name = format("fa-%s-%s-%s-%s-client-secret", var.workload, var.env, var.region, var.instance)

  // Key Vault
  key_vault_name = format("kv-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  ## Azure Mgmt Resources

  // Log Analytics
  log_analytics_rg_name = "rg-log-mgmt-eastus-01"
  log_analytics_name    = "log-mgmt-eastus-01"
}
