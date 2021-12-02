terraform {
  required_providers {
    # https://registry.terraform.io/providers/hashicorp/azurerm/latest
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 2.86.0"
    }
  }

  backend "azurerm" {}
}

provider "azurerm" {
  features {}
  subscription_id = var.subscriptionId
}

provider "azurerm" {
  features {}
  alias = "mgmt"
  subscription_id = "b2b3132f-92b4-448c-adf3-c763056f8e94"
}

data "azurerm_client_config" "current" {
}