terraform {
  required_providers {
    # https://registry.terraform.io/providers/hashicorp/azurerm/latest
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 2.90.0"
    }
  }

  backend "azurerm" {}
}

provider "azurerm" {
  features {
    key_vault {
      purge_soft_delete_on_destroy = false
    }
  }
  subscription_id = var.subscriptionId
}

provider "azurerm" {
  features {}
  alias           = "mgmt"
  subscription_id = "b2b3132f-92b4-448c-adf3-c763056f8e94"
}

provider "azuread" {
  alias = "b2c_tenant"
  client_id       = var.b2c_tenant_client_id
  client_secret   = var.b2c_tenant_client_secret
  tenant_id       = local.b2c_tenant_id
}

data "azurerm_client_config" "current" {}
data "azuread_client_config" "current" {}
data "azurerm_subscription" "current" {
}

resource "azurerm_resource_group" "core_resource_group" {
  name     = local.core_rg_name
  location = var.region
}
