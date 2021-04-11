# Make sure to set the following environment variables are set:
# AZDO_PERSONAL_ACCESS_TOKEN
# AZDO_ORG_SERVICE_URL

// This section creates a project
resource "azuredevops_project" "project" {
  name               = var.project_name
  visibility         = "private"
  version_control    = "Git"
  work_item_template = "Agile"
}

# data "azuredevops_group" "group" {
#   project_id = azuredevops_project.project.id
#   name       = "Build Administrators"
# }

// Configuration of AzureRm service end point
resource "azuredevops_serviceendpoint_azurerm" "dev_endpoint" {
  project_id            = azuredevops_project.project.id
  service_endpoint_name = "DevServiceEndpoint"
  credentials {
    serviceprincipalid  = azuread_application.app.application_id
    serviceprincipalkey = azuread_application_password.password.value
  }
  azurerm_spn_tenantid      = data.azurerm_subscription.current.tenant_id
  azurerm_subscription_id   = data.azurerm_subscription.current.subscription_id
  azurerm_subscription_name = data.azurerm_subscription.current.display_name
}
