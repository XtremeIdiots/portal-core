resource "azurerm_api_management_api" "apim_events" {
  name                = "events"
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name

  revision     = "1"
  display_name = "Events"
  path         = "events"
  protocols    = ["https"]

  import {
    content_format = "openapi+json"
    content_value  = file(format("./apis/Events.openapi+json.json"))
  }
}

locals {
  events_policy_1 = file("./policies/EventsPolicy.xml")
  events_policy_2 = replace(local.events_policy_1, "__backend_service_id__", azurerm_api_management_backend.events_funcapp_backend.name)
  events_policy_3 = replace(local.events_policy_2, "__tenant_id__", data.azurerm_client_config.current.tenant_id)
  events_policy_4 = replace(local.events_policy_3, "__audience__", local.events_api_application_audience)
}

resource "azurerm_api_management_api_policy" "events_api_policy" {
  api_name            = azurerm_api_management_api.apim_events.name
  api_management_name = azurerm_api_management.api_management.name
  resource_group_name = azurerm_resource_group.core_resource_group.name

  xml_content = local.events_policy_4
}

resource "azurerm_api_management_api_diagnostic" "events_api_diagnostic" {
  identifier               = "applicationinsights"
  resource_group_name      = azurerm_resource_group.core_resource_group.name
  api_management_name      = azurerm_api_management.api_management.name
  api_name                 = azurerm_api_management_api.apim_events.name
  api_management_logger_id = azurerm_api_management_logger.api_management_api_logger.id

  sampling_percentage       = 100
  always_log_errors         = true
  log_client_ip             = true
  verbosity                 = "verbose"
  http_correlation_protocol = "W3C"
}
