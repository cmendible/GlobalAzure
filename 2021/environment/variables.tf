variable "project_name" {
  default = "globalazure2021"
}

variable "resource_group_name" {
  default = "globalazure2021"
}

variable "location" {
  default = "westeurope"
}

variable aad_users {
  type    = list(string)
  default = []
}
