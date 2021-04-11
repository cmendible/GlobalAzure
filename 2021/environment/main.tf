data "azurerm_subscription" "current" {}

data "azurerm_client_config" "current" {}

# Create Application Registration. 
resource "azuread_application" "app" {
  name                       = var.project_name
  homepage                   = "http://${var.project_name}"
  identifier_uris            = ["http://${var.project_name}"]
  reply_urls                 = []
  available_to_other_tenants = false
  oauth2_allow_implicit_flow = false
}

# Create Service Principal
resource "azuread_service_principal" "sp" {
  application_id               = azuread_application.app.application_id
  app_role_assignment_required = false

  tags = []
}

resource "random_password" "password" {
  length           = 16
  special          = true
  override_special = "_%@"
  keepers = {
    app_id = azuread_application.app.application_id
  }
}

resource "azuread_application_password" "password" {
  application_object_id = azuread_application.app.id
  value                 = random_password.password.result
  end_date_relative     = "87600h"
}

# Create Resource Group for the Resource Group
resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location

  tags = {}
}

# Add Service Principal as Resource Group Owner
resource "azurerm_role_assignment" "owner" {
  scope                = azurerm_resource_group.rg.id
  role_definition_name = "Owner"
  principal_id         = azuread_service_principal.sp.object_id
}

# Deploy Key Vault
resource "azurerm_key_vault" "kv" {
  name                = var.project_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tenant_id           = data.azurerm_subscription.current.tenant_id

  sku_name = "standard"

  access_policy {
    tenant_id = data.azurerm_subscription.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    key_permissions = []

    secret_permissions = [
      "set",
      "get",
      "list"
    ]

    storage_permissions = []
  }

  access_policy {
    tenant_id = data.azurerm_subscription.current.tenant_id
    object_id = azuread_service_principal.sp.object_id

    key_permissions = []

    secret_permissions = [
      "get",
      "list"
    ]

    storage_permissions = []
  }
}

resource "azurerm_key_vault_secret" "sp" {
  key_vault_id = azurerm_key_vault.kv.id
  name         = azuread_application.app.application_id
  value        = azuread_application_password.password.value
}

resource "azurerm_key_vault_secret" "owner" {
  key_vault_id = azurerm_key_vault.kv.id
  name         = "owner"
  value        = azuread_application.app.application_id
}

resource "azurerm_storage_account" "tf_backend" {
  name                      = "${replace(var.resource_group_name, "-", "")}tf"
  resource_group_name       = azurerm_resource_group.rg.name
  location                  = var.location
  account_tier              = "Standard"
  account_replication_type  = "GRS"
  enable_https_traffic_only = true
}

resource "azurerm_storage_container" "tf_container" {
  name                  = "tfstate"
  storage_account_name  = azurerm_storage_account.tf_backend.name
  container_access_type = "private"
}