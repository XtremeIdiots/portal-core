variable "subscriptionId" {
  type = string
}

variable "env" {
  type = string
}

variable "workload" {
  type    = string
  default = "portal"
}

variable "region" {
  type = string
}

variable "instance" {
  type = string
}

variable "override_principal_object_id" {
  type    = string
  default = ""
}

variable "b2c_tenant_client_id" {
  type    = string
  default = ""
}

variable "b2c_tenant_client_secret" {
  type    = string
  default = ""
}
