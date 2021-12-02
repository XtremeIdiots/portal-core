variable "subscriptionId" {
  type = string
}

variable "env" {
  type = string
}

variable "workload" {
  type = string
  default = "portal"
}

variable "region" {
  type = string
}

variable "instance" {
  type = string
}
