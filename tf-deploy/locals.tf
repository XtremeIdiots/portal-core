locals {
  // API Management
  apim_rg_name = format("rg-%s_%s-%s-%s-%s", var.workload, "apim", var.env, var.region, var.instance)
  apim_name    = format("apim-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
  apim_logger  = format("apim-logger-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  // App Insights
  app_insights_rg_name = format("rg-%s_%s-%s-%s-%s", var.workload, "ai", var.env, var.region, var.instance)
  app_insights_name    = format("ai-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)

  ## Azure Mgmt Resources

  // Log Analytics
  log_analytics_rg_name = "rg-log-mgmt-eastus-01"
  log_analytics_name    = "log-mgmt-eastus-01"
}
