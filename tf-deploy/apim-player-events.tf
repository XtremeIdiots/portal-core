resource "azurerm_api_management_api" "apim_player_events" {
  name                = "player-events"
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name

  service_url = format("https://%s/api/", azurerm_function_app.events_function_app.default_hostname)

  revision     = "1"
  display_name = "Player Events"
  path         = "player-events"
  protocols    = ["https"]

  import {
    content_format = "openapi+json"
    content_value  = file(format("./apis/Player Events.openapi+json.json"))
  }
}

resource "azurerm_api_management_api_policy" "player_events_api_policy" {
  api_name            = azurerm_api_management_api.apim_player_events.name
  api_management_name = azurerm_api_management.api_management.name
  resource_group_name = azurerm_resource_group.core_resource_group.name

  xml_content = replace(file("./policies/PlayerEventsPolicy.xml"), "__backend_service_id__", azurerm_api_management_backend.events_funcapp_backend.name)
}

resource "azurerm_api_management_api_diagnostic" "player_events_api_diagnostic" {
  identifier               = "applicationinsights"
  resource_group_name      = azurerm_resource_group.core_resource_group.name
  api_management_name      = azurerm_api_management.api_management.name
  api_name                 = azurerm_api_management_api.apim_player_events.name
  api_management_logger_id = azurerm_api_management_logger.api_management_api_logger.id

  sampling_percentage       = 100
  always_log_errors         = true
  log_client_ip             = true
  verbosity                 = "verbose"
  http_correlation_protocol = "W3C"
}
