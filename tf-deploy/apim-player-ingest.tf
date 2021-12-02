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

resource "azurerm_api_management_api_policy" "example" {
  api_name            = azurerm_api_management_api.apim_player_ingest.name
  api_management_name = azurerm_api_management.api_management.name
  resource_group_name = azurerm_resource_group.core_resource_group.name

  xml_content = replace(file("./policies/PlayerIngestPolicy.xml"), "__CODE__", data.azurerm_function_app_host_keys.function_app_host_key.default_function_key)
}