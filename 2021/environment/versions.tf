terraform {
  required_version = "> 0.13"

  required_providers {
    azuread = {
      source  = "hashicorp/azuread"
      version = "~> 0.7"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 2.2"
    }
    azuredevops = {
      source = "microsoft/azuredevops"
      version = ">=0.1.0"
    }
    azurerm = {
      source = "hashicorp/azurerm"
      version = ">= 2.0"
      features = {}
    }
  }
}

provider "azurerm" {
  features {}
}
