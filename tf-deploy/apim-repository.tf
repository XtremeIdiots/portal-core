resource "azurerm_api_management_api" "apim_repository" {
  name                = "repository"
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name

  revision     = "1"
  display_name = "Repository"
  path         = "repository"
  protocols    = ["https"]

  import {
    content_format = "openapi+json"
    content_value  = file(format("./apis/Repository.openapi+json.json"))
  }
}

locals {
  gsra_policy_1 = file("./policies/RepositoryPolicy.xml")
  gsra_policy_2 = replace(local.gsra_policy_1, "__backend_service_id__", azurerm_api_management_backend.repository_webapi_backend.name)
}

resource "azurerm_api_management_api_policy" "repository_api_policy" {
  api_name            = azurerm_api_management_api.apim_repository.name
  api_management_name = azurerm_api_management.api_management.name
  resource_group_name = azurerm_resource_group.core_resource_group.name

  xml_content = local.gsra_policy_2
}

resource "azurerm_api_management_api_diagnostic" "repository_api_diagnostic" {
  identifier               = "applicationinsights"
  resource_group_name      = azurerm_resource_group.core_resource_group.name
  api_management_name      = azurerm_api_management.api_management.name
  api_name                 = azurerm_api_management_api.apim_repository.name
  api_management_logger_id = azurerm_api_management_logger.api_management_api_logger.id

  sampling_percentage       = 100
  always_log_errors         = true
  log_client_ip             = true
  verbosity                 = "verbose"
  http_correlation_protocol = "W3C"
}
