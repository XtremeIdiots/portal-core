resource "azurerm_api_management_api" "apim_player_ingest" {
  name                = "player-ingest"
  resource_group_name = azurerm_resource_group.core_resource_group.name
  api_management_name = azurerm_api_management.api_management.name

  service_url = format("https://%s/api/", azurerm_function_app.function_app.default_hostname)

  revision            = "1"
  display_name        = "Player Ingest"
  path                = "player-ingest"
  protocols           = ["https"]

  import {
    content_format = "openapi+json"
    content_value  = file(format("./apis/Player Ingest.openapi+json.json"))
  }
}

resource "azurerm_api_management_api_policy" "player_ingest_api_policy" {
  api_name            = azurerm_api_management_api.apim_player_ingest.name
  api_management_name = azurerm_api_management.api_management.name
  resource_group_name = azurerm_resource_group.core_resource_group.name

  xml_content = replace(file("./policies/PlayerIngestPolicy.xml"), "__CODE__", data.azurerm_function_app_host_keys.function_app_host_key.default_function_key)
}

resource "azurerm_api_management_api_diagnostic" "player_ingest_api_diagnostic" {
  identifier               = "applicationinsights"
  resource_group_name      = azurerm_resource_group.core_resource_group.name
  api_management_name      = azurerm_api_management.api_management.name
  api_name                 = azurerm_api_management_api.apim_player_ingest.name
  api_management_logger_id = azurerm_api_management_logger.api_management_api_logger.id

  sampling_percentage       = 100
  always_log_errors         = true
  log_client_ip             = true
  verbosity                 = "verbose"
  http_correlation_protocol = "W3C"
}