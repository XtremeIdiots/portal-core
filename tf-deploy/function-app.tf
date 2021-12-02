resource "azurerm_storage_account" "function_app_storage_account" {
    name                = local.function_app_storage_name
    resource_group_name = azurerm_resource_group.core_resource_group.name
    location            = azurerm_resource_group.core_resource_group.location

    account_tier = "Standard"
    account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "function_app_service_plan" {
    name                = local.function_app_service_plan_name
    resource_group_name = azurerm_resource_group.core_resource_group.name
    location            = azurerm_resource_group.core_resource_group.location
    kind                = "FunctionApp"

    sku {
        tier = "Dynamic"
        size = "Y1"
    }
}

resource "azurerm_function_app" "function_app" {
    name                = local.function_app_name
    location            = azurerm_resource_group.core_resource_group.location
    resource_group_name = azurerm_resource_group.core_resource_group.name
    app_service_plan_id = azurerm_app_service_plan.function_app_service_plan.id

    storage_account_name       = azurerm_storage_account.function_app_storage_account.name
    storage_account_access_key = azurerm_storage_account.function_app_storage_account.primary_access_key
    
    version = "~4"

    identity {
        type = "SystemAssigned"
    }

    site_config {
        ftps_state = "Disabled"
        min_tls_version = "1.2"
    }

    app_settings = {
        "APPINSIGHTS_INSTRUMENTATIONKEY" = azurerm_application_insights.application_insights.instrumentation_key
        "WEBSITE_RUN_FROM_PACKAGE" = 1
    }
}

data "azurerm_function_app_host_keys" "function_app_host_key" {
  name                = azurerm_function_app.function_app.name
  resource_group_name = azurerm_resource_group.core_resource_group.name
}