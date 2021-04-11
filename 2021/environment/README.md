# Cloud Environment Terraform module

## Steps

1. Creates an Application Registration
1. Adds a password for the Application Registration
1. Creates a Service Principal
1. Creates a Resource Group
1. Add the Service Principal as Resource Group Owner
1. Creates a Key Vault in the Resource Group
    1. Adds a Get, List secrets policy for the Service Principal
1. Adds the Application Registration secret to the Key Vault
1. Creates a Storage Account and a container to hold the Terraform states
1. Creates an Azure DevOps Project
    1. Adds a service enpoint with the service principal