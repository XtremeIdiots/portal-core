locals {
  apim_rg_name = format("rg-%s_%s-%s-%s-%s", var.workload, "apim", var.env, var.region, var.instance)
  apim_name    = format("apim-%s-%s-%s-%s", var.workload, var.env, var.region, var.instance)
}
