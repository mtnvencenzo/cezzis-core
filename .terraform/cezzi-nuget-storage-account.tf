resource "azurerm_storage_account" "cezzi_global_nuget_storage_account" {
  name						 = "st${var.sub}${var.environment}${var.domain}nuget${var.sequence}"
  resource_group_name               = data.azurerm_resource_group.cezzi_global_nuget_resource_group.name
  location					                = data.azurerm_resource_group.cezzi_global_nuget_resource_group.location
  account_tier					            = "Standard"
  account_replication_type          = "LRS"
  cross_tenant_replication_enabled  = false
  access_tier					              = "Hot"
  https_traffic_only_enabled        = true
  min_tls_version					          = "TLS1_2"
  shared_access_key_enabled			    = true
  public_network_access_enabled		  = true
  tags						                  = local.tags

  lifecycle {
    prevent_destroy = true
  }
}

resource "azurerm_storage_container" "cezzi_global_nuget_storage_account_nuget_container" {
  name                  = "nuget"
  storage_account_name    = azurerm_storage_account.cezzi_global_nuget_storage_account.name
  container_access_type = "blob"

  lifecycle {
    prevent_destroy = true
  }
}